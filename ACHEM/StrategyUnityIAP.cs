using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;

public class StrategyUnityIAP : BillingStrategy, IStoreListener
{
	public const string platform = "apple";

	private IStoreController m_StoreController;

	private IExtensionProvider m_StoreExtensionProvider;

	private ConfigurationBuilder builder;

	private static HashSet<string> PendingTransactions = new HashSet<string>();

	public override void Init()
	{
		RequestProductData();
		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		if (OnInitSuccess != null)
		{
			OnInitSuccess();
		}
	}

	public bool IsInitialized()
	{
		if (m_StoreController != null)
		{
			return m_StoreExtensionProvider != null;
		}
		return false;
	}

	public override string GetLocalizedPriceString(string productID)
	{
		if (m_StoreController == null)
		{
			return null;
		}
		return m_StoreController.products.all.Where((Product p) => p.definition.id == productID).FirstOrDefault()?.metadata.localizedPriceString;
	}

	public override decimal GetLocalizedPrice(string productID)
	{
		if (m_StoreController == null)
		{
			return 0m;
		}
		return m_StoreController.products.all.Where((Product p) => p.definition.id == productID).FirstOrDefault()?.metadata.localizedPrice ?? 0m;
	}

	public override string GetISOCurrencyCode(string productID)
	{
		if (m_StoreController == null)
		{
			return null;
		}
		return m_StoreController.products.all.Where((Product p) => p.definition.id == productID).FirstOrDefault()?.metadata.isoCurrencyCode;
	}

	public override void RequestProductData()
	{
		StandardPurchasingModule first = StandardPurchasingModule.Instance();
		builder = ConfigurationBuilder.Instance(first);
		foreach (string item in Products.ProductPackages.Values.Select((ProductDetail p) => p.StoreProductID))
		{
			builder.AddProduct(item, ProductType.Consumable);
		}
	}

	public override void ValidatePurchase(ITransaction t)
	{
	}

	public override void PurchaseProduct(ProductDetail productdetail)
	{
		try
		{
			if (!IsInitialized())
			{
				Debug.Log("BuyProductID FAIL. Not initialized.");
				OnPurchaseFailed("Store not initialized.");
				return;
			}
			Product product = m_StoreController.products.WithID(productdetail.StoreProductID);
			if (product == null)
			{
				Debug.Log("BuyProductID: FAIL. product not found");
				OnPurchaseFailed("Product not found.");
			}
			else if (IsProductPending(product))
			{
				Debug.Log("BuyProductID: FAIL. product pending.. Let's try to complete.");
				StartCoroutine(ValidateTransaction(product));
			}
			else if (!product.availableToPurchase)
			{
				Debug.Log("BuyProductID: FAIL. product not available for purchase");
				OnPurchaseFailed("Product not available for purchase.");
			}
			else
			{
				Debug.Log("Purchasing product asychronously: " + product.definition.id + " " + product.availableToPurchase);
				m_StoreController.InitiatePurchase(product);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			Debug.Log("BuyProductID: FAIL. Exception during purchase. " + ex);
			OnPurchaseFailed("Could not initiate purchase transaction. Error: " + ex.Message);
		}
	}

	private bool IsProductPending(Product product)
	{
		if (PendingTransactions.Contains(product.definition.id))
		{
			return product.hasReceipt;
		}
		return false;
	}

	public override void ConsumeProduct(string sku)
	{
		Debug.Log("Consume Product: " + sku);
	}

	public override void RestoreTransactions()
	{
		Debug.Log("RestoreTransactions - Query inventory and fire purchase events for google platform.");
		try
		{
			ValidateTransactions();
			if (OnRestoreFinished != null)
			{
				OnRestoreFinished();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			if (OnRestoreFailed != null)
			{
				OnRestoreFailed();
			}
		}
	}

	public void ValidateTransactions()
	{
		if (transactions.Count > 0)
		{
			int count = transactions.Count;
			for (int i = 0; i < count; i++)
			{
				ValidatePurchase(transactions[i]);
			}
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		throw new NotImplementedException();
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		throw new NotImplementedException();
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		StartCoroutine(ValidateTransaction(e.purchasedProduct));
		PendingTransactions.Add(e.purchasedProduct.definition.id);
		return PurchaseProcessingResult.Pending;
	}

	private IEnumerator ValidateTransaction(Product product)
	{
		string productID = product.definition.id;
		int userID = Session.Account.UserID;
		string strToken = Session.Account.strToken;
		int iD = Session.MyPlayerData.ID;
		string uri = Main.WebServiceURL + "/Game/ValidateReceipt";
		string value = (string?)JObject.Parse(product.receipt)["Payload"];
		WWWForm postForm = new WWWForm();
		postForm.AddField("Platform", "apple");
		postForm.AddField("Receipt", value);
		postForm.AddField("CID", iD);
		postForm.AddField("UID", userID);
		postForm.AddField("Token", strToken);
		using UnityWebRequest www = UnityWebRequest.Post(uri, postForm);
		string errorTitle = "Purchase Failed";
		string friendlyMsg = "Failed to Validate Transaction. Please try again or contact support at " + Main.SupportURL;
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, postForm, customContext);
			OnValidateFailed();
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, postForm, customContext);
			OnValidateFailed();
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, postForm, customContext);
			OnValidateFailed();
			yield break;
		}
		friendlyMsg = "Transaction cound not be validated!";
		customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
		try
		{
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			if (aPIResponse.Message == "Success")
			{
				if (OnValidateSuccess != null)
				{
					OnValidateSuccess(productID);
				}
				if (product.definition.type == ProductType.Consumable)
				{
					if (OnConsumeSuccess != null)
					{
						OnConsumeSuccess(null);
					}
					PendingTransactions.Remove(product.definition.id);
					m_StoreController.ConfirmPendingPurchase(product);
				}
			}
			else
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, aPIResponse.Message, postForm, customContext);
				OnValidateFailed();
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, postForm, customContext, ex);
			OnValidateFailed();
		}
	}

	void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason p)
	{
		Debug.LogError("StratefyUnityIAP.OnPurchaseFailed() => " + p);
		OnPurchaseFailed("Could not complete transaction. Error Code: " + p);
	}
}

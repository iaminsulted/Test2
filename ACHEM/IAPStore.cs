using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public static class IAPStore
{
	private static BillingStrategy Strategy;

	public static bool IsInitialized;

	public static Action OnInitComplete;

	public static Action<ITransaction> OnPurchaseSuccess;

	public static Action<ITransaction> OnConsumeSuccess;

	public static Action<string> OnValidateSuccess;

	public static Action OnValidateFailed;

	public static Action<string> OnPurchaseFailed;

	public static Action<string> OnPurchaseCancelled;

	public static Action<string> OnConsumeFailed;

	public static Action<string> OnRestoreFailed;

	public static Action<string> OnRestoreFinished;

	static IAPStore()
	{
		if (!IsInitialized)
		{
			Strategy = new GameObject("IAP").AddComponent<StrategySteam>();
			AppsFlyer.sendEvent("af_initiated_checkout", new Dictionary<string, string> { 
			{
				"af_customer_user_id",
				Session.Account.UserID.ToString()
			} });
			BillingStrategy strategy = Strategy;
			strategy.OnInitSuccess = (Action)Delegate.Combine(strategy.OnInitSuccess, new Action(OnInitSuccess));
			BillingStrategy strategy2 = Strategy;
			strategy2.OnInitFailed = (Action)Delegate.Combine(strategy2.OnInitFailed, new Action(OnInitFailed));
			BillingStrategy strategy3 = Strategy;
			strategy3.OnRequestProductFailed = (Action<string>)Delegate.Combine(strategy3.OnRequestProductFailed, new Action<string>(ProductsReceivedFailed));
			BillingStrategy strategy4 = Strategy;
			strategy4.OnValidateFailed = (Action)Delegate.Combine(strategy4.OnValidateFailed, new Action(ValidateFailed));
			BillingStrategy strategy5 = Strategy;
			strategy5.OnValidateSuccess = (Action<string>)Delegate.Combine(strategy5.OnValidateSuccess, new Action<string>(ValidateSuccess));
			BillingStrategy strategy6 = Strategy;
			strategy6.OnPurchaseSuccess = (Action<ITransaction>)Delegate.Combine(strategy6.OnPurchaseSuccess, new Action<ITransaction>(PurchaseSuccess));
			BillingStrategy strategy7 = Strategy;
			strategy7.OnPurchaseCancelled = (Action)Delegate.Combine(strategy7.OnPurchaseCancelled, new Action(PurchaseCancelled));
			Strategy.PurchaseFailed += PurchaseFailed;
			BillingStrategy strategy8 = Strategy;
			strategy8.OnConsumeFailed = (Action)Delegate.Combine(strategy8.OnConsumeFailed, new Action(ConsumeFailed));
			BillingStrategy strategy9 = Strategy;
			strategy9.OnConsumeSuccess = (Action<ITransaction>)Delegate.Combine(strategy9.OnConsumeSuccess, new Action<ITransaction>(ConsumeSuccess));
			BillingStrategy strategy10 = Strategy;
			strategy10.OnRestoreFinished = (Action)Delegate.Combine(strategy10.OnRestoreFinished, new Action(RestoreFinished));
			BillingStrategy strategy11 = Strategy;
			strategy11.OnRestoreFailed = (Action)Delegate.Combine(strategy11.OnRestoreFailed, new Action(RestoreFailed));
		}
	}

	public static void Init()
	{
		if (!IsInitialized)
		{
			Strategy.Init();
			IsInitialized = true;
		}
		else
		{
			OnInitSuccess();
		}
	}

	public static string GetLocalizedPriceString(string productID)
	{
		return Strategy.GetLocalizedPriceString(productID);
	}

	public static decimal GetLocalizedPrice(string productID)
	{
		return Strategy.GetLocalizedPrice(productID);
	}

	public static string GetISOCurrencyCode(string productID)
	{
		return Strategy.GetISOCurrencyCode(productID);
	}

	public static void OnInitSuccess()
	{
		Debug.Log("Store Init Success Retrieving product Info");
		IsInitialized = true;
		if (OnInitComplete != null)
		{
			OnInitComplete();
		}
	}

	public static void OnInitFailed()
	{
		Debug.Log("Store Init Failed");
	}

	public static void PurchaseProduct(ProductDetail product)
	{
		Strategy.PurchaseProduct(product);
	}

	public static void ProductsReceivedFailed(string message)
	{
		Debug.Log("Failed to receive products. " + message);
	}

	public static void ValidateFailed()
	{
		if (OnValidateFailed != null)
		{
			OnValidateFailed();
		}
	}

	public static void ValidateSuccess(string productID)
	{
		if (OnValidateSuccess != null)
		{
			OnValidateSuccess(productID);
			try
			{
				decimal localizedPrice = GetLocalizedPrice(productID);
				string iSOCurrencyCode = GetISOCurrencyCode(productID);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("af_currency", iSOCurrencyCode);
				dictionary.Add("af_revenue", localizedPrice.ToString());
				AppsFlyer.sendEvent("af_purchase", dictionary);
			}
			catch (Exception)
			{
			}
		}
	}

	public static void ConsumeProduct(string productID)
	{
		Strategy.ConsumeProduct(productID);
	}

	public static void PurchaseSuccess(ITransaction transaction)
	{
		Debug.Log("Success:  Purchased Product: " + transaction.GetProductID());
		if (OnPurchaseSuccess != null)
		{
			OnPurchaseSuccess(transaction);
		}
	}

	public static void PurchaseCancelled()
	{
		Debug.Log("Purchase Cancelled");
		if (OnPurchaseCancelled != null)
		{
			OnPurchaseCancelled("Purchase was cancelled by user.");
		}
	}

	public static void PurchaseFailed(string msg)
	{
		Debug.Log("Purchase Failed - custom");
		if (OnPurchaseFailed != null)
		{
			OnPurchaseFailed(msg);
		}
	}

	public static void ConsumeFailed()
	{
		Debug.Log("Consume Failed");
		if (OnConsumeFailed != null)
		{
			OnConsumeFailed("Failed to consume purchase.");
		}
	}

	public static void ConsumeSuccess(ITransaction t)
	{
		if (OnConsumeSuccess != null)
		{
			OnConsumeSuccess(t);
		}
	}

	public static void RestoreFinished()
	{
		Debug.Log("Restore Finished");
		if (OnRestoreFinished != null)
		{
			OnRestoreFinished("Finished restoring purchases.");
		}
	}

	public static void RestoreFailed()
	{
		Debug.Log("Restore Failed");
		if (OnRestoreFinished != null)
		{
			OnRestoreFinished("Failed to restore purchases.");
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BillingStrategy : MonoBehaviour
{
	public List<ITransaction> transactions = new List<ITransaction>();

	public Action OnInitSuccess;

	public Action OnInitFailed;

	public Action<string> OnRequestProductFailed;

	public Action OnValidateFailed;

	public Action<string> OnValidateSuccess;

	public Action<ITransaction> OnPurchaseSuccess;

	public Action OnPurchaseCancelled;

	public Action OnConsumeFailed;

	public Action<ITransaction> OnConsumeSuccess;

	public Action OnRestoreFinished;

	public Action OnRestoreFailed;

	public event Action<string> PurchaseFailed;

	public abstract void Init();

	public abstract void RequestProductData();

	public abstract void ValidatePurchase(ITransaction t);

	public abstract void PurchaseProduct(ProductDetail product);

	public abstract void ConsumeProduct(string sku);

	public abstract void RestoreTransactions();

	public void OnPurchaseFailed(string message)
	{
		if (this.PurchaseFailed != null)
		{
			this.PurchaseFailed(message);
		}
	}

	public abstract string GetLocalizedPriceString(string productID);

	public abstract decimal GetLocalizedPrice(string productID);

	public abstract string GetISOCurrencyCode(string productID);
}

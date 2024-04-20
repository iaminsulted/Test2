using Steamworks;

public class StrategySteam : BillingStrategy
{
	public static StrategySteam Instance;

	private void Awake()
	{
		Instance = this;
	}

	public override void ConsumeProduct(string sku)
	{
	}

	public override void Init()
	{
		if (OnInitSuccess != null)
		{
			OnInitSuccess();
		}
	}

	public override void PurchaseProduct(ProductDetail product)
	{
		try
		{
			SteamSystem.StartTransaction(product.SteamProductID, Session.MyPlayerData.UserID, SteamUser.GetSteamID().ToString());
		}
		catch
		{
			SteamStore.NewTxnResponse.response = new InitTxnR
			{
				error = new InitTxnResponseError
				{
					errorcode = -20,
					errordesc = "Code: -20: Steam was not initialized"
				}
			};
			SteamPurchaseFailed(SteamStore.NewTxnResponse.response.error.errordesc);
		}
	}

	public override void RequestProductData()
	{
	}

	public override void RestoreTransactions()
	{
	}

	public override void ValidatePurchase(ITransaction t)
	{
	}

	public void SteamPurchaseFailed(string response)
	{
		OnPurchaseFailed(response);
	}

	public void PurchaseSuccess(ITransaction txn)
	{
		if (OnPurchaseSuccess != null)
		{
			OnPurchaseSuccess(txn);
		}
	}

	public void ConsumeSuccess(ITransaction txn)
	{
		if (OnConsumeSuccess != null)
		{
			OnConsumeSuccess(txn);
		}
	}

	public void ConsumeFailed(string msg)
	{
		if (OnConsumeFailed != null)
		{
			OnConsumeFailed();
		}
	}

	public override string GetLocalizedPriceString(string productID)
	{
		return null;
	}

	public override decimal GetLocalizedPrice(string productID)
	{
		return 0m;
	}

	public override string GetISOCurrencyCode(string productID)
	{
		return null;
	}
}

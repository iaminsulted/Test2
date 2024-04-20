public class SteamTxn : InitTxnResponse, ITransaction
{
	public SteamTxn(InitTxnResponse rsp)
	{
		AddResponse(rsp.response);
	}

	public string GetProductID()
	{
		return response.result;
	}

	public string GetReceipt()
	{
		return response.@params.orderid.ToString();
	}

	public string GetTransactionID()
	{
		return response.@params.transid.ToString();
	}
}

public class ResponseMergeShopLoad : Response
{
	public MergeShop mergeShop;

	public string Message;

	public ResponseMergeShopLoad()
	{
		type = 28;
		cmd = 2;
	}

	public ResponseMergeShopLoad(MergeShop shop, string message)
	{
		type = 28;
		cmd = 2;
		mergeShop = shop;
		Message = message;
	}
}

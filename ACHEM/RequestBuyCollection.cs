public class RequestBuyCollection : Request
{
	public int ShopID;

	public RequestBuyCollection(int shopID)
	{
		type = 11;
		cmd = 4;
		ShopID = shopID;
	}
}

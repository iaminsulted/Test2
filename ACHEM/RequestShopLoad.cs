public class RequestShopLoad : Request
{
	public int ShopID;

	public ShopType ShopType;

	public RequestShopLoad()
	{
		type = 11;
		cmd = 3;
	}

	public RequestShopLoad(int shopID, ShopType shopType)
	{
		type = 11;
		cmd = 3;
		ShopID = shopID;
		ShopType = shopType;
	}
}

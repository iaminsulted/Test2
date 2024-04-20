public class RequestTradeBuy : Request
{
	public int ShopID;

	public ShopType ShopType;

	public int ItemID;

	public int Qty = 1;

	public RequestTradeBuy()
	{
		type = 11;
		cmd = 1;
	}
}

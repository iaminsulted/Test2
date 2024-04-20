public class RequestTradeSell : Request
{
	public int CharItemID;

	public int Qty = 1;

	public RequestTradeSell()
	{
		type = 11;
		cmd = 2;
	}
}

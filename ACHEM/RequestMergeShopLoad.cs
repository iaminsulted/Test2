public class RequestMergeShopLoad : Request
{
	public int MergeShopID;

	public RequestMergeShopLoad()
	{
		type = 28;
		cmd = 2;
	}
}

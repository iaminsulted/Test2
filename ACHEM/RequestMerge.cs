public class RequestMerge : Request
{
	public int ShopID;

	public int MergeID;

	public RequestMerge()
	{
		type = 28;
		cmd = 1;
	}
}

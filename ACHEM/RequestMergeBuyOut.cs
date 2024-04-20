public class RequestMergeBuyOut : Request
{
	public int ShopID;

	public int MergeID;

	public RequestMergeBuyOut()
	{
		type = 28;
		cmd = 4;
	}
}

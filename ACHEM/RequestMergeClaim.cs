public class RequestMergeClaim : Request
{
	public int MergeID;

	public RequestMergeClaim()
	{
		type = 28;
		cmd = 3;
	}
}

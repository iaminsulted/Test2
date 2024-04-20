public class RequestMergeSpeedup : Request
{
	public int MergeID;

	public RequestMergeSpeedup()
	{
		type = 28;
		cmd = 5;
	}
}

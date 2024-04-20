public class RequestSummon : Request
{
	public int CharID;

	public int TargetCharID;

	public string From;

	public RequestSummon()
	{
		type = 29;
		cmd = 5;
	}
}

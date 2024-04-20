public class RequestPartyJoin : Request
{
	public int LeaderID;

	public bool Accept = true;

	public RequestPartyJoin()
	{
		type = 31;
		cmd = 2;
	}
}

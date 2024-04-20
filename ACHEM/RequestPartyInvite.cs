public class RequestPartyInvite : Request
{
	public int FromID;

	public string To;

	public RequestPartyInvite()
	{
		type = 31;
		cmd = 1;
	}
}

public class RequestPartyPromote : Request
{
	public int CharID;

	public RequestPartyPromote()
	{
		type = 31;
		cmd = 4;
	}
}

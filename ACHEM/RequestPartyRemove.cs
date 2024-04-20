public class RequestPartyRemove : Request
{
	public int CharID;

	public RequestPartyRemove()
	{
		type = 31;
		cmd = 3;
	}
}

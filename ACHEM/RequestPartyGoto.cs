public class RequestPartyGoto : Request
{
	public int CharID;

	public RequestPartyGoto()
	{
		type = 31;
		cmd = 7;
	}

	public RequestPartyGoto(int charID)
	{
		type = 31;
		cmd = 7;
		CharID = charID;
	}
}

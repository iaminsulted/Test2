public class ResponsePartyInvite : Response, IMessage
{
	public int LeaderID;

	public string Message;

	public string LeaderName;

	public ResponsePartyInvite()
	{
		type = 31;
		cmd = 1;
	}

	public int GetID()
	{
		return LeaderID;
	}

	public string GetMessage()
	{
		return Message;
	}

	public string GetName()
	{
		return LeaderName;
	}
}

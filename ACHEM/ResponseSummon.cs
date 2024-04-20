public class ResponseSummon : Response, IMessage
{
	public int CharID;

	public int FriendID;

	public string From;

	public ResponseSummon()
	{
		type = 29;
		cmd = 5;
	}

	public int GetID()
	{
		return CharID;
	}

	public string GetName()
	{
		return From;
	}

	public string GetMessage()
	{
		return "invited you to play.";
	}
}

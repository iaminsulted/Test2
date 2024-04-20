public class ResponseFriend : Response, IMessage
{
	public int CharID;

	public int FriendID;

	public string From;

	public ResponseFriend()
	{
		type = 29;
		cmd = 1;
	}

	public int GetID()
	{
		return FriendID;
	}

	public string GetName()
	{
		return From;
	}

	public string GetMessage()
	{
		return "Add friend?";
	}
}

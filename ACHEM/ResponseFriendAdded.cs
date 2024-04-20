public class ResponseFriendAdded : Response, IMessage
{
	public string Name;

	public ResponseFriendAdded()
	{
		type = 29;
		cmd = 1;
	}

	public int GetID()
	{
		return -1;
	}

	public string GetName()
	{
		return Name;
	}

	public string GetMessage()
	{
		return "You are now friends with " + Name;
	}
}

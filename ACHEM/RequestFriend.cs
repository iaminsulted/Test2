public class RequestFriend : Request
{
	public int CharID;

	public int FriendID;

	public string Name = "";

	public RequestFriend()
	{
		type = 29;
		cmd = 1;
	}
}

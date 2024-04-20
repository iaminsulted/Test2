public class RequestFriendDelete : Request
{
	public int CharID;

	public int FriendID;

	public RequestFriendDelete()
	{
		type = 29;
		cmd = 3;
	}
}

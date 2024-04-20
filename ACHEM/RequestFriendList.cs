public class RequestFriendList : Request
{
	public int CharID;

	public RequestFriendList()
	{
		type = 29;
		cmd = 4;
	}
}

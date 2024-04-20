public class RequestFriendAdd : Request
{
	public int FriendID;

	public bool Confirm;

	public RequestFriendAdd()
	{
		type = 29;
		cmd = 2;
	}
}

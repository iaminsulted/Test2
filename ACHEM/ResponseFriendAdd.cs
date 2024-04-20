public class ResponseFriendAdd : Response
{
	public int CharID;

	public int FriendID;

	public FriendData friend;

	public ResponseFriendAdd()
	{
		type = 29;
		cmd = 2;
	}

	public ResponseFriendAdd(FriendData f)
	{
		type = 29;
		cmd = 2;
		friend = f;
	}
}

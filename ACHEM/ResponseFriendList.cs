using System.Collections.Generic;

public class ResponseFriendList : Response
{
	public List<FriendData> Friends;

	public ResponseFriendList()
	{
		type = 29;
		cmd = 4;
	}

	public ResponseFriendList(List<FriendData> list)
	{
		type = 29;
		cmd = 4;
		Friends = list;
	}
}

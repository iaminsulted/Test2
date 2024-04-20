using System.Collections.Generic;

public class RequestFriendSyncIgnore : Request
{
	public int CharID;

	public List<string> ignoreList;

	public bool isWhisperEnabled;

	public bool isFriendRequestEnabled;

	public bool isPartyInviteEnabled;

	public bool isGuildInviteEnabled;

	public bool isGotoEnabled;

	public RequestFriendSyncIgnore()
	{
		type = 24;
	}
}

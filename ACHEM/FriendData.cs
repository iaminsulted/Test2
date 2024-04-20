public class FriendData
{
	public string strName;

	public int FriendID;

	public int ServerID;

	public string ServerName;

	public int MapID;

	public string MapName;

	public string MapDisplayName;

	public string SummonLink;

	public int Level;

	public bool IsOnline => ServerID > 0;

	public bool OnSameServer
	{
		get
		{
			if (ServerID != Game.Instance.aec.ID)
			{
				return ServerID == 999999;
			}
			return true;
		}
	}

	public FriendData()
	{
	}

	public FriendData(string name, int friendID, int serverID, int mapID, string mapName, string mapDisplayName, string summonLink, int level)
	{
		strName = name;
		FriendID = friendID;
		ServerID = serverID;
		MapID = mapID;
		MapName = mapName;
		MapDisplayName = mapDisplayName;
		SummonLink = summonLink;
		Level = level;
	}
}

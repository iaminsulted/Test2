using System;

public class GuildMember
{
	public int ID;

	public string Name;

	public GuildRole GuildRole;

	public int ServerID;

	public string ServerName;

	public int MapID;

	public string MapName;

	public string MapDisplayName;

	public string SummonLink;

	public int Level;

	public DateTime LastOnlineDate;

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

	public GuildMember(int id, string name, GuildRole guildRole, int serverID, string serverName, int mapID, string mapName, string mapDisplayName, string summonLink, int level, DateTime lastOnlineDate = default(DateTime))
	{
		ID = id;
		Name = name;
		GuildRole = guildRole;
		ServerID = serverID;
		ServerName = serverName;
		MapID = mapID;
		MapName = mapName;
		MapDisplayName = mapDisplayName;
		SummonLink = summonLink;
		Level = level;
		LastOnlineDate = lastOnlineDate;
	}
}

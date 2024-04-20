using System.Collections.Generic;
using StatCurves;

public class Guild
{
	public const int Min_Level_To_Create = 10;

	public const int Guild_Power_Shop_ID = 569;

	public int guildID;

	public string name;

	public string MOTD;

	public string tag;

	public Dictionary<int, GuildMember> guildMembers;

	public long TotalXP;

	public long XpToNextLevel;

	public long XpForCurrentLevel;

	public long MonthlyXP;

	public int TaxRate;

	public long Gold;

	public int memberLimit;

	public int currentOverallLeaderboardRank = int.MaxValue;

	public int currentMonthlyLeaderboardRank = int.MaxValue;

	public static List<GuildLeaderboardEntry> GuildLeaderboardEntries = new List<GuildLeaderboardEntry>();

	public int Level
	{
		get
		{
			return Levels.GetGuildLevelByXp(TotalXP, Session.MyPlayerData.LevelCap, guildMembers.Count);
		}
		private set
		{
		}
	}

	public bool MemberLimitHasNotReached => guildMembers.Count < memberLimit;

	public GuildRole MyGuildRole => guildMembers[Session.MyPlayerData.ID].GuildRole;

	public void UpdateMemberRole(int memberID, GuildRole newRole)
	{
		guildMembers[memberID].GuildRole = newRole;
	}

	public bool HasAuthority(int memberID, bool rankChange = false)
	{
		if (!rankChange)
		{
			if (MyGuildRole <= guildMembers[memberID].GuildRole || MyGuildRole == GuildRole.Member)
			{
				return MyGuildRole == GuildRole.Leader;
			}
			return true;
		}
		return MyGuildRole == GuildRole.Leader;
	}

	public GuildMember GetGuildLeader()
	{
		foreach (GuildMember value in guildMembers.Values)
		{
			if (value.GuildRole == GuildRole.Leader)
			{
				return value;
			}
		}
		return null;
	}

	public GuildMember Me()
	{
		return guildMembers[Session.MyPlayerData.ID];
	}

	public void RemoveGuildMember(int memberID)
	{
		guildMembers.Remove(memberID);
	}

	public override string ToString()
	{
		if (this == null)
		{
			return null;
		}
		return "Guild ID: " + guildID + " Name: " + name;
	}
}

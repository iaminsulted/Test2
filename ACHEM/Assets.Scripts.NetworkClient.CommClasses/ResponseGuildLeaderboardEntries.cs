using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseGuildLeaderboardEntries : Response
{
	public List<GuildLeaderboardEntry> LeaderboardEntries;
}

using System.Collections.Generic;

public class Leaderboard
{
	public enum LeadboardType
	{
		NULL,
		Giftthulu,
		Cr1tikalSpeedrun,
		SeledenMonsterHunter,
		MinuteMan,
		RingOfDoom
	}

	public LeadboardType type;

	public List<LeaderboardData> data = new List<LeaderboardData>();
}

using System;
using System.Collections.Generic;

public static class Scoreboard
{
	private static Dictionary<int, int> teamScores;

	private static Dictionary<int, int> teamNPCs;

	public static bool IsActive => Target > 0;

	public static int Target { get; private set; }

	public static event Action ScoreUpdated;

	static Scoreboard()
	{
		teamScores = new Dictionary<int, int>();
		teamNPCs = new Dictionary<int, int>();
	}

	public static void Start(int target, Dictionary<int, int> teamScores)
	{
		Target = target;
		Scoreboard.teamScores = teamScores;
	}

	public static void UpdateTeamNPC(int spawnID, int teamID)
	{
		if (!teamNPCs.ContainsKey(teamID))
		{
			teamNPCs.Add(teamID, 0);
		}
		teamNPCs[teamID] = spawnID;
	}

	public static void UpdateTeamScore(Dictionary<int, int> teamScores)
	{
		Scoreboard.teamScores = teamScores;
		Scoreboard.ScoreUpdated?.Invoke();
	}

	public static int GetTeamScore(int teamID)
	{
		if (teamScores.ContainsKey(teamID))
		{
			return teamScores[teamID];
		}
		return 0;
	}

	public static int GetTeamNPC(int teamID)
	{
		if (teamNPCs.ContainsKey(teamID))
		{
			return teamNPCs[teamID];
		}
		return 0;
	}
}

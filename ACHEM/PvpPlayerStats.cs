public class PvpPlayerStats
{
	public int playerID;

	public int kills;

	public int deaths;

	public int assists;

	public float damageDealt;

	public float healingDone;

	public int objectivesTaken;

	public PvpPlayerStats()
	{
	}

	public PvpPlayerStats(int playerID, int kills, int deaths, int assists, float damageDealt, float healingDone, int objectivesTaken)
	{
		this.playerID = playerID;
		this.kills = kills;
		this.deaths = deaths;
		this.assists = assists;
		this.damageDealt = damageDealt;
		this.healingDone = healingDone;
		this.objectivesTaken = objectivesTaken;
	}
}

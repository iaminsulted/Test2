public class PvpPlayerRecords
{
	public int WinsTotal;

	public int LossesTotal;

	public int KillsTotal;

	public int DeathsTotal;

	public int AssistsTotal;

	public int ObjectivesTakenTotal;

	public int ObjectivesDefendedTotal;

	public PvpPlayerRecords()
	{
		WinsTotal = 0;
		LossesTotal = 0;
		KillsTotal = 0;
		DeathsTotal = 0;
		AssistsTotal = 0;
		ObjectivesTakenTotal = 0;
		ObjectivesDefendedTotal = 0;
	}

	public PvpPlayerRecords(int winsTotal, int lossesTotal, int killsTotal, int deathsTotal, int assistsTotal, int objectivesTakenTotal, int objectivesDefendedTotal)
	{
		WinsTotal = winsTotal;
		LossesTotal = lossesTotal;
		KillsTotal = killsTotal;
		DeathsTotal = deathsTotal;
		AssistsTotal = assistsTotal;
		ObjectivesTakenTotal = objectivesTakenTotal;
		ObjectivesDefendedTotal = objectivesDefendedTotal;
	}
}

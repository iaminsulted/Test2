using System.Collections.Generic;

public class PvpMatchInfo
{
	public string areaName;

	public string mapImage;

	public int matchSeconds;

	public float timeStampServer;

	public List<PvpPlayerStats> yourTeam;

	public List<PvpPlayerStats> enemyTeam;

	public int yourScore;

	public int enemyScore;

	public bool isWinner;
}

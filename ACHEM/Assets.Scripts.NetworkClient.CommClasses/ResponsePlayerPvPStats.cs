namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponsePlayerPvPStats : Response
{
	public PvpPlayerStats pvpStats;

	public ResponsePlayerPvPStats()
	{
		type = 36;
		cmd = 7;
	}

	public ResponsePlayerPvPStats(PvpPlayerStats pvpStats)
	{
		type = 36;
		cmd = 7;
		this.pvpStats = pvpStats;
	}
}

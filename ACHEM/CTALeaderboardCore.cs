public class CTALeaderboardCore : ClientTriggerActionCore
{
	public int leaderboardType;

	protected override void OnExecute()
	{
		UILeaderboard.Load(leaderboardType);
	}
}

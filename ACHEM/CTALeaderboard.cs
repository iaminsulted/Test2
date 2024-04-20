public class CTALeaderboard : ClientTriggerAction
{
	public int leaderboardType;

	protected override void OnExecute()
	{
		UILeaderboard.Load(leaderboardType);
	}
}

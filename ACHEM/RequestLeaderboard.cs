internal class RequestLeaderboard : Request
{
	public int leaderboardType;

	public RequestLeaderboard()
	{
		type = 43;
		cmd = 1;
	}

	public RequestLeaderboard(int _LeaderboardType)
	{
		leaderboardType = _LeaderboardType;
		type = 43;
		cmd = 1;
	}
}

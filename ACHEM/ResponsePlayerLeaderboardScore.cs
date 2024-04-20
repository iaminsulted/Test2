public class ResponsePlayerLeaderboardScore : Response
{
	public int leaderboardPos;

	public int score;

	public ResponsePlayerLeaderboardScore()
	{
		type = 43;
		cmd = 2;
	}
}

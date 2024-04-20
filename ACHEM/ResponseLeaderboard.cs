internal class ResponseLeaderboard : Response
{
	public Leaderboard leaderboard;

	public int myScore;

	public int myPos;

	public ResponseLeaderboard()
	{
		type = 43;
		cmd = 1;
	}

	public ResponseLeaderboard(Leaderboard board, int score, int pos)
	{
		type = 43;
		cmd = 1;
		leaderboard = board;
	}
}

public class ResponseVoteKickStart : Response
{
	public int playerID;

	public ResponseVoteKickStart()
	{
		type = 31;
		cmd = 8;
	}

	public ResponseVoteKickStart(int playerID)
	{
		type = 31;
		cmd = 8;
		this.playerID = playerID;
	}
}

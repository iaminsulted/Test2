public class ResponsePvPDuelChallenge : Response
{
	public int ChallengerId;

	public ResponsePvPDuelChallenge(int challengerId)
	{
		type = 20;
		cmd = 2;
		ChallengerId = challengerId;
	}
}

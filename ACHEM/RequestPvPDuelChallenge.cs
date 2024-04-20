public class RequestPvPDuelChallenge : Request
{
	public string ChallengeeName;

	public RequestPvPDuelChallenge(string challengeeName)
	{
		type = 20;
		cmd = 2;
		ChallengeeName = challengeeName;
	}
}

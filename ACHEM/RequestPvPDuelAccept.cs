public class RequestPvPDuelAccept : Request
{
	public int ChallengerId;

	public bool Accept;

	public RequestPvPDuelAccept(int challengerId, bool accept)
	{
		type = 20;
		cmd = 3;
		ChallengerId = challengerId;
		Accept = accept;
	}
}

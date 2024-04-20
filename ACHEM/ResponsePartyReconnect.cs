public class ResponsePartyReconnect : Response
{
	public int playerID;

	public ResponsePartyReconnect(int playerID)
	{
		type = 31;
		cmd = 13;
		this.playerID = playerID;
	}
}

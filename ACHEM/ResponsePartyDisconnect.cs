public class ResponsePartyDisconnect : Response
{
	public int playerID;

	public ResponsePartyDisconnect(int playerID)
	{
		type = 31;
		cmd = 12;
		this.playerID = playerID;
	}
}

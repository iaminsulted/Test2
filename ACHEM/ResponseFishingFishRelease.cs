public class ResponseFishingFishRelease : Response
{
	public int playerID;

	public int machineID;

	public ResponseFishingFishRelease()
	{
		type = 39;
		cmd = 5;
	}

	public ResponseFishingFishRelease(int playerID, int machineID)
	{
		type = 39;
		cmd = 5;
		this.playerID = playerID;
		this.machineID = machineID;
	}
}

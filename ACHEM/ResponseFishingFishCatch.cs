public class ResponseFishingFishCatch : Response
{
	public int playerID;

	public int machineID;

	public ResponseFishingFishCatch()
	{
		type = 39;
		cmd = 3;
	}

	public ResponseFishingFishCatch(int playerID, int machineID)
	{
		type = 39;
		cmd = 3;
		this.playerID = playerID;
		this.machineID = machineID;
	}
}

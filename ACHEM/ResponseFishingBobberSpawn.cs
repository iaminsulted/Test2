public class ResponseFishingBobberSpawn : Response
{
	public int playerID;

	public int machineID;

	public ResponseFishingBobberSpawn()
	{
		type = 39;
		cmd = 2;
	}

	public ResponseFishingBobberSpawn(int playerID, int machineID)
	{
		type = 39;
		cmd = 2;
		this.playerID = playerID;
		this.machineID = machineID;
	}
}

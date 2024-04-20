public class ResponseFishingBobberDespawn : Response
{
	public int playerID;

	public int machineID;

	public ResponseFishingBobberDespawn()
	{
		type = 39;
		cmd = 1;
	}

	public ResponseFishingBobberDespawn(int playerID, int machineID)
	{
		type = 39;
		cmd = 1;
		this.playerID = playerID;
		this.machineID = machineID;
	}
}

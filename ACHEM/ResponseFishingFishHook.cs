using StatCurves;

public class ResponseFishingFishHook : Response
{
	public int playerID;

	public int machineID;

	public RarityType rarity;

	public ResponseFishingFishHook()
	{
		type = 39;
		cmd = 4;
	}

	public ResponseFishingFishHook(int playerID, int machineID, RarityType rarity)
	{
		type = 39;
		cmd = 4;
		this.playerID = playerID;
		this.machineID = machineID;
		this.rarity = rarity;
	}
}

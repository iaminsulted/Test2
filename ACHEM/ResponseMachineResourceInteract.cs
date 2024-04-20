using StatCurves;

public class ResponseMachineResourceInteract : Response
{
	public int MachineID;

	public int PrimaryItemID;

	public int RareItemID;

	public RarityType Rarity;

	public ResponseMachineResourceInteract(int machineID, int primaryItemID, int rareItemID, RarityType rarity)
	{
		type = 19;
		cmd = 21;
		MachineID = machineID;
		PrimaryItemID = primaryItemID;
		RareItemID = rareItemID;
		Rarity = rarity;
	}
}

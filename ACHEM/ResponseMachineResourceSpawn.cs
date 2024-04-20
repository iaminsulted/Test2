using System.Collections.Generic;
using StatCurves;

public class ResponseMachineResourceSpawn : Response
{
	public int machineID;

	public int totalUsages;

	public int usages;

	public int nodeID;

	public RarityType rarity;

	public int tradeSkillLevel;

	public string requirements;

	public List<int> dropIDs;

	public ResponseMachineResourceSpawn()
	{
		type = 19;
		cmd = 23;
	}

	public ResponseMachineResourceSpawn(int machineID, int totalUsages, int usages, int nodeID, RarityType rarity, int tradeSkillLevel, string requirements, List<int> dropIDs)
	{
		type = 19;
		cmd = 23;
		this.machineID = machineID;
		this.totalUsages = totalUsages;
		this.usages = usages;
		this.nodeID = nodeID;
		this.rarity = rarity;
		this.tradeSkillLevel = tradeSkillLevel;
		this.requirements = requirements;
		this.dropIDs = dropIDs;
	}
}

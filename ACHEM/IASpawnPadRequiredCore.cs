using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Spawn Pad Required")]
public class IASpawnPadRequiredCore : IARequiredCore
{
	public int SpawnPadID;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		NPC nPC = Entities.Instance?.me?.interactingNPC;
		if (nPC != null && nPC.SpawnID == SpawnPadID)
		{
			return true;
		}
		return false;
	}
}

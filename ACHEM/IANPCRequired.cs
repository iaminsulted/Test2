using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Map Required")]
public class IANPCRequired : InteractionRequirement
{
	public int NpcID;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		NPC nPC = Entities.Instance?.me?.interactingNPC;
		if (nPC != null && nPC.NPCID == NpcID)
		{
			return true;
		}
		return false;
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC Bait Required")]
public class IANPCBaitRequired : InteractionRequirement
{
	public NPCSpawn spawn;

	public bool baitState;

	public void Awake()
	{
		spawn.BaitUpdated += base.OnRequirementUpdate;
	}

	public void OnDestroy()
	{
		spawn.BaitUpdated -= base.OnRequirementUpdate;
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(spawn.ID);
		if (npcBySpawnId != null)
		{
			return npcBySpawnId.BaitState == baitState;
		}
		return false;
	}
}

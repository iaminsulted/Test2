using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC Reaction Required")]
public class IANPCReactionRequired : InteractionRequirement
{
	public NPCSpawn spawn;

	public int NPCSpawnID;

	public Entity.Reaction reaction;

	public void Awake()
	{
		if (spawn != null)
		{
			spawn.StateUpdated += Machine_StateUpdated;
		}
	}

	public void OnDestroy()
	{
		if (spawn != null)
		{
			spawn.StateUpdated -= Machine_StateUpdated;
		}
	}

	private void Machine_StateUpdated(byte obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (spawn == null)
		{
			return false;
		}
		NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(spawn.ID);
		if (npcBySpawnId != null)
		{
			return npcBySpawnId.react == reaction;
		}
		return false;
	}
}

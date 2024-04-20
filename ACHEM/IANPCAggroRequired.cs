using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC Aggro Required")]
public class IANPCAggroRequired : InteractionRequirement
{
	public NPCSpawn Spawn;

	public int NPCSpawnID;

	public bool HasAggro;

	public void Awake()
	{
		if (Spawn != null)
		{
			Spawn.StateUpdated += Machine_StateUpdated;
		}
	}

	public void OnDestroy()
	{
		if (Spawn != null)
		{
			Spawn.StateUpdated -= Machine_StateUpdated;
		}
	}

	private void Machine_StateUpdated(byte obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Spawn == null)
		{
			return false;
		}
		NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(Spawn.ID);
		if (npcBySpawnId != null)
		{
			return npcBySpawnId.HasAggro() == HasAggro;
		}
		return false;
	}
}

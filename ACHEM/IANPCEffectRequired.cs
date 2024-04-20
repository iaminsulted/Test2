using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC Effect Required")]
public class IANPCEffectRequired : InteractionRequirement
{
	public NPCSpawn spawn;

	public int NPCSpawnID;

	public int effectID;

	public bool hasEffect;

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
			foreach (Effect effect in npcBySpawnId.effects)
			{
				if (effect.ID == effectID)
				{
					return hasEffect;
				}
			}
			return !hasEffect;
		}
		return false;
	}
}

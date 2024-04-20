using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC Health Percent Required")]
public class IANPCHealthPercentRequired : InteractionRequirement
{
	public NPCSpawn spawn;

	public int NPCSpawnID;

	public ComparisonType ComparisonType;

	public float percent;

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
			switch (ComparisonType)
			{
			case ComparisonType.GreaterThanOrEqual:
				return npcBySpawnId.HealthPercent >= percent;
			case ComparisonType.GreaterThan:
				return npcBySpawnId.HealthPercent > percent;
			case ComparisonType.Equal:
				return Mathf.Approximately(npcBySpawnId.HealthPercent, percent);
			case ComparisonType.LessThan:
				return npcBySpawnId.HealthPercent < percent;
			case ComparisonType.LessThanOrEqual:
				return npcBySpawnId.HealthPercent <= percent;
			case ComparisonType.NotEqual:
				return !Mathf.Approximately(npcBySpawnId.HealthPercent, percent);
			}
		}
		return false;
	}
}

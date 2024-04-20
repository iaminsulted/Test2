using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA All Monsters Cleared")]
public class IAAllMonstersCleared : InteractionRequirement
{
	public void Awake()
	{
		NPC.AnyNPCStateUpdated += base.OnRequirementUpdate;
	}

	public void OnDestroy()
	{
		NPC.AnyNPCStateUpdated -= base.OnRequirementUpdate;
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		foreach (NPC npc in Entities.Instance.NpcList)
		{
			if ((npc.react == Entity.Reaction.Hostile || npc.react == Entity.Reaction.Neutral) && npc.serverState != Entity.State.Dead)
			{
				return false;
			}
		}
		return true;
	}
}

using UnityEngine;

[AddComponentMenu("Interaction/Requirements/IA Player Team ID Required")]
public class IAPlayerTeamIDRequired : InteractionRequirement
{
	public int teamID;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Entities.Instance.me.teamID == teamID;
	}
}

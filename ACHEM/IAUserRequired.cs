using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA User Required")]
public class IAUserRequired : InteractionRequirement
{
	public int UserID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = UserID == myPlayerData.UserID;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}
}

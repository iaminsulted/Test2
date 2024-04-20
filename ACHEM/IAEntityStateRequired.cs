using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Entity State Required")]
public class IAEntityStateRequired : InteractionRequirement
{
	public Entity.State EntityState;

	public bool not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Entities.Instance.me.serverState == EntityState;
		if (not)
		{
			flag = !flag;
		}
		return flag;
	}
}

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Or")]
public class IAOr : InteractionRequirement
{
	public List<InteractionRequirement> Requirements = new List<InteractionRequirement>();

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = isRequirementMet(myPlayerData);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public bool isRequirementMet(MyPlayerData myPlayerData)
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			if (requirement.IsRequirementMet(myPlayerData))
			{
				return true;
			}
		}
		return false;
	}

	public void OnEnable()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated += OnChildRequirementUpdated;
		}
	}

	private void OnChildRequirementUpdated()
	{
		OnRequirementUpdate();
	}

	public void OnDisable()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated -= OnChildRequirementUpdated;
		}
	}
}

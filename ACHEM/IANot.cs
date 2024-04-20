using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Not")]
public class IANot : InteractionRequirement
{
	public List<InteractionRequirement> Requirements = new List<InteractionRequirement>();

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			if (requirement.IsRequirementMet(myPlayerData))
			{
				return false;
			}
		}
		return true;
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

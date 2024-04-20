using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Effect Required")]
public class IAEffectRequired : InteractionRequirement
{
	public List<int> EffectIDs = new List<int>();

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Entities.Instance.me.effects.Where((Effect x) => EffectIDs.Contains(x.template.ID)).FirstOrDefault() != null;
		if (Not)
		{
			flag = !flag;
		}
		return flag;
	}

	private void OnEnable()
	{
		Entities.Instance.me.EffectAdded += OnEffectAddedOrRemoved;
		Entities.Instance.me.EffectRemoved += OnEffectAddedOrRemoved;
	}

	private void OnEffectAddedOrRemoved(Effect effect)
	{
		if (EffectIDs.Contains(effect.template.ID))
		{
			OnRequirementUpdate();
		}
	}

	private void OnDisable()
	{
		Entities.Instance.me.EffectAdded -= OnEffectAddedOrRemoved;
		Entities.Instance.me.EffectRemoved -= OnEffectAddedOrRemoved;
	}
}

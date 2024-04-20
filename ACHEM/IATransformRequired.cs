using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Transform Required")]
public class IATransformRequired : InteractionRequirement
{
	public List<int> NPCIDs = new List<int>();

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Entities.Instance.me.overrideAsset != null && NPCIDs.Contains(Entities.Instance.me.overrideAsset.NPCID);
		if (Not)
		{
			flag = !flag;
		}
		return flag;
	}

	private void OnEnable()
	{
		Entities.Instance.me.OverrideAssetUpdated += OnOverrideAssetUpdated;
	}

	private void OnOverrideAssetUpdated(EntityAsset effect)
	{
		OnRequirementUpdate();
	}

	private void OnDisable()
	{
		Entities.Instance.me.OverrideAssetUpdated -= OnOverrideAssetUpdated;
	}
}

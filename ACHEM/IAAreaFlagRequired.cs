using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Area Flag Required")]
public class IAAreaFlagRequired : InteractionRequirement
{
	public string key;

	public string value;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		foreach (KeyValuePair<string, string> areaFlag in Game.Instance.AreaData.areaFlags)
		{
			if (areaFlag.Key == key)
			{
				if (areaFlag.Value == value)
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	public void OnEnable()
	{
		Game.Instance.AreaFlagUpdated += OnAreaFlagUpdated;
	}

	public void OnDisable()
	{
		Game.Instance.AreaFlagUpdated -= OnAreaFlagUpdated;
	}

	private void OnAreaFlagUpdated(string key, string value)
	{
		if (this.key == key && this.value == value)
		{
			OnRequirementUpdate();
		}
	}
}

using System;
using UnityEngine;

public class InteractionRequirement : MonoBehaviour
{
	public event Action Updated;

	protected void OnRequirementUpdate()
	{
		this.Updated?.Invoke();
	}

	public virtual bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return true;
	}
}

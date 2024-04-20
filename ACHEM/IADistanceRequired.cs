using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Distance Required")]
public class IADistanceRequired : InteractionRequirement
{
	public float Distance = 7f;

	private bool withinDistance;

	private bool setWithinDistance
	{
		set
		{
			if (withinDistance != value)
			{
				withinDistance = value;
				OnRequirementUpdate();
			}
		}
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return withinDistance;
	}

	public void Update()
	{
		float num = Vector3.Distance(base.transform.position, Entities.Instance.me.wrapper.transform.position);
		setWithinDistance = num < Distance;
	}
}

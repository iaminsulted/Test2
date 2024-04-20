using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Class Equipped Required")]
public class IAClassEquippedRequired : InteractionRequirement
{
	public int ClassID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.CurrentClass.ClassID == ClassID;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	private void OnEnable()
	{
		Session.MyPlayerData.ClassEquipped += OnClassEquipped;
	}

	private void OnClassEquipped(int id)
	{
		OnRequirementUpdate();
	}

	private void OnDisable()
	{
		Session.MyPlayerData.ClassEquipped -= OnClassEquipped;
	}
}

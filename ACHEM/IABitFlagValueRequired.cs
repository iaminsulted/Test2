using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Bit Flag Value Required")]
public class IABitFlagValueRequired : InteractionRequirement
{
	public string BitFlagName;

	public byte BitFlagIndex;

	public bool Value;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex) == Value;
	}

	public void OnEnable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.BitFlagUpdated += OnBitFlagUpdated;
		}
	}

	private void OnBitFlagUpdated(string name, byte index, bool value)
	{
		if (name == BitFlagName && index == BitFlagIndex)
		{
			OnRequirementUpdate();
		}
	}

	public void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
		}
	}
}

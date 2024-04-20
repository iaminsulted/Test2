using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Bit Flag Required")]
public class IABitFlagRequired : InteractionRequirement
{
	public string BitFlagName;

	public byte BitFlagIndex;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex);
	}

	public void OnEnable()
	{
		Session.MyPlayerData.BitFlagUpdated += OnBitFlagUpdated;
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
		Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
	}
}

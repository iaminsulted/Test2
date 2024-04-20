public class IABitFlagRequiredCore : IARequiredCore
{
	public string BitFlagName;

	public byte BitFlagIndex;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex);
	}

	public IABitFlagRequiredCore()
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

	~IABitFlagRequiredCore()
	{
		Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
	}
}

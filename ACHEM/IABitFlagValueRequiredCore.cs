public class IABitFlagValueRequiredCore : IARequiredCore
{
	public string BitFlagName;

	public byte BitFlagIndex;

	public bool Value;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex) == Value;
	}

	public IABitFlagValueRequiredCore()
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

	~IABitFlagValueRequiredCore()
	{
		Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
	}
}

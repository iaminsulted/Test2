public class IASessionDataRequiredCore : IARequiredCore
{
	public string Key;

	public bool Value;

	public IASessionDataRequiredCore()
	{
		Session.OnSessionDataUpdated += OnSessionDataUpdated;
	}

	~IASessionDataRequiredCore()
	{
		Session.OnSessionDataUpdated -= OnSessionDataUpdated;
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.Has(Key) == Value;
	}

	private void OnSessionDataUpdated(string key)
	{
		if (Key == key)
		{
			OnRequirementUpdate();
		}
	}
}

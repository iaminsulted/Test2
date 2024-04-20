public class IASessionDataRequired : InteractionRequirement
{
	public string Key;

	public bool Value;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Session.Has(Key) == Value;
	}

	public void OnEnable()
	{
		Session.OnSessionDataUpdated += OnSessionDataUpdated;
	}

	private void OnSessionDataUpdated(string key)
	{
		if (Key == key)
		{
			OnRequirementUpdate();
		}
	}

	public void OnDisable()
	{
		Session.OnSessionDataUpdated -= OnSessionDataUpdated;
	}
}

public class IAClassEquippedRequiredCore : IARequiredCore
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

	public IAClassEquippedRequiredCore()
	{
		Session.MyPlayerData.ClassEquipped += OnClassEquipped;
	}

	private void OnClassEquipped(int id)
	{
		OnRequirementUpdate();
	}

	~IAClassEquippedRequiredCore()
	{
		Session.MyPlayerData.ClassEquipped -= OnClassEquipped;
	}
}

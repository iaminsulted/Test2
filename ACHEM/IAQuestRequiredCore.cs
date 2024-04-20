public class IAQuestRequiredCore : IARequiredCore
{
	public int QuestID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.HasQuest(QuestID) && !myPlayerData.IsQuestComplete(QuestID);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public IAQuestRequiredCore()
	{
		Game.Instance.QuestLoaded += CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	~IAQuestRequiredCore()
	{
		Game.Instance.QuestLoaded -= CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated -= CheckRequirement;
	}
}

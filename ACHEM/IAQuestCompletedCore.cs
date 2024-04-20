public class IAQuestCompletedCore : IARequiredCore
{
	public int QuestID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.HasQuest(QuestID) && myPlayerData.IsQuestComplete(QuestID);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public IAQuestCompletedCore()
	{
		Game.Instance.QuestLoaded += CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	~IAQuestCompletedCore()
	{
		Game.Instance.QuestLoaded -= CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated -= CheckRequirement;
	}
}

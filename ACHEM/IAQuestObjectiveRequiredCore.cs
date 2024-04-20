public class IAQuestObjectiveRequiredCore : IARequiredCore
{
	public int QuestID;

	public int QOID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.HasQuest(QuestID) && myPlayerData.IsQuestObjectiveInProgress(QuestID, QOID);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public IAQuestObjectiveRequiredCore()
	{
		Game.Instance.QuestLoaded += CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	~IAQuestObjectiveRequiredCore()
	{
		Game.Instance.QuestLoaded -= CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated -= CheckRequirement;
	}
}

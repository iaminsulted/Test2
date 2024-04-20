public class IAQuestObjectiveCompletedCore : IARequiredCore
{
	public int QuestID;

	public int QOID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.HasQuest(QuestID) && myPlayerData.IsQuestObjectiveCompleted(QuestID, QOID);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public IAQuestObjectiveCompletedCore()
	{
		Game.Instance.QuestLoaded += CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	~IAQuestObjectiveCompletedCore()
	{
		Game.Instance.QuestLoaded -= CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated -= CheckRequirement;
	}
}

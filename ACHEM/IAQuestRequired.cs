using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Quest Required")]
public class IAQuestRequired : InteractionRequirement
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

	public void OnEnable()
	{
		Game.Instance.QuestLoaded += CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	public void OnDisable()
	{
		Game.Instance.QuestLoaded -= CheckRequirement;
		Session.MyPlayerData.QuestStateUpdated -= CheckRequirement;
	}
}

using UnityEngine;

public class UIQuestTrackerContainer : MonoBehaviour
{
	public UIQuestTracker QuestTracker;

	public UIQuestOfferer QuestOfferer;

	public UIGrid Grid;

	public void OnEnable()
	{
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated += OnTrackedQuestUpdated;
		Session.MyPlayerData.AvailableQuestUpdated += OnAvailableQuestUpdated;
		Entities.Instance.me.PvpStateCheck += OnPvpStateCheck;
		QuestOfferer.Init();
		QuestTracker.ShowQuest(Session.MyPlayerData.CurrentlyTrackedQuest);
		QuestOfferer.ShowQuest(Session.MyPlayerData.AvailableQuest);
	}

	public void OnDisable()
	{
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated -= OnTrackedQuestUpdated;
		Session.MyPlayerData.AvailableQuestUpdated -= OnAvailableQuestUpdated;
		Entities.Instance.me.PvpStateCheck -= OnPvpStateCheck;
	}

	public void OnTrackedQuestUpdated(Quest quest)
	{
		QuestTracker.ShowQuest(Session.MyPlayerData.CurrentlyTrackedQuest);
		Grid.Reposition();
	}

	public void OnAvailableQuestUpdated(Quest quest)
	{
		QuestOfferer.ShowQuest(Session.MyPlayerData.AvailableQuest);
		Grid.Reposition();
	}

	private void OnPvpStateCheck()
	{
		GameObject obj = Grid.gameObject;
		AreaData areaData = Game.Instance.AreaData;
		obj.SetActive(areaData != null && !areaData.HasPvp);
	}
}

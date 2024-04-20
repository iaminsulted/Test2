using System.Collections.Generic;
using UnityEngine;

public class QuestNotification : MonoBehaviour
{
	public List<GameObject> prefabs;

	private List<NotificationQuest> notifications = new List<NotificationQuest>();

	public void OnEnable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
			Session.MyPlayerData.QuestAdded += OnQuestAccepted;
			Session.MyPlayerData.QuestTurnedIn += OnQuestTurnedIn;
		}
	}

	public void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
			Session.MyPlayerData.QuestAdded -= OnQuestAccepted;
			Session.MyPlayerData.QuestTurnedIn -= OnQuestTurnedIn;
		}
	}

	public void NotifyAccept(Quest quest)
	{
		if (quest.QSIndex != 43 && quest.QSIndex != 51)
		{
			GameObject obj = Object.Instantiate(prefabs[0], base.transform);
			obj.transform.parent = base.transform;
			NotificationQuestAccept component = obj.GetComponent<NotificationQuestAccept>();
			component.Init(quest);
			AddNotification(component);
		}
	}

	public void NotifyTurnIn(Quest quest)
	{
		if (quest.QSIndex != 43 && quest.QSIndex != 51)
		{
			GameObject obj = Object.Instantiate(prefabs[1], base.transform);
			obj.transform.parent = base.transform;
			NotificationQuestTurnIn component = obj.GetComponent<NotificationQuestTurnIn>();
			component.Init(quest);
			AddNotification(component);
		}
	}

	public void NotifyComplete(Quest quest)
	{
		if (quest.QSIndex != 43 && quest.QSIndex != 51)
		{
			GameObject obj = Object.Instantiate(prefabs[2], base.transform);
			obj.transform.parent = base.transform;
			NotificationQuestComplete component = obj.GetComponent<NotificationQuestComplete>();
			component.Init(quest);
			AddNotification(component);
		}
	}

	private void AddNotification(NotificationQuest notification)
	{
		notifications.Add(notification);
		if (notifications.Count == 1)
		{
			notifications[0].Activate();
		}
	}

	public void RemoveNotification(NotificationQuest notification)
	{
		notifications.Remove(notification);
		Object.Destroy(notification.gameObject);
		if (notifications.Count > 0)
		{
			notifications[0].Activate();
		}
	}

	private void OnQuestObjectiveUpdated(int questID, int qoid)
	{
		QuestObjective objective = Quests.GetObjective(qoid);
		if (Session.MyPlayerData.IsQuestComplete(questID))
		{
			Quest quest = Quests.Get(questID);
			if (quest != null && objective != null && quest.TurnInType != QuestTurnInType.Auto)
			{
				NotifyTurnIn(quest);
			}
			return;
		}
		Notification.ShowText(objective.Desc + " - " + Session.MyPlayerData.GetQuestObjectiveProgress(objective) + "/" + objective.Qty);
		Chat.Notify("Quest Objective: " + objective.Desc + " - " + Session.MyPlayerData.GetQuestObjectiveProgress(objective) + "/" + objective.Qty);
	}

	private void OnQuestAccepted(int questID)
	{
		Quest quest = Quests.Get(questID);
		Chat.Notify("Quest Accepted: " + quest.DisplayName);
		if (quest != null)
		{
			NotifyAccept(quest);
		}
	}

	private void OnQuestTurnedIn(int questID)
	{
		Quest quest = Quests.Get(questID);
		Chat.Notify("Quest Completed: " + quest.DisplayName);
		if (quest != null)
		{
			NotifyComplete(quest);
		}
	}
}

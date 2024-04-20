using UnityEngine;

public class DailyTaskButtonData : MonoBehaviour
{
	public CombinedDailyTask task;

	public void sendCompletionRequest()
	{
		GetComponent<UIButton>().isEnabled = false;
		Session.MyPlayerData.charDailyTasks[task.taskID - 1].collected = true;
		Game.Instance.SendDailyTaskUpdateRequest(task.taskID);
		Session.MyPlayerData.UpdateDailyTaskInfo(Session.MyPlayerData.charDailyTasks[task.taskID - 1]);
	}
}

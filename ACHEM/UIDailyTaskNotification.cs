public class UIDailyTaskNotification : UINotificationIcon
{
	public override bool ShouldIBeOn()
	{
		bool flag = false;
		for (int i = 0; i < Session.MyPlayerData.serverDailyTasks.Count; i++)
		{
			if (Session.MyPlayerData.charDailyTasks[i].curQty >= Session.MyPlayerData.serverDailyTasks[i].targetQty && !Session.MyPlayerData.charDailyTasks[i].collected)
			{
				flag = true;
			}
		}
		if (notifIcon != null)
		{
			notifIcon.SetActive(flag);
		}
		return flag;
	}
}

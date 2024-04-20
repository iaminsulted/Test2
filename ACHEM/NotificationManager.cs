public class NotificationManager
{
	private enum NotificationType
	{
		CraftReady,
		DailyRewardReady,
		AdventureAwaits
	}

	public delegate void NotificationSettingEventHandler(bool notificationsEnabled);

	private static NotificationManager instance;

	public static NotificationSettingEventHandler onNotificationSettingChange;

	private bool areNotificationsOn = true;

	private readonly int[] reminders = new int[14]
	{
		3, 7, 30, 60, 90, 120, 150, 180, 210, 240,
		270, 300, 330, 360
	};

	public static NotificationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new NotificationManager();
			}
			return instance;
		}
	}

	public bool AreNotificationsOn
	{
		get
		{
			return areNotificationsOn;
		}
		set
		{
			areNotificationsOn = value;
			if (!areNotificationsOn)
			{
				CancelAllNotifications();
			}
			if (onNotificationSettingChange != null)
			{
				onNotificationSettingChange(areNotificationsOn);
			}
		}
	}

	public static NotificationManager Init()
	{
		return Instance;
	}

	private NotificationManager()
	{
		_ = Platform.IsMobile;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
	}

	private void ScheduleNotification(int delayInSeconds, string title, string text, int id)
	{
		if (Platform.IsMobile && AreNotificationsOn)
		{
			_ = 0;
		}
	}

	private void CancelAndHideNotification(int id)
	{
		_ = Platform.IsMobile;
	}

	private void CancelAllNotifications()
	{
	}

	private static int GetNotificationID(NotificationType type, int id)
	{
		return (type.ToString() + id).GetHashCode();
	}

	public void ScheduleAdventureAwaitsNotification()
	{
		int[] array = reminders;
		foreach (int num in array)
		{
			int notificationID = GetNotificationID(NotificationType.AdventureAwaits, num);
			Instance.ScheduleNotification(num * 24 * 60 * 60, "More Adventures Await!", "Claim your Daily Reward and continue your Quest!", notificationID);
		}
	}

	public void CancelAdventureAwaitsNotification()
	{
		int[] array = reminders;
		foreach (int id in array)
		{
			int notificationID = GetNotificationID(NotificationType.AdventureAwaits, id);
			CancelAndHideNotification(notificationID);
		}
	}

	public void ScheduleCraftReadyNotification(Merge merge)
	{
		int notificationID = GetNotificationID(NotificationType.CraftReady, merge.MergeID);
		ScheduleNotification((int)(merge.MergeMinutes * 60f), "Crafting complete!", "Claim your item!", notificationID);
	}

	public void HideCraftReadyNotification(Merge merge)
	{
		int notificationID = GetNotificationID(NotificationType.CraftReady, merge.MergeID);
		CancelAndHideNotification(notificationID);
	}

	public void ScheduleDailyRewardReadyNotification()
	{
		int num = (int)(Session.MyPlayerData.RewardDate - GameTime.ServerTime).TotalSeconds;
		if (num <= 0)
		{
			num = 86400;
		}
		int notificationID = GetNotificationID(NotificationType.DailyRewardReady, 0);
		ScheduleNotification(num, "Daily reward available!", "Claim your Daily Reward!", notificationID);
	}

	public void HideDailyRewardReadyNotification()
	{
		int notificationID = GetNotificationID(NotificationType.DailyRewardReady, 0);
		CancelAndHideNotification(notificationID);
	}
}

using UnityEngine;

public class DailyChestNotificationIcon : MonoBehaviour
{
	public GameObject NotificationCirlce;

	public bool requiresOn;

	private void Start()
	{
		if (NotificationCirlce != null)
		{
			NotificationCirlce.SetActive(value: false);
		}
		InvokeRepeating("CheckDailyTimer", 0f, 2f);
	}

	public bool CheckDailyTimer()
	{
		requiresOn = Session.MyPlayerData.RewardDate < GameTime.ServerTime;
		if (NotificationCirlce != null)
		{
			NotificationCirlce.SetActive(requiresOn);
		}
		return requiresOn;
	}
}

using UnityEngine;

public class UICraftingNotification : UINotificationIcon
{
	public override bool ShouldIBeOn()
	{
		int num = 0;
		bool flag = false;
		for (int i = 0; i < Session.MyPlayerData.merges.Count; i++)
		{
			if (Session.MyPlayerData.merges[i].MergeMinutes <= 0f || (Session.MyPlayerData.merges[i].TSComplete.Value - GameTime.ServerTime).TotalSeconds <= 0.0)
			{
				num++;
			}
		}
		if (num > PlayerPrefs.GetInt("recordedMergeTotal"))
		{
			flag = true;
		}
		if (notifIcon != null)
		{
			notifIcon.SetActive(flag);
		}
		return flag;
	}
}

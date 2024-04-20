using System.Collections.Generic;

public class UITitleNotification : UINotificationIcon
{
	private void BadgeLoaded()
	{
		Badges.BadgesLoaded -= BadgeLoaded;
	}

	public override bool ShouldIBeOn()
	{
		LoadCheck();
		if (Badges.IsLoaded)
		{
			bool flag = false;
			foreach (KeyValuePair<int, Badge> item in Badges.map)
			{
				if (item.Value.isNew)
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
		return false;
	}

	public void LoadCheck()
	{
		if (!Badges.IsLoaded)
		{
			Badges.BadgesLoaded += BadgeLoaded;
			Game.Instance.SendEntityLoadBadgesRequest();
		}
	}
}

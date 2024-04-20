public class UIInventoryNotification : UINotificationIcon
{
	public override bool ShouldIBeOn()
	{
		bool flag = false;
		foreach (InventoryItem item in Session.MyPlayerData.items)
		{
			if (item.IsNew)
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

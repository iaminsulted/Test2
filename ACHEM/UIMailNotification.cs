public class UIMailNotification : UINotificationIcon
{
	public override bool ShouldIBeOn()
	{
		bool flag = false;
		foreach (MailMessage item in Session.MyPlayerData.mailbox)
		{
			if (!item.hasSeen)
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

public class UIFriendsListNotification : UINotificationIcon
{
	public override bool ShouldIBeOn()
	{
		bool flag = Session.MyPlayerData.friendRequests.Count > 0;
		if (notifIcon != null)
		{
			notifIcon.SetActive(flag);
		}
		return flag;
	}
}

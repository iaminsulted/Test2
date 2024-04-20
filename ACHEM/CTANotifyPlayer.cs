using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Notify Player")]
public class CTANotifyPlayer : ClientTriggerAction
{
	public string Text;

	public GameNotificationType NotificationType = GameNotificationType.Both;

	protected override void OnExecute()
	{
		if ((NotificationType & GameNotificationType.Chat) == GameNotificationType.Chat)
		{
			Chat.Notify(Text);
		}
		if ((NotificationType & GameNotificationType.Standard) == GameNotificationType.Standard)
		{
			Notification.ShowText(Text);
		}
	}
}

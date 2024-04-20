using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Notify Cell")]
public class MANotifyCell : ListenerAction
{
	public string Message;

	public GameNotificationType NotificationType = GameNotificationType.Both;

	public bool PlayerOnly;
}

using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Notify Team Cell")]
public class MANotifyTeamCell : ListenerAction
{
	public string Message;

	public GameNotificationType NotificationType = GameNotificationType.Both;

	public int teamID;
}

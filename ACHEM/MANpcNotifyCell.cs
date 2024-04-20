using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Npc Notify Cell")]
public class MANpcNotifyCell : ListenerAction
{
	public NPCSpawn npc;

	public int NPCSpawnID;

	public int cellID;

	public string Message;

	public GameNotificationType NotificationType;
}

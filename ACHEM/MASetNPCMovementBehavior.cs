using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC Movement Behavior")]
public class MASetNPCMovementBehavior : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn spawn;

	public NPCMovementBehavior behavior;
}

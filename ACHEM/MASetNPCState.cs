using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC State")]
public class MASetNPCState : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn NPCSpawn;

	public byte State;
}

using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC Pathing")]
public class MASetNPCPathing : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn spawn;

	public bool pathingActive;

	public float duration;
}

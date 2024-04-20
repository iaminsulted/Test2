using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC Level")]
public class MASetNPCLevel : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn NPCSpawn;

	public int level;
}

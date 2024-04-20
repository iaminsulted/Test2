using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC Regeneration")]
public class MASetNPCRegeneration : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn NPCSpawn;

	public bool allowRegeneration;
}

using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set NPC Reaction")]
public class MASetNPCReaction : ListenerAction
{
	[HideInInspector]
	public int NPCSpawnID;

	public NPCSpawn NPCSpawn;

	public Entity.Reaction Reaction;

	public bool setAll;
}

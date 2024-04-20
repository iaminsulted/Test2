using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA NPC Bait")]
public class MANpcBait : CompletionAction
{
	public NPCSpawn baitSpawn;

	public float baitDuration;

	public float baitDistance;

	public Entity.Reaction initialReaction;

	public string targetAnimation;

	public float targetAnimationDuration;

	public Entity.Reaction targetReaction;

	public NPCPathNodeMoveSpeed targetMoveSpeed;

	public List<NPCSpawn> targetSpawns;
}

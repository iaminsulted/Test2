using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Play Animation")]
public class MAPlayAnimation : ListenerAction
{
	public enum Target
	{
		Everyone,
		AllFriendlies,
		AllHostile,
		AllPlayers,
		TargetNPC,
		Self
	}

	public Target target;

	public AnimationType[] animations;

	public NPCSpawn npcSpawn;

	public float crossfadeDuration;

	public int layer;

	public float normalizedTime;

	public bool onlyUser;
}

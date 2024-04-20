using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Triggers/Collide Client Trigger")]
public class CollideClientTrigger : ClientTrigger
{
	public CollisionMode Mode;

	public EntityCollision entityCollision = EntityCollision.Client;

	public List<NPCSpawn> collisionTargets = new List<NPCSpawn>();

	public float StayDuration;

	public int soundTrackID;

	private float currentDuration;

	public void OnTriggerEnter(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity))
		{
			if (Mode == CollisionMode.Enter)
			{
				Trigger(checkRequirements: true);
			}
			if (soundTrackID > 0)
			{
				StartCoroutine(SoundTracks.Instance.Play(soundTrackID, showLoader: false));
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity))
		{
			if (Mode == CollisionMode.Exit)
			{
				Trigger(checkRequirements: true);
			}
			else if (Mode == CollisionMode.Stay)
			{
				currentDuration = 0f;
				ProgressBar.Hide();
			}
			if (soundTrackID > 0)
			{
				StartCoroutine(SoundTracks.Instance.Play(Game.Instance.AreaData.SoundTrackID, showLoader: false));
			}
		}
	}

	public void OnTriggerStay(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity) && Mode == CollisionMode.Stay)
		{
			currentDuration += Time.deltaTime;
			ProgressBar.Show(currentDuration / StayDuration, "Interacting...");
			if (currentDuration > StayDuration)
			{
				Trigger(checkRequirements: true);
				ProgressBar.Hide();
			}
		}
	}

	private bool CheckEntityCollision(Entity entity)
	{
		switch (entityCollision)
		{
		case EntityCollision.All:
			return true;
		case EntityCollision.Client:
			return entity.isMe;
		case EntityCollision.Friendly:
			return entity.react == Entity.Reaction.Friendly;
		case EntityCollision.Hostile:
			if (entity.react != Entity.Reaction.AgroAll && entity.react != Entity.Reaction.AgroOtherKind)
			{
				return entity.react == Entity.Reaction.Hostile;
			}
			return true;
		case EntityCollision.Neutral:
			return entity.react == Entity.Reaction.Neutral;
		case EntityCollision.Passive:
			return entity.react == Entity.Reaction.Passive;
		case EntityCollision.Player:
			return entity is Player;
		case EntityCollision.TargetNPC:
			if (entity is NPC)
			{
				return collisionTargets.Contains((entity as NPC).Spawn);
			}
			return false;
		case EntityCollision.PlayerSummon:
			if (entity is NPC nPC2)
			{
				return nPC2.summoner is Player;
			}
			return false;
		case EntityCollision.NPCSummon:
			if (entity is NPC nPC)
			{
				return nPC.summoner is NPC;
			}
			return false;
		default:
			return false;
		}
	}
}

using UnityEngine;

public abstract class EntityCollisionTrigger : MonoBehaviour
{
	public EntityCollision entityCollision;

	protected bool CheckEntityCollision(Entity entity)
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

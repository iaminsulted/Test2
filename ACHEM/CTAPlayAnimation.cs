using UnityEngine;

public class CTAPlayAnimation : ClientTriggerAction
{
	public enum Target
	{
		Everyone,
		AllFriendlies,
		AllHostile,
		TargetNPC,
		OnlyMe
	}

	public Target target;

	public AnimationType[] animations;

	public NPCSpawn npcSpawn;

	public const float crossfadeDuration = 0.25f;

	private string RandomAnim
	{
		get
		{
			int num = 0;
			AnimationType[] array = animations;
			for (int i = 0; i < array.Length; i++)
			{
				AnimationType animationType = array[i];
				num += animationType.probabilty;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < animations.Length; j++)
			{
				num3 += animations[j].probabilty;
				if (num2 < num3)
				{
					num4 = j;
					break;
				}
			}
			return animations[num4].animName;
		}
	}

	protected override void OnExecute()
	{
		switch (target)
		{
		case Target.Everyone:
		{
			foreach (Entity allEntity in Entities.Instance.AllEntities)
			{
				allEntity.PlayAnimation(RandomAnim, 0.25f);
			}
			break;
		}
		case Target.AllFriendlies:
		{
			foreach (Entity allEntity2 in Entities.Instance.AllEntities)
			{
				if (allEntity2.react == Entity.Reaction.Friendly)
				{
					allEntity2.PlayAnimation(RandomAnim, 0.25f);
				}
			}
			break;
		}
		case Target.AllHostile:
		{
			foreach (Entity allEntity3 in Entities.Instance.AllEntities)
			{
				if (allEntity3.react == Entity.Reaction.Hostile)
				{
					allEntity3.PlayAnimation(RandomAnim, 0.25f);
				}
			}
			break;
		}
		case Target.TargetNPC:
			if (npcSpawn.State != 0)
			{
				NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(npcSpawn.ID);
				if (!npcBySpawnId.HasAggro())
				{
					npcBySpawnId.PlayAnimation(RandomAnim, 0.25f);
				}
			}
			break;
		case Target.OnlyMe:
			Entities.Instance.me?.PlayAnimation(RandomAnim, 0.25f);
			break;
		}
	}
}

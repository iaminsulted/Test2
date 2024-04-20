namespace Assets.Scripts.Game;

public class SpellRequirement
{
	public enum TargetType
	{
		Caster,
		Target,
		PrevTarget
	}

	public TargetType targetType = TargetType.Target;

	public float minHp = -1f;

	public float maxHp = 1f;

	public float minResource;

	public float maxResource = 1f;

	public bool notMaxLevel;

	public bool notMaxRank;

	public CombatSolver.Element element;

	public bool IsMet(Entity caster, Entity target, Entity prevTarget)
	{
		Entity entity = null;
		switch (targetType)
		{
		case TargetType.Caster:
			entity = caster;
			break;
		case TargetType.Target:
			entity = target;
			break;
		case TargetType.PrevTarget:
			entity = prevTarget;
			break;
		}
		if (entity == null)
		{
			return false;
		}
		if (MeetsHealthCheck(entity) && MeetsResourceCheck(entity) && MeetsMaxLevelCheck(entity) && MeetsMaxRankCheck(entity))
		{
			return MeetsElementCheck(entity);
		}
		return false;
	}

	private bool MeetsHealthCheck(Entity reqTarget)
	{
		if (reqTarget == null)
		{
			return false;
		}
		if (reqTarget.HealthPercent <= maxHp)
		{
			return reqTarget.HealthPercent >= minHp;
		}
		return false;
	}

	public bool MeetsResourceCheck(Entity reqTarget)
	{
		if (reqTarget == null)
		{
			return false;
		}
		if (maxResource == 1f || reqTarget.ResourcePercent <= maxResource)
		{
			if (minResource != 0f)
			{
				return reqTarget.ResourcePercent >= minResource;
			}
			return true;
		}
		return false;
	}

	private bool MeetsMaxLevelCheck(Entity reqTarget)
	{
		if (reqTarget == null)
		{
			return false;
		}
		if (notMaxLevel)
		{
			return reqTarget.Level < Session.MyPlayerData.LevelCap;
		}
		return true;
	}

	private bool MeetsMaxRankCheck(Entity reqTarget)
	{
		if (reqTarget == null)
		{
			return false;
		}
		if (notMaxRank)
		{
			return reqTarget.EquippedClassRank < 100;
		}
		return true;
	}

	private bool MeetsElementCheck(Entity reqTarget)
	{
		if (reqTarget == null)
		{
			return false;
		}
		if (element != 0)
		{
			return reqTarget.elements.Contains(element);
		}
		return true;
	}
}

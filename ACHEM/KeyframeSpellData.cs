using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;

public class KeyframeSpellData
{
	private static int GlobalID;

	public int ID;

	public readonly List<ComEntityUpdate> entityUpdates;

	public readonly List<Entity> targets;

	public readonly Entity caster;

	public readonly SpellTemplate spellT;

	public readonly List<AoeLocation> aoeLocations = new List<AoeLocation>();

	public SpellAction spellAction;

	public bool hasProjectileLaunched;

	public bool hasCastFxPlayed;

	public int currentImpact;

	public int totalImpacts = int.MaxValue;

	public int fxImpacts;

	public int totalFxImpacts = int.MaxValue;

	public List<float> hpDeltaSoFar;

	public float multihitLeftover;

	public bool wasInterrupted;

	public Dictionary<Entity, CombatPopup> multihitPopups;

	public KeyframeSpellData(List<ComEntityUpdate> entityUpdates, Entity caster, SpellTemplate spellT, List<Entity> targets, List<AoeLocation> aoeLocations)
	{
		ID = GlobalID++;
		this.entityUpdates = entityUpdates ?? new List<ComEntityUpdate>();
		this.caster = caster;
		this.spellT = spellT;
		this.targets = targets ?? new List<Entity>();
		multihitPopups = new Dictionary<Entity, CombatPopup>();
		hpDeltaSoFar = new List<float>(new float[this.entityUpdates.Count]);
		spellAction = spellT.GetFirstAction(caster, entityUpdates);
		this.aoeLocations = aoeLocations ?? new List<AoeLocation>();
	}

	public KeyframeSpellData(KeyframeSpellData s)
	{
		ID = s.ID;
		entityUpdates = s.entityUpdates;
		targets = s.targets;
		caster = s.caster;
		spellT = s.spellT;
		spellAction = s.spellAction;
		hasProjectileLaunched = s.hasProjectileLaunched;
		hasCastFxPlayed = s.hasCastFxPlayed;
		currentImpact = s.currentImpact;
		totalImpacts = s.totalImpacts;
		fxImpacts = s.fxImpacts;
		totalFxImpacts = s.totalFxImpacts;
		hpDeltaSoFar = s.hpDeltaSoFar;
		multihitLeftover = s.multihitLeftover;
		multihitPopups = s.multihitPopups;
		wasInterrupted = s.wasInterrupted;
	}

	public bool AreImpactsDone()
	{
		bool num = currentImpact >= totalImpacts && !spellAction.usesFXImpacts;
		bool flag = fxImpacts >= totalFxImpacts && spellAction.usesFXImpacts;
		bool flag2 = entityUpdates.Count <= 0;
		return num || flag || flag2;
	}

	public float SolveMultihitDelta(ComEntityUpdate entityUpdate, int updateIndex, float statDeltaPercent)
	{
		float num = 0f;
		if (entityUpdate.statDeltas.ContainsKey(0))
		{
			num = entityUpdate.rawHpDelta;
			if (IsLastImpact())
			{
				num -= hpDeltaSoFar[updateIndex];
			}
			else
			{
				num *= statDeltaPercent;
				num += multihitLeftover;
			}
		}
		multihitLeftover = num - num;
		hpDeltaSoFar[updateIndex] += num;
		return num;
	}

	public List<ComEntityUpdate> GetActionUpdates()
	{
		List<ComEntityUpdate> list = entityUpdates.Where((ComEntityUpdate update) => update.spellActionId == spellAction.ID).ToList();
		if (spellAction == spellT.GetFirstAction(caster, entityUpdates))
		{
			list.AddRange(entityUpdates.Where((ComEntityUpdate update) => update.spellActionId == 0).ToList());
		}
		return list;
	}

	public List<Entity> GetParentActionTargets()
	{
		List<SpellAction> parentActions = spellT.GetParentActions(spellAction);
		List<Entity> list = new List<Entity>();
		foreach (SpellAction item in parentActions)
		{
			list.AddRange(GetActionTargets(item));
		}
		return list.Distinct().ToList();
	}

	public List<Entity> GetCurrentActionTargets()
	{
		return GetActionTargets(spellAction);
	}

	public List<Entity> GetActionTargets(SpellAction action)
	{
		return (from entityUpdate in entityUpdates
			where entityUpdate.spellActionId == action.ID
			select Entities.Instance.GetEntity(entityUpdate.entityType, entityUpdate.entityID)).ToList();
	}

	public bool AffectsMe()
	{
		return entityUpdates.Any((ComEntityUpdate u) => u.entityID == Entities.Instance.me.ID && u.entityType == Entities.Instance.me.type);
	}

	public bool IsFirstImpact()
	{
		if (currentImpact != 1 || spellAction.usesFXImpacts)
		{
			if (fxImpacts == 1)
			{
				return spellAction.usesFXImpacts;
			}
			return false;
		}
		return true;
	}

	public bool IsLastImpact()
	{
		if (currentImpact != totalImpacts || spellAction.usesFXImpacts)
		{
			if (fxImpacts == totalFxImpacts)
			{
				return spellAction.usesFXImpacts;
			}
			return false;
		}
		return true;
	}
}

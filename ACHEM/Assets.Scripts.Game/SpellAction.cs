using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Game;

public class SpellAction
{
	public int ID = 1;

	public int[] chainedActionIDs = new int[0];

	public float delay;

	public string[] chargeAnimsOverride = new string[0];

	public string[] animsOverride = new string[0];

	public string impactFX;

	public string castSpotFX;

	public bool useWeaponCastPrefab;

	public bool usesFXImpacts;

	public bool onlyShowLastImpactFX;

	[JsonProperty("camShakes")]
	private List<SpellCamShake> _camShakes = new List<SpellCamShake>();

	public string impactSFX;

	public string critSFX;

	public CombatSolver.ProjectileType projType;

	public string projFX;

	public string projSFX;

	public string projImpactCasterFX;

	public float projSpeed = 30f;

	public float projStayTime;

	public float projImpactDelay;

	public CombatSolver.TargetType targetType;

	public CombatSolver.AreaTarget areaTarget;

	public AuraTemplate aura;

	public SpellChannel channel;

	public float range = 4f;

	public List<AoeShape> aoes = new List<AoeShape>();

	public bool casterAlwaysAoeSource;

	public int maxTargets = int.MaxValue;

	public bool hideTelegraphs;

	public CombatSolver.DamageModel damageModel;

	public float damageMult;

	public List<SpellEffect> spellEffects = new List<SpellEffect>();

	public bool removeCC;

	public List<SpellRequirement> requirements = new List<SpellRequirement>();

	public EffectReq effectReq;

	public bool stopIfReqsMet;

	[JsonIgnore]
	public List<SpellCamShake> camShakes
	{
		get
		{
			if (_camShakes == null || _camShakes.Count == 0)
			{
				return new List<SpellCamShake>
				{
					new SpellCamShake
					{
						target = SpellCamShake.Target.Caster,
						trigger = SpellCamShake.Trigger.Impact,
						multiplier = 1f,
						style = SpellCamShake.Style.Random
					}
				};
			}
			return _camShakes;
		}
		set
		{
			_camShakes = value;
		}
	}

	[JsonIgnore]
	public bool isHarmful => targetType == CombatSolver.TargetType.Hostile;

	[JsonIgnore]
	public bool isAoe => aoes.Count > 0;

	[JsonIgnore]
	public bool makesAura => aura != null;

	[JsonIgnore]
	public bool isProjectile => projType != CombatSolver.ProjectileType.None;

	public bool ShouldReuseChargeProjectile => GetCastSpotFX() == projFX;

	public AoeShape GetAoeById(int aoeID, bool isAura)
	{
		if (!isAura)
		{
			return aoes.FirstOrDefault((AoeShape aoe) => aoe.ID == aoeID);
		}
		return aura.aoes.FirstOrDefault((AoeShape aoe) => aoe.ID == aoeID);
	}

	public string GetCastSpotFX()
	{
		if (!CombatSolver.DoesProjTypeCreateCastSpotFX(projType))
		{
			return "";
		}
		if (!string.IsNullOrEmpty(castSpotFX))
		{
			return castSpotFX;
		}
		if (!string.IsNullOrEmpty(projFX))
		{
			return projFX;
		}
		return "";
	}

	public Vector3 GetProjectileStartPosition(Vector3 casterPosition, Vector3 targetPosition)
	{
		Vector3 vector = default(Vector3);
		return projType switch
		{
			CombatSolver.ProjectileType.Meteor => targetPosition + new Vector3(ArtixRandom.Range(-4f, 4f), 8f, 0f), 
			CombatSolver.ProjectileType.Reverse => targetPosition, 
			_ => casterPosition, 
		};
	}

	public float GetProjectileImpactDelay(Vector3 castPosition, Vector3 targetPosition)
	{
		return (targetPosition - castPosition).magnitude / projSpeed;
	}

	public bool IsCastable(Entity caster)
	{
		if (CheckCasterEffectReqs(caster))
		{
			return MeetsSpellReqs(SpellRequirement.TargetType.Caster, caster);
		}
		return false;
	}

	public bool CheckCasterEffectReqs(Entity caster)
	{
		if (effectReq == null || !effectReq.isOnSelf)
		{
			return true;
		}
		Effect effect = caster.effects.FirstOrDefault((Effect e) => e.template.ID == effectReq.effectID);
		if (effectReq.mustNotHave)
		{
			if (effect != null)
			{
				if (effectReq.needMaxStacks)
				{
					return effect.stacks < effect.template.maxStack;
				}
				return false;
			}
			return true;
		}
		if (effect != null)
		{
			if (effectReq.needMaxStacks)
			{
				return effect.stacks == effect.template.maxStack;
			}
			return true;
		}
		return false;
	}

	public bool CheckRequirements(Entity caster, Entity target)
	{
		if ((target.serverState != Entity.State.Dead || !isAoe) && CheckTargetEffectReqs(caster, target))
		{
			return MeetsSpellReqs(SpellRequirement.TargetType.Target, target);
		}
		return false;
	}

	public bool CheckTargetEffectReqs(Entity caster, Entity target)
	{
		if (effectReq == null || effectReq.isOnSelf)
		{
			return true;
		}
		Effect effect = ((!effectReq.reqMyEffect) ? target.effects.FirstOrDefault((Effect e) => e.template.ID == effectReq.effectID) : target.effects.FirstOrDefault((Effect e) => e.template.ID == effectReq.effectID && e.casterID == caster.ID && e.casterType == caster.type));
		if (effectReq.mustNotHave)
		{
			if (effect != null)
			{
				if (effectReq.needMaxStacks)
				{
					return effect.stacks < effect.template.maxStack;
				}
				return false;
			}
			return true;
		}
		if (effect != null)
		{
			if (effectReq.needMaxStacks)
			{
				return effect.stacks == effect.template.maxStack;
			}
			return true;
		}
		return false;
	}

	private bool MeetsSpellReqs(SpellRequirement.TargetType targetType, Entity entity)
	{
		Entity caster = null;
		Entity target = null;
		Entity prevTarget = null;
		switch (targetType)
		{
		case SpellRequirement.TargetType.Caster:
			caster = entity;
			break;
		case SpellRequirement.TargetType.Target:
			target = entity;
			break;
		case SpellRequirement.TargetType.PrevTarget:
			prevTarget = entity;
			break;
		}
		return requirements.Where((SpellRequirement r) => r.targetType == targetType).All((SpellRequirement r) => r.IsMet(caster, target, prevTarget));
	}

	public List<AoeShape> GetAllAoes()
	{
		List<AoeShape> list = new List<AoeShape>();
		list.AddRange(aoes);
		if (aura != null)
		{
			list.AddRange(aura.aoes);
		}
		return list;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AQ3DServer.GameServer.CommClasses;
using Assets.Scripts.Game;
using PigeonCoopToolkit.Effects.Trails;
using StatCurves;
using UnityEngine;

public abstract class Entity
{
	public enum Type
	{
		Undefined,
		Player,
		NPC
	}

	[Flags]
	public enum StatusType : long
	{
		None = 0L,
		Slow = 1L,
		Sleep = 2L,
		Blind = 4L,
		Fear = 8L,
		Root = 0x10L,
		Silence = 0x20L,
		Stun = 0x40L,
		Taunt = 0x80L,
		Freeze = 0x100L,
		Suppress = 0x200L,
		SlowMovement = 1L,
		NoMovement = 0x152L,
		StopAnimation = 0x140L,
		NoSpells = 0x16AL,
		NoAA = 0x14AL,
		CC = 0x17BL
	}

	public enum State
	{
		Normal,
		InCombat,
		Dead,
		Interacting,
		Leashing
	}

	public enum Reaction
	{
		Friendly,
		Hostile,
		Neutral,
		Passive,
		PassiveAggressive,
		AgroAll,
		AgroOtherKind
	}

	public enum Resource
	{
		None,
		Mana,
		Spirit,
		Determination,
		Bullets,
		Souls,
		Chi,
		Focus,
		Fury,
		ManaNoRegen,
		BreathOfTheWind
	}

	public enum ImpactSource
	{
		Animation,
		FX,
		Projectile,
		Aura
	}

	public enum EffectsToApply
	{
		All,
		Immediate,
		Delayed
	}

	public enum PhaseState
	{
		None,
		Phase1,
		Phase2
	}

	public const float Walk_Speed = 1.5f;

	public const float Backpedal_Speed_Percent = 0.6f;

	public const float Side_Speed_Percent = 0.9f;

	public const float Run_Speed = 6.5f;

	public const float Max_Ult_Charge = 1000f;

	public const float Melee_Distance = 4f;

	public readonly Vector3 LOSOffset = new Vector3(0f, 1.75f, 0f);

	public bool IsSheathed;

	public int ID;

	public Type type;

	public string name;

	public int EquippedClassID;

	public int EquippedClassXP;

	public int TotalClassRank;

	public string Description;

	public int XP;

	public OwnerMachine OwnedMachine;

	public bool isBoss;

	public EntityAnimation delayedAnimation;

	public EntityAsset baseAsset;

	public EntityAsset overrideAsset;

	public float combatRadius = 0.55f;

	public bool IsTargetReticleHidden;

	public int areaID;

	public int cellID;

	public string ClassName;

	public Transform wrapperTransform;

	public GameObject _wrapper;

	public AssetController assetController;

	public EntityController entitycontroller;

	public MovementController moveController;

	public bool IsMovementDisabled;

	private bool isAssetTransforming;

	protected Entity target;

	protected ResourceNode targetNode;

	protected Transform targetTransform;

	private uint lastSpellRecasts;

	private GameObject levelUpPopup;

	public Vector3 position;

	public Quaternion rotation = Quaternion.Euler(Vector3.zero);

	public bool isPvPFlagged;

	public int teamID;

	protected Entities entities = Entities.Instance;

	protected Camera mainCamera;

	private List<SequencedEvent> frameEvents = new List<SequencedEvent>();

	public int ExpectedTotalStat;

	public EntityStats statsBaseline;

	public EntityStats statsCurrent;

	public EntityResists resists;

	public List<CombatSolver.Element> elements = new List<CombatSolver.Element>();

	public StatusType statusMap;

	public List<Effect> effects = new List<Effect>();

	public List<int> serverEffectIDs = new List<int>();

	public SpellComboState comboState = new SpellComboState();

	public List<Aura> auras = new List<Aura>();

	private DateTime lastEffectSyncTime;

	private DateTime lastStateSyncTime;

	private DateTime lastStatusSyncTime;

	private DateTime lastStatsSyncTime;

	private DateTime lastReactSyncTime;

	private DateTime lastAssetUpdateTime;

	private State _serverState;

	private State _visualState;

	public Reaction react;

	public StateEmote stateEmote;

	public PhaseState phaseState;

	public SpellCastData spellCastData;

	public int Portrait;

	public int Title;

	public string TitleName;

	private KeyframeSpellData keyframeSpellData;

	private GameObject chargeProjectile;

	public EntityAssetData entitySpots;

	public NamePlate namePlate;

	private SmoothTrail aaSmoothTrail;

	private SmoothTrail spellSmoothTrail;

	public GameObject petGO;

	public bool isAFK;

	private int scaledLevel;

	private int level;

	private readonly List<string> stepSounds = new List<string> { "Footsteps_B2", "Footsteps_B3" };

	private List<SequencedEvent> queuedCombatEvents = new List<SequencedEvent>();

	public int AccessLevel { get; set; }

	public bool IsInTransform => overrideAsset != null;

	public CombatClass CombatClass => CombatClass.GetClassByID(EquippedClassID);

	public int EquippedClassRank => ClassRanks.GetRank(EquippedClassXP);

	public int ScaledClassRank
	{
		get
		{
			if (!Game.Instance.AreaData.HasPvp && !Game.Instance.AreaData.IsPvpLobby)
			{
				return EquippedClassRank;
			}
			return 10;
		}
	}

	public bool OwnsMachine => OwnedMachine != null;

	public GameObject wrapper
	{
		get
		{
			return _wrapper;
		}
		private set
		{
			_wrapper = value;
		}
	}

	public SpellTemplate lastSpell { get; protected set; }

	public string ElementString => string.Join(",", elements.Select((CombatSolver.Element p) => p.ToString()).ToArray());

	public virtual bool IsNamePlateHidden => false;

	public virtual bool CanRotate => true;

	public virtual bool CanMove => true;

	public float WalkSpeed => 1.5f;

	public float BackpedalSpeed => 0.6f * RunSpeed;

	public float SideSpeed => 0.9f * RunSpeed;

	public float RunSpeed => statsCurrent[Stat.RunSpeed];

	public Transform CastSpot
	{
		get
		{
			if (entitySpots != null && entitySpots.CastSpot != null)
			{
				return entitySpots.CastSpot;
			}
			if (wrapper != null)
			{
				return wrapper.transform;
			}
			return null;
		}
	}

	public Transform HitSpot
	{
		get
		{
			if (entitySpots != null && entitySpots.HitSpot != null)
			{
				return entitySpots.HitSpot;
			}
			return wrapper.transform;
		}
	}

	public virtual Transform HeadSpot
	{
		get
		{
			if (entitySpots != null && entitySpots.HeadSpot != null)
			{
				return entitySpots.HeadSpot;
			}
			return wrapper.transform;
		}
	}

	public virtual Transform CameraSpot
	{
		get
		{
			if (entitySpots != null && entitySpots.CameraSpot != null)
			{
				return entitySpots.CameraSpot;
			}
			return null;
		}
	}

	public int ScaledLevel
	{
		get
		{
			return scaledLevel;
		}
		set
		{
			scaledLevel = value;
		}
	}

	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public int DisplayLevel
	{
		get
		{
			int num = Entities.Instance.me.Level;
			int num2 = Entities.Instance.me.ScaledLevel;
			if (type == Type.NPC && num2 < num)
			{
				return Level + num - num2;
			}
			if (type == Type.Player && (Entities.Instance.me.DuelOpponentID == ID || Game.Instance.AreaData.HasPvp || Game.Instance.AreaData.IsPvpLobby))
			{
				return num;
			}
			return Level;
		}
	}

	public Resource resource
	{
		get
		{
			if (CombatClass != null)
			{
				return CombatClass.Resource;
			}
			return Resource.None;
		}
	}

	public abstract string ClassIcon { get; }

	public float Damage => GetDerivedStatValue(Stat.Attack, Stat.DamageBonus);

	public float Defense => GetDerivedStatValue(Stat.Armor, Stat.DefenseBonus);

	public float DodgeChance => GetDerivedStatValue(Stat.Evasion, Stat.DodgeBonus);

	public float CritChance => GetDerivedStatValue(Stat.Crit, Stat.CritBonus);

	public float CastSpeed => (1f - GetDerivedStatValue(Stat.Haste, Stat.CastSpeedBonus)).Clamp(0.1f, 10f);

	public float SkillSpeed => (1f - GetDerivedStatValue(Stat.Haste, Stat.SkillSpeedBonus)).Clamp(0.1f, 10f);

	public float AASpeed => (1f - GetDerivedStatValue(Stat.Haste, Stat.AASpeedBonus)).Clamp(0.1f, 10f);

	public float HealthPercent
	{
		get
		{
			float num = statsCurrent[Stat.Health] / statsCurrent[Stat.MaxHealth];
			if (!(num > 1f))
			{
				return num;
			}
			return 1f;
		}
	}

	public float ResourcePercent
	{
		get
		{
			if (statsCurrent[Stat.MaxResource] == 0f)
			{
				return 1f;
			}
			return statsCurrent[Stat.Resource] / statsCurrent[Stat.MaxResource];
		}
	}

	public State serverState
	{
		get
		{
			return _serverState;
		}
		set
		{
			if (_serverState != value)
			{
				State previousState = _serverState;
				_serverState = value;
				OnServerStateChanged(previousState, value);
			}
		}
	}

	public State visualState
	{
		get
		{
			return _visualState;
		}
		protected set
		{
			if (_visualState != value)
			{
				_visualState = value;
				OnVisualStateChanged();
			}
		}
	}

	public abstract bool IsInPvp { get; }

	public Entity Target
	{
		get
		{
			return target;
		}
		set
		{
			if (target == value)
			{
				if (target == null)
				{
					OnTargetUpdateEvent(target);
				}
				return;
			}
			if (target != null)
			{
				this.TargetDeselected?.Invoke(target);
				target.DeathEvent -= OnTargetDeath;
				target.Destroyed -= OnTargetDestroyed;
				if (isMe)
				{
					target.ReactUpdated -= Game.Instance.UpdateTargetPortrait;
				}
				target = null;
				targetTransform = null;
			}
			if (value != null)
			{
				target = value;
				targetNode = null;
				targetTransform = target.wrapperTransform;
				target.DeathEvent += OnTargetDeath;
				target.Destroyed += OnTargetDestroyed;
				if (isMe)
				{
					target.ReactUpdated += Game.Instance.UpdateTargetPortrait;
				}
				ClearCombatEvents();
				this.TargetSelected?.Invoke(target);
			}
			OnTargetUpdateEvent(target);
		}
	}

	public ResourceNode TargetNode
	{
		get
		{
			return targetNode;
		}
		set
		{
			if (targetNode == value)
			{
				if (targetNode != null)
				{
					((ClientMovementController)Entities.Instance.me.moveController).TargetAutoRun(targetTransform, 6f, AutoRun.ResourceInteract);
				}
				return;
			}
			if (targetNode != null)
			{
				this.TargetNodeDeselected?.Invoke(targetNode);
				targetNode = null;
				targetTransform = null;
			}
			if (value != null)
			{
				target = null;
				targetNode = value;
				targetTransform = targetNode.transform;
				((ClientMovementController)Entities.Instance.me.moveController).TargetAutoRun(targetTransform, 6f, AutoRun.ResourceInteract);
				this.TargetNodeSelected?.Invoke(targetNode);
			}
			this.TargetNodeUpdated?.Invoke(targetNode);
		}
	}

	public Transform TargetTransform
	{
		get
		{
			return targetTransform;
		}
		private set
		{
			targetTransform = value;
		}
	}

	public bool isMe => entities.me == this;

	public EntityAsset currentAsset
	{
		get
		{
			if (overrideAsset == null)
			{
				return baseAsset;
			}
			return overrideAsset;
		}
	}

	private float CurrentScaleFactor
	{
		get
		{
			List<float> list = (from e in effects
				where e.template.entityScaleFactor > 0f
				select 1f + (e.template.entityScaleFactor - 1f) * (float)e.stacks).ToList();
			float num = 1f;
			foreach (float item in list)
			{
				num *= item;
			}
			return Mathf.Max(num, 0.25f);
		}
	}

	public Hashtable CurrentShaderFx
	{
		get
		{
			List<Hashtable> list = (from p in effects
				where p.template.shaderFX != null
				select p.template.shaderFX).ToList();
			EffectTemplate.ShaderFxType shaderFxType = EffectTemplate.ShaderFxType.None;
			Hashtable result = null;
			if (list != null && list.Count > 0)
			{
				foreach (Hashtable item in list)
				{
					try
					{
						EffectTemplate.ShaderFxType shaderFxType2 = (EffectTemplate.ShaderFxType)Enum.Parse(typeof(EffectTemplate.ShaderFxType), (string)item["Type"]);
						if (shaderFxType2 > shaderFxType)
						{
							shaderFxType = shaderFxType2;
							result = item;
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
			return result;
		}
	}

	public List<string> CurrentEffectParticles => (from p in effects
		where p.template.type != EffectTemplate.EffectType.Passive && !string.IsNullOrEmpty(p.template.particleFX)
		select p.template.particleFX).Distinct().ToList();

	public Vector3 spawnPostion => position;

	public Quaternion spawnRotation => rotation;

	public bool IsCharging
	{
		get
		{
			SpellCastData obj = spellCastData;
			if (obj == null)
			{
				return false;
			}
			return obj.state == SpellCastData.State.Charging;
		}
	}

	public bool IsChargingAoe
	{
		get
		{
			if (spellCastData?.spellT == null)
			{
				return false;
			}
			SpellAction firstCastableAction = spellCastData.spellT.GetFirstCastableAction(this);
			if (firstCastableAction.isAoe || firstCastableAction.makesAura)
			{
				return IsCharging;
			}
			return false;
		}
	}

	public bool IsChanneling
	{
		get
		{
			SpellCastData obj = spellCastData;
			if (obj == null)
			{
				return false;
			}
			return obj.state == SpellCastData.State.Channeling;
		}
	}

	public event Action<int> PortraitUpdated;

	public event Action<int, string> TitleUpdated;

	public event Action<Entity> DeathEvent;

	public event Action<Entity> TargetUpdateEvent;

	public event Action<Entity> TargetSelected;

	public event Action<Entity> TargetDeselected;

	public event Action<ResourceNode> TargetNodeUpdated;

	public event Action<ResourceNode> TargetNodeSelected;

	public event Action<ResourceNode> TargetNodeDeselected;

	public event Action StatUpdateEvent;

	public event Action ClassUpdated;

	public event Action RespawnEvent;

	public event Action<Effect> EffectAdded;

	public event Action<Effect> EffectRemoved;

	public event Action Destroyed;

	public event Action LevelUpdated;

	public event Action<int> ClassRankUpdated;

	public event Action<Entity> ReactUpdated;

	public event Action<State, State> ServerStateChanged;

	public event Action PvpStateCheck;

	public event Action<Entity> VisualStateChanged;

	public event Action<SpellTemplate> Cast;

	public event Action ChargeEnded;

	public event Action CastCanceled;

	public event Action<SpellTemplate, float> ChargeStarted;

	public event Action<SpellTemplate, int, float> ChannelStarted;

	public event Action ChannelEnded;

	public event Action<EntityAsset> OverrideAssetUpdated;

	public event Action<bool> ChangeAFKStatus;

	public event Action<int> AoeDestroyed;

	public Entity()
	{
		baseAsset = new EntityAsset();
		statsBaseline = new EntityStats();
		statsCurrent = new EntityStats();
		resists = new EntityResists();
	}

	public Entity(ComEntity comEntity)
	{
		statsBaseline = new EntityStats();
		statsCurrent = new EntityStats();
		resists = new EntityResists();
		ComSync(comEntity);
	}

	public virtual void ComSync(ComEntity comEntity)
	{
		ID = comEntity.ID;
		name = comEntity.name;
		Description = comEntity.Description;
		EquippedClassID = comEntity.ClassID;
		EquippedClassXP = comEntity.ClassXP;
		TotalClassRank = comEntity.TotalClassRank;
		level = comEntity.Level;
		XP = comEntity.XP;
		scaledLevel = comEntity.ScaledLevel;
		ExpectedTotalStat = comEntity.ExpectedStat;
		AccessLevel = comEntity.AccessLevel;
		Portrait = comEntity.Portrait;
		Title = comEntity.Title;
		TitleName = comEntity.TitleName;
		baseAsset = comEntity.baseAsset;
		overrideAsset = comEntity.overrideAsset;
		IsSheathed = comEntity.IsSheathed;
		combatRadius = comEntity.combatRadius;
		IsTargetReticleHidden = comEntity.IsTargetReticleHidden;
		areaID = comEntity.areaID;
		cellID = comEntity.cellID;
		position = new Vector3(comEntity.posX, comEntity.posY, comEntity.posZ);
		rotation = Quaternion.Euler(new Vector3(0f, comEntity.rotY, 0f));
		statsBaseline.SetValues(comEntity.statsBaseline);
		statsCurrent.SetValues(comEntity.statsCurrent);
		if (comEntity.resists != null)
		{
			resists.SetValues(comEntity.resists);
		}
		elements = comEntity.elements;
		isPvPFlagged = comEntity.isPvPFlagged;
		teamID = comEntity.teamID;
		visualState = comEntity.state;
		serverState = comEntity.state;
		react = comEntity.react;
		stateEmote = comEntity.stateEmote;
		phaseState = comEntity.phaseState;
		statusMap = comEntity.statusMap;
		spellCastData = comEntity.spellCastData;
		isAFK = comEntity.isAFK;
		isBoss = comEntity.IsBoss;
		SyncServerEffects(comEntity);
		RemoveAllEffects();
		if (comEntity.effects != null)
		{
			foreach (ComEffect effect in comEntity.effects)
			{
				AddEffect(new Effect(effect));
			}
		}
		RemoveAllAuras();
		if (comEntity.auras != null)
		{
			foreach (ComAura aura in comEntity.auras)
			{
				AddAura(aura);
			}
		}
		OnLevelUpdated();
		OnClassRankUpdated();
		OnStatUpdate();
	}

	public Transform GetAttachPoint(Spellfx.AttachSpot spot)
	{
		return spot switch
		{
			Spellfx.AttachSpot.Cast => CastSpot, 
			Spellfx.AttachSpot.Head => HeadSpot, 
			Spellfx.AttachSpot.Hit => HitSpot, 
			_ => wrapper.transform, 
		};
	}

	public int GetRelativeLevel(int areaScaleLevel, int referenceLevel)
	{
		if (areaScaleLevel < Level)
		{
			return (Level - areaScaleLevel + referenceLevel).Clamp(1, Level - 1);
		}
		return referenceLevel;
	}

	public float GetStatModValue(StatMod mod)
	{
		return mod.flat + mod.percent * statsBaseline[mod.stat];
	}

	public void SetClass(int classID)
	{
		EquippedClassID = classID;
		this.ClassUpdated?.Invoke();
		SpellFXContainer.mInstance.HandleImpactFX(this, "Beast_Bamf_Purple", isMe);
	}

	private float GetDerivedStatValue(Stat baseStat, Stat bonusStat)
	{
		float derivedStartingValue = GameCurves.GetDerivedStartingValue(baseStat);
		float num = statsCurrent[bonusStat];
		float statWeight = GameCurves.GetStatWeight(baseStat);
		float extraStatWeight = GameCurves.GetExtraStatWeight(baseStat);
		float dynamicStatAmount = GetDynamicStatAmount(baseStat);
		float dynamicStatAmount2 = GetDynamicStatAmount(bonusStat);
		float num2 = statsCurrent[baseStat] + dynamicStatAmount;
		if (baseStat == Stat.Attack)
		{
			return derivedStartingValue + num2 * statWeight + num + dynamicStatAmount2;
		}
		float num3 = ((type == Type.NPC) ? 0f : GameCurves.GetExpectedStatFromLevel(ScaledLevel));
		float num4 = ((type == Type.NPC) ? 1f : 0.5f);
		float num5 = (float)ExpectedTotalStat * num4;
		float num6 = (num2 - num3) / num5;
		float num7 = ((num6 > 1f) ? statWeight : (num6 * statWeight));
		float num8 = ((num6 > 1f) ? ((num6 - 1f) * extraStatWeight) : 0f);
		return derivedStartingValue + num + dynamicStatAmount2 + num7 + num8;
	}

	public float GetDynamicStatAmount(Stat stat)
	{
		List<StatMod> list = (from mod in effects.SelectMany((Effect e) => e.template.statMods)
			where mod.stat == stat && mod.IsDynamic
			select mod).ToList();
		if (list.Count == 0)
		{
			return 0f;
		}
		float num = 0f;
		float num2 = 1f - HealthPercent;
		foreach (StatMod item in list)
		{
			if (item.useInverseHealth)
			{
				num += GetStatModValue(item) * num2;
			}
		}
		return num;
	}

	protected virtual void OnServerStateChanged(State previousState, State newState)
	{
		this.ServerStateChanged?.Invoke(previousState, newState);
	}

	protected virtual void OnVisualStateChanged()
	{
		if (isMe && visualState == State.Dead)
		{
			AudioManager.Play2DSFX("UI_Death");
		}
		this.VisualStateChanged?.Invoke(this);
	}

	public void CheckPvpState()
	{
		this.PvpStateCheck?.Invoke();
	}

	public abstract bool CanAttack(Entity target);

	public bool CanAttackWithSpell(Entity spellTarget, SpellTemplate spellT)
	{
		foreach (SpellAction startingAction in spellT.GetStartingActions())
		{
			if (startingAction.IsCastable(this))
			{
				if (((CanAttack(spellTarget) && startingAction.isHarmful) || (!CanAttack(spellTarget) && !startingAction.isHarmful)) && startingAction.targetType != CombatSolver.TargetType.Self)
				{
					return true;
				}
				if (startingAction.stopIfReqsMet)
				{
					break;
				}
			}
		}
		return false;
	}

	private void OnTargetDestroyed()
	{
		Target = null;
	}

	private void OnTargetDeath(Entity _)
	{
		Target = null;
	}

	public void SelectedByPlayer()
	{
		if ((bool)SettingsManager.IsTargetHighlightOn)
		{
			Highlight(GetReactionColor(Entities.Instance.me));
		}
		namePlate.SetMyTarget(isMyTarget: true);
	}

	public void DeselectedByPlayer()
	{
		Unhighlight();
		if (namePlate == null)
		{
			Debug.LogException(new Exception("AQ3DException: Entity.DeselectedByPlayer() - nameplate is null."));
		}
		else
		{
			namePlate.SetMyTarget(isMyTarget: false);
		}
	}

	public void OnDeathEvent()
	{
		this.DeathEvent?.Invoke(this);
	}

	protected void OnTargetUpdateEvent(Entity e)
	{
		this.TargetUpdateEvent?.Invoke(e);
	}

	public void OnStatUpdate()
	{
		this.StatUpdateEvent?.Invoke();
	}

	public void OnLevelUpdated()
	{
		this.LevelUpdated?.Invoke();
	}

	public void OnClassRankUpdated()
	{
		this.ClassRankUpdated?.Invoke(EquippedClassRank);
	}

	public void OnDestroy()
	{
		this.Destroyed?.Invoke();
	}

	protected void OnChargeStart(SpellTemplate spellT, float chargeTime)
	{
		this.ChargeStarted?.Invoke(spellT, chargeTime);
	}

	protected void OnChargeEnd()
	{
		this.ChargeEnded?.Invoke();
	}

	protected void OnChannelStart(SpellTemplate spellT, int channelTicks, float channelTime)
	{
		this.ChannelStarted?.Invoke(spellT, channelTicks, channelTime);
	}

	protected void OnChannelEnd()
	{
		this.ChannelEnded?.Invoke();
	}

	protected void OnCastCancel()
	{
		this.CastCanceled?.Invoke();
	}

	protected void OnRespawn()
	{
		PlayerAssetController playerAssetController = assetController as PlayerAssetController;
		if (playerAssetController != null)
		{
			playerAssetController.IsBlinking = true;
			playerAssetController.SetWeaponsMounted(IsSheathed);
		}
		this.RespawnEvent?.Invoke();
	}

	protected void OnCast(SpellTemplate spellT)
	{
		this.Cast?.Invoke(spellT);
	}

	public virtual void Interact()
	{
		Entities.Instance.me.Target = this;
	}

	public void SetLevel(int level, int scaledLevel, int expectedTotalStat)
	{
		this.level = level;
		this.scaledLevel = scaledLevel;
		ExpectedTotalStat = expectedTotalStat;
		OnLevelUpdated();
	}

	public void SetClassXPAndTotalClassRank(int classXP, int totalClassRank)
	{
		EquippedClassXP = classXP;
		TotalClassRank = totalClassRank;
		OnClassRankUpdated();
	}

	public void SetStats(int level, int scaledLevel, int expectedTotalStat, float[] statsBaseline, float[] statsCurrent)
	{
		this.level = level;
		this.scaledLevel = scaledLevel;
		OnLevelUpdated();
		ExpectedTotalStat = expectedTotalStat;
		this.statsBaseline.SetValues(statsBaseline);
		this.statsCurrent.SetValues(statsCurrent);
		OnStatUpdate();
	}

	public virtual void Dispose()
	{
		Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>close for '" + name + "'");
		Destroy();
	}

	protected virtual void Destroy()
	{
		Target = null;
		TargetNode = null;
		if (assetController != null)
		{
			assetController.AssetUpdated -= OnAssetUpdated;
			assetController.OrbCreated -= OnOrbCreated;
		}
		UnityEngine.Object.Destroy(wrapper);
		wrapperTransform = null;
		wrapper = null;
		aaSmoothTrail = null;
		spellSmoothTrail = null;
		RemoveAllAuras();
		OnDestroy();
		DestroyNamePlate();
		if (baseAsset.equips.ContainsKey(EquipItemSlot.Pet))
		{
			UnequipPet();
		}
		SettingsManager.OtherPetsVisibleUpdated -= OnOtherPetsVisibleUpdated;
	}

	private void SetupAssetController()
	{
		if (currentAsset.gender == "N")
		{
			assetController = wrapper.AddComponent<NPCAssetController>();
		}
		else
		{
			assetController = wrapper.AddComponent<PlayerAssetController>();
		}
		assetController.Init(currentAsset, this);
		assetController.SetRandomizedBones(effects.Any((Effect e) => e.template.randomizeBones));
		assetController.ApplyShaderFxAndParticles(CurrentShaderFx, CurrentEffectParticles);
		assetController.SetEntityScaleFactor(CurrentScaleFactor);
		assetController.AssetUpdated += OnAssetUpdated;
		assetController.OrbCreated += OnOrbCreated;
	}

	protected virtual void OnOrbCreated(GameObject asset)
	{
		entitySpots = asset.GetComponentInChildren<EntityAssetData>();
	}

	protected virtual void OnAssetUpdated(GameObject asset)
	{
		if (entitySpots != null)
		{
			EntityAssetData entityAssetData = entitySpots;
			entityAssetData.RunStepLand = (Action)Delegate.Remove(entityAssetData.RunStepLand, new Action(OnRunStepLand));
			EntityAssetData entityAssetData2 = entitySpots;
			entityAssetData2.JumpBegin = (Action)Delegate.Remove(entityAssetData2.JumpBegin, new Action(OnJumpBegin));
			EntityAssetData entityAssetData3 = entitySpots;
			entityAssetData3.JumpLand = (Action)Delegate.Remove(entityAssetData3.JumpLand, new Action(OnJumpLand));
			EntityAssetData entityAssetData4 = entitySpots;
			entityAssetData4.SpellImpact = (Action<int, int, float>)Delegate.Remove(entityAssetData4.SpellImpact, new Action<int, int, float>(HandleSpellImpactFrame));
			EntityAssetData entityAssetData5 = entitySpots;
			entityAssetData5.CastFx = (Action)Delegate.Remove(entityAssetData5.CastFx, new Action(OnSpellCastFXFrame));
			EntityAssetData entityAssetData6 = entitySpots;
			entityAssetData6.MakeProjectile = (Action<int, int, float>)Delegate.Remove(entityAssetData6.MakeProjectile, new Action<int, int, float>(HandleSpellProjectileFrame));
			EntityAssetData entityAssetData7 = entitySpots;
			entityAssetData7.TrailStart = (Action)Delegate.Remove(entityAssetData7.TrailStart, new Action(OnStartTrail));
			EntityAssetData entityAssetData8 = entitySpots;
			entityAssetData8.TrailEnd = (Action)Delegate.Remove(entityAssetData8.TrailEnd, new Action(OnEndTrail));
			EntityAssetData entityAssetData9 = entitySpots;
			entityAssetData9.Sheathe = (Action)Delegate.Remove(entityAssetData9.Sheathe, new Action(OnSheathe));
			EntityAssetData entityAssetData10 = entitySpots;
			entityAssetData10.Unsheathe = (Action)Delegate.Remove(entityAssetData10.Unsheathe, new Action(OnUnsheathe));
		}
		entitycontroller.setAnimPhaseState(phaseState);
		entitycontroller.SetAsset(asset);
		entitySpots = asset.GetComponentInChildren<EntityAssetData>();
		if (entitySpots == null)
		{
			Debug.LogError("EntitySpots not found on Entity: " + name, asset);
		}
		else
		{
			EntityAssetData entityAssetData11 = entitySpots;
			entityAssetData11.RunStepLand = (Action)Delegate.Combine(entityAssetData11.RunStepLand, new Action(OnRunStepLand));
			EntityAssetData entityAssetData12 = entitySpots;
			entityAssetData12.JumpBegin = (Action)Delegate.Combine(entityAssetData12.JumpBegin, new Action(OnJumpBegin));
			EntityAssetData entityAssetData13 = entitySpots;
			entityAssetData13.JumpLand = (Action)Delegate.Combine(entityAssetData13.JumpLand, new Action(OnJumpLand));
			EntityAssetData entityAssetData14 = entitySpots;
			entityAssetData14.SpellImpact = (Action<int, int, float>)Delegate.Combine(entityAssetData14.SpellImpact, new Action<int, int, float>(HandleSpellImpactFrame));
			EntityAssetData entityAssetData15 = entitySpots;
			entityAssetData15.CastFx = (Action)Delegate.Combine(entityAssetData15.CastFx, new Action(OnSpellCastFXFrame));
			EntityAssetData entityAssetData16 = entitySpots;
			entityAssetData16.MakeProjectile = (Action<int, int, float>)Delegate.Combine(entityAssetData16.MakeProjectile, new Action<int, int, float>(HandleSpellProjectileFrame));
			EntityAssetData entityAssetData17 = entitySpots;
			entityAssetData17.TrailStart = (Action)Delegate.Combine(entityAssetData17.TrailStart, new Action(OnStartTrail));
			EntityAssetData entityAssetData18 = entitySpots;
			entityAssetData18.TrailEnd = (Action)Delegate.Combine(entityAssetData18.TrailEnd, new Action(OnEndTrail));
			EntityAssetData entityAssetData19 = entitySpots;
			entityAssetData19.Sheathe = (Action)Delegate.Combine(entityAssetData19.Sheathe, new Action(OnSheathe));
			EntityAssetData entityAssetData20 = entitySpots;
			entityAssetData20.Unsheathe = (Action)Delegate.Combine(entityAssetData20.Unsheathe, new Action(OnUnsheathe));
		}
		if (stateEmote != 0)
		{
			PlayEmote(stateEmote);
		}
		if (isAssetTransforming)
		{
			SpellFXContainer.mInstance.HandleImpactFX(this, "Beast_Bamf_Purple", isMe);
			SFXType sFXType = (isMe ? SFXType.Me : SFXType.NotMe);
			if (entitycontroller.customSFXPlayer != null && entitycontroller.customSFXPlayer.HasTrackWithTag(MixerTrack.TagType.TRAVELFORM))
			{
				PlaySound(MixerTrack.TagType.TRAVELFORM, asset.transform, sFXType);
			}
			else
			{
				AudioManager.PlayCombatSFX((overrideAsset != null) ? "sfx_engine_dragon_roar2" : "SFX_Engine_impact_magic", sFXType, asset.transform);
			}
		}
		BuildWeaponTrails();
		BuildNamePlate();
		PlayDelayedAnimation();
	}

	public bool HasStatus(StatusType statusType)
	{
		return (statusMap & statusType) > StatusType.None;
	}

	public Vector3 GetWeaponDirection()
	{
		Vector3 vector = Game.Instance.cam.transform.InverseTransformPoint(assetController.GetWeaponTipLocation());
		Vector3 vector2 = Game.Instance.cam.transform.InverseTransformPoint(assetController.GetWeaponTipLastLocation());
		return vector - vector2;
	}

	private void BuildWeaponTrails()
	{
		PlayerAssetController playerAssetController = assetController as PlayerAssetController;
		if (!(playerAssetController != null) || !(playerAssetController.weaponGO != null) || currentAsset.WeaponRequired != EquipItemSlot.Weapon)
		{
			return;
		}
		Mesh mesh = null;
		MeshFilter component = playerAssetController.weaponGO.GetComponent<MeshFilter>();
		if (component != null)
		{
			mesh = component.sharedMesh;
		}
		else
		{
			SkinnedMeshRenderer component2 = playerAssetController.weaponGO.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				mesh = component2.sharedMesh;
			}
		}
		if (mesh != null)
		{
			Vector3 vector = new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z + mesh.bounds.extents.z);
			float valueStart = Mathf.Clamp(vector.magnitude, 0.1f, 0.9f);
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("TrailAA"));
			gameObject.transform.SetParent(playerAssetController.weaponGO.transform, worldPositionStays: false);
			gameObject.transform.localPosition = vector * 1.2f;
			aaSmoothTrail = gameObject.GetComponent<SmoothTrail>();
			aaSmoothTrail.TrailData.SizeOverLife = AnimationCurve.EaseInOut(0f, valueStart, 1f, 0f);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("TrailSpell"));
			gameObject2.transform.SetParent(playerAssetController.weaponGO.transform, worldPositionStays: false);
			gameObject2.transform.localPosition = vector * 1.2f;
			spellSmoothTrail = gameObject2.GetComponent<SmoothTrail>();
			spellSmoothTrail.TrailData.SizeOverLife = AnimationCurve.EaseInOut(0f, valueStart, 1f, 0f);
		}
	}

	public virtual void BuildNamePlate()
	{
		if (namePlate != null)
		{
			NamePlates.Destroy(namePlate);
		}
		namePlate = NamePlates.Create(this);
	}

	public void DestroyNamePlate()
	{
		if (namePlate != null)
		{
			NamePlates.Destroy(namePlate);
			namePlate = null;
		}
	}

	protected abstract string GetChargeAnimationBySpell(SpellTemplate spellT, SpellAction spellAction);

	protected abstract List<string> GetSpellAnimations(SpellTemplate spellT, SpellAction spellAction);

	private void OnSheathe()
	{
		(assetController as PlayerAssetController).SetWeaponsMounted(toSheathed: true);
	}

	private void OnUnsheathe()
	{
		(assetController as PlayerAssetController).SetWeaponsMounted(toSheathed: false);
	}

	private void OnRunStepLand()
	{
		PlayFootStepSfx();
	}

	private void OnJumpBegin()
	{
		PlayFootStepSfx();
	}

	private void OnJumpLand()
	{
		AudioManager.Play3DSFX("Player_Jump", isMe ? SFXType.Me : SFXType.NotMe, wrapper.transform);
	}

	private void PlayFootStepSfx()
	{
		AudioManager.Play3DSFX(ArtixRandom.GetElementOfList(stepSounds), isMe ? SFXType.Me : SFXType.NotMe, wrapper.transform);
	}

	public void SetPosRot(float x, float y, float z, float rotY)
	{
		position = new Vector3(x, y, z);
		rotation = Quaternion.Euler(0f, rotY, 0f);
		if (wrapper != null)
		{
			wrapper.transform.position = position;
			wrapper.transform.rotation = rotation;
		}
	}

	public void Teleport(float x, float y, float z, float rotY)
	{
		if (wrapper != null)
		{
			SFXType sFXType = (isMe ? SFXType.Me : SFXType.NotMe);
			AudioManager.PlayCombatSFX("sfx_engine_teleport", sFXType, wrapper.transform);
		}
		SetPosRot(x, y, z, rotY);
		if (moveController != null)
		{
			moveController.Stop();
		}
		if (entitycontroller != null)
		{
			entitycontroller.StopDash();
		}
	}

	public void UpdateCustomization(int haircolor, int skincolor, int eyecolor, int lipcolor, int hair, int braid, int stache, int beard)
	{
		baseAsset.ColorHair = haircolor;
		baseAsset.ColorSkin = skincolor;
		baseAsset.ColorEye = eyecolor;
		baseAsset.ColorLip = lipcolor;
		baseAsset.Hair = hair;
		baseAsset.Braid = braid;
		baseAsset.Stache = stache;
		baseAsset.Beard = beard;
		UpdateAsset(baseAsset);
	}

	public void UpdateGender(string gender)
	{
		baseAsset.gender = gender;
		UpdateAsset(baseAsset);
	}

	public void UpdateAsset(EntityAsset asset)
	{
		baseAsset = asset;
		InterruptKeyframeSpell();
		if (assetController != null && overrideAsset == null)
		{
			isAssetTransforming = false;
			assetController.Init(baseAsset, this);
			assetController.Load();
		}
	}

	public void UpdateWeaponAsset(EquipItem equipItem)
	{
		baseAsset.equips[equipItem.EquipSlot] = equipItem;
		if (assetController != null && overrideAsset == null)
		{
			isAssetTransforming = false;
			assetController.ClearBundle(EquipItemSlot.Weapon);
			assetController.Init(baseAsset, this);
			assetController.LoadWeapon();
		}
	}

	public void UpdateToolAsset(EquipItem equipItem)
	{
		baseAsset.equips[equipItem.EquipSlot] = equipItem;
		if (assetController != null && overrideAsset == null)
		{
			isAssetTransforming = false;
			assetController.ClearBundle(EquipItemSlot.Weapon);
			assetController.Init(baseAsset);
			assetController.LoadTool(equipItem.EquipSlot);
		}
	}

	public void UpdateOverrideAsset(EntityAsset asset)
	{
		overrideAsset = asset;
		InterruptKeyframeSpell();
		if (assetController != null)
		{
			assetController.AssetUpdated -= OnAssetUpdated;
			assetController.OrbCreated -= OnOrbCreated;
			UnityEngine.Object.Destroy(assetController);
			SetupAssetController();
			isAssetTransforming = true;
			assetController.Load();
		}
		this.OverrideAssetUpdated?.Invoke(asset);
	}

	public void BuildController()
	{
		entitycontroller = wrapper.AddComponent<EntityController>();
		entitycontroller.Init(this);
		if (isMe)
		{
			moveController = wrapper.AddComponent<OmniMovementController>();
		}
		else if (type == Type.Player)
		{
			moveController = wrapper.AddComponent<RemoteMovementController>();
		}
		else
		{
			moveController = wrapper.AddComponent<NPCMovementController>();
		}
		SetupAssetController();
		if (baseAsset.equips.ContainsKey(EquipItemSlot.Pet))
		{
			EquipPet(baseAsset.equips[EquipItemSlot.Pet]);
		}
		SettingsManager.OtherPetsVisibleUpdated += OnOtherPetsVisibleUpdated;
		assetController.Load();
	}

	public void SetWrapper(GameObject newWrapper)
	{
		wrapper = newWrapper;
		wrapperTransform = newWrapper.transform;
		int layer = ((type == Type.NPC) ? Layers.NPCS : ((!isMe) ? Layers.OTHER_PLAYERS : Layers.PLAYER_ME));
		newWrapper.SetLayerRecursively(layer);
	}

	public void SetCamera(Camera mainCamera)
	{
		this.mainCamera = mainCamera;
	}

	public void UpdatePortrait(int portrait)
	{
		Portrait = portrait;
		this.PortraitUpdated?.Invoke(Portrait);
	}

	public void UpdateTitle(int badgeID, string titleName)
	{
		Title = badgeID;
		TitleName = titleName;
		this.TitleUpdated?.Invoke(Title, TitleName);
	}

	public void setSpawnPosRot(Vector3 pos, float rotY)
	{
		position = pos;
		rotation = Quaternion.AngleAxis(rotY, Vector3.up);
	}

	public void setSpawnPosRot(Vector3 pos, Quaternion qrotation)
	{
		position = pos;
		rotation = qrotation;
	}

	public void StartCharge(SpellTemplate spellT, SpellAction spellAction, float chargeTime)
	{
		if (chargeTime > 0f)
		{
			ProjectionController.ShowSpellProjections(this, spellT, ProjectionController.TelegraphType.SpellCharge, chargeTime);
		}
		OnChargeStart(spellT, chargeTime);
		ClearCombatEvents();
		if (!(wrapper == null))
		{
			EntityAnimation entityAnimation = EntityAnimations.Get(GetChargeAnimationBySpell(spellT, spellAction));
			entityAnimation.priority = 10;
			entityAnimation.blockMovement = spellT.blockMove;
			PlayAnimation(entityAnimation, 0f, isCancellableByMovement: false, CastSpeed);
			chargeProjectile = SpellFXContainer.mInstance.CreateCastSpotFX(this, spellT, chargeTime);
			if (!string.IsNullOrEmpty(spellT.chargeFX))
			{
				SpellFXContainer.mInstance.CreateEntityFX(this, isMe, spellT.chargeFX, null, spellT, spellAction);
			}
			SFXType sFXType = ((isMe || (Target != null && Target.isMe)) ? SFXType.MyCombat : SFXType.Combat);
			if (isBoss)
			{
				sFXType = SFXType.Boss;
			}
			AudioManager.PlayCombatSFX(spellT.chargeSFX, sFXType, wrapper.transform);
		}
	}

	public void HandleCast(SpellTemplate spellT, SpellAction spellAction, List<Entity> targets)
	{
		if (spellCastData != null)
		{
			if (spellCastData.spellTemplateID == spellT.ID && IsCharging)
			{
				OnChargeEnd();
			}
			else
			{
				ProjectionController.ShowSpellProjections(this, spellT, ProjectionController.TelegraphType.SpellCast, 0f);
			}
			if (IsChanneling)
			{
				OnChannelStart(spellT, spellAction.channel.tickCount, spellCastData.channelTime);
			}
			else
			{
				spellCastData = null;
			}
		}
		OnCast(spellT);
		if (wrapper == null || assetController.IsOrb)
		{
			InterruptKeyframeSpell();
			return;
		}
		QueueSpellAnimations(spellT, spellAction, targets);
		if (serverState != State.Dead)
		{
			visualState = serverState;
		}
		float chargeTime = spellT.GetChargeTime(this);
		if (chargeTime <= 0f)
		{
			chargeProjectile = SpellFXContainer.mInstance.CreateCastSpotFX(this, spellT, chargeTime);
			SFXType sFXType = ((isMe || (Target != null && Target.isMe)) ? SFXType.MyCombat : SFXType.Combat);
			if (isBoss)
			{
				sFXType = SFXType.Boss;
			}
			AudioManager.PlayCombatSFX(spellT.chargeSFX, sFXType, wrapper.transform);
		}
		UpdateLastSpellCast(spellT);
	}

	public void EndChannel()
	{
		spellCastData = null;
		entitycontroller?.CancelAction();
		OnChannelEnd();
	}

	public void QueueSpellAnimations(SpellTemplate spellT, SpellAction spellAction, List<Entity> targets)
	{
		List<string> spellAnimations = GetSpellAnimations(spellT, spellAction);
		if (spellAnimations.Count == 0)
		{
			return;
		}
		spellAnimations = new List<string> { ArtixRandom.GetElementOfList(spellAnimations) };
		float num = 0f;
		for (int i = 0; i < spellAnimations.Count; i++)
		{
			string[] array = spellAnimations[i].Split(',');
			CasterSequencedEvent casterSequencedEvent = new CasterSequencedEvent
			{
				targets = targets,
				spellT = spellT,
				animLabel = array[0],
				time = Time.time + num,
				castSpeed = (spellT.isAA ? AASpeed : SkillSpeed)
			};
			if (array.Length > 1)
			{
				casterSequencedEvent.length = float.Parse(array[1]);
			}
			num += casterSequencedEvent.length;
			if (i == 0)
			{
				ClearCombatEvents();
				HandleCasterSequencedEvent(casterSequencedEvent);
			}
			else
			{
				AddEvent(casterSequencedEvent);
			}
		}
	}

	private bool ShouldSpellPlayImpactFx(KeyframeSpellData spellData, int totalImpacts)
	{
		int num = ((!spellData.spellAction.onlyShowLastImpactFX) ? 1 : totalImpacts);
		if (spellData.spellAction.usesFXImpacts && spellData.fxImpacts == 0 && spellData.currentImpact == num)
		{
			return true;
		}
		if (spellData.spellAction.usesFXImpacts)
		{
			return false;
		}
		if ((spellData.currentImpact == totalImpacts && spellData.spellAction.onlyShowLastImpactFX) || !spellData.spellAction.onlyShowLastImpactFX)
		{
			return true;
		}
		return false;
	}

	protected bool ShouldSpellUseAltAnim(SpellTemplate spellT)
	{
		if (spellT.animsAlt.Length != 0 && lastSpell == spellT)
		{
			return lastSpellRecasts % 2 == 0;
		}
		return false;
	}

	private void UpdateLastSpellCast(SpellTemplate spellT)
	{
		if (lastSpell == spellT)
		{
			lastSpellRecasts++;
		}
		else
		{
			lastSpellRecasts = 0u;
		}
		lastSpell = spellT;
	}

	public void StoreKeyframeSpellData(KeyframeSpellData newSpellData)
	{
		if (entitycontroller == null || entitycontroller.IsDoingPriorityAnimation(newSpellData.spellT))
		{
			newSpellData.wasInterrupted = true;
			HandleSpellImpact(newSpellData, ImpactSource.Animation, newSpellData.currentImpact + 1);
			return;
		}
		InterruptKeyframeSpell();
		keyframeSpellData = newSpellData;
		if (!IsChanneling)
		{
			KeyframeDataTimeoutEvent evt = new KeyframeDataTimeoutEvent
			{
				time = Time.time + 5f,
				frameData = keyframeSpellData
			};
			AddEvent(evt);
		}
	}

	public void ClearKeyframeSpellData()
	{
		keyframeSpellData = null;
	}

	public void InterruptKeyframeSpell()
	{
		if (keyframeSpellData == null)
		{
			return;
		}
		if (!keyframeSpellData.AreImpactsDone())
		{
			keyframeSpellData.wasInterrupted = true;
			entitycontroller.InterruptAnimation();
			if (keyframeSpellData.spellAction.isProjectile)
			{
				HandleSpellProjectile(keyframeSpellData, keyframeSpellData.currentImpact + 1);
			}
			else
			{
				HandleSpellImpact(keyframeSpellData, ImpactSource.Animation, keyframeSpellData.currentImpact + 1);
			}
		}
		ClearKeyframeSpellData();
	}

	public void HandleProjectileImpact(KeyframeSpellData spellData, float statDeltaPercent)
	{
		HandleSpellImpact(spellData, ImpactSource.Projectile, spellData.totalImpacts, statDeltaPercent);
	}

	private void HandleSpellImpactFrame(int animatorStateHash, int totalImpacts, float statDeltaPercent)
	{
		if (animatorStateHash == Animator.StringToHash(entitycontroller.currentAnimName) && keyframeSpellData != null && !keyframeSpellData.spellT.GetFirstAction(keyframeSpellData.caster, keyframeSpellData.entityUpdates).isProjectile)
		{
			HandleSpellImpact(keyframeSpellData, ImpactSource.Animation, totalImpacts, statDeltaPercent);
		}
	}

	public void HandleSpellImpact(KeyframeSpellData spellData, ImpactSource source = ImpactSource.Animation, int totalImpacts = 1, float statDeltaPercent = 1f)
	{
		if (spellData == null)
		{
			return;
		}
		if (source == ImpactSource.FX)
		{
			spellData.fxImpacts++;
			spellData.totalFxImpacts = totalImpacts;
		}
		else
		{
			spellData.currentImpact++;
			spellData.totalImpacts = totalImpacts;
		}
		if ((spellData.currentImpact > spellData.totalImpacts && source != ImpactSource.FX) || (spellData.fxImpacts > spellData.totalFxImpacts && source == ImpactSource.FX))
		{
			return;
		}
		bool flag = (source == ImpactSource.FX && spellData.spellAction.usesFXImpacts) || (source != ImpactSource.FX && !spellData.spellAction.usesFXImpacts);
		bool flag2 = ShouldSpellPlayImpactFx(spellData, totalImpacts);
		bool flag3 = spellData.fxImpacts > 0 || !spellData.spellAction.usesFXImpacts;
		bool flag4 = spellData.currentImpact == 1 && spellData.fxImpacts == 0;
		bool flag5 = spellData.IsFirstImpact();
		if (source != ImpactSource.FX && spellData.spellAction.isAoe)
		{
			PlayAoeFX(spellData);
		}
		List<ComEntityUpdate> actionUpdates = spellData.GetActionUpdates();
		if ((actionUpdates.Count == 0 || actionUpdates.All((ComEntityUpdate u) => u.isCasterUpdate)) && flag2 && !spellData.spellT.isAA && !spellData.spellAction.isProjectile && !spellData.spellAction.isAoe && !spellData.spellAction.makesAura)
		{
			SpellFXContainer.mInstance.CreateEntityFX(spellData.caster, isCasterMe: true, spellData.spellAction.impactFX, spellData, spellData.spellT, spellData.spellAction, hasTarget: false);
		}
		foreach (ComEntityUpdate entityUpdate in actionUpdates)
		{
			Entity entity = entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID);
			if (entity == null)
			{
				continue;
			}
			if (flag4)
			{
				Game.Instance.combat.HandleEntityUpdate(entityUpdate, updateServerState: false, updateVisualState: false, EffectsToApply.Delayed);
			}
			if (flag5)
			{
				entity.ApplyDash(entityUpdate.dash);
			}
			if (!entityUpdate.isCasterUpdate)
			{
				int num = spellData.entityUpdates.FindIndex((ComEntityUpdate u) => u == entityUpdate);
				if (flag2 && spellData.caster.CanTargetWithAction(spellData.spellAction, entity))
				{
					SpellFXContainer.mInstance.HandleImpactFX(entity, spellData.spellAction.impactFX, spellData.caster.isMe, spellData);
				}
				if (flag3)
				{
					entityUpdate.statDeltas.TryGetValue(0, out var value);
					entity.PlayImpactSounds(spellData.caster, spellData.spellAction, entityUpdate.spellResult, value < 0f);
				}
				if (flag)
				{
					float multihitHpDelta = spellData.SolveMultihitDelta(entityUpdate, num, statDeltaPercent);
					entity.PlayImpactDamage(entityUpdate, spellData, multihitHpDelta, spellData.hpDeltaSoFar[num], flag5);
				}
			}
		}
		ChainSpellActions(spellData, ImpactSource.Animation);
	}

	private static void PlayAoeFX(KeyframeSpellData spellData)
	{
		if (spellData?.caster == null)
		{
			return;
		}
		foreach (AoeLocation aoeLocation in spellData.aoeLocations)
		{
			if (aoeLocation.actionId != spellData.spellAction?.ID || aoeLocation.isAura)
			{
				continue;
			}
			AoeShape aoeById = spellData.spellAction.GetAoeById(aoeLocation.aoeId, aoeLocation.isAura);
			Entity entity = Entities.Instance.GetEntity(aoeLocation.aoeSourceType, aoeLocation.aoeSourceId);
			if (entity == null || aoeById == null || string.IsNullOrEmpty(aoeById.aoeFX))
			{
				continue;
			}
			GameObject gameObject;
			if (aoeById.isFixed)
			{
				gameObject = SpellFXContainer.mInstance.CreateFXAtPosition(aoeById.aoeFX, aoeLocation.position + aoeLocation.randomOffset, aoeLocation.rotation, spellData.spellAction);
			}
			else
			{
				Vector3 worldPosition = aoeById.GetPositionBySource(spellData.caster, entity) + (Vector3)aoeLocation.randomOffset;
				Quaternion rotationBySource = aoeById.GetRotationBySource(spellData.caster, entity);
				gameObject = SpellFXContainer.mInstance.CreateFXAtPosition(aoeById.aoeFX, worldPosition, rotationBySource, spellData.spellAction);
			}
			if (gameObject != null && !string.IsNullOrEmpty(aoeById.aoeSFX))
			{
				SFXType sFXType = ((spellData.caster.isMe || entity.isMe) ? SFXType.MyCombat : SFXType.Combat);
				if (spellData.caster.isBoss || entity.isBoss)
				{
					sFXType = SFXType.Boss;
				}
				AudioManager.PlayCombatSFX(aoeById.aoeSFX, sFXType, gameObject.transform);
			}
		}
	}

	private void ChainSpellActions(KeyframeSpellData spellData, ImpactSource source)
	{
		if (spellData.currentImpact == 1 && spellData.fxImpacts == 0 && spellData.spellAction == spellData.spellT.GetFirstAction(spellData.caster, spellData.entityUpdates) && spellData.spellT.GetStartingActions().Contains(spellData.spellAction))
		{
			foreach (SpellAction startingAction in spellData.spellT.GetStartingActions())
			{
				if (startingAction.ID != spellData.spellAction.ID && spellData.entityUpdates.Any((ComEntityUpdate update) => update.spellActionId == startingAction.ID))
				{
					StartNewActionImpact(spellData, startingAction, source);
				}
			}
		}
		if (!spellData.IsFirstImpact())
		{
			return;
		}
		foreach (SpellAction item in spellData.spellAction.chainedActionIDs.Select(spellData.spellT.GetActionById).ToList())
		{
			StartNewActionImpact(spellData, item, source);
		}
	}

	public void StartNewActionImpact(KeyframeSpellData spellData, SpellAction newSpellAction, ImpactSource source)
	{
		if (newSpellAction.delay > 0f)
		{
			KeyframeSpellData frameData = new KeyframeSpellData(spellData);
			SpellActionDelayEvent evt = new SpellActionDelayEvent
			{
				time = Time.time + newSpellAction.delay,
				frameData = frameData,
				spellAction = newSpellAction,
				impactSource = source
			};
			AddEvent(evt);
		}
		else
		{
			CreateNewActionImpact(spellData, newSpellAction, source);
		}
	}

	private void CreateNewActionImpact(KeyframeSpellData spellData, SpellAction newSpellAction, ImpactSource source)
	{
		KeyframeSpellData keyframeSpellData = new KeyframeSpellData(spellData.entityUpdates, spellData.caster, spellData.spellT, spellData.targets, spellData.aoeLocations);
		keyframeSpellData.spellAction = newSpellAction;
		keyframeSpellData.totalImpacts = 1;
		if (keyframeSpellData.spellAction.isProjectile)
		{
			HandleSpellProjectile(keyframeSpellData, keyframeSpellData.currentImpact + 1);
			return;
		}
		int num = ((source == ImpactSource.FX) ? keyframeSpellData.fxImpacts : keyframeSpellData.currentImpact);
		HandleSpellImpact(keyframeSpellData, source, num + 1);
	}

	private void OnSpellCastFXFrame()
	{
		if (keyframeSpellData == null || keyframeSpellData.hasCastFxPlayed)
		{
			return;
		}
		string value = keyframeSpellData.spellT.castSFX;
		if (string.IsNullOrEmpty(value) && keyframeSpellData.targets.Count == 0)
		{
			value = "sfx_engine_whoosh1";
		}
		SFXType sFXType = ((isMe || keyframeSpellData.targets.Any((Entity x) => x?.isMe ?? false)) ? SFXType.MyCombat : SFXType.Combat);
		if (isBoss)
		{
			sFXType = SFXType.Boss;
		}
		AudioManager.PlayCombatSFX(value, sFXType, wrapper?.transform);
		SpellFXContainer.mInstance.CreateEntityFX(this, isMe, keyframeSpellData.spellT.casterFX, keyframeSpellData, keyframeSpellData.spellT, keyframeSpellData.spellAction);
		foreach (Entity target in keyframeSpellData.targets)
		{
			SpellFXContainer.mInstance.CreateEntityFX(target, isMe, keyframeSpellData.spellT.targetFX, keyframeSpellData, keyframeSpellData.spellT, keyframeSpellData.spellAction);
		}
		keyframeSpellData.hasCastFxPlayed = true;
	}

	private void HandleSpellProjectileFrame(int animatorStateHash, int totalImpacts, float statDeltaPercent)
	{
		if (animatorStateHash == Animator.StringToHash(entitycontroller.currentAnimName) && keyframeSpellData != null && keyframeSpellData.spellAction.isProjectile)
		{
			HandleSpellProjectile(keyframeSpellData, totalImpacts, statDeltaPercent);
		}
	}

	private void HandleSpellProjectile(KeyframeSpellData spellData, int totalImpacts, float statDeltaPercent = 1f)
	{
		if (spellData == null || !spellData.spellAction.isProjectile || spellData.AreImpactsDone())
		{
			return;
		}
		spellData.totalImpacts = totalImpacts;
		if (spellData.currentImpact <= totalImpacts)
		{
			List<Entity> currentActionTargets = spellData.GetCurrentActionTargets();
			if (!spellData.hasProjectileLaunched || !spellData.spellAction.usesFXImpacts)
			{
				SpellFXContainer.mInstance.CreateProjectiles(this, currentActionTargets, spellData, chargeProjectile, statDeltaPercent);
			}
			if (!spellData.spellAction.ShouldReuseChargeProjectile)
			{
				UnityEngine.Object.Destroy(chargeProjectile);
			}
			chargeProjectile = null;
			spellData.hasProjectileLaunched = true;
		}
	}

	private void OnStartTrail()
	{
		if (keyframeSpellData != null)
		{
			SmoothTrail smoothTrail = (keyframeSpellData.spellT.isAA ? aaSmoothTrail : spellSmoothTrail);
			if (smoothTrail != null && !smoothTrail.Emit)
			{
				smoothTrail.Emit = true;
			}
		}
	}

	private void OnEndTrail()
	{
		if (aaSmoothTrail != null && aaSmoothTrail.Emit)
		{
			aaSmoothTrail.Emit = false;
		}
		if (spellSmoothTrail != null && spellSmoothTrail.Emit)
		{
			spellSmoothTrail.Emit = false;
		}
	}

	private void AddEvent(SequencedEvent evt)
	{
		queuedCombatEvents.Add(evt);
	}

	public void ClearCombatEvents()
	{
		for (int num = queuedCombatEvents.Count - 1; num >= 0; num--)
		{
			if (queuedCombatEvents[num] is CasterSequencedEvent)
			{
				queuedCombatEvents.RemoveAt(num);
			}
		}
	}

	public void Update()
	{
		if (wrapper != null)
		{
			moveController.IsEnabled = !IsMovementDisabled && !HasStatus(StatusType.NoMovement) && visualState != State.Dead;
		}
		frameEvents.Clear();
		foreach (SequencedEvent queuedCombatEvent in queuedCombatEvents)
		{
			if (queuedCombatEvent.time <= Time.time)
			{
				frameEvents.Add(queuedCombatEvent);
			}
		}
		foreach (SequencedEvent frameEvent in frameEvents)
		{
			queuedCombatEvents.Remove(frameEvent);
			if (!(frameEvent is CasterSequencedEvent e))
			{
				if (!(frameEvent is KeyframeDataTimeoutEvent e2))
				{
					if (frameEvent is SpellActionDelayEvent e3)
					{
						HandleSpellActionDelayEvent(e3);
					}
				}
				else
				{
					HandleKeyframeDataTimeout(e2);
				}
			}
			else
			{
				HandleCasterSequencedEvent(e);
			}
		}
		IdleAI();
	}

	protected virtual void IdleAI()
	{
	}

	private void HandleCasterSequencedEvent(CasterSequencedEvent e)
	{
		HandleCasterAnimation(e.spellT, e.animLabel, e.length, e.targets, e.castSpeed);
	}

	private void HandleKeyframeDataTimeout(KeyframeDataTimeoutEvent e)
	{
		if (keyframeSpellData != null && keyframeSpellData.ID == e.frameData.ID && e.frameData.currentImpact < e.frameData.totalImpacts && e.frameData.fxImpacts < e.frameData.totalFxImpacts)
		{
			if (Entities.Instance.me.AccessLevel >= 30)
			{
				Chat.Notify("Animation keyframe timed out: " + e.frameData.spellT.name + " from " + e.frameData.caster.name);
			}
			HandleSpellImpact(e.frameData);
		}
	}

	private void HandleSpellActionDelayEvent(SpellActionDelayEvent e)
	{
		CreateNewActionImpact(e.frameData, e.spellAction, e.impactSource);
	}

	public void CheckAndUpdateVisualState()
	{
		if (serverState == State.Dead && visualState != State.Dead)
		{
			Die();
			if ((isMe && Entities.Instance.me.DuelOpponentID > 0) || (type == Type.Player && Entities.Instance.me.DuelOpponentID == ID))
			{
				Session.MyPlayerData.duelResult?.Show();
			}
		}
		visualState = serverState;
	}

	protected virtual void Die()
	{
		if (wrapper != null)
		{
			entitycontroller.Die();
			SFXType sFXType = (isMe ? SFXType.MyCombat : SFXType.Combat);
			if (this is Player)
			{
				AudioManager.PlayCombatSFX((baseAsset.gender == "M") ? "sfx_engine_maledeath" : "sfx_engine_femaledeath", sFXType, wrapper.transform);
			}
			else if (this is NPC)
			{
				PlaySound(MixerTrack.TagType.DEATH, wrapper.transform, sFXType);
			}
		}
		if (assetController is PlayerAssetController playerAssetController)
		{
			playerAssetController.IsBlinking = false;
		}
		comboState.ResetCombos();
		OnDeathEvent();
		Target = null;
		TargetNode = null;
		InterruptKeyframeSpell();
	}

	private void PlayImpactDamage(ComEntityUpdate entityUpdate, KeyframeSpellData spellData, float multihitHpDelta, float hpDeltaSoFar, bool isFirstImpact)
	{
		SpellTemplate spellT = spellData.spellT;
		CheckAndUpdateVisualState();
		if (visualState != State.Dead && spellData.spellAction.isHarmful && spellData.spellAction.damageMult != 0f && (entityUpdate.spellResult == CombatSolver.SpellResult.Hit || entityUpdate.spellResult == CombatSolver.SpellResult.Crit) && (assetController is PlayerAssetController || (moveController != null && !moveController.IsMoving())))
		{
			PlayAnimation(EntityAnimations.Get("Hit"));
		}
		PlayImpactCamShakes(spellData, entityUpdate.spellResult);
		bool flag = spellData.caster.effects.Any((Effect effect) => effect.template.showDamageToCaster && effect.caster.isMe);
		bool flag2 = entityUpdate.rawHpDelta < 0f;
		if (this == entities.me || spellData.caster == entities.me || (flag && flag2))
		{
			if (multihitHpDelta != 0f)
			{
				ShowCombatPopupsMultihit(entityUpdate, spellData, multihitHpDelta, hpDeltaSoFar, spellT.isAA, isFirstImpact);
			}
			else
			{
				ShowCombatPopups(entityUpdate, spellT.isAA, isFirstImpact);
			}
		}
	}

	private void PlayImpactCamShakes(KeyframeSpellData spellData, CombatSolver.SpellResult spellResult)
	{
		if (spellData.wasInterrupted)
		{
			return;
		}
		SpellCamShake spellCamShake = null;
		spellCamShake = ((spellData.caster == entities.me) ? spellData.spellAction.camShakes.FirstOrDefault((SpellCamShake shake) => shake.trigger == SpellCamShake.Trigger.Impact && shake.target == SpellCamShake.Target.Caster) : ((!spellData.targets.Contains(entities.me)) ? spellData.spellAction.camShakes.FirstOrDefault((SpellCamShake shake) => shake.trigger == SpellCamShake.Trigger.Impact && shake.target == SpellCamShake.Target.All) : spellData.spellAction.camShakes.FirstOrDefault((SpellCamShake shake) => shake.trigger == SpellCamShake.Trigger.Impact && shake.target == SpellCamShake.Target.Target)));
		if (spellCamShake != null)
		{
			float num = spellCamShake.GetImpactMultiplier(spellResult, spellData.spellT.isAA);
			float num2 = 0.1f;
			if ((spellData.totalImpacts > 1 || spellData.totalFxImpacts > 1) && spellData.IsLastImpact())
			{
				num2 *= 2.5f;
				num *= 2f;
			}
			Game.Instance.camController.PlayCameraShake(spellCamShake.style, num, num2);
		}
	}

	protected void PlayImpactSounds(Entity caster, SpellAction spellAction, CombatSolver.SpellResult spellResult, bool isTakingDamage)
	{
		if (spellResult == CombatSolver.SpellResult.Miss || spellResult == CombatSolver.SpellResult.Dodge)
		{
			if (wrapper != null)
			{
				SFXType sFXType = (isMe ? SFXType.MyCombat : SFXType.Combat);
				AudioManager.PlayCombatSFX("sfx_engine_whoosh1", sFXType, wrapper.transform);
			}
			return;
		}
		string text = spellAction.impactSFX;
		if (spellResult == CombatSolver.SpellResult.Crit && !string.IsNullOrEmpty(spellAction.critSFX))
		{
			text = spellAction.critSFX;
		}
		if (wrapper != null)
		{
			AudioManager.PlayCombatSFX(text, isMe || caster.isMe, wrapper.transform);
		}
		if (isMe && wrapper != null && isTakingDamage)
		{
			if (ArtixRandom.Range(1, 100) > 50)
			{
				AudioManager.PlayCombatSFX((baseAsset.gender == "M") ? "SFX_Engine_MaleHurt" : "SFX_Engine_FemaleHurt", SFXType.Me, wrapper.transform);
			}
			AudioManager.PlayCombatSFX("SFX_Engine_Punch_Boxing_FaceHit9", SFXType.Me, wrapper.transform);
		}
	}

	public void Highlight(Color color)
	{
		if (entitySpots == null)
		{
			return;
		}
		Renderer[] componentsInChildren = entitySpots.GetComponentsInChildren<Renderer>();
		Shader shader = Shader.Find("AE/AQ3D/Selection Outline");
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (renderer.enabled && !(renderer is ParticleSystemRenderer) && renderer.sharedMaterials != null && renderer.sharedMaterial != null && renderer.sharedMaterial.renderQueue < 2400)
			{
				Material[] array2 = new Material[2]
				{
					renderer.sharedMaterial,
					new Material(shader)
				};
				array2[1].SetColor("_OutlineColor", color);
				renderer.sharedMaterials = array2;
			}
		}
	}

	public void Unhighlight()
	{
		if (entitySpots == null)
		{
			return;
		}
		Renderer[] componentsInChildren = entitySpots.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.enabled && !(renderer is ParticleSystemRenderer) && renderer.sharedMaterials != null && renderer.sharedMaterials.Any((Material p) => p != null && p.shader.name == "AE/AQ3D/Selection Outline"))
			{
				renderer.sharedMaterials = renderer.sharedMaterials.Where((Material p) => p.shader.name != "AE/AQ3D/Selection Outline").ToArray();
			}
		}
	}

	public bool InCameraViewAngle(float viewAngle)
	{
		return Mathf.Abs(Vector3.Angle(mainCamera.transform.forward, wrapper.transform.position - mainCamera.transform.position)) < viewAngle;
	}

	public bool InCameraView()
	{
		if (wrapper == null)
		{
			return false;
		}
		if (!IsPointInCameraView(HitSpot.position))
		{
			return IsPointInCameraView(position);
		}
		return true;
	}

	private bool IsPointInCameraView(Vector3 worldPoint)
	{
		Vector3 vector = mainCamera.WorldToViewportPoint(worldPoint);
		if (vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f)
		{
			return vector.y < 1f;
		}
		return false;
	}

	private float GetCloseSpellRange(Entity target)
	{
		int num = Game.Instance.combat.spellIDs[InputAction.Spell_1];
		SpellTemplate spellTemplate = SpellTemplates.Get(num, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(num));
		return Mathf.Clamp(GetActionRange(spellTemplate, spellTemplate.GetFirstCastableAction(this), target), 5f, 8f);
	}

	public float GetActionRange(SpellTemplate spellT, SpellAction spellAction, Entity actionTarget)
	{
		if (!spellAction.isAoe && !spellAction.makesAura)
		{
			return spellAction.range + combatRadius + actionTarget.combatRadius - 0.5f;
		}
		float num = ((spellAction.range > 4f) ? (spellAction.range - 4f) : 0f);
		float num2 = ((actionTarget.type == Type.NPC) ? actionTarget.combatRadius : 0f);
		AoeShape aoeShape = spellAction.GetAllAoes().First();
		Entity aoeSource = spellT.GetAoeSource(spellAction, this, actionTarget);
		float radius = aoeShape.GetRadius(this, aoeSource);
		return num + num2 + radius - 0.5f;
	}

	public List<Entity> FindTabTargets(float maxDistance, bool prioritizePvpPlayers, bool ignoreCombatStatus, SpellTemplate spellT = null, SpellAction spellAction = null)
	{
		List<Entity> list = new List<Entity>();
		Dictionary<Entity, float> allDistances = new Dictionary<Entity, float>();
		List<Entity> list2 = new List<Entity>();
		List<Entity> list3 = new List<Entity>();
		List<Entity> list4 = new List<Entity>();
		foreach (Entity allEntity in entities.AllEntities)
		{
			float num = maxDistance;
			if (spellT != null && spellAction != null)
			{
				num = GetActionRange(spellT, spellAction, allEntity);
			}
			if (allEntity.serverState != State.Dead && CanAttack(allEntity) && allEntity.InCameraView() && HasLineOfSight(allEntity))
			{
				float num2 = (allEntity.position - position).magnitude - allEntity.combatRadius;
				if (num2 <= num)
				{
					list.Add(allEntity);
					allDistances[allEntity] = num2;
				}
			}
		}
		if (prioritizePvpPlayers && (Game.Instance.AreaData.HasPvp || Game.Instance.AreaData.IsPvpLobby || Entities.Instance.me.DuelOpponentID > 0))
		{
			list2 = list.Where((Entity e) => e.type == Type.Player).ToList();
			if (spellAction == null)
			{
				list3 = list2.Where((Entity e) => allDistances[e] <= GetCloseSpellRange(e)).ToList();
				list2 = list2.Except(list3).ToList();
			}
			list2.Sort((Entity p1, Entity p2) => allDistances[p1].CompareTo(allDistances[p2]));
		}
		List<Entity> list5 = list.Except(list3).ToList();
		if (serverState == State.InCombat)
		{
			if (ignoreCombatStatus)
			{
				list4 = list5.Where((Entity e) => e.target == this && allDistances[e] <= GetCloseSpellRange(e)).ToList();
			}
			else
			{
				list4 = list5.Where((Entity e) => e.target == this).ToList();
				list4.Sort((Entity t1, Entity t2) => allDistances[t1].CompareTo(allDistances[t2]));
				list5 = list5.Except(list4).ToList();
				List<Entity> first = list5.Where((Entity e) => e.serverState == State.InCombat).ToList();
				first = first.Concat(list2).Distinct().ToList();
				if (first.Count > 0)
				{
					first.Sort((Entity t1, Entity t2) => allDistances[t1].CompareTo(allDistances[t2]));
					List<Entity> list6 = list3.Concat(list4).Concat(first).ToList();
					if (list6.Count > 1)
					{
						return list6;
					}
				}
			}
		}
		list5.Sort((Entity t1, Entity t2) => allDistances[t1].CompareTo(allDistances[t2]));
		return list3.Concat(list4).Concat(list5).ToList();
	}

	public bool CanTargetWithAction(SpellAction action, Entity target)
	{
		bool num = (action.isHarmful && CanAttack(target)) || (!action.isHarmful && !CanAttack(target));
		bool flag = action.targetType != CombatSolver.TargetType.Self || (action.targetType == CombatSolver.TargetType.Self && target == this);
		bool flag2 = action.targetType != CombatSolver.TargetType.Ally || (action.targetType == CombatSolver.TargetType.Ally && target != this);
		bool flag3 = target == null || (target.react != Reaction.Passive && target.react != Reaction.PassiveAggressive);
		bool flag4 = true;
		if (this != target && this is Player player && target is Player player2 && !action.isHarmful && (player.DuelOpponentID > 0 || player2.DuelOpponentID > 0))
		{
			flag4 = false;
		}
		return num && flag && flag2 && flag3 && flag4;
	}

	public abstract int CalculateDisplayStat(Stat stat);

	public abstract int GetExpectedStatAt(int level);

	public int CalculateDisplayHpDelta(float rawHpDelta)
	{
		if (statsCurrent[Stat.MaxHealth] == 0f)
		{
			return 0;
		}
		return Mathf.CeilToInt(rawHpDelta / statsCurrent[Stat.MaxHealth] * (float)CalculateDisplayStat(Stat.MaxHealth));
	}

	public bool HasLineOfSight(Entity target)
	{
		if (wrapper == null || target.wrapper == null)
		{
			return false;
		}
		Vector3 start = position + LOSOffset;
		if (target.type == Type.NPC)
		{
			bool num = Physics.Linecast(start, target.HitSpot.position, Layers.MASK_LOS);
			bool flag = Physics.Linecast(start, target.HeadSpot.position, Layers.MASK_LOS);
			if (num)
			{
				return !flag;
			}
			return true;
		}
		return !Physics.Linecast(start, target.position + LOSOffset, Layers.MASK_LOS);
	}

	public bool IsFacingTarget(Entity target)
	{
		Vector3 forward = wrapper.transform.forward;
		Vector3 to = target.wrapper.transform.position - wrapper.transform.position;
		return Mathf.Abs(Vector3.Angle(forward, to)) < 90f;
	}

	public Entity GetClosestTarget(float maxDistance, SpellTemplate spellT = null, SpellAction spellAction = null)
	{
		return FindTabTargets(maxDistance, SettingsManager.PrioritizePvpTargets, ignoreCombatStatus: true, spellT, spellAction).FirstOrDefault();
	}

	public void HandleCasterAnimation(SpellTemplate spellT, string anim, float length, List<Entity> targets, float castSpeed)
	{
		if (spellT == null)
		{
			return;
		}
		EntityAnimation entityAnimation = EntityAnimations.Get(anim);
		if (entityAnimation == null)
		{
			return;
		}
		entityAnimation.length = length;
		entityAnimation.priority = spellT.GetAnimationPriority();
		entityAnimation.blockMovement = spellT.blockMove;
		if (type != Type.NPC && !moveController.IsMoving() && targets != null && targets.Count > 0)
		{
			SpellAction firstCastableAction = spellT.GetFirstCastableAction(this);
			if (CanRotate && targets[0] != null && targets[0] != this && firstCastableAction != null && !firstCastableAction.isAoe)
			{
				moveController.LookAt2D(targets[0].wrapperTransform);
			}
		}
		if (PlayAnimation(entityAnimation, length, isCancellableByMovement: false, castSpeed))
		{
			if (isMe && spellT.isAA && ArtixRandom.Range(1, 100) < 10)
			{
				AudioManager.PlayCombatSFX((baseAsset.gender == "M") ? "sfx_engine_malegrunt" : "sfx_engine_femalegrunt", SFXType.Me, wrapper.transform);
			}
			if (this is NPC && ArtixRandom.Range(1, 100) < 35)
			{
				PlaySound(MixerTrack.TagType.ATTACK, wrapper.transform, (targets != null && targets.Contains(entities.me)) ? SFXType.Me : SFXType.NotMe);
			}
		}
	}

	public bool PlayAnimation(EntityAnimation animation, float length = 0f, bool isCancellableByMovement = false, float castSpeed = 1f)
	{
		if (wrapper == null || !moveController.IsEnabled)
		{
			return false;
		}
		return entitycontroller.PlayAnimation(animation, length, isCancellableByMovement, castSpeed);
	}

	public void PlayEmote(StateEmote emoteState)
	{
		AudioManager.StopLoopingTrack(ID);
		Emote byState = Emotes.GetByState(emoteState);
		if (byState != null)
		{
			string[] array = byState.anim.Split(',');
			PlayAnimation(array[UnityEngine.Random.Range(0, array.Length)], 0.25f);
			if (!string.IsNullOrEmpty(byState.sfxName) && wrapper != null)
			{
				string[] array2 = byState.sfxName.Split(',');
				AudioManager.Play3DSFX(position: byState.sfxLocation switch
				{
					SFXLocation.Feet => wrapper.transform, 
					SFXLocation.CastSpot => CastSpot, 
					_ => wrapper.transform, 
				}, name: array2[UnityEngine.Random.Range(0, array.Length)], sfxType: isMe ? SFXType.Me : SFXType.NotMe, playDelay: 0f, toFollow: true, loop: byState.looping, entityID: ID);
			}
		}
		else if (entitycontroller != null && !entitycontroller.IsJumping())
		{
			entitycontroller.CancelAction();
			entitycontroller.ResetAnimation();
		}
	}

	public bool PlayAnimation(string animation, float crossfadeDuration, int layer = -1, float normalizedTime = 0f)
	{
		if (string.IsNullOrEmpty(animation))
		{
			return false;
		}
		EntityAnimation entityAnimation = EntityAnimations.Get(animation);
		entityAnimation.crossfadeSpeed = crossfadeDuration;
		entityAnimation.layer = layer;
		entityAnimation.normalizedTime = normalizedTime;
		return PlayAnimation(entityAnimation, 0f, isCancellableByMovement: true);
	}

	public void ShowCombatPopups(ComEntityUpdate entityUpdate, bool isAA, bool isFirstImpact)
	{
		if (wrapper == null)
		{
			return;
		}
		foreach (KeyValuePair<int, float> statDelta in entityUpdate.statDeltas)
		{
			Stat key = (Stat)statDelta.Key;
			float f = ((key == Stat.Health) ? entityUpdate.rawHpDelta : entityUpdate.statDeltas[statDelta.Key]);
			if (key == Stat.Health || isFirstImpact)
			{
				ShowCombatPopup(entityUpdate.spellResult, key, Mathf.CeilToInt(f), isAA);
			}
		}
		if (entityUpdate.spellResult == CombatSolver.SpellResult.Dodge || entityUpdate.spellResult == CombatSolver.SpellResult.Miss)
		{
			CombatPopup.PlayBattleMessagePopup(wrapper.transform.position, entityUpdate.spellResult.ToString(), isMe);
		}
	}

	public void ShowCombatPopupsMultihit(ComEntityUpdate entityUpdate, KeyframeSpellData spellData, float multihitHpDelta, float multihitDeltaSoFar, bool isAA, bool isFirstImpact)
	{
		if (wrapper == null)
		{
			return;
		}
		foreach (KeyValuePair<int, float> statDelta2 in entityUpdate.statDeltas)
		{
			Stat key = (Stat)statDelta2.Key;
			float rawStatDelta = ((key == Stat.Health) ? multihitHpDelta : entityUpdate.statDeltas[statDelta2.Key]);
			if (key == Stat.Health || isFirstImpact)
			{
				ShowCombatPopup(entityUpdate.spellResult, key, rawStatDelta, isAA);
			}
		}
		if (entityUpdate.spellResult == CombatSolver.SpellResult.Dodge || entityUpdate.spellResult == CombatSolver.SpellResult.Miss)
		{
			CombatPopup.PlayBattleMessagePopup(wrapper.transform.position, entityUpdate.spellResult.ToString(), isMe);
		}
		if (spellData.totalImpacts > 1 && multihitHpDelta != 0f && this != entities.me)
		{
			if (spellData.multihitPopups.TryGetValue(this, out var value))
			{
				value.DestroyPopup();
			}
			int statDelta = CalculateDisplayHpDelta(multihitDeltaSoFar);
			spellData.multihitPopups[this] = CombatPopup.CreateMultihitPopup(wrapper.transform.position, statDelta);
		}
	}

	protected void ShowCombatPopup(CombatSolver.SpellResult spellResult, Stat stat, float rawStatDelta, bool isAA)
	{
		if (!(wrapper == null) && EntityStats.DoesCreatePopup(stat) && (!isMe || spellResult != CombatSolver.SpellResult.Miss))
		{
			int statDelta = CalculateDisplayHpDelta(rawStatDelta);
			switch (spellResult)
			{
			case CombatSolver.SpellResult.Hit:
				CombatPopup.PlayBattleHitPopup(wrapper.transform.position, stat, statDelta, isMe, isAA);
				break;
			case CombatSolver.SpellResult.Crit:
				CombatPopup.PlayBattleCritPopup(wrapper.transform.position, stat, statDelta, isMe, isAA);
				break;
			case CombatSolver.SpellResult.Tick:
				CombatPopup.PlayBattleDotPopup(wrapper.transform.position, stat, statDelta, isMe);
				break;
			default:
				CombatPopup.PlayBattleMessagePopup(wrapper.transform.position, spellResult.ToString(), isMe);
				break;
			case CombatSolver.SpellResult.None:
				break;
			}
		}
	}

	public void ShowLevelUp()
	{
		LevelUp.Show();
	}

	public void ShowLevelUpParticles()
	{
		SpellFXContainer.mInstance.CreateEntityFX(this, isMe, "LevelUpParticles2");
	}

	public void ShowRankUp()
	{
		RankUp.Show();
	}

	public void ShowCapstoneFill()
	{
		CapstoneFill.Show();
	}

	public void ShowRankUpParticles()
	{
		SpellFXContainer.mInstance.CreateEntityFX(this, isMe, "RankUp");
	}

	public void ShowTradeSkillUp(TradeSkillType type)
	{
		TradeSkillUp.Show(type);
	}

	public void ShowTradeSkillUpParticles(TradeSkillType type)
	{
		string entityFX = "";
		switch (type)
		{
		case TradeSkillType.Fishing:
			entityFX = "FishingUp";
			break;
		}
		SpellFXContainer.mInstance.CreateEntityFX(this, isMe, entityFX);
	}

	public void CastCancel()
	{
		if (spellCastData != null)
		{
			Debug.Log("************************ " + name + " canceling " + SpellTemplates.GetBaseSpell(spellCastData.spellTemplateID).name);
			OnCastCancel();
			spellCastData = null;
		}
		if (entitycontroller != null)
		{
			entitycontroller.CancelAction();
		}
		if (chargeProjectile != null)
		{
			UnityEngine.Object.Destroy(chargeProjectile);
		}
	}

	public virtual Color32 GetReactionColor(Entity entity)
	{
		if (visualState == State.Dead)
		{
			return Color.gray;
		}
		switch (react)
		{
		case Reaction.Neutral:
			return InterfaceColors.EntityReaction.Neutral;
		case Reaction.Passive:
			return InterfaceColors.EntityReaction.Inactive;
		case Reaction.PassiveAggressive:
			return InterfaceColors.EntityReaction.Inactive;
		default:
			if (Entities.Instance.me.CanAttack(this))
			{
				return InterfaceColors.EntityReaction.Hostile;
			}
			return InterfaceColors.EntityReaction.Friendly;
		}
	}

	public virtual Color32 GetNamePlateColor()
	{
		if (visualState == State.Dead)
		{
			return Color.gray;
		}
		return GetReactionColor(Entities.Instance.me);
	}

	public void ApplyStatusEffects(ComEntityUpdate entityUpdate, EffectsToApply effectsToApply = EffectsToApply.All)
	{
		UpdateReaction(entityUpdate.react, force: false, entityUpdate.time);
		if (type == Type.NPC && serverState != State.InCombat && Target != null)
		{
			Target = null;
		}
		stateEmote = entityUpdate.stateEmote;
		AudioManager.StopLoopingTrack(ID);
		if (entityUpdate.isStatusUpdate && lastStatusSyncTime <= entityUpdate.time)
		{
			lastStatusSyncTime = entityUpdate.time;
			statusMap = entityUpdate.statusMap;
		}
		if (entityUpdate.isEffectsFinal)
		{
			SyncEffects(entityUpdate);
		}
		else
		{
			ApplyEffectOperations(entityUpdate, effectsToApply);
		}
		if (entityUpdate.IsAssetUpdate && lastAssetUpdateTime <= entityUpdate.time)
		{
			lastAssetUpdateTime = entityUpdate.time;
			UpdateOverrideAsset(entityUpdate.overrideAsset);
		}
	}

	private void SyncEffects(ComEntityUpdate entityUpdate)
	{
		lastEffectSyncTime = entityUpdate.time;
		serverEffectIDs.Clear();
		RemoveAllEffects();
		foreach (ComEffect effect in entityUpdate.effects)
		{
			serverEffectIDs.Add(effect.ID);
			AddEffect(new Effect(effect));
		}
	}

	public void ApplyEffectOperations(ComEntityUpdate entityUpdate, EffectsToApply effectsToApply)
	{
		if (lastEffectSyncTime > entityUpdate.time)
		{
			return;
		}
		for (int i = 0; i < entityUpdate.effects.Count; i++)
		{
			ComEffect comEffect = entityUpdate.effects[i];
			if ((!comEffect.applyImmediately || effectsToApply != EffectsToApply.Delayed) && (comEffect.applyImmediately || effectsToApply != EffectsToApply.Immediate))
			{
				switch (entityUpdate.effectOperations[i])
				{
				case Effect.Operation.Add:
					AddEffect(comEffect);
					break;
				case Effect.Operation.Update:
					UpdateEffect(comEffect);
					break;
				case Effect.Operation.Remove:
					RemoveEffect(comEffect);
					break;
				case Effect.Operation.Immune:
					ShowImmuneEffect(comEffect);
					break;
				}
			}
		}
	}

	public void UpdateServerEffects(ComEntityUpdate entityUpdate)
	{
		for (int i = 0; i < entityUpdate.effects.Count; i++)
		{
			ComEffect comEffect = entityUpdate.effects[i];
			switch (entityUpdate.effectOperations[i])
			{
			case Effect.Operation.Add:
				serverEffectIDs.Add(comEffect.ID);
				break;
			case Effect.Operation.Remove:
				serverEffectIDs.Remove(comEffect.ID);
				break;
			}
		}
	}

	protected void SyncServerEffects(ComEntity comEntity)
	{
		serverEffectIDs.Clear();
		if (comEntity.effects == null)
		{
			return;
		}
		foreach (ComEffect effect in comEntity.effects)
		{
			serverEffectIDs.Add(effect.ID);
		}
	}

	private bool CanAddEffect(Effect effect)
	{
		if (!serverEffectIDs.Contains(effect.ID))
		{
			return false;
		}
		return true;
	}

	private void AddEffect(ComEffect e)
	{
		Effect effect = new Effect(e);
		effect.clientTimeApplied = GameTime.realtimeSinceServerStartup;
		if (CanAddEffect(effect))
		{
			AddEffect(effect);
			if (!effect.template.hideText && wrapper != null && (isMe || entities.GetEntity(e.casterType, e.casterID) == entities.me))
			{
				CombatPopup.PlayMessagePopup(wrapper.transform.position, effect.template.name, isMe);
			}
		}
	}

	protected void AddEffect(Effect effect)
	{
		if (!CanAddEffect(effect))
		{
			return;
		}
		effects.Add(effect);
		this.EffectAdded?.Invoke(effect);
		if (!string.IsNullOrEmpty(effect.template.stackFX))
		{
			SpellFXContainer.mInstance.CreateEntityFX(this, isCasterMe: false, effect.template.stackFX);
		}
		if (assetController != null)
		{
			if (effect.template.randomizeBones)
			{
				assetController.SetRandomizedBones(isRandomized: true);
			}
			assetController.ApplyShaderFxAndParticles(CurrentShaderFx, CurrentEffectParticles);
			assetController.SetEntityScaleFactor(CurrentScaleFactor);
		}
	}

	private void UpdateEffect(ComEffect e)
	{
		using IEnumerator<Effect> enumerator = effects.Where((Effect effect) => effect.ID == e.ID).GetEnumerator();
		if (!enumerator.MoveNext())
		{
			return;
		}
		Effect current = enumerator.Current;
		if ((current.template.type == EffectTemplate.EffectType.Guild || current.template.type == EffectTemplate.EffectType.Perma || current.template.type == EffectTemplate.EffectType.PermaOnline) && !current.template.hideText && wrapper != null && !e.hideText && (isMe || entities.GetEntity(e.casterType, e.casterID) == entities.me))
		{
			CombatPopup.PlayMessagePopup(wrapper.transform.position, current.template.name, isMe, isEffectFading: true);
		}
		if (e.stacks > current.stacks && !string.IsNullOrEmpty(current.template.stackFX))
		{
			SpellFXContainer.mInstance.CreateEntityFX(this, isCasterMe: false, current.template.stackFX);
		}
		current.Update(e);
		if (assetController != null)
		{
			if (current.template.randomizeBones)
			{
				assetController.SetRandomizedBones(isRandomized: true);
			}
			assetController.SetEntityScaleFactor(CurrentScaleFactor);
		}
	}

	private void RemoveAllEffects()
	{
		if (effects != null)
		{
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				RemoveEffect(effects[num]);
			}
			effects.Clear();
		}
	}

	private void RemoveEffect(ComEffect e)
	{
		foreach (Effect effect in effects)
		{
			if (effect.ID == e.ID)
			{
				if (!effect.template.hideText && wrapper != null && !e.hideText && (isMe || entities.GetEntity(e.casterType, e.casterID) == entities.me))
				{
					CombatPopup.PlayMessagePopup(wrapper.transform.position, effect.template.name + " Fades", isMe, isEffectFading: true);
				}
				RemoveEffect(effect);
				break;
			}
		}
	}

	protected void RemoveEffect(Effect effect)
	{
		effect.Destroy();
		effects.Remove(effect);
		this.EffectRemoved?.Invoke(effect);
		if (assetController != null)
		{
			if (effect.template.randomizeBones && !effects.Any((Effect e) => e.template.randomizeBones))
			{
				assetController.SetRandomizedBones(isRandomized: false);
			}
			assetController.ApplyShaderFxAndParticles(CurrentShaderFx, CurrentEffectParticles);
			assetController.SetEntityScaleFactor(CurrentScaleFactor);
		}
	}

	protected void ShowImmuneEffect(ComEffect comEffect)
	{
		Effect effect = new Effect(comEffect);
		if (!effect.template.hideText && !comEffect.hideText && (isMe || entities.GetEntity(comEffect.casterType, comEffect.casterID) == entities.me))
		{
			CombatPopup.PlayMessagePopup(wrapper.transform.position, effect.template.name + " <Immune>", isMe);
		}
	}

	public void UpdateAuras(ComEntityUpdate entityUpdate)
	{
		foreach (AuraOperation auraOperation in entityUpdate.auraOperations)
		{
			switch (auraOperation.operation)
			{
			case Aura.Operation.Add:
				AddAura(auraOperation.comAura);
				break;
			case Aura.Operation.Remove:
				RemoveAura(auraOperation.comAura);
				break;
			}
		}
	}

	private void RemoveAllAuras()
	{
		if (auras != null)
		{
			for (int num = auras.Count - 1; num >= 0; num--)
			{
				RemoveAura(auras[num]);
			}
		}
	}

	private void AddAura(ComAura comAura)
	{
		if (comAura != null)
		{
			AddAura(new Aura(comAura));
		}
	}

	protected void AddAura(Aura aura)
	{
		aura.Init();
		auras.Add(aura);
	}

	private void RemoveAura(ComAura comAura)
	{
		Aura aura = auras.FirstOrDefault((Aura a) => a.ID == comAura.auraId);
		RemoveAura(aura);
	}

	protected void RemoveAura(Aura aura)
	{
		if (aura == null)
		{
			return;
		}
		if (this.AoeDestroyed != null)
		{
			foreach (AoeLocation aoeLocation in aura.aoeLocations)
			{
				this.AoeDestroyed(aoeLocation.ID);
			}
		}
		aura.Destroy();
		auras.Remove(aura);
	}

	public void ApplyStatChanges(ComEntityUpdate entityUpdate, bool shouldUpdateVisualState)
	{
		if (lastStatsSyncTime <= entityUpdate.time)
		{
			if (entityUpdate.statsCurrent == null)
			{
				foreach (KeyValuePair<int, float> statDelta in entityUpdate.statDeltas)
				{
					Stat key = (Stat)statDelta.Key;
					if (!entityUpdate.isCasterUpdate || (key != Stat.Resource && key != Stat.UltCharge))
					{
						statsCurrent[key] = (float)Math.Round(statsCurrent[key] + entityUpdate.statDeltas[statDelta.Key], 2);
					}
				}
			}
			else
			{
				statsCurrent.SetValues(entityUpdate.statsCurrent);
				if (entityUpdate.statsBaseline != null)
				{
					statsBaseline.SetValues(entityUpdate.statsBaseline);
				}
				if (entityUpdate.XP != -1)
				{
					XP = entityUpdate.XP;
				}
				lastStatsSyncTime = entityUpdate.time;
			}
			OnStatUpdate();
		}
		if (lastStateSyncTime <= entityUpdate.time)
		{
			lastStateSyncTime = entityUpdate.time;
			if (shouldUpdateVisualState)
			{
				CheckAndUpdateVisualState();
			}
		}
	}

	public void ApplyDash(ComDash dash)
	{
		if (dash != null)
		{
			entitycontroller.DashToPosition(dash.dashEndPosition, dash.speed);
		}
	}

	public void RefreshReaction()
	{
		UpdateReaction(react, force: true, DateTime.MaxValue);
	}

	protected void UpdateReaction(Reaction newReaction, bool force, DateTime time)
	{
		if ((force || react != newReaction) && lastReactSyncTime <= time)
		{
			lastReactSyncTime = time;
			react = newReaction;
			if (Entities.Instance.me.target == this)
			{
				Unhighlight();
				Highlight(GetReactionColor(Entities.Instance.me));
			}
			this.ReactUpdated?.Invoke(this);
		}
	}

	public void EquipPet(EquipItem pet)
	{
		if (!(wrapper == null) && (isMe || (bool)SettingsManager.OtherPetsVisible))
		{
			if (petGO != null)
			{
				UnityEngine.Object.Destroy(petGO);
			}
			petGO = new GameObject("controller_pet_of_" + ID)
			{
				layer = Layers.NPCS
			};
			SetUpPetInteractions(pet);
		}
	}

	public void UnequipPet()
	{
		UnityEngine.Object.Destroy(petGO);
	}

	private void OnOtherPetsVisibleUpdated(bool visible)
	{
		if (isMe || !baseAsset.equips.ContainsKey(EquipItemSlot.Pet))
		{
			return;
		}
		if (visible)
		{
			if (petGO == null)
			{
				EquipPet(baseAsset.equips[EquipItemSlot.Pet]);
			}
		}
		else
		{
			UnequipPet();
		}
	}

	public void SetNamePlateAFK(bool newAFKStatus)
	{
		isAFK = newAFKStatus;
		this.ChangeAFKStatus?.Invoke(newAFKStatus);
	}

	private void PlaySound(MixerTrack.TagType tag, Transform t, SFXType type)
	{
		if (entitycontroller.customSFXPlayer != null)
		{
			entitycontroller.customSFXPlayer.Play(tag, type, t);
		}
	}

	private void SetUpPetInteractions(EquipItem pet)
	{
		petGO.AddComponent<PetMovementController>().Init(this, pet.AssetName, pet.bundle, pet.animations, pet.ColorR, pet.ColorG, pet.ColorB);
	}

	public float TurnTo(Vector3 targetDirection, float threshold = 1f)
	{
		Vector3 lhs = wrapperTransform.TransformDirection(Vector3.forward);
		float num = Mathf.Acos(Mathf.Clamp(Vector3.Dot(lhs, targetDirection), -1f, 1f)) * 57.29578f;
		if (num > threshold)
		{
			float num2 = num / 180f * 0.65f;
			if (Vector3.Dot(Vector3.Cross(lhs, targetDirection), Vector3.up) < 0f)
			{
				num = 0f - num;
			}
			Quaternion quaternion = wrapperTransform.rotation * Quaternion.Euler(0f, num, 0f);
			iTween.RotateTo(wrapper, iTween.Hash("y", quaternion.eulerAngles.y, "time", num2, "easetype", iTween.EaseType.linear));
		}
		return num;
	}

	public void PlayDelayedAnimation()
	{
		if (delayedAnimation != null)
		{
			PlayAnimation(delayedAnimation, 0f, isCancellableByMovement: true);
			delayedAnimation = null;
		}
	}

	public void NamePlateBubble(string msg)
	{
		namePlate?.ChatBubble(msg);
	}

	public void NamePlateCutsceneStart()
	{
		namePlate?.CutsceneStart();
	}

	public void NamePlateCutsceneStop()
	{
		namePlate?.CutsceneStop();
	}

	public void NamePlateEmojiShow(string msg)
	{
		namePlate?.EmojiShow(msg);
	}
}

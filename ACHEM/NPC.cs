using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;

public class NPC : Entity, ITrackable
{
	public const float IATrigger_Icon_Offset = 0.75f;

	public static readonly string[] StaticAnimations = new string[30]
	{
		"Chillout", "CrossSit", "DeathForever", "DramaticNooo", "Eating", "FishCast", "FishCasting", "Fishing", "FishingCatch", "FishingLoop",
		"Flailing", "IdleInjured", "IdleInjuredEnd", "IdleScared", "Idle-Shivering", "Kneel", "Kneeling", "KneelEnd", "LyingDown", "Meditate",
		"Reeling", "ReelingLoop", "Seated", "SeatedThrone", "Sit", "Sit2", "SitEnd", "SitEnd2", "Sitting", "Sitting2"
	};

	public int SpawnID;

	public int NPCID;

	public List<Vector3> Path;

	public float PathTS;

	public float PathSpeed;

	private List<NPCNodeAnimation> AllNodeAnimations = new List<NPCNodeAnimation>();

	private List<NPCNodeAnimation> NodeAnimations = new List<NPCNodeAnimation>();

	private bool NodeSequentialAnimations;

	private int currentAnimationNode;

	private bool NodeUseRotation;

	public int NodeRotationY;

	public ResponseMovePath.PathType PathType;

	private string currentIdleAnim;

	private float tsIdleAnim;

	private float nextIdleAnimDelay;

	public string sfxAttack;

	public string sfxDeath;

	private GameObject waypointGameObject;

	private static GameObject beamPrefab;

	private static GameObject sparklesPrefab;

	private ParticleSystem[] waypointParticles;

	public bool isInteracting;

	private bool baitState;

	public NPCMovementBehavior moveBehavior;

	public List<int> ApopIDs;

	public Dictionary<string, string> AnimationOverrides;

	public Dictionary<int, List<int>> questObjectiveItems;

	public List<NpcSpell> Spells;

	private Transform parentTform;

	public int NamePlate;

	public float MaxHealthScale;

	public float AttackScale;

	public float ArmorScale;

	public float CritScale;

	public float HasteScale;

	public bool ForceDynamicScaling;

	public int DynamicPlayerCount;

	public Entity summoner;

	private DateTime timestampInteraction = DateTime.UtcNow;

	private NPCNodeAnimation nextIdleAnim;

	private List<NPCNodeAnimation> possibleIdleAnims = new List<NPCNodeAnimation>();

	public bool BaitState
	{
		get
		{
			return baitState;
		}
		set
		{
			if (baitState != value)
			{
				baitState = value;
				if (Spawn != null)
				{
					Spawn.UpdateBaitState();
				}
			}
		}
	}

	public NPCSpawn Spawn { get; private set; }

	public NPCIATrigger npciatrigger { get; private set; }

	public bool IsDynamicScaling
	{
		get
		{
			if (!Game.Instance.AreaData.IsDynamicScaling)
			{
				return ForceDynamicScaling;
			}
			return true;
		}
	}

	public override string ClassIcon
	{
		get
		{
			if (Entities.Instance.me.CanAttack(this))
			{
				if (!isBoss)
				{
					return "icon_class_enemymob";
				}
				return "icon_class_enemyboss";
			}
			return "Ico_Warrior_64";
		}
	}

	public override bool IsNamePlateHidden => NamePlate == 1;

	public override bool CanRotate => moveBehavior != NPCMovementBehavior.Static;

	public override bool CanMove
	{
		get
		{
			if (moveBehavior != NPCMovementBehavior.Static)
			{
				return moveBehavior != NPCMovementBehavior.Stationary;
			}
			return false;
		}
	}

	public Transform TrackedTransform => wrapperTransform;

	public override Transform HeadSpot
	{
		get
		{
			if (Spawn != null && Spawn.HeadSpot != null)
			{
				return Spawn.HeadSpot;
			}
			return base.HeadSpot;
		}
	}

	public override Transform CameraSpot
	{
		get
		{
			if (Spawn != null && Spawn.CameraSpot != null)
			{
				return Spawn.CameraSpot;
			}
			return base.CameraSpot;
		}
	}

	private bool IsInteractive
	{
		get
		{
			if (npciatrigger != null && npciatrigger.gameObject.activeSelf)
			{
				return true;
			}
			if (Spawn != null)
			{
				return Spawn.InteractiveObjects.Any((InteractiveObject io) => io.IsActive);
			}
			return false;
		}
	}

	public bool HasTalkObjective
	{
		get
		{
			foreach (int curQuest in Session.MyPlayerData.CurQuests)
			{
				Quest quest = Quests.Get(curQuest);
				if (quest == null)
				{
					continue;
				}
				foreach (QuestObjective objective in quest.Objectives)
				{
					if (objective.MapID == Game.Instance.AreaData.id && objective.CellID == cellID && objective.Type == QuestObjectiveType.Talk && objective.RefID == SpawnID && !Session.MyPlayerData.IsQuestObjectiveCompleted(quest.ID, objective.ID))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public override bool IsInPvp => false;

	public static event Action AnyNPCStateUpdated;

	public NPC(ComEntity comEntity)
		: base(comEntity)
	{
		if (beamPrefab == null)
		{
			beamPrefab = Resources.Load("Particles/IndicatorBeam_HD") as GameObject;
		}
		if (sparklesPrefab == null)
		{
			sparklesPrefab = Resources.Load("Particles/IndicatorSparkles") as GameObject;
		}
		type = Type.NPC;
		SpawnID = comEntity.SpawnID;
		NPCID = comEntity.NPCID;
		NamePlate = comEntity.NamePlate;
		moveBehavior = comEntity.MoveBehavior;
		ApopIDs = comEntity.ApopIDs;
		AnimationOverrides = comEntity.AnimationOverrides;
		questObjectiveItems = comEntity.questObjectiveItems;
		if (!string.IsNullOrEmpty(comEntity.NodeAnimations))
		{
			PopulateNodeAnimations(comEntity.NodeAnimations);
			NodeSequentialAnimations = comEntity.NodeSequentialAnimations;
		}
		if (comEntity.Path != null)
		{
			Path = comEntity.Path.Select((ComVector3 p) => new Vector3(p.x, p.y, p.z)).ToList();
			PathSpeed = comEntity.PathSpeed;
			PathTS = comEntity.PathTS;
			NodeUseRotation = comEntity.NodeUseRotation;
			NodeRotationY = comEntity.NodeRotationY;
		}
		MaxHealthScale = comEntity.MaxHealthScale;
		AttackScale = comEntity.AttackScale;
		ArmorScale = comEntity.ArmorScale;
		CritScale = comEntity.CritScale;
		HasteScale = comEntity.HasteScale;
		ForceDynamicScaling = comEntity.ForceDynamicScaling;
		DynamicPlayerCount = comEntity.DynamicPlayerCount;
		sfxAttack = comEntity.sfxAttack;
		sfxDeath = comEntity.sfxDeath;
		Spells = comEntity.npcSpells;
		summoner = Entities.Instance.GetEntity(comEntity.summonerType, comEntity.summonerId);
		if (teamID != 0)
		{
			Scoreboard.UpdateTeamNPC(SpawnID, teamID);
		}
	}

	private void PopulateNodeAnimations(string nodeAnimations)
	{
		if (string.IsNullOrEmpty(nodeAnimations))
		{
			return;
		}
		string[] array = nodeAnimations.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			string anim = array2[0];
			if (array2.Length == 1 || !float.TryParse(array2[1], out var result))
			{
				result = 15f;
			}
			AllNodeAnimations.Add(new NPCNodeAnimation(anim, result));
		}
	}

	private void UpdateNodeAnimations()
	{
		NodeAnimations.Clear();
		foreach (NPCNodeAnimation allNodeAnimation in AllNodeAnimations)
		{
			EntityAnimation entityAnimation = EntityAnimations.Get(allNodeAnimation.anim);
			if (entityAnimation != null && entitycontroller.HasAnimatorState(entityAnimation.name, entityAnimation.layer))
			{
				NodeAnimations.Add(allNodeAnimation);
			}
		}
	}

	public void Track(IndicatorFX indicatorFX)
	{
		UpdateState(active: true, indicatorFX);
	}

	public void Untrack()
	{
		UpdateState(active: false);
	}

	private void UpdateState(bool active, IndicatorFX indicatorFX = IndicatorFX.None)
	{
		if (!active && waypointGameObject != null)
		{
			UnityEngine.Object.Destroy(waypointGameObject);
		}
		if (!(waypointGameObject != null) && !(wrapperTransform == null))
		{
			Vector3 vector = wrapperTransform.position;
			switch (indicatorFX)
			{
			case IndicatorFX.Beam:
				waypointGameObject = UnityEngine.Object.Instantiate(beamPrefab, vector, Quaternion.identity);
				break;
			case IndicatorFX.Sparkles:
				waypointGameObject = UnityEngine.Object.Instantiate(sparklesPrefab, vector, Quaternion.identity);
				break;
			case IndicatorFX.BeamAndSparkles:
				waypointGameObject = new GameObject();
				waypointGameObject.transform.SetPositionAndRotation(vector, Quaternion.identity);
				UnityEngine.Object.Instantiate(beamPrefab, vector, Quaternion.identity, waypointGameObject.transform);
				UnityEngine.Object.Instantiate(sparklesPrefab, vector, Quaternion.identity, waypointGameObject.transform);
				break;
			case IndicatorFX.None:
				return;
			}
			waypointParticles = waypointGameObject.GetComponentsInChildren<ParticleSystem>();
			waypointGameObject.transform.SetParent(wrapperTransform);
			waypointGameObject.SetLayerRecursively(Layers.CLICKIES);
			ParticleSystem[] array = waypointParticles;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
		}
	}

	public void Load()
	{
		if (!(base.wrapper != null) && !(parentTform == null) && !(mainCamera == null) && (!(Spawn != null) || Spawn.IsRequirementMet()))
		{
			GameObject gameObject = new GameObject("npc_" + ID);
			gameObject.layer = Layers.OTHER_PLAYERS;
			gameObject.transform.SetParent(parentTform, worldPositionStays: false);
			gameObject.transform.localPosition = position;
			gameObject.transform.localRotation = rotation;
			SetCamera(mainCamera);
			SetWrapper(gameObject);
			BuildController();
			if (Path != null)
			{
				(moveController as NPCMovementController).Move(Path, PathType, PathSpeed, PathTS, NodeUseRotation, NodeRotationY);
			}
			BuildNamePlate();
			BuildNPCPlate();
			NPC.AnyNPCStateUpdated?.Invoke();
		}
	}

	protected override void OnVisualStateChanged()
	{
		if (Spawn != null)
		{
			Spawn.SetState((base.visualState != State.Dead) ? ((byte)1) : ((byte)0));
		}
		base.OnVisualStateChanged();
	}

	public void Init(Transform parentTform, NPCSpawn spawn, Camera mainCamera)
	{
		this.parentTform = parentTform;
		base.mainCamera = mainCamera;
		if (spawn != null)
		{
			Spawn = spawn;
			Spawn.RequirementUpdated += SpawnRequirementUpdated;
			Spawn.SetState((base.serverState != State.Dead) ? ((byte)1) : ((byte)0));
		}
	}

	private void SpawnRequirementUpdated(bool isReqmtMet)
	{
		if (isReqmtMet)
		{
			Load();
		}
		else
		{
			Destroy();
		}
	}

	protected override void OnAssetUpdated(GameObject asset)
	{
		base.OnAssetUpdated(asset);
		Animator component = asset.GetComponent<Animator>();
		SetAnimationOverrides(asset, component);
		if (npciatrigger != null)
		{
			npciatrigger.transform.position = HeadSpot.position + Vector3.up * 0.75f;
			npciatrigger.SetCameraAndFocus(CameraSpot, HeadSpot);
		}
		UpdateNodeAnimations();
	}

	public bool HasAggro()
	{
		return target != null;
	}

	private void BuildNPCPlate()
	{
		if (ApopIDs != null && ApopIDs.Count > 0)
		{
			CreateNPCPlate();
			npciatrigger.Init(ApopIDs, name, CameraSpot, HeadSpot, SpawnID);
		}
	}

	private void CreateNPCPlate()
	{
		if (npciatrigger != null)
		{
			npciatrigger.Click -= NpciatriggerOnClick;
			npciatrigger.OnApopLoad -= NpciatriggerOnApopLoad;
			UnityEngine.Object.Destroy(npciatrigger.gameObject);
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("NPCPlate"));
		gameObject.name = "npcPlate";
		gameObject.transform.position = HeadSpot.position + Vector3.up * 0.75f;
		gameObject.transform.SetParent(base.wrapper.transform);
		npciatrigger = gameObject.GetComponent<NPCIATrigger>();
		npciatrigger.Click += NpciatriggerOnClick;
		npciatrigger.OnApopLoad += NpciatriggerOnApopLoad;
	}

	protected override string GetChargeAnimationBySpell(SpellTemplate spellT, SpellAction spellAction)
	{
		if (entitycontroller.isStandardRig)
		{
			string elementOfList = ArtixRandom.GetElementOfList(spellT.chargeAnims);
			if (!string.IsNullOrEmpty(elementOfList))
			{
				return elementOfList;
			}
			return "CastLoop";
		}
		int num = -1;
		if (spellCastData != null)
		{
			NpcSpell npcSpell = Spells.FirstOrDefault((NpcSpell spell) => spell.ID == spellCastData.npcSpellID);
			if (npcSpell != null)
			{
				num = npcSpell.AnimSlot;
			}
		}
		if (num == -1)
		{
			List<int> list = (from spell in Spells
				where spell.SpellID == spellT.ID
				select spell.AnimSlot).ToList();
			if (list.Count > 0)
			{
				num = ArtixRandom.GetElementOfList(list);
			}
		}
		if (num == -1)
		{
			return "";
		}
		if (entitycontroller.HasOverrideAnimation("D_Cast" + num))
		{
			return "Cast" + num;
		}
		if (entitycontroller.HasOverrideAnimation("D_CastLoop"))
		{
			return "CastLoop";
		}
		return "Idle";
	}

	protected override List<string> GetSpellAnimations(SpellTemplate spellT, SpellAction spellAction)
	{
		if (base.currentAsset == null || spellT == null)
		{
			return new List<string>();
		}
		if (entitycontroller.isStandardRig)
		{
			return new List<string>(ShouldSpellUseAltAnim(spellT) ? spellT.animsAlt : spellT.anims);
		}
		List<int> list = new List<int>();
		if (spellCastData != null)
		{
			NpcSpell npcSpell = Spells.FirstOrDefault((NpcSpell spell) => spell.ID == spellCastData.npcSpellID);
			if (npcSpell != null)
			{
				list.Add(npcSpell.AnimSlot);
			}
		}
		if (list.Count == 0)
		{
			list = (from spell in Spells
				where spell.SpellID == spellT.ID
				select spell.AnimSlot).ToList();
		}
		return list.Select((int slot) => "Skill" + slot).ToList();
	}

	private void NpciatriggerOnClick()
	{
		Interact();
	}

	private void NpciatriggerOnApopLoad()
	{
		if (!(npciatrigger == null) && npciatrigger.CurrentApop != null)
		{
			UpdateTitle(0, npciatrigger.CurrentApop.Subtitle);
		}
	}

	public override void Interact()
	{
		base.Interact();
		if (Game.Instance.TesterMode && UINPCEditor.Instance != null)
		{
			UINPCEditor.Instance.OnNpcClickedInScene(this);
		}
		if (!IsInteractive)
		{
			return;
		}
		if (Entities.Instance.me.visualState == State.InCombat)
		{
			Notification.ShowText("Cannot interact while in combat");
		}
		else if (Entities.Instance.me.visualState == State.Dead)
		{
			Notification.ShowText("Cannot interact while dead");
		}
		else if ((Entities.Instance.me.position - position).magnitude <= 7f + combatRadius)
		{
			if (Spawn != null)
			{
				foreach (InteractiveObject interactiveObject in Spawn.InteractiveObjects)
				{
					if (interactiveObject.IsActive)
					{
						interactiveObject.Trigger(checkRequirements: true);
						return;
					}
				}
			}
			NPCInteract();
			Game.Instance.camController.panToTarget = false;
		}
		else if (npciatrigger != null)
		{
			((ClientMovementController)Entities.Instance.me.moveController).TargetAutoRun(wrapperTransform, 5f, AutoRun.NPCInteract);
		}
	}

	public bool HasActiveApop()
	{
		List<NPCIA> apops = npciatrigger.Apops;
		for (int i = 0; i < apops.Count; i++)
		{
			if (apops[i].IsAvailable())
			{
				return true;
			}
		}
		return false;
	}

	public void NPCInteract()
	{
		if (npciatrigger == null || (!HasActiveApop() && !HasTalkObjective))
		{
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		if ((utcNow - timestampInteraction).TotalMilliseconds > 1000.0)
		{
			timestampInteraction = utcNow;
			Player me = Entities.Instance.me;
			Game.Instance.SendNPCDialogueStartRequest(SpawnID, HasTalkObjective);
			ClientMovementController clientMovementController = me.moveController as ClientMovementController;
			if (clientMovementController != null)
			{
				clientMovementController.BroadcastMovement(forcesync: true);
			}
			me.interactingNPC = this;
		}
	}

	public void OnDialogComplete(Quest quest, NPCIATrigger npciatrigger)
	{
		if (quest.TurnInType == QuestTurnInType.Auto && Session.MyPlayerData.IsQuestComplete(quest.ID) && (quest.Rewards == null || quest.Rewards.Count == 0))
		{
			Game.Instance.SendQuestCompleteRequest(quest.ID);
		}
		npciatrigger.Trigger();
	}

	private void OnTargetInRange(bool isCombatAutoRun)
	{
		if (!isCombatAutoRun)
		{
			Interact();
		}
	}

	public void Move(List<ComVector3> path, ResponseMovePath.PathType pathType, float speed, float ts, string animations, bool useRotation, float finalRotation, bool sequentialAnimations)
	{
		if (!string.IsNullOrEmpty(animations))
		{
			PopulateNodeAnimations(animations);
			currentIdleAnim = "";
			NodeSequentialAnimations = sequentialAnimations;
		}
		List<Vector3> list = new List<Vector3>();
		foreach (ComVector3 item in path)
		{
			list.Add(new Vector3(item.x, item.y, item.z));
		}
		Path = list;
		PathSpeed = speed;
		PathTS = ts;
		PathType = pathType;
		if (moveController != null)
		{
			(moveController as NPCMovementController).Move(Path, pathType, PathSpeed, PathTS, useRotation, finalRotation);
		}
	}

	public override void Dispose()
	{
		if (Spawn != null)
		{
			Spawn.RequirementUpdated -= SpawnRequirementUpdated;
		}
		base.Dispose();
	}

	protected override void Destroy()
	{
		if (npciatrigger != null)
		{
			npciatrigger.Click -= NpciatriggerOnClick;
			npciatrigger.OnApopLoad -= NpciatriggerOnApopLoad;
		}
		base.Destroy();
		if (NPC.AnyNPCStateUpdated != null)
		{
			NPC.AnyNPCStateUpdated();
		}
	}

	private void SetAnimationOverrides(GameObject asset, Animator assetAnimator)
	{
		if (AnimationOverrides == null || AnimationOverrides.Count <= 0)
		{
			return;
		}
		RuntimeAnimatorController runtimeAnimatorController = assetAnimator.runtimeAnimatorController;
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
		if (assetAnimator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController))
		{
			animatorOverrideController = UnityEngine.Object.Instantiate(runtimeAnimatorController) as AnimatorOverrideController;
		}
		else
		{
			animatorOverrideController.runtimeAnimatorController = runtimeAnimatorController;
		}
		foreach (KeyValuePair<string, string> animationOverride in AnimationOverrides)
		{
			string key = animationOverride.Key;
			string value = animationOverride.Value;
			AnimationClip animationClip = null;
			AnimationOverrides component = asset.GetComponent<AnimationOverrides>();
			if (component != null)
			{
				AnimationClip[] animations = component.Animations;
				foreach (AnimationClip animationClip2 in animations)
				{
					if (animationClip2.name == value)
					{
						animationClip = animationClip2;
						break;
					}
				}
			}
			if (animationClip == null)
			{
				AnimationClip[] animations = runtimeAnimatorController.animationClips;
				foreach (AnimationClip animationClip3 in animations)
				{
					if (animationClip3.name == value)
					{
						animationClip = animationClip3;
						break;
					}
				}
			}
			if (animationClip != null)
			{
				animatorOverrideController[key] = animationClip;
			}
		}
		assetAnimator.runtimeAnimatorController = animatorOverrideController;
	}

	protected override void IdleAI()
	{
		base.IdleAI();
		if (isInteracting)
		{
			return;
		}
		NPCMovementController nPCMovementController = moveController as NPCMovementController;
		if (assetController == null || !assetController.IsAssetLoadComplete || NodeAnimations == null || NodeAnimations.Count <= 0 || nPCMovementController == null || nPCMovementController.IsMoving() || base.visualState != 0 || GameTime.realtimeSinceServerStartup < tsIdleAnim + nextIdleAnimDelay)
		{
			return;
		}
		nextIdleAnim = null;
		if (NodeSequentialAnimations)
		{
			int index = currentAnimationNode + 1 % NodeAnimations.Count;
			nextIdleAnim = NodeAnimations[index];
		}
		else
		{
			possibleIdleAnims.Clear();
			foreach (NPCNodeAnimation nodeAnimation in NodeAnimations)
			{
				if (nodeAnimation.anim != currentIdleAnim)
				{
					possibleIdleAnims.Add(nodeAnimation);
				}
			}
			if (possibleIdleAnims.Count > 0)
			{
				nextIdleAnim = ArtixRandom.GetElementOfList(possibleIdleAnims);
			}
		}
		if (nextIdleAnim != null && PlayAnimation(nextIdleAnim.anim, 0.35f))
		{
			currentIdleAnim = nextIdleAnim.anim;
			nextIdleAnimDelay = nextIdleAnim.nextAnimDelay;
			tsIdleAnim = GameTime.realtimeSinceServerStartup;
		}
	}

	public override bool CanAttack(Entity target)
	{
		if (target == null || target == this)
		{
			return false;
		}
		if (target.react == Reaction.Passive || target.react == Reaction.Neutral || target.react == Reaction.PassiveAggressive || react == Reaction.Passive || react == Reaction.Neutral || react == Reaction.PassiveAggressive)
		{
			return false;
		}
		if (react == Reaction.AgroAll)
		{
			return true;
		}
		if (react == Reaction.AgroOtherKind)
		{
			if (target.type == Type.NPC)
			{
				return ((NPC)target).NPCID != NPCID;
			}
			return true;
		}
		if (teamID > 0 && target.teamID > 0)
		{
			return teamID != target.teamID;
		}
		return react != target.react;
	}

	public override int GetExpectedStatAt(int level)
	{
		return Mathf.CeilToInt(GameCurves.GetNPCStatCurve(level));
	}

	public override int CalculateDisplayStat(Stat stat)
	{
		switch (stat)
		{
		case Stat.MaxResource:
			return Mathf.CeilToInt(statsCurrent[stat]);
		case Stat.Resource:
			return Mathf.CeilToInt(Mathf.Clamp(statsCurrent[stat], 0f, CalculateDisplayStat(Stat.MaxResource)));
		case Stat.Health:
			return Mathf.CeilToInt((float)CalculateDisplayStat(Stat.MaxHealth) * base.HealthPercent);
		default:
		{
			float statMultiplier = GetStatMultiplier(stat);
			float num = GameCurves.GetNPCStatCurve(base.ScaledLevel) * statMultiplier;
			float num2 = statsCurrent[stat] - num;
			float num3 = GameCurves.GetNPCStatCurve(base.DisplayLevel) / GameCurves.GetNPCStatCurve(base.ScaledLevel);
			return Mathf.CeilToInt(GameCurves.GetNPCStatCurve(base.DisplayLevel) * statMultiplier + num2 * num3);
		}
		}
	}

	private float GetStatMultiplier(Stat stat)
	{
		switch (stat)
		{
		case Stat.Health:
		case Stat.MaxHealth:
			return MaxHealthScale;
		case Stat.Attack:
			return AttackScale;
		case Stat.Crit:
			return CritScale;
		case Stat.Armor:
			return ArmorScale;
		case Stat.Haste:
			return HasteScale;
		case Stat.Resource:
		case Stat.MaxResource:
		case Stat.CritPower:
		case Stat.RunSpeed:
		case Stat.DamageBonus:
		case Stat.DefenseBonus:
		case Stat.DodgeBonus:
		case Stat.CritBonus:
		case Stat.CastSpeedBonus:
		case Stat.UltCharge:
		case Stat.AoeRadiusBonus:
			return 1f;
		case Stat.Evasion:
		case Stat.XPBoost:
		case Stat.GoldBoost:
		case Stat.ClassXPBoost:
			return 0f;
		default:
			return 1f;
		}
	}

	public void HotBuildStats(float maxHealthMultiplier)
	{
		float num = statsCurrent[Stat.MaxHealth];
		statsCurrent[Stat.MaxHealth] = statsBaseline[Stat.MaxHealth] * maxHealthMultiplier;
		statsCurrent[Stat.Health] *= statsCurrent[Stat.MaxHealth] / num;
		OnStatUpdate();
	}

	public void InteractWithPlayer(float angle)
	{
		Transform cameraSpot = CameraSpot;
		if (cameraSpot != null)
		{
			cameraSpot.RotateAround(wrapperTransform.position, Vector3.up, angle);
		}
		Quest TurninQuest = npciatrigger.Apops.FirstOrDefault((NPCIA p) => p.IsAvailable() && p.TurnInQuest != null)?.TurnInQuest;
		Quest quest = TurninQuest;
		if (quest != null && quest.EndDialogID > 0)
		{
			DialogueSlotManager.Show(TurninQuest.EndDialogID, delegate
			{
				OnDialogComplete(TurninQuest, npciatrigger);
			});
		}
		else
		{
			npciatrigger.Trigger();
			if (npciatrigger.Apops.Count == 1 && npciatrigger.Apops[0].parent == null)
			{
				AudioManager.PlayNpcGreeting(Entities.Instance?.GetNpcBySpawnId(SpawnID));
			}
		}
		if (cameraSpot != null)
		{
			cameraSpot.RotateAround(wrapperTransform.position, Vector3.up, 0f - angle);
		}
	}

	public NpcSpell GetCurrentNpcSpell()
	{
		if (spellCastData == null)
		{
			return null;
		}
		return Spells.FirstOrDefault((NpcSpell spell) => spell.ID == spellCastData.npcSpellID);
	}

	protected override void Die()
	{
		base.Die();
		NPC.AnyNPCStateUpdated?.Invoke();
	}
}

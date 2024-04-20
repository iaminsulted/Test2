using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.NetworkClient.CommClasses;
using StatCurves;
using UnityEngine;

public class CombatSolver : MonoBehaviour
{
	public enum Element
	{
		None,
		Fire,
		Energy,
		Nature,
		Ice,
		Water,
		Light,
		Darkness,
		Dragon,
		Heal,
		Undead
	}

	public enum AreaTarget
	{
		All,
		LowestHealth,
		Random
	}

	public enum DamageModel
	{
		None,
		Damage,
		Heal,
		Resource,
		Kill,
		DoT,
		HoT,
		Berserk,
		PercentHpBonus,
		PercentHp,
		PercentCurrentHp,
		CasterHealByTargetHp,
		BerserkHeal,
		UltChargePercent
	}

	public enum SpellResult
	{
		None,
		Hit,
		Crit,
		Tick,
		Miss,
		Dodge
	}

	public enum DamageSource
	{
		All,
		Spell,
		Dot
	}

	public enum TargetType
	{
		Hostile,
		Friendly,
		Self,
		Ally
	}

	public enum MachineTargetType
	{
		Hostile,
		Friendly,
		Both
	}

	public enum ProjectileType
	{
		None,
		Caster,
		Meteor,
		Reverse
	}

	public const float Max_Target_Distance = 30f;

	public const float Target_Out_Of_Range_Padding = 11f;

	public const float GCDBaseLength = 1f;

	public const float Spell_Queue_Window = 0.5f;

	public const float Auto_Attack_Response_Delay = 0.2f;

	private static int _ID = 1;

	private Dictionary<int, RequestCombat> requestMap;

	private Entities entities;

	private bool isAA;

	private int aaRequestID;

	private float aaResponseDelay;

	private Coroutine aaRoutine;

	private float lastGCDtrigger = -999f;

	private float lastGCDLength;

	private Dictionary<int, float> spellTimestamps = new Dictionary<int, float>();

	private Dictionary<int, float> spellCooldowns = new Dictionary<int, float>();

	private InventoryItem queuedItem;

	private SpellTemplate _queuedSpell;

	private SpellTemplate queuedSpellAfterUnsheathe;

	public Dictionary<InputAction, int> spellIDs;

	private Dictionary<InputAction, int> actionMap = new Dictionary<InputAction, int>
	{
		{
			InputAction.Spell_1,
			0
		},
		{
			InputAction.Spell_2,
			1
		},
		{
			InputAction.Spell_3,
			2
		},
		{
			InputAction.Spell_4,
			3
		},
		{
			InputAction.Spell_5,
			4
		}
	};

	private OmniMovementController moveController;

	private int pendingSpellID = -1;

	private SpellTemplate queuedSpell
	{
		get
		{
			return _queuedSpell;
		}
		set
		{
			if (_queuedSpell != value)
			{
				_queuedSpell = value;
				if (this.QueuedSpellChange != null)
				{
					int obj = _queuedSpell?.ID ?? (-1);
					this.QueuedSpellChange(obj);
				}
			}
		}
	}

	public bool IsAA
	{
		get
		{
			return isAA;
		}
		set
		{
			if (isAA != value)
			{
				isAA = value;
				OnAAStateChanged(isAA);
				if (aaRoutine != null)
				{
					StopCoroutine(aaRoutine);
				}
				if (isAA)
				{
					aaRoutine = StartCoroutine(AutoAttackRoutine());
				}
				else
				{
					entities.me.ClearCombatEvents();
				}
			}
		}
	}

	public event Action<SpellTemplate> CastRequest;

	public event Action<Dictionary<InputAction, int>> SpellsUpdated;

	public event Action<bool> AAStateChanged;

	public event Action<float> GCDTrigger;

	public event Action<int, float> CDTrigger;

	public event Action<int, float> ResetSpellCD;

	public event Action<int> QueuedSpellChange;

	private void Update()
	{
		TryCastQueuedSpell();
	}

	protected void OnAAStateChanged(bool isEnabled)
	{
		this.AAStateChanged?.Invoke(isEnabled);
	}

	public void SetSkills(List<int> spells)
	{
		spellIDs = new Dictionary<InputAction, int>();
		foreach (InputAction key in actionMap.Keys)
		{
			spellIDs.Add(key, spells[actionMap[key]]);
		}
		this.SpellsUpdated?.Invoke(spellIDs);
	}

	public void SetGCDTrigger(float timestamp, float newGCDLength)
	{
		if (newGCDLength > 0f)
		{
			lastGCDLength = newGCDLength;
			lastGCDtrigger = timestamp;
			this.GCDTrigger?.Invoke(newGCDLength);
		}
	}

	public void SetSpellTimestamp(int spellId, float timestamp, float cooldown)
	{
		if (spellTimestamps.ContainsKey(spellId))
		{
			spellTimestamps[spellId] = timestamp;
		}
		else
		{
			spellTimestamps.Add(spellId, timestamp);
		}
		if (spellCooldowns.ContainsKey(spellId))
		{
			spellCooldowns[spellId] = cooldown;
		}
		else
		{
			spellCooldowns.Add(spellId, cooldown);
		}
		if (cooldown > 0f)
		{
			this.CDTrigger?.Invoke(spellId, cooldown);
		}
	}

	public float GetSpellTimestamp(SpellTemplate spellT)
	{
		if (!spellTimestamps.ContainsKey(spellT.ID))
		{
			return -1f;
		}
		return spellTimestamps[spellT.ID];
	}

	public float GetRemainingCooldown(SpellTemplate spellT)
	{
		float num = GameTime.realtimeSinceServerStartup - GetSpellTimestamp(spellT);
		float spellCooldown = GetSpellCooldown(spellT);
		if (spellCooldown == -1f)
		{
			return 0f;
		}
		return Mathf.Clamp(spellCooldown - num, 0f, spellCooldown);
	}

	public float GetSpellCooldown(SpellTemplate spellT)
	{
		if (!spellCooldowns.ContainsKey(spellT.ID))
		{
			return -1f;
		}
		return spellCooldowns[spellT.ID];
	}

	public bool IsSpellCooldownOver(SpellTemplate spellT)
	{
		if (!(GetSpellTimestamp(spellT) <= 0f))
		{
			return GetSpellTimestamp(spellT) + GetSpellCooldown(spellT) <= GameTime.realtimeSinceServerStartup;
		}
		return true;
	}

	public bool IsGcdOver()
	{
		if (!(lastGCDLength <= 0f))
		{
			return lastGCDLength + lastGCDtrigger <= GameTime.realtimeSinceServerStartup;
		}
		return true;
	}

	public bool IsCoreSpell(int spellID)
	{
		if (spellIDs.Values.Contains(spellID))
		{
			return true;
		}
		return false;
	}

	public void Init()
	{
		requestMap = new Dictionary<int, RequestCombat>();
		entities = Entities.Instance;
	}

	public RequestCombat GetRequest(byte combatCommand)
	{
		RequestCombat requestCombat = new RequestCombat(combatCommand);
		requestCombat.ID = _ID++;
		requestMap.Add(requestCombat.ID, requestCombat);
		return requestCombat;
	}

	public void CloseRequestByID(int requestID)
	{
		requestMap.Remove(requestID);
	}

	public void TargetNextEnemy()
	{
		List<Entity> list = entities.me.FindTabTargets(30f, SettingsManager.PrioritizePvpTargets, ignoreCombatStatus: false);
		if (list.Count != 0)
		{
			if (entities.me.Target == null)
			{
				entities.me.Target = list[0];
			}
			else
			{
				entities.me.Target = list[(list.IndexOf(entities.me.Target) + 1) % list.Count];
			}
		}
	}

	public void TargetPreviousEnemy()
	{
		List<Entity> list = entities.me.FindTabTargets(30f, SettingsManager.PrioritizePvpTargets, ignoreCombatStatus: false);
		if (list.Count != 0)
		{
			if (entities.me.Target == null)
			{
				entities.me.Target = list[0];
			}
			else
			{
				entities.me.Target = list[ArtixMath.Mod(list.IndexOf(entities.me.Target) - 1, list.Count)];
			}
		}
	}

	public void TargetClosestEnemy()
	{
		Entity closestTarget = entities.me.GetClosestTarget(30f);
		if (closestTarget != null)
		{
			entities.me.Target = closestTarget;
		}
	}

	public void TryCastSpell(InputAction action)
	{
		SpellTemplate mySpellTemplate = GetMySpellTemplate(action);
		if (mySpellTemplate != null)
		{
			TryCastSpell(mySpellTemplate);
			Game.Instance.camController.panToTarget = true;
		}
	}

	public SpellTemplate GetMySpellTemplate(InputAction action)
	{
		switch (action)
		{
		case InputAction.Cross_Skill:
		{
			int currentCrossSkill = Session.MyPlayerData.CurrentCrossSkill;
			if (currentCrossSkill > 0)
			{
				return SpellTemplates.Get(currentCrossSkill, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(currentCrossSkill));
			}
			break;
		}
		case InputAction.CustomAction_1:
		case InputAction.CustomAction_2:
		case InputAction.CustomAction_3:
		case InputAction.CustomAction_4:
			return UIGame.Instance.ActionBar.GetPvpAction(action);
		default:
		{
			if (spellIDs.TryGetValue(action, out var value))
			{
				return SpellTemplates.Get(value, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(value));
			}
			break;
		}
		}
		return null;
	}

	public void RemoveSheathingEffects()
	{
		entities.me.effects.Where((Effect x) => x.template.ID == 159).ToList().ForEach(delegate(Effect sheatheEffect)
		{
			Game.Instance.SendRequestEffectRemove(sheatheEffect.ID);
		});
	}

	public void TryCastSpell(SpellTemplate spellT)
	{
		if (spellT == null || entities.me.serverState == Entity.State.Dead)
		{
			return;
		}
		Effect effect = entities.me.effects.FirstOrDefault((Effect p) => p.template.isTravelForm);
		if (effect != null)
		{
			Game.Instance.SendRequestEffectRemove(effect.ID);
			return;
		}
		SpellAction firstCastableAction = spellT.GetFirstCastableAction(entities.me);
		if (firstCastableAction == null)
		{
			TryQueueingSpell(spellT);
		}
		else
		{
			if (!CanPlayerCastSpell(spellT))
			{
				return;
			}
			bool flag = firstCastableAction.makesAura && (firstCastableAction.targetType == TargetType.Self || (firstCastableAction.targetType == TargetType.Friendly && (entities.me.Target == null || entities.me.CanAttack(entities.me.Target))));
			bool flag2 = firstCastableAction.isAoe || flag;
			bool flag3 = ((bool)SettingsManager.AreaSpellCastAutorun && flag2) || ((bool)SettingsManager.SingleSpellCastAutorun && !flag2);
			bool num = firstCastableAction.isHarmful && (entities.me.Target == null || !entities.me.CanAttack(entities.me.Target));
			bool flag4 = firstCastableAction.isHarmful && entities.me.Target != null && entities.me.CanAttack(entities.me.Target);
			if (num)
			{
				entities.me.Target = entities.me.GetClosestTarget(30f);
			}
			else if (flag4)
			{
				float actionRange = entities.me.GetActionRange(spellT, firstCastableAction, entities.me.Target);
				if ((entities.me.TargetTransform.position - entities.me.position).magnitude >= actionRange + 11f)
				{
					entities.me.Target = entities.me.GetClosestTarget(30f);
				}
			}
			if (entities.me.Target != null && entities.me.Target.wrapper != null && entities.me.CanAttackWithSpell(entities.me.Target, spellT) && (!spellT.isUlt || entities.me.statsCurrent[Stat.UltCharge] >= 1000f) && IsSpellCooldownOver(spellT))
			{
				float num2 = entities.me.GetActionRange(spellT, firstCastableAction, entities.me.Target) - 0.25f;
				if ((entities.me.TargetTransform.position - entities.me.position).magnitude > num2)
				{
					if (flag3)
					{
						AutoRunToCast(spellT.ID, entities.me.Target.wrapper.transform, num2);
						return;
					}
					if (!firstCastableAction.isAoe)
					{
						Notification.ShowSpellWarning("Target is out of range", 0.75f);
						return;
					}
				}
			}
			PlayerHasValidTarget(spellT, out var target);
			if (entities.me.Target == null && target != Entities.Instance.me)
			{
				bool flag5 = spellT.requireTarget || spellT.isUlt || spellT.GetCooldown() >= 20f || !SettingsManager.CanAttackWithoutTarget || entities.me.IsInPvp;
				if (!firstCastableAction.isAoe && !firstCastableAction.makesAura && flag5)
				{
					Notification.ShowSpellWarning("Target required", 0.75f);
					return;
				}
			}
			if (!spellT.isAA || !IsAA)
			{
				CastSpell(spellT.ID);
			}
		}
	}

	public void SetMoveController(OmniMovementController controller)
	{
		if (moveController != null)
		{
			moveController.TargetInRange -= OnTargetInRange;
		}
		moveController = controller;
		moveController.TargetInRange += OnTargetInRange;
	}

	private void AutoRunToCast(int spellID, Transform target, float range)
	{
		if (moveController != null)
		{
			pendingSpellID = spellID;
			moveController.TargetAutoRun(target, range, AutoRun.Combat);
		}
	}

	private void OnTargetInRange(AutoRun autoRun)
	{
		switch (autoRun)
		{
		case AutoRun.Combat:
			CastSpell(pendingSpellID);
			break;
		case AutoRun.NPCInteract:
			if (entities.me.Target != null)
			{
				entities.me.Target.Interact();
			}
			break;
		case AutoRun.ResourceInteract:
			if (entities.me.TargetNode != null)
			{
				entities.me.TargetNode.Interact();
			}
			break;
		}
	}

	private void CastSpell(int spellID)
	{
		Debug.Log("CastSpell(" + spellID + ")");
		SpellTemplate spellTemplate = SpellTemplates.Get(spellID, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(spellID));
		if (!CanPlayerCastSpell(spellTemplate))
		{
			return;
		}
		RequestCombat request = GetRequest(1);
		if (!PlayerHasValidTarget(spellTemplate, out var target))
		{
			return;
		}
		if (target != null)
		{
			request.targetType = target.type;
			request.targetID = target.ID;
		}
		if (moveController != null)
		{
			if (target != null && target != entities.me && !moveController.IsMoving())
			{
				SpellAction firstCastableAction = spellTemplate.GetFirstCastableAction(entities.me);
				if ((!firstCastableAction.isAoe && !firstCastableAction.makesAura) || (bool)SettingsManager.AoeFaceTarget)
				{
					moveController.LookAt2D(target.wrapper.transform);
				}
				else if (!SettingsManager.AoeFaceTarget)
				{
					OmniMovementController omniMovementController = entities.me.moveController as OmniMovementController;
					if (omniMovementController != null)
					{
						omniMovementController.SnapToCameraOrientation();
					}
				}
			}
			moveController.BroadcastMovement(forcesync: true);
		}
		request.rct = RequestCombatType.Cast;
		request.spellTemplateID = spellTemplate.ID;
		AEC.getInstance().sendRequest(request);
		queuedSpell = null;
		queuedItem = null;
		aaResponseDelay = 0.2f;
		if (spellTemplate.isAA)
		{
			aaRequestID = request.ID;
		}
		this.CastRequest?.Invoke(spellTemplate);
	}

	private void TryQueueingSpell(SpellTemplate spellT, InventoryItem item = null)
	{
		float num = lastGCDtrigger + lastGCDLength - GameTime.realtimeSinceServerStartup;
		float num2 = GetSpellTimestamp(spellT) + GetSpellCooldown(spellT) - GameTime.realtimeSinceServerStartup;
		bool flag = num > 0f && num < 0.5f && IsSpellCooldownOver(spellT);
		bool flag2 = num2 > 0f && num2 < 0.5f;
		bool flag3 = !spellT.isAA && (flag || flag2) && !Entities.Instance.me.IsChanneling;
		queuedSpell = (flag3 ? spellT : null);
		queuedItem = (flag3 ? item : null);
	}

	private void TryCastQueuedSpell()
	{
		if (queuedSpell != null && IsSpellCooldownOver(queuedSpell) && IsGcdOver())
		{
			if (queuedItem != null)
			{
				CastItem(queuedItem);
			}
			else
			{
				TryCastSpell(queuedSpell);
			}
		}
	}

	public bool CastItem(InventoryItem iItem)
	{
		if (iItem.SpellID == 0)
		{
			Debug.Log("This item is not useable!");
			return false;
		}
		if (iItem.TravelParams != null && Game.Instance.CellData.MorphMode == MorphMode.Disabled)
		{
			Notification.ShowSpellWarning("Travel Forms are not allowed in this map.");
			return false;
		}
		SpellTemplate spellTemplate = SpellTemplates.Get(iItem.SpellID, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, 0);
		if (!CanPlayerCastSpell(spellTemplate, iItem))
		{
			return false;
		}
		Effect effect = entities.me.effects.FirstOrDefault((Effect p) => p.template.isTravelForm);
		if (effect != null)
		{
			Game.Instance.SendRequestEffectRemove(effect.ID);
			return false;
		}
		if (!PlayerHasValidTarget(spellTemplate, out var target))
		{
			return false;
		}
		RequestCombat request = GetRequest(1);
		if (target != null)
		{
			request.targetType = target.type;
			request.targetID = target.ID;
		}
		else if (!spellTemplate.primaryAction.isAoe && !spellTemplate.primaryAction.makesAura)
		{
			Notification.ShowSpellWarning("Target required", 0.75f);
		}
		request.charItemID = iItem.CharItemID;
		request.rct = RequestCombatType.Cast;
		AEC.getInstance().sendRequest(request);
		return true;
	}

	private bool CanPlayerCastSpell(SpellTemplate spellT, InventoryItem item = null)
	{
		if (spellT == null)
		{
			return false;
		}
		if (spellT.reqClassRank > Session.MyPlayerData.ScaledClassRank)
		{
			Notification.ShowWarning("Unlocked at " + entities.me.CombatClass.Name + " Rank " + spellT.reqClassRank);
			return false;
		}
		TryQueueingSpell(spellT, item);
		if (entities.me.IsCharging)
		{
			return false;
		}
		if (spellT.isOnGCD && !IsGcdOver())
		{
			return false;
		}
		if (spellT.isUlt && entities.me.statsCurrent[Stat.UltCharge] < 1000f)
		{
			return false;
		}
		if (!IsSpellCooldownOver(spellT))
		{
			return false;
		}
		if (entities.me.statsCurrent[Stat.Resource] < (float)spellT.GetResourceCost(entities.me))
		{
			Notification.ShowSpellWarning("Not enough " + entities.me.CombatClass.GetResourceString());
			return false;
		}
		if (entities.me.HasStatus(Entity.StatusType.NoSpells) && !spellT.isAA && !spellT.canUseDuringCC)
		{
			Notification.ShowSpellWarning("Cannot use skills while Silenced");
			return false;
		}
		if (entities.me.HasStatus(Entity.StatusType.NoAA) && spellT.isAA && !spellT.canUseDuringCC)
		{
			return false;
		}
		if (!spellT.canUseInCombat && entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowSpellWarning("Cannot be used while in combat");
			return false;
		}
		if (entities.me.serverState == Entity.State.Dead)
		{
			return false;
		}
		if (entities.me.IsInPvp && item != null && !item.IsTravelForm)
		{
			return false;
		}
		if (!entities.me.IsInPvp && !Game.Instance.AreaData.IsPvpLobby && spellT.isPvpAction)
		{
			return false;
		}
		string text = "";
		foreach (SpellAction startingAction in spellT.GetStartingActions())
		{
			List<SpellRequirement> source = startingAction.requirements.Where((SpellRequirement req) => req.targetType == SpellRequirement.TargetType.Caster).ToList();
			if (source.Any((SpellRequirement req) => entities.me.HealthPercent > req.maxHp && req.maxHp < 1f))
			{
				text = "Your Health is too ";
				text += ((entities.me.HealthPercent >= 1f) ? "full" : "too high");
				continue;
			}
			if (source.Any((SpellRequirement req) => entities.me.HealthPercent < req.minHp) && entities.me.HealthPercent > 0f)
			{
				text = "Your Health is too low";
				continue;
			}
			if (source.Any((SpellRequirement req) => !req.MeetsResourceCheck(entities.me)))
			{
				text = "Resource requirement not met";
				continue;
			}
			if (startingAction.effectReq == null || startingAction.CheckCasterEffectReqs(entities.me))
			{
				text = "";
				break;
			}
			text = ((!startingAction.effectReq.mustNotHave) ? "Requirements not met" : "This effect is already active");
		}
		if (text != "")
		{
			Notification.ShowSpellWarning(text);
			return false;
		}
		return true;
	}

	public bool IsSpellVisibleOnParty(SpellTemplate spellT)
	{
		if (spellT == null)
		{
			return false;
		}
		if (!spellT.showOnParty)
		{
			return false;
		}
		if (spellT.reqClassRank > Session.MyPlayerData.ScaledClassRank)
		{
			return false;
		}
		if (spellT.isUlt && entities.me.statsCurrent[Stat.UltCharge] < 1000f)
		{
			return false;
		}
		if (!IsSpellCooldownOver(spellT))
		{
			return false;
		}
		if (!spellT.canUseInCombat && entities.me.serverState == Entity.State.InCombat)
		{
			return false;
		}
		if (entities.me.serverState == Entity.State.Dead)
		{
			return false;
		}
		bool flag = true;
		foreach (SpellAction startingAction in spellT.GetStartingActions())
		{
			if (startingAction.effectReq == null || startingAction.CheckCasterEffectReqs(entities.me))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return false;
		}
		return true;
	}

	public List<AoeLocation> GetActionAoeLocations(Entity caster, SpellTemplate spellT, SpellAction spellAction)
	{
		List<AoeLocation> list = new List<AoeLocation>();
		foreach (Entity item in GetTargetsByRootAction(spellT, spellAction, caster))
		{
			Entity aoeSource = spellT.GetAoeSource(spellAction, caster, item);
			foreach (AoeShape allAo in spellAction.GetAllAoes())
			{
				Vector3 vector = Vector3.zero;
				if (allAo.randomOffsetRadius > 0f)
				{
					ArtixRandom.RandomPositionInCircle(allAo.randomOffsetRadius, out var x, out var y);
					vector = new Vector3(x, 0f, y);
				}
				if (allAo.isFixed)
				{
					Vector3 positionBySource = allAo.GetPositionBySource(caster, aoeSource);
					float yRotation = allAo.GetYRotation(caster, aoeSource);
					list.Add(new AoeLocation(spellAction.ID, allAo.ID, allAo.isAura, positionBySource, vector, yRotation, aoeSource));
				}
				else
				{
					list.Add(new AoeLocation(spellAction.ID, allAo.ID, allAo.isAura, Vector3.zero, vector, 0f, aoeSource));
				}
			}
		}
		return list;
	}

	public List<Entity> GetTargetsByRootAction(SpellTemplate spellT, SpellAction spellAction, Entity caster)
	{
		List<Entity> list = new List<Entity>();
		SpellAction castableRootAction = spellT.GetCastableRootAction(spellAction, caster);
		if (castableRootAction != spellAction && castableRootAction.isAoe)
		{
			list = FindPossibleTargetsInAoeRadius(caster, spellT, castableRootAction, castableRootAction.aoes.First());
		}
		else
		{
			list.Add(Game.Instance.combat.GetValidTarget(spellT, castableRootAction, caster, caster.Target));
		}
		return list;
	}

	public Entity GetValidTarget(SpellTemplate spellT, SpellAction spellAction, Entity caster, Entity currentTarget)
	{
		if (!spellAction.IsCastable(caster))
		{
			return currentTarget;
		}
		Entity entity = currentTarget;
		bool flag = spellT.HasHybridTargets(caster);
		if (caster.Target == null && flag)
		{
			entity = caster;
		}
		if (spellAction.isHarmful)
		{
			if (entity == null && caster.isMe)
			{
				entity = entities.me.GetClosestTarget(spellAction.range, spellT, spellAction);
			}
		}
		else if (spellAction.targetType == TargetType.Self || entity == null || (!flag && caster.CanAttack(entity)))
		{
			entity = caster;
		}
		return entity;
	}

	private bool PlayerHasValidTarget(SpellTemplate spellT, out Entity target)
	{
		string text = "";
		target = entities.me.Target;
		foreach (SpellAction startingAction in spellT.GetStartingActions())
		{
			if (!startingAction.IsCastable(entities.me))
			{
				continue;
			}
			target = GetValidTarget(spellT, startingAction, entities.me, entities.me.Target);
			if (startingAction.isAoe)
			{
				return true;
			}
			if (target != null)
			{
				List<SpellRequirement> source = startingAction.requirements.Where((SpellRequirement req) => req.targetType == SpellRequirement.TargetType.Target).ToList();
				float targetHpPercent = target.HealthPercent;
				if (source.Any((SpellRequirement req) => targetHpPercent > req.maxHp && req.maxHp < 1f))
				{
					text = (target.isMe ? "Your" : "Target's");
					text += " Health is ";
					text += ((targetHpPercent >= 1f) ? "full" : "too high");
					continue;
				}
				if (source.Any((SpellRequirement req) => targetHpPercent < req.minHp) && targetHpPercent > 0f)
				{
					text = (target.isMe ? "Your" : "Target's");
					text += " Health is too low";
					continue;
				}
				Entity targetCopy = target;
				if (source.Any((SpellRequirement req) => !req.MeetsResourceCheck(targetCopy)))
				{
					text = ((target.resource != 0) ? (target.isMe ? "Resource requirement not met" : "Target's Resource requirement not met") : (target.isMe ? "Current class does not use Mana" : "Target does not use Mana"));
					continue;
				}
				if (source.Any((SpellRequirement req) => req.notMaxLevel) && target.Level >= Session.MyPlayerData.LevelCap)
				{
					text = "Congratulations! You are already Max Level!";
					continue;
				}
				if (source.Any((SpellRequirement req) => req.notMaxRank) && target.EquippedClassRank >= 100)
				{
					text = "Congratulations! " + target.CombatClass.Name + " is already Max Rank!";
					continue;
				}
			}
			if (target == null || target.serverState == Entity.State.Dead || target.react == Entity.Reaction.PassiveAggressive)
			{
				continue;
			}
			if (startingAction.isHarmful && !entities.me.CanAttack(target))
			{
				text = "Target is friendly";
				continue;
			}
			float magnitude = (target.position - entities.me.position).magnitude;
			if (magnitude > entities.me.GetActionRange(spellT, startingAction, target))
			{
				text = "Target is out of range";
				continue;
			}
			if (!entities.me.HasLineOfSight(target))
			{
				text = "Target out of sight";
				continue;
			}
			if (startingAction.isHarmful && magnitude > 4f && !entities.me.IsFacingTarget(target) && (Game.Instance.AreaData.HasPvp || Game.Instance.AreaData.IsPvpLobby || target.ID == entities.me.DuelOpponentID))
			{
				text = "Not facing target";
				continue;
			}
			if (startingAction.effectReq == null || startingAction.CheckTargetEffectReqs(entities.me, target))
			{
				text = "";
				break;
			}
			text = ((!startingAction.effectReq.mustNotHave) ? "Requirements not met" : "This effect is already active");
		}
		if (target != null && target.serverState == Entity.State.Dead)
		{
			return false;
		}
		if (target != entities.me)
		{
			entities.me.Target = target;
		}
		if (text != "")
		{
			Notification.ShowSpellWarning(text);
			return false;
		}
		return true;
	}

	public static List<Entity> FindPossibleTargetsInAoeRadius(Entity caster, SpellTemplate spellT, SpellAction spellAction, AoeShape aoe)
	{
		List<Entity> list = new List<Entity>();
		foreach (Entity allEntity in Entities.Instance.AllEntities)
		{
			Entity aoeSource = spellT.GetAoeSource(spellAction, caster, allEntity);
			float radius = aoe.GetRadius(caster, aoeSource);
			float minRadius = aoe.GetMinRadius(caster, aoeSource);
			radius *= radius;
			minRadius *= minRadius;
			if (allEntity.serverState != Entity.State.Dead && caster.CanTargetWithAction(spellAction, allEntity) && spellAction.CheckRequirements(caster, allEntity))
			{
				if (allEntity.type == Entity.Type.NPC)
				{
					radius += allEntity.combatRadius;
				}
				float sqrMagnitude = (allEntity.position.xz() - caster.position.xz()).sqrMagnitude;
				if (!(sqrMagnitude > radius) && !(sqrMagnitude < minRadius))
				{
					list.Add(allEntity);
				}
			}
		}
		if (list.Count == 0)
		{
			return list;
		}
		return GetTargetsByTargetType(list, spellAction.areaTarget).Distinct().Take(spellAction.maxTargets).ToList();
	}

	public static List<Entity> GetTargetsByTargetType(List<Entity> startTargets, AreaTarget areaTarget)
	{
		List<Entity> result = new List<Entity>();
		switch (areaTarget)
		{
		case AreaTarget.All:
			result = startTargets;
			break;
		case AreaTarget.LowestHealth:
			result = startTargets.OrderBy((Entity t) => t.HealthPercent).ToList();
			break;
		case AreaTarget.Random:
			result = ArtixRandom.ShuffleList(startTargets).ToList();
			break;
		}
		return result;
	}

	public void HandleMachineSpell(ResponseMachineSpell e)
	{
		Debug.Log("CombatSolver.handleMachineSpell()");
		if (e.EntityUpdates == null)
		{
			return;
		}
		foreach (ComEntityUpdate entityUpdate in e.EntityUpdates)
		{
			Entity entity = entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID);
			if (entity != null)
			{
				MachineSpellTemplate machineSpellTemplate = MachineSpellTemplates.Get(e.SpellId);
				if (machineSpellTemplate.isHarmful && machineSpellTemplate.damageMult != 0f)
				{
					entity.PlayAnimation(EntityAnimations.Get("Hit"));
				}
				entity.UpdateServerEffects(entityUpdate);
				HandleEntityUpdate(entityUpdate, updateServerState: true, updateVisualState: true);
				SpellFXContainer.mInstance.HandleImpactFX(entity, machineSpellTemplate.impactFX, isCasterMe: false);
				if (entity.isMe && machineSpellTemplate.isHarmful)
				{
					Game.Instance.camController.PlayCameraShake(machineSpellTemplate.camShakes, SpellCamShake.Trigger.Impact, SpellCamShake.Target.Target);
				}
				if (entity.isMe || entity == Entities.Instance.me.Target)
				{
					entity.ShowCombatPopups(entityUpdate, isAA: false, isFirstImpact: true);
				}
			}
		}
	}

	public void HandleCombatSpell(ResponseCombatSpell e)
	{
		Debug.Log("CombatSolver.handleCombatSpell()");
		if (e.code == 1)
		{
			HandleSpellFailure(e);
			return;
		}
		foreach (ComEntityUpdate entityUpdate in e.entityUpdates)
		{
			Entity entity = entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID);
			if (entity != null)
			{
				entity.serverState = entityUpdate.state;
				entity.UpdateServerEffects(entityUpdate);
				entity.ApplyEffectOperations(entityUpdate, Entity.EffectsToApply.Immediate);
				if (entityUpdate.respawnTime > 0f && entity is Player player)
				{
					player.respawnTime = entityUpdate.respawnTime;
				}
			}
		}
		Entity entity2 = entities.GetEntity(e.casterType, e.casterID);
		if (entity2 == entities.me)
		{
			HandleMyCombatSpell(e);
		}
		ComEntityUpdate comEntityUpdate = e.entityUpdates.FirstOrDefault((ComEntityUpdate u) => u.isCasterUpdate);
		if (comEntityUpdate != null && entity2 != null && comEntityUpdate.entityType == Entity.Type.NPC && comEntityUpdate.targetType != 0)
		{
			Entity entity3 = entities.GetEntity(comEntityUpdate.targetType, comEntityUpdate.targetID);
			if (entity3 != entity2)
			{
				entity2.Target = entity3;
			}
		}
		ApplyCasterUpdates(e);
	}

	private void HandleSpellFailure(ResponseCombatSpell e)
	{
		if (e.entityUpdates != null && e.entityUpdates.Count > 0)
		{
			ComEntityUpdate entityUpdate = e.entityUpdates[0];
			HandleEntityUpdate(entityUpdate);
		}
		if (e.ID == aaRequestID)
		{
			aaResponseDelay = 0.2f;
		}
	}

	public void HandleResetSpellCD(ResponseResetCD response)
	{
		foreach (int spellTemplateID in response.spellTemplateIDs)
		{
			SetSpellTimestamp(spellTemplateID, GameTime.realtimeSinceServerStartup, response.cooldown);
			this.ResetSpellCD?.Invoke(spellTemplateID, response.cooldown);
		}
	}

	public void HandleAuraUpdate(ResponseAuraUpdate response)
	{
		entities.GetEntity(response.entityType, response.entityID)?.UpdateAuras(response.auraUpdate);
	}

	private void ApplyCasterUpdates(ResponseCombatSpell e)
	{
		Entity entity = entities.GetEntity(e.casterType, e.casterID);
		if (entity == null)
		{
			return;
		}
		entity.comboState = e.comboState ?? new SpellComboState();
		SpellTemplate spellTemplate = SpellTemplates.Get(e.spellTemplateID, entity.effects, entity.ScaledClassRank, entity.EquippedClassID, entity.comboState.Get(e.spellTemplateID));
		if (spellTemplate == null)
		{
			return;
		}
		SpellAction firstAction = spellTemplate.GetFirstAction(entity, e.entityUpdates);
		List<Entity> list = new List<Entity>();
		if (e.entityUpdates != null)
		{
			list = (from update in e.entityUpdates
				where update.spellActionId == firstAction.ID
				select update into actionUpdate
				select entities.GetEntity(actionUpdate.entityType, actionUpdate.entityID)).ToList();
		}
		if (entities.me == null)
		{
			Debug.LogException(new Exception("AQ3DException: CombatSolver.cs - entities.me is null."));
			return;
		}
		if (entity.isMe && firstAction.isHarmful && e.rcst == ResponseCombatSpellType.Cast && list.Count > 0)
		{
			if (entities.me.Target == null && list[0] != entity && list[0].wrapper != null)
			{
				entities.me.Target = list[0];
			}
			if (entities.me.CanAttack(entities.me.Target) && list.Contains(entities.me.Target))
			{
				IsAA = true;
			}
		}
		ComEntityUpdate comEntityUpdate = e.entityUpdates.FirstOrDefault((ComEntityUpdate u) => u.isCasterUpdate);
		if (comEntityUpdate != null && comEntityUpdate.statsCurrent == null)
		{
			if (comEntityUpdate.statDeltas.TryGetValue(1, out var value))
			{
				float num = entity.statsCurrent[Stat.Resource];
				entity.statsCurrent[Stat.Resource] = (float)Math.Round(num + value, 2);
				entity.OnStatUpdate();
			}
			if (comEntityUpdate.statDeltas.TryGetValue(18, out var value2))
			{
				float num2 = entity.statsCurrent[Stat.UltCharge];
				entity.statsCurrent[Stat.UltCharge] = (float)Math.Round(num2 + value2, 2);
				entity.OnStatUpdate();
			}
		}
		switch (e.rcst)
		{
		case ResponseCombatSpellType.StartCharge:
			entity.InterruptKeyframeSpell();
			entity.spellCastData = e.spellCastData;
			entity.spellCastData.spellT = SpellTemplates.Get(e.spellCastData.spellTemplateID, entity.effects, entity.ScaledClassRank, entity.EquippedClassID, entity.comboState.Get(e.spellCastData.spellTemplateID));
			if (comEntityUpdate != null && comEntityUpdate.entityType == Entity.Type.NPC && comEntityUpdate.targetType != 0)
			{
				entity.Target = entities.GetEntity(comEntityUpdate.targetType, comEntityUpdate.targetID);
			}
			entity.StartCharge(spellTemplate, firstAction, e.spellCastData.chargeTime);
			break;
		case ResponseCombatSpellType.Cast:
		{
			entity.spellCastData = e.spellCastData;
			KeyframeSpellData newSpellData = new KeyframeSpellData(e.entityUpdates, entity, spellTemplate, list, e.spellCastData?.aoeLocations);
			entity.StoreKeyframeSpellData(newSpellData);
			entity.HandleCast(spellTemplate, firstAction, list);
			break;
		}
		case ResponseCombatSpellType.Cancel:
			entity.CastCancel();
			break;
		case ResponseCombatSpellType.AuraPulse:
		{
			foreach (int pulseActionId in e.pulseActionIds)
			{
				KeyframeSpellData spellData3 = new KeyframeSpellData(e.entityUpdates, entity, spellTemplate, list, e.spellCastData?.aoeLocations);
				entity.StartNewActionImpact(spellData3, spellTemplate.GetActionById(pulseActionId), Entity.ImpactSource.Aura);
			}
			break;
		}
		case ResponseCombatSpellType.ChannelPulse:
		{
			foreach (int pulseActionId2 in e.pulseActionIds)
			{
				KeyframeSpellData spellData2 = new KeyframeSpellData(e.entityUpdates, entity, spellTemplate, list, e.spellCastData?.aoeLocations);
				entity.StartNewActionImpact(spellData2, spellTemplate.GetActionById(pulseActionId2), Entity.ImpactSource.Animation);
			}
			break;
		}
		case ResponseCombatSpellType.ChannelEnd:
			foreach (int pulseActionId3 in e.pulseActionIds)
			{
				KeyframeSpellData spellData = new KeyframeSpellData(e.entityUpdates, entity, spellTemplate, list, e.spellCastData?.aoeLocations);
				entity.StartNewActionImpact(spellData, spellTemplate.GetActionById(pulseActionId3), Entity.ImpactSource.Animation);
			}
			entity.EndChannel();
			break;
		}
	}

	public void HandleCombatEffectPulse(ResponseCombatEffectPulse response)
	{
		Entity entity = entities.GetEntity(response.casterType, response.casterID);
		foreach (ComEntityUpdate entityUpdate in response.entityUpdates)
		{
			Entity entity2 = entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID);
			if (entity2 != null)
			{
				Debug.Log(entity2.name + "/" + entity2.ID + " handleCombatEffectPulse");
				entity2.UpdateServerEffects(entityUpdate);
				HandleEntityUpdate(entityUpdate, updateServerState: true, updateVisualState: true);
				if (entity == entities.me || entity2.isMe)
				{
					entity2.ShowCombatPopups(entityUpdate, isAA: false, isFirstImpact: true);
				}
			}
		}
	}

	private void HandleMyCombatSpell(ResponseCombatSpell e)
	{
		if (entities.me == null)
		{
			return;
		}
		SpellTemplate spellT = SpellTemplates.Get(e.spellTemplateID, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(e.spellTemplateID));
		if (e.ID > -1 && requestMap.ContainsKey(e.ID) && e.spellCastData == null)
		{
			CloseRequestByID(e.ID);
		}
		if (spellT != null)
		{
			if (e.gcdLength > 0f)
			{
				SetGCDTrigger(GameTime.realtimeSinceServerStartup, e.gcdLength);
			}
			if (e.cooldown > 0f)
			{
				SetSpellTimestamp(spellT.ID, GameTime.realtimeSinceServerStartup, e.cooldown);
			}
			List<SpellCamShake> list = e.entityUpdates.Where((ComEntityUpdate update) => spellT.GetActionById(update.spellActionId) != null).SelectMany((ComEntityUpdate update) => spellT.GetActionById(update.spellActionId).camShakes).ToList();
			if (list.Count > 0)
			{
				Game.Instance.camController.PlayCameraShake(list, SpellCamShake.Trigger.Cast, SpellCamShake.Target.Caster);
			}
		}
	}

	public void HandleEntityUpdate(ComEntityUpdate entityUpdate, bool updateServerState = false, bool updateVisualState = false, Entity.EffectsToApply effectsToApply = Entity.EffectsToApply.All)
	{
		Debug.Log("HandleEntityUpdate() > ");
		Entity entity = entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID);
		if (entity != null)
		{
			Debug.Log(entity.name + "/" + entity.ID);
			if (entityUpdate.isCastCancel)
			{
				entity.CastCancel();
			}
			if (updateServerState)
			{
				entity.serverState = entityUpdate.state;
			}
			if (entityUpdate.respawnTime > 0f && entity is Player player)
			{
				player.respawnTime = entityUpdate.respawnTime;
			}
			entity.ApplyStatusEffects(entityUpdate, effectsToApply);
			entity.UpdateAuras(entityUpdate);
			entity.ApplyStatChanges(entityUpdate, updateVisualState);
		}
	}

	public static bool DoesProjTypeCreateCastSpotFX(ProjectileType projType)
	{
		switch (projType)
		{
		case ProjectileType.None:
		case ProjectileType.Meteor:
		case ProjectileType.Reverse:
			return false;
		case ProjectileType.Caster:
			return true;
		default:
			throw new ArgumentOutOfRangeException("projType", projType, null);
		}
	}

	public static int GetBaseResourceAmount(Entity.Resource resource)
	{
		switch (resource)
		{
		case Entity.Resource.None:
			return 0;
		case Entity.Resource.Mana:
		case Entity.Resource.ManaNoRegen:
			return 1000;
		case Entity.Resource.Spirit:
		case Entity.Resource.Determination:
		case Entity.Resource.Souls:
		case Entity.Resource.Chi:
		case Entity.Resource.Focus:
		case Entity.Resource.Fury:
			return 100;
		case Entity.Resource.Bullets:
			return 6;
		default:
			return 0;
		}
	}

	public void Destroy()
	{
		IsAA = false;
	}

	private IEnumerator AutoAttackRoutine()
	{
		while (true)
		{
			if (!IsAA)
			{
				yield break;
			}
			if (entities.me.Target == null || entities.me.Target.serverState == Entity.State.Dead)
			{
				break;
			}
			aaResponseDelay -= Time.deltaTime;
			if (aaResponseDelay <= 0f)
			{
				RequestAutoAttack();
			}
			yield return null;
		}
		IsAA = false;
	}

	private void RequestAutoAttack()
	{
		if (CanCastAutoAttack())
		{
			int spellID = spellIDs[InputAction.Spell_1];
			CastSpell(spellID);
		}
	}

	private bool CanCastAutoAttack()
	{
		int num = spellIDs[InputAction.Spell_1];
		SpellTemplate spellTemplate = SpellTemplates.Get(num, entities.me.effects, entities.me.ScaledClassRank, entities.me.EquippedClassID, entities.me.comboState.Get(num));
		SpellAction firstCastableAction = spellTemplate.GetFirstCastableAction(entities.me);
		if (firstCastableAction != null && IsSpellCooldownOver(spellTemplate) && IsGcdOver() && queuedSpell == null && entities.me.spellCastData == null && entities.me.Target != null)
		{
			return (entities.me.TargetTransform.position - entities.me.position).magnitude <= entities.me.GetActionRange(spellTemplate, firstCastableAction, entities.me.Target);
		}
		return false;
	}

	public void ClearQueuedSpell()
	{
		queuedSpell = null;
		queuedItem = null;
	}
}

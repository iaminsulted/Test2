using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class CombatPopup : MonoBehaviour
{
	public enum EffectType
	{
		None,
		SmallMoveUp,
		MoveUp,
		Crit,
		MoveDown,
		Arc,
		MultihitMoveUp
	}

	public enum PopupType
	{
		None,
		Battle,
		Message,
		Gold,
		XP,
		ClassXP,
		MultiHit
	}

	private const int Default_Font_Size = 45;

	private const int Crit_Effect_Font_Size = 55;

	private const int DoT_Font_Size = 25;

	private const int Other_Heal_Font_Size = 25;

	private const int Gold_XP_Font_Size = 45;

	private const int Trade_Skill_XP_Font_Size = 35;

	private const int Message_Font_Size = 30;

	private const int AA_Hit_Font_Size = 35;

	private static readonly Vector3 Gold_Offset = new Vector3(-2.5f, -0.7f, 0f);

	private static readonly Vector3 XP_Offset = new Vector3(-2.5f, 0.7f, 0f);

	private static readonly Vector3 TradeSkillXP_Offset = new Vector3(-2.5f, 0f, 0f);

	private static readonly Vector3 Class_XP_Offset = new Vector3(-2.5f, 0f, 0f);

	private static readonly Vector3 My_Damage_Offset = new Vector3(1.7f, -0.25f, 0f);

	private const float Duration = 1.5f;

	private static GameObject PopupObject;

	private static ObjectPool<GameObject> PopupPool;

	private EffectType popupEffect;

	public UILabel label;

	private float startTime;

	private float delayTime;

	private static Dictionary<EffectType, float> lastStartTime = new Dictionary<EffectType, float>();

	private static Transform parent;

	private static Camera mainCamera;

	private static Camera uiCamera;

	private Animator animator;

	private Vector3 worldPosition;

	public bool keepWorldPosition;

	public static CombatPopup CreateDefaultPopup()
	{
		InitPopupPool();
		GameObject obj = PopupPool.Get();
		obj.SetActive(value: true);
		obj.transform.SetParent(parent, worldPositionStays: false);
		obj.transform.localScale = Vector3.one;
		CombatPopup component = obj.GetComponent<CombatPopup>();
		component.label.fontSize = 45;
		component.label.gradientTop = InterfaceColors.Popups.Default_Top;
		component.label.gradientBottom = InterfaceColors.Popups.Default_Bot;
		component.label.effectStyle = UILabel.Effect.None;
		component.label.fontStyle = FontStyle.Bold;
		component.label.effectStyle = UILabel.Effect.Shadow;
		component.delayTime = 0f;
		component.animator.SetFloat("HorizontalBlend", 0.5f);
		component.animator.ResetTrigger("TriggerMoveUp");
		component.keepWorldPosition = true;
		return component;
	}

	public static void Init()
	{
		parent = GameObject.Find("PanelCombatPopup").transform;
		uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		mainCamera = Game.Instance.cam;
		InitPopupPool();
	}

	private static void InitPopupPool()
	{
		if (PopupObject == null)
		{
			PopupObject = (GameObject)Resources.Load("CombatPopup");
		}
		if (PopupPool == null)
		{
			PopupPool = new ObjectPool<GameObject>(PopupObject, 5);
		}
	}

	private void Awake()
	{
		animator = label.GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		if (delayTime > 0f)
		{
			delayTime -= Time.deltaTime;
			if (delayTime <= 0f)
			{
				StartPopup();
			}
		}
		if (Time.time - startTime > 1.5f)
		{
			DestroyPopup();
		}
		else if (keepWorldPosition)
		{
			UpdateUIPosition();
		}
	}

	private void Begin(Vector3 targetPosition, Vector3 localOffset, EffectType popupEffect)
	{
		this.popupEffect = popupEffect;
		PlaceOnUI(targetPosition, localOffset);
		startTime = Time.time;
		if (lastStartTime.TryGetValue(popupEffect, out var value))
		{
			float num = Time.time - value;
			if (num < GetMinimumDelay(popupEffect))
			{
				label.gradientTop = new Color(label.gradientTop.r, label.gradientTop.g, label.gradientTop.b, 0f);
				label.gradientBottom = new Color(label.gradientBottom.r, label.gradientBottom.g, label.gradientBottom.b, 0f);
				float num2 = Mathf.Clamp(GetMinimumDelay(popupEffect) - num, 0f, GetMaximumDelay(popupEffect));
				startTime += num2;
				delayTime = num2;
				lastStartTime[popupEffect] = startTime;
				return;
			}
		}
		lastStartTime[popupEffect] = startTime;
		StartPopup();
	}

	private void StartPopup()
	{
		label.gradientTop = new Color(label.gradientTop.r, label.gradientTop.g, label.gradientTop.b, 1f);
		label.gradientBottom = new Color(label.gradientBottom.r, label.gradientBottom.g, label.gradientBottom.b, 1f);
		PlayAnimationByEffectType(popupEffect);
	}

	public void DestroyPopup()
	{
		PopupPool.Release(base.gameObject);
		base.gameObject.SetActive(value: false);
	}

	public static void PlayBattleHitPopup(Vector3 position, Stat stat, int statDelta, bool isMe, bool isAA)
	{
		if (statDelta == 0 && stat != 0)
		{
			return;
		}
		CombatPopup combatPopup = CreateDefaultPopup();
		combatPopup.label.fontStyle = FontStyle.BoldAndItalic;
		combatPopup.label.text = Mathf.Abs(statDelta).ToString("G");
		combatPopup.keepWorldPosition = !isMe;
		if (statDelta > 0)
		{
			combatPopup.label.gradientTop = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Top : InterfaceColors.Popups.MP_Gain_Top);
			combatPopup.label.gradientBottom = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Bot : InterfaceColors.Popups.MP_Gain_Bot);
			combatPopup.label.fontSize = (isMe ? 45 : 25);
		}
		else
		{
			combatPopup.label.fontSize = (isAA ? 35 : 45);
			if (isMe)
			{
				combatPopup.label.gradientTop = InterfaceColors.Popups.My_HP_Loss_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.My_HP_Loss_Bot;
			}
			else
			{
				combatPopup.label.gradientTop = (isAA ? InterfaceColors.Popups.Other_HP_Loss_AA_Top : InterfaceColors.Popups.Other_HP_Loss_Top);
				combatPopup.label.gradientBottom = (isAA ? InterfaceColors.Popups.Other_HP_Loss_AA_Bot : InterfaceColors.Popups.Other_HP_Loss_Bot);
			}
		}
		Vector3 localOffset = (isMe ? My_Damage_Offset : Vector3.zero);
		EffectType effectType = EffectType.Arc;
		if (statDelta > 0 && !isMe)
		{
			effectType = EffectType.MoveUp;
		}
		else if (isMe)
		{
			effectType = EffectType.MoveDown;
		}
		combatPopup.Begin(position, localOffset, effectType);
	}

	public static void PlayBattleCritPopup(Vector3 position, Stat stat, int statDelta, bool isMe, bool isAA)
	{
		if (statDelta == 0 && stat != 0)
		{
			return;
		}
		CombatPopup combatPopup = CreateDefaultPopup();
		combatPopup.label.fontStyle = FontStyle.BoldAndItalic;
		combatPopup.label.text = Mathf.Abs(statDelta).ToString("G");
		combatPopup.label.fontSize = 55;
		combatPopup.keepWorldPosition = !isMe;
		if (statDelta > 0)
		{
			combatPopup.label.gradientTop = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Crit_Top : InterfaceColors.Popups.MP_Gain_Crit_Top);
			combatPopup.label.gradientBottom = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Crit_Bot : InterfaceColors.Popups.MP_Gain_Crit_Bot);
			combatPopup.label.fontSize = (isMe ? 55 : 25);
		}
		else
		{
			combatPopup.transform.localScale *= (isAA ? 0.66f : 1f);
			if (isMe)
			{
				combatPopup.label.gradientTop = InterfaceColors.Popups.My_HP_Loss_Crit_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.My_HP_Loss_Crit_Bot;
			}
			else
			{
				combatPopup.label.gradientTop = (isAA ? InterfaceColors.Popups.Other_HP_Loss_AA_Top : InterfaceColors.Popups.Other_HP_Loss_Top);
				combatPopup.label.gradientBottom = (isAA ? InterfaceColors.Popups.Other_HP_Loss_AA_Bot : InterfaceColors.Popups.Other_HP_Loss_Bot);
			}
		}
		EffectType effectType = (isMe ? EffectType.MoveDown : EffectType.Crit);
		Vector3 localOffset = My_Damage_Offset;
		if (!isMe)
		{
			localOffset = Vector3.up + new Vector3(ArtixRandom.Range(-0.4f, 0.4f), ArtixRandom.Range(-0.5f, 0f));
		}
		combatPopup.Begin(position, localOffset, effectType);
	}

	public static void PlayBattleDotPopup(Vector3 position, Stat stat, int statDelta, bool isMe)
	{
		CombatPopup combatPopup = CreateDefaultPopup();
		combatPopup.label.fontStyle = FontStyle.BoldAndItalic;
		combatPopup.label.text = Mathf.Abs(statDelta).ToString("G");
		combatPopup.keepWorldPosition = !isMe;
		if (statDelta > 0)
		{
			combatPopup.label.gradientTop = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Top : InterfaceColors.Popups.MP_Gain_Top);
			combatPopup.label.gradientBottom = ((stat == Stat.Health) ? InterfaceColors.Popups.HP_Gain_Bot : InterfaceColors.Popups.MP_Gain_Bot);
		}
		else
		{
			combatPopup.label.fontSize = 25;
			if (isMe)
			{
				combatPopup.label.gradientTop = InterfaceColors.Popups.My_HP_Loss_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.My_HP_Loss_Bot;
			}
			else
			{
				combatPopup.label.gradientTop = InterfaceColors.Popups.Other_HP_Loss_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.Other_HP_Loss_Bot;
			}
		}
		EffectType effectType = (isMe ? EffectType.MoveDown : EffectType.Arc);
		Vector3 localOffset = (isMe ? My_Damage_Offset : Vector3.zero);
		combatPopup.Begin(position, localOffset, effectType);
	}

	public static void PlayBattleMessagePopup(Vector3 position, string message, bool isMe)
	{
		CombatPopup combatPopup = CreateDefaultPopup();
		combatPopup.label.text = message;
		combatPopup.label.fontSize = 30;
		combatPopup.keepWorldPosition = !isMe;
		combatPopup.label.gradientTop = (isMe ? InterfaceColors.Popups.My_Normal_Text_Top : InterfaceColors.Popups.Other_Normal_Text_Top);
		combatPopup.label.gradientBottom = (isMe ? InterfaceColors.Popups.My_Normal_Text_Bot : InterfaceColors.Popups.Other_Normal_Text_Bot);
		EffectType effectType = (isMe ? EffectType.MoveDown : EffectType.Arc);
		Vector3 localOffset = (isMe ? My_Damage_Offset : Vector3.zero);
		combatPopup.Begin(position, localOffset, effectType);
	}

	public static void PlayMessagePopup(Vector3 position, string message, bool isMe, bool isEffectFading = false)
	{
		if (!string.IsNullOrEmpty(message))
		{
			CombatPopup combatPopup = CreateDefaultPopup();
			combatPopup.label.text = message;
			combatPopup.label.fontSize = 30;
			combatPopup.keepWorldPosition = !isMe;
			if (isEffectFading)
			{
				combatPopup.label.fontStyle = FontStyle.BoldAndItalic;
				combatPopup.label.gradientTop = InterfaceColors.Popups.Effect_Fades_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.Effect_Fades_Bot;
			}
			else
			{
				combatPopup.label.gradientTop = InterfaceColors.Popups.Message_Top;
				combatPopup.label.gradientBottom = InterfaceColors.Popups.Message_Bot;
			}
			Vector3 localOffset = (isMe ? Vector3.zero : (Vector3.up * 0.75f));
			EffectType effectType = (isMe ? EffectType.MoveDown : EffectType.MoveUp);
			combatPopup.Begin(position, localOffset, effectType);
		}
	}

	public static void PlayGoldXPPopups(Vector3 position, int gold, int xp, int classXP)
	{
		PlayXPPopup(position, xp);
		if (classXP > 0)
		{
			PlayClassXPPopup(position, classXP);
		}
		if (gold > 0)
		{
			PlayGoldPopup(position, gold);
		}
	}

	public static void PlayGoldPopup(Vector3 position, int gold)
	{
		if (gold != 0)
		{
			CombatPopup combatPopup = CreateDefaultPopup();
			if (Session.MyPlayerData.GoldMultiplier == 1f)
			{
				combatPopup.label.text = gold + "g";
			}
			else
			{
				combatPopup.label.text = gold + " + " + (Session.MyPlayerData.GoldMultiplier * (float)gold - (float)gold) + "g";
			}
			combatPopup.label.fontSize = 45;
			combatPopup.label.gradientTop = InterfaceColors.Popups.Gold_Top;
			combatPopup.label.gradientBottom = InterfaceColors.Popups.Gold_Bot;
			combatPopup.label.effectStyle = UILabel.Effect.Outline;
			combatPopup.keepWorldPosition = false;
			combatPopup.Begin(position, Gold_Offset, EffectType.SmallMoveUp);
		}
	}

	public static void PlayXPPopup(Vector3 position, int xp)
	{
		if (xp != 0)
		{
			CombatPopup combatPopup = CreateDefaultPopup();
			if (Session.MyPlayerData.XPMultiplier == 1f)
			{
				combatPopup.label.text = xp + "xp";
			}
			else
			{
				combatPopup.label.text = xp + " + " + (Session.MyPlayerData.XPMultiplier * (float)xp - (float)xp) + "xp";
			}
			combatPopup.label.fontSize = 45;
			combatPopup.label.gradientTop = InterfaceColors.Popups.XP_Top;
			combatPopup.label.gradientBottom = InterfaceColors.Popups.XP_Bot;
			combatPopup.label.effectStyle = UILabel.Effect.Outline;
			combatPopup.keepWorldPosition = false;
			combatPopup.Begin(position, XP_Offset, EffectType.SmallMoveUp);
		}
	}

	public static void PlayClassXPPopup(Vector3 position, int xp)
	{
		if (xp != 0)
		{
			CombatPopup combatPopup = CreateDefaultPopup();
			if (Session.MyPlayerData.CXPMultiplier == 1f)
			{
				combatPopup.label.text = xp + "xp";
			}
			else
			{
				combatPopup.label.text = xp + " + " + (Session.MyPlayerData.CXPMultiplier * (float)xp - (float)xp) + "xp";
			}
			combatPopup.label.fontSize = 45;
			combatPopup.label.gradientTop = InterfaceColors.Popups.Class_XP_Top;
			combatPopup.label.gradientBottom = InterfaceColors.Popups.Class_XP_Bot;
			combatPopup.label.effectStyle = UILabel.Effect.Outline;
			combatPopup.keepWorldPosition = false;
			combatPopup.Begin(position, Class_XP_Offset, EffectType.SmallMoveUp);
		}
	}

	public static void PlayTradeSkillXPPopup(TradeSkillType type, Vector3 position, int xp)
	{
		if (xp != 0)
		{
			CombatPopup combatPopup = CreateDefaultPopup();
			combatPopup.label.text = xp + " " + Enum.GetName(typeof(TradeSkillType), type).ToLower() + " xp";
			combatPopup.label.fontSize = 35;
			combatPopup.label.gradientTop = InterfaceColors.Popups.Trade_Skill_XP_Top;
			combatPopup.label.gradientBottom = InterfaceColors.Popups.Trade_Skill_XP_Bot;
			combatPopup.label.effectStyle = UILabel.Effect.Outline;
			combatPopup.keepWorldPosition = false;
			combatPopup.Begin(position, TradeSkillXP_Offset, EffectType.SmallMoveUp);
		}
	}

	public static CombatPopup CreateMultihitPopup(Vector3 position, int statDelta)
	{
		CombatPopup combatPopup = CreateDefaultPopup();
		if (statDelta == 0)
		{
			combatPopup.label.text = "";
			return combatPopup;
		}
		combatPopup.label.gradientTop = InterfaceColors.Popups.Multihit_Top;
		combatPopup.label.gradientBottom = InterfaceColors.Popups.Multihit_Bot;
		combatPopup.label.text = Mathf.Abs(statDelta).ToString("G");
		combatPopup.label.effectStyle = UILabel.Effect.Outline;
		combatPopup.label.fontSize = 55;
		combatPopup.Begin(position, Vector3.up * 1.75f, EffectType.MultihitMoveUp);
		return combatPopup;
	}

	private void PlaceOnUI(Vector3 targetPosition, Vector3 localOffset)
	{
		Vector3 vector = mainCamera.transform.TransformDirection(localOffset);
		worldPosition = targetPosition + Vector3.up * 1.5f + vector;
		UpdateUIPosition();
	}

	private void UpdateUIPosition()
	{
		Vector3 position = mainCamera.WorldToViewportPoint(worldPosition);
		Vector3 position2 = uiCamera.ViewportToWorldPoint(position);
		position2.z = 0f;
		base.gameObject.transform.position = position2;
	}

	private void PlayAnimationByEffectType(EffectType popupEffect)
	{
		string stateName;
		switch (popupEffect)
		{
		default:
			return;
		case EffectType.SmallMoveUp:
			stateName = "SmallMoveUp";
			break;
		case EffectType.MultihitMoveUp:
			stateName = "MultihitMoveUp";
			break;
		case EffectType.MoveUp:
			stateName = "MoveUp";
			break;
		case EffectType.Crit:
			stateName = "Crit";
			break;
		case EffectType.MoveDown:
			stateName = "MoveDown";
			break;
		case EffectType.Arc:
		{
			float num = ArtixRandom.Range(0f, 0.125f);
			num = (ArtixRandom.RandomBool() ? num : (1f - num));
			animator.SetFloat("HorizontalBlend", num);
			animator.SetTrigger("TriggerMoveUp");
			return;
		}
		case EffectType.None:
			return;
		}
		animator.Play(stateName);
	}

	private float GetMaximumDelay(EffectType effectType)
	{
		return GetMinimumDelay(effectType) * 3f;
	}

	private float GetMinimumDelay(EffectType effectType)
	{
		return effectType switch
		{
			EffectType.SmallMoveUp => 0.05f, 
			EffectType.MultihitMoveUp => 0f, 
			EffectType.Arc => 0.05f, 
			_ => 0.2f, 
		};
	}
}

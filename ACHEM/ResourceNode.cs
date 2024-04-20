using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
	public Renderer tradeSkillRing;

	public ParticleSystem sparklesColored;

	public ParticleSystem sparklesWhite;

	public Material ringDisabled;

	private ParticleSystem.MainModule module;

	private ParticleSystem.MinMaxGradient startColor;

	private Material ringEnabled;

	private ResourceMachine machine;

	[HideInInspector]
	public TradeSkillType tradeSkill;

	[HideInInspector]
	public EquipItemSlot slot;

	[HideInInspector]
	public int usagesTotal;

	[HideInInspector]
	public int usages;

	[HideInInspector]
	public int level;

	[HideInInspector]
	public int power;

	[HideInInspector]
	private List<InteractionRequirement> reqBitFlags;

	[HideInInspector]
	private List<InteractionRequirement> reqEquippedItems;

	[HideInInspector]
	private List<InteractionRequirement> reqItems;

	[HideInInspector]
	private List<InteractionRequirement> reqLevels;

	[HideInInspector]
	private List<InteractionRequirement> reqQuests;

	[HideInInspector]
	private List<InteractionRequirement> reqWarProgress;

	public Action UsageUpdate;

	public void Init(ResourceMachine machine, int usagesTotal, int usages)
	{
		this.machine = machine;
		tradeSkill = machine.tradeSkillType;
		slot = machine.equipItemSlot;
		level = machine.tradeSkillLevel;
		power = machine.power;
		this.usagesTotal = usagesTotal;
		this.usages = usages;
		reqBitFlags = machine.reqBitFlags;
		reqEquippedItems = machine.reqEquippedItems;
		reqItems = machine.reqItems;
		reqLevels = machine.reqLevels;
		reqQuests = machine.reqQuests;
		reqWarProgress = machine.reqWars;
		if (tradeSkillRing != null)
		{
			module = sparklesColored.main;
			startColor = module.startColor;
			ringEnabled = tradeSkillRing.material;
			Entities.Instance.me.LevelUpdated += OnAction;
			Entities.Instance.me.TradeSkillLevelUpdated += OnTradeSkillLevelUp;
			Session.MyPlayerData.BitFlagUpdated += OnBitFlagUpdated;
			Session.MyPlayerData.ItemAdded += OnItem;
			Session.MyPlayerData.ItemEquipped += OnItem;
			Session.MyPlayerData.ItemRemoved += OnItem;
			Session.MyPlayerData.ItemUnequipped += OnItem;
			Session.MyPlayerData.ItemUpdated += OnItem;
			Session.MyPlayerData.QuestAdded += OnQuest;
			Session.MyPlayerData.QuestObjectiveUpdated += OnQuestUpdated;
			Session.MyPlayerData.QuestRemoved += OnQuest;
			Session.MyPlayerData.QuestStringUpdated += OnQuestUpdated;
			Session.MyPlayerData.QuestTurnedIn += OnQuest;
			Wars.WarsUpdated += OnAction;
			SetVisualState();
		}
	}

	public void Interact()
	{
		if (machine != null)
		{
			machine.Trigger(checkRequirements: true);
		}
	}

	public void UpdateUsage(int remainingUsages)
	{
		usages = remainingUsages;
		UsageUpdate?.Invoke();
	}

	public void Destroy()
	{
		if (tradeSkillRing != null)
		{
			Entities.Instance.me.LevelUpdated -= OnAction;
			Entities.Instance.me.TradeSkillLevelUpdated -= OnTradeSkillLevelUp;
			Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
			Session.MyPlayerData.ItemAdded -= OnItem;
			Session.MyPlayerData.ItemEquipped -= OnItem;
			Session.MyPlayerData.ItemRemoved -= OnItem;
			Session.MyPlayerData.ItemUnequipped -= OnItem;
			Session.MyPlayerData.ItemUpdated -= OnItem;
			Session.MyPlayerData.QuestAdded -= OnQuest;
			Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestUpdated;
			Session.MyPlayerData.QuestRemoved -= OnQuest;
			Session.MyPlayerData.QuestStringUpdated -= OnQuestUpdated;
			Session.MyPlayerData.QuestTurnedIn -= OnQuest;
			Wars.WarsUpdated -= OnAction;
		}
	}

	public bool InCameraView()
	{
		Vector3 vector = Game.Instance.cam.WorldToViewportPoint(base.transform.position);
		if (vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f)
		{
			return vector.y < 1f;
		}
		return false;
	}

	private void OnAction()
	{
		SetVisualState();
	}

	private void OnBitFlagUpdated(string name, byte value, bool set)
	{
		SetVisualState();
	}

	private void OnItem(InventoryItem item)
	{
		SetVisualState();
	}

	private void OnQuestUpdated(int questIID, int questObjectiveID)
	{
		SetVisualState();
	}

	private void OnQuest(int id)
	{
		SetVisualState();
	}

	private void OnTradeSkillLevelUp(TradeSkillType tradeSkill, int level)
	{
		SetVisualState();
	}

	private void SetVisualState()
	{
		if (!(tradeSkillRing != null))
		{
			return;
		}
		InventoryItem equippedItem = Session.MyPlayerData.GetEquippedItem(slot);
		if (equippedItem == null)
		{
			if (Entities.Instance.me.tradeSkillLevel[tradeSkill] == 1 && Session.MyPlayerData.tradeSkillXP[tradeSkill] == 0)
			{
				ShowTradeSkillFXDisabled();
			}
			else
			{
				HideTradeSkillFX();
			}
		}
		else if (AreRequirementsMet(equippedItem))
		{
			ShowTradeSkillFXNormal();
		}
		else
		{
			ShowTradeSkillFXDisabled();
		}
	}

	private void HideTradeSkillFX()
	{
		sparklesColored.Stop();
		sparklesWhite.Stop();
		tradeSkillRing.gameObject.SetActive(value: false);
	}

	private void ShowTradeSkillFXDisabled()
	{
		sparklesColored.Play(withChildren: true);
		sparklesWhite.Play();
		tradeSkillRing.gameObject.SetActive(value: true);
		tradeSkillRing.material = ringDisabled;
		module.startColor = Color.red;
	}

	private void ShowTradeSkillFXNormal()
	{
		sparklesColored.Play();
		sparklesWhite.Play();
		tradeSkillRing.gameObject.SetActive(value: true);
		tradeSkillRing.material = ringEnabled;
		module.startColor = startColor;
	}

	private bool AreRequirementsMet(InventoryItem item)
	{
		if (!CheckRequirements())
		{
			return false;
		}
		if (item.GetTradeSkillPower() < power)
		{
			return false;
		}
		return true;
	}

	private bool CheckRequirements()
	{
		if (reqBitFlags.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		if (reqEquippedItems.Count > 0 && !reqEquippedItems.Exists((InteractionRequirement x) => x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		if (reqItems.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		if (reqLevels.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		if (reqQuests.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		if (reqWarProgress.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			return false;
		}
		return true;
	}
}

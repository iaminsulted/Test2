using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Newtonsoft.Json;
using UnityEngine;

public class SpellTemplate : IUIItem
{
	public enum DescriptionType
	{
		Sum,
		Separate,
		First
	}

	public int ID;

	public string name;

	public string icon;

	public bool isAA;

	public bool isUlt;

	public bool isPvpAction;

	public bool canUseInCombat = true;

	public bool canUseDuringCC;

	public bool requireTarget;

	public bool showOnParty;

	public SpellAction[] spellActions = new SpellAction[0];

	public int[] startActions = new int[1] { 1 };

	public string desc = "No Description Available";

	public int[] descActions = new int[1] { 1 };

	public DescriptionType descType;

	public int reqClassRank;

	public List<SpellUpgrade> spellUpgrades = new List<SpellUpgrade>();

	public float resourceCostPercent;

	public int resourceCost;

	public float cooldown;

	public bool isOnGCD = true;

	public string[] chargeAnims = new string[0];

	public string chargeFX;

	public string chargeSFX;

	public float chargeTime;

	public bool hideCastBar;

	public bool hideCastSpotFX;

	public string casterFX;

	public string targetFX;

	public string castSFX;

	public string[] anims = new string[0];

	public string[] animsAlt = new string[0];

	public bool blockMove;

	[JsonIgnore]
	public List<SpellAction> descSpellActions => descActions.Select(GetActionById).ToList();

	[JsonIgnore]
	public bool isHarmful => GetStartingActions().All((SpellAction a) => a.isHarmful);

	[JsonIgnore]
	public bool isFriendly => GetStartingActions().All((SpellAction a) => !a.isHarmful);

	public string Icon => icon;

	[JsonIgnore]
	public string ToolTipText
	{
		get
		{
			int scaledClassRank = Entities.Instance.me.ScaledClassRank;
			int equippedClassID = Entities.Instance.me.EquippedClassID;
			int combo = Entities.Instance.me.comboState.Get(ID);
			if (scaledClassRank < reqClassRank)
			{
				return GetLockedDescription();
			}
			string text = GetBaseSpellDescription();
			foreach (SpellTemplate overrideSpell in SpellTemplates.GetOverrideSpells(ID, scaledClassRank, equippedClassID, combo))
			{
				text += overrideSpell.GetOverrideDescription();
			}
			if (Entities.Instance.me.effects.Any((Effect e) => e.template.ID == 100))
			{
				text = text.ToUpperInvariant();
				text = text.Replace(". ", "!! ");
				text = text.Replace(".[-]\n", "!![-]\n");
			}
			return text;
		}
	}

	[JsonIgnore]
	public SpellAction primaryAction => GetStartingActions().First();

	[JsonIgnore]
	public List<SpellAction> baseActions
	{
		get
		{
			List<SpellAction> list = new List<SpellAction>();
			SpellAction[] array = spellActions;
			foreach (SpellAction action in array)
			{
				if (action.effectReq == null || spellUpgrades.All((SpellUpgrade upgrade) => upgrade.reqEffectID != action.effectReq.effectID))
				{
					list.Add(action);
				}
			}
			return list;
		}
	}

	public int GetResourceCost(Entity entity)
	{
		if (entity is NPC)
		{
			return 0;
		}
		return Mathf.RoundToInt((float)resourceCost + resourceCostPercent * (float)CombatSolver.GetBaseResourceAmount(entity.resource));
	}

	public float GetChargeTime(Entity caster)
	{
		return chargeTime;
	}

	public float GetCooldown()
	{
		if (isUlt)
		{
			return 0f;
		}
		return cooldown;
	}

	public int GetAnimationPriority()
	{
		if (!isAA)
		{
			return 10;
		}
		return 10;
	}

	public string GetDescriptionForItem(int level)
	{
		string text = GetBaseSpellInfo();
		foreach (SpellEffect item in descSpellActions.SelectMany((SpellAction action) => action.spellEffects))
		{
			text += item.effectT.GetBaseEffectInfo(level);
		}
		return text;
	}

	public string GetDescriptionForItemWithoutBaseSpellInfo(int level)
	{
		string text = "";
		foreach (SpellEffect item in descSpellActions.SelectMany((SpellAction action) => action.spellEffects))
		{
			text += item.effectT.GetBaseEffectInfo(level);
		}
		return text;
	}

	public bool IsAoeHarmful(SpellAction aoeAction)
	{
		if (aoeAction.makesAura)
		{
			return aoeAction.aura.actionIDs.All((int id) => GetActionById(id).isHarmful);
		}
		return aoeAction.isHarmful;
	}

	private string GetLockedDescription()
	{
		string text = "[892800]" + name + "[-]";
		text = text + "\n[000000]" + desc + "[-]";
		return text + "\n[FF0707]Unlocked at " + Entities.Instance.me.CombatClass.Name + " Rank " + reqClassRank + "[-]";
	}

	private string GetBaseSpellDescription()
	{
		int scaledClassRank = Entities.Instance.me.ScaledClassRank;
		int equippedClassID = Entities.Instance.me.EquippedClassID;
		int combo = Entities.Instance.me.comboState.Get(ID);
		SpellTemplate spellTemplate = SpellTemplates.Get(ID, null, scaledClassRank, equippedClassID, combo);
		string text = "[892800]" + spellTemplate.name + "[-]";
		text = text + "\n[000000]" + spellTemplate.desc + "[-]";
		text += GetBaseSpellInfo();
		List<SpellAction> overrideActions = GetOverrideActions(scaledClassRank);
		foreach (SpellAction item in descSpellActions.Except(overrideActions))
		{
			foreach (SpellEffect spellEffect in item.spellEffects)
			{
				text += GetSpellEffectDescription(spellEffect);
			}
		}
		return text;
	}

	private string GetBaseSpellInfo()
	{
		string text = "";
		List<SpellAction> list = baseActions.Intersect(descSpellActions).ToList();
		switch (descType)
		{
		case DescriptionType.Sum:
		{
			float damageMult = ((IEnumerable<SpellAction>)list).Sum((Func<SpellAction, float>)GetDamageMult);
			bool flag = true;
			if (list.Count > 0)
			{
				flag = (list[0].makesAura ? GetActionById(list[0].aura.actionIDs.First()).isHarmful : list[0].isHarmful);
			}
			text += GetPowerDescription(damageMult, flag);
			break;
		}
		case DescriptionType.Separate:
			foreach (SpellAction item in list)
			{
				text += GetPowerDescription(GetDamageMult(item), item.isHarmful);
			}
			break;
		case DescriptionType.First:
		{
			SpellAction spellAction = list.First();
			text += GetPowerDescription(GetDamageMult(spellAction), spellAction.isHarmful);
			break;
		}
		}
		Entity me = Entities.Instance.me;
		SpellAction spellAction2 = list.FirstOrDefault((SpellAction a) => a.aura != null);
		if (spellAction2 != null)
		{
			text = text + "\n[892800]Duration: [-][D73D00]" + ArtixString.FormatDuration(spellAction2.aura.duration) + "[-]";
		}
		SpellAction spellAction3 = list.FirstOrDefault((SpellAction a) => a.channel != null);
		if (spellAction3 != null)
		{
			text = text + "\n[892800]Duration: [-][D73D00]" + spellAction3.channel.duration + "s[-]";
		}
		int num = GetResourceCost(me);
		if (num > 0)
		{
			string resourceString = me.CombatClass.GetResourceString(num);
			string text2 = InterfaceColors.Resource.GetTextColor(me.resource).ToBBCode();
			text = text + "\n[892800]Cost: [-]" + text2 + num + " " + resourceString + "[-]";
		}
		if (chargeTime > 0f && !hideCastBar)
		{
			string text3 = (chargeTime * me.CastSpeed).ToString("0.##");
			text = text + "\n[892800]Cast Time: [-][D73D00]" + text3 + "s[-]";
		}
		if (GetCooldown() > 0f)
		{
			float seconds = (isAA ? (GetCooldown() * me.AASpeed) : GetCooldown());
			text = text + "\n[892800]Cooldown: [-][D73D00]" + ArtixString.FormatDuration(seconds) + "[-]";
		}
		return text;
	}

	private string GetPowerDescription(float damageMult, bool isHarmful)
	{
		string text = "";
		if (damageMult > 0f)
		{
			text = ((!isHarmful) ? (text + "\n[892800]Heal Power: [-][077007]" + damageMult * 100f + "%[-]") : (text + "\n[892800]Attack Power: [-][D73D00]" + damageMult * 100f + "%[-]"));
		}
		return text;
	}

	private float GetDamageMult(SpellAction action)
	{
		float num = 0f;
		AuraTemplate auraThatTriggersAction = GetAuraThatTriggersAction(action);
		int channelTicksWithAction = GetChannelTicksWithAction(action);
		if (auraThatTriggersAction != null)
		{
			return num + action.damageMult * (float)auraThatTriggersAction.tickCount;
		}
		if (channelTicksWithAction > 0)
		{
			return num + action.damageMult * (float)channelTicksWithAction;
		}
		return action.damageMult;
	}

	private AuraTemplate GetAuraThatTriggersAction(SpellAction action)
	{
		return spellActions.FirstOrDefault((SpellAction a) => a.aura != null && a.aura.actionIDs.Contains(action.ID))?.aura;
	}

	private int GetChannelTicksWithAction(SpellAction action)
	{
		int num = 0;
		foreach (SpellAction item in descSpellActions.Where((SpellAction a) => a.channel != null && a.channel.tickCount > 0))
		{
			int[][] tickActionIDs = item.channel.tickActionIDs;
			foreach (int[] source in tickActionIDs)
			{
				num += source.Count((int id) => id == action.ID);
			}
		}
		return num;
	}

	private string GetOverrideDescription()
	{
		SpellUpgrade overrideUpgrade = spellUpgrades.FirstOrDefault((SpellUpgrade u) => u.isSpellOverride);
		if (overrideUpgrade == null)
		{
			return "";
		}
		SpellAction spellAction = spellActions.Where((SpellAction a) => a.effectReq != null).FirstOrDefault((SpellAction a) => a.effectReq.effectID == overrideUpgrade.reqEffectID);
		if (spellAction == null)
		{
			return "";
		}
		Entity me = Entities.Instance.me;
		string text = "\n[000000]________________________[-]";
		text = text + "\n[892800]" + name + "[-]";
		text = text + "\n[000000]" + desc + "[-]";
		text += GetPowerDescription(spellAction.damageMult, spellAction.isHarmful);
		SpellTemplate baseSpell = SpellTemplates.GetBaseSpell(ID);
		int num = GetResourceCost(me);
		if (num > 0 && num != baseSpell.GetResourceCost(me))
		{
			string resourceString = me.CombatClass.GetResourceString(num);
			string text2 = InterfaceColors.Resource.GetTextColor(me.resource).ToBBCode();
			text = text + "\n[892800]Cost: [-]" + text2 + num + " " + resourceString + "[-]";
		}
		if (chargeTime > 0f && !hideCastBar && chargeTime != baseSpell.chargeTime)
		{
			string text3 = (chargeTime * me.CastSpeed).ToString("0.##");
			text = text + "\n[892800]Cast Time: [-][D73D00]" + text3 + "s[-]";
		}
		if (GetCooldown() != baseSpell.GetCooldown())
		{
			float seconds = (isAA ? (GetCooldown() * me.AASpeed) : GetCooldown());
			text = text + "\n[892800]Cooldown: [-][D73D00]" + ArtixString.FormatDuration(seconds) + "[-]";
		}
		foreach (SpellEffect spellEffect in spellAction.spellEffects)
		{
			text += GetSpellEffectDescription(spellEffect);
		}
		return text;
	}

	private string GetSpellEffectDescription(SpellEffect effect)
	{
		if (effect.hideDesc)
		{
			return "";
		}
		EffectTemplate effectT = effect.effectT;
		if (effectT == null)
		{
			return "";
		}
		return "\n\n" + effectT.GetDescription();
	}

	public SpellAction GetActionById(int spellActionId)
	{
		return spellActions.FirstOrDefault((SpellAction action) => action.ID == spellActionId);
	}

	public List<SpellAction> GetStartingActions()
	{
		return startActions.Distinct().Select(GetActionById).ToList();
	}

	public List<SpellAction> GetParentActions(SpellAction action)
	{
		return spellActions.Where((SpellAction a) => a.chainedActionIDs.Contains(action.ID)).ToList();
	}

	private List<SpellAction> GetOverrideActions(int classRank)
	{
		List<int> overrideEffects = SpellTemplates.GetOverrideEffectIDs(ID, classRank);
		if (overrideEffects.Count == 0)
		{
			return new List<SpellAction>();
		}
		return spellActions.Where((SpellAction a) => a.effectReq != null && overrideEffects.Contains(a.effectReq.effectID)).ToList();
	}

	public SpellAction GetFirstCastableAction(Entity caster)
	{
		foreach (SpellAction startingAction in GetStartingActions())
		{
			if (startingAction.IsCastable(caster))
			{
				return startingAction;
			}
		}
		return null;
	}

	public List<SpellAction> GetCastableActions(Entity caster)
	{
		List<SpellAction> list = new List<SpellAction>();
		Queue<SpellAction> queue = new Queue<SpellAction>();
		foreach (SpellAction startingAction in GetStartingActions())
		{
			if (startingAction.IsCastable(caster))
			{
				queue.Enqueue(startingAction);
				if (startingAction.stopIfReqsMet)
				{
					break;
				}
			}
		}
		while (queue.Count > 0)
		{
			SpellAction spellAction = queue.Dequeue();
			list.Add(spellAction);
			foreach (SpellAction chainedAction in GetChainedActions(spellAction))
			{
				if (chainedAction.IsCastable(caster))
				{
					queue.Enqueue(chainedAction);
					if (chainedAction.stopIfReqsMet)
					{
						break;
					}
				}
			}
		}
		return list;
	}

	public SpellAction GetCastableRootAction(SpellAction spellAction, Entity caster)
	{
		while (!GetStartingActions().Contains(spellAction))
		{
			spellAction = spellActions.FirstOrDefault((SpellAction a) => a.IsCastable(caster) && a.chainedActionIDs.Contains(spellAction.ID));
			if (spellAction == null)
			{
				return null;
			}
		}
		return spellAction;
	}

	public SpellAction GetFirstAction(Entity caster, List<ComEntityUpdate> updates)
	{
		int num = int.MaxValue;
		foreach (ComEntityUpdate update in updates)
		{
			if (update.spellActionId > 0 && update.spellActionId < num)
			{
				num = update.spellActionId;
			}
		}
		if (num == int.MaxValue)
		{
			num = GetFirstCastableAction(caster)?.ID ?? primaryAction.ID;
		}
		return GetActionById(num);
	}

	public List<SpellAction> GetChainedActions(SpellAction action)
	{
		return action.chainedActionIDs.Select(GetActionById).ToList();
	}

	public bool HasHybridTargets(Entity caster)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (SpellAction startingAction in GetStartingActions())
		{
			if (startingAction.IsCastable(caster))
			{
				if (startingAction.isHarmful)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
		}
		return flag && flag2;
	}

	public Entity GetAoeSource(SpellAction spellAction, Entity caster, Entity target)
	{
		if (target == null || GetStartingActions().Contains(spellAction) || spellAction.casterAlwaysAoeSource)
		{
			return caster;
		}
		return target;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;

public class Item : EquipItem, IUIItem
{
	public const int TreasureShardID = 1742;

	public string Name;

	public string Desc;

	public bool IsMC;

	public int Cost;

	public int Level;

	public ItemType Type;

	private string icon;

	public int Qty;

	public RarityType Rarity;

	public float MaxHealthControl;

	public float AttackControl;

	public float ArmorControl;

	public float EvasionControl;

	public float CritControl;

	public float HasteControl;

	public float MaxHealth;

	public float Attack;

	public float Armor;

	public float Evasion;

	public float Crit;

	public float Haste;

	public int MaxStack = 1;

	public int SpellID;

	public bool OverrideSellback;

	public float SellbackMultiplier;

	public bool IsGuardian;

	public bool IsSeasonal;

	public bool IsUnique;

	public bool IsNontransferable;

	public bool IsCosmetic;

	public bool IsCombat;

	public bool IsTravelForm;

	public bool IsLootBoxItem;

	public bool IsNew;

	public int timesInfusable;

	public TravelParams TravelParams;

	public ItemAction Action;

	public int PowerOffset;

	public int TradeSkillLevel;

	public TradeSkillType TradeSkillID;

	public int GuildLevel;

	public int MapAssetID;

	public int MapID;

	public string ImagePath;

	public static Dictionary<RarityType, string> RarityColorMap = new Dictionary<RarityType, string>
	{
		{
			RarityType.Junk,
			"4D4D4D"
		},
		{
			RarityType.Common,
			"000000"
		},
		{
			RarityType.Uncommon,
			"005000"
		},
		{
			RarityType.Rare,
			"003E63"
		},
		{
			RarityType.Epic,
			"61008C"
		},
		{
			RarityType.Legendary,
			"C03E00"
		}
	};

	public static Dictionary<RarityType, string> ChatRarityColorMap = new Dictionary<RarityType, string>
	{
		{
			RarityType.Junk,
			"444444"
		},
		{
			RarityType.Common,
			"FFFFFF"
		},
		{
			RarityType.Uncommon,
			"00BC00"
		},
		{
			RarityType.Rare,
			"227fFF"
		},
		{
			RarityType.Epic,
			"C031FF"
		},
		{
			RarityType.Legendary,
			"FF9C1E"
		}
	};

	public virtual int DisplayLevel => Level;

	public virtual int DisplayPowerOffset => PowerOffset;

	public virtual float DisplayMaxHealth => MaxHealth;

	public virtual float DisplayAttack => Attack;

	public virtual float DisplayArmor => Armor;

	public virtual float DisplayEvasion => Evasion;

	public virtual float DisplayCrit => Crit;

	public virtual float DisplayHaste => Haste;

	public virtual int DisplayTimesInfusable => timesInfusable;

	public bool IsHidden => Type == ItemType.Chest;

	public bool TakesBagSpace
	{
		get
		{
			if (Type != ItemType.Chest && Type != ItemType.Token && Type != ItemType.ClassToken && Type != ItemType.Crystal && Type != ItemType.Pet && Type != ItemType.Moment && Type != ItemType.HouseItem && !IsResource)
			{
				return !IsTravelForm;
			}
			return false;
		}
	}

	public bool IsRemovable
	{
		get
		{
			if (!TakesBagSpace)
			{
				return IsResource;
			}
			return true;
		}
	}

	public bool IsResource
	{
		get
		{
			if (Type != ItemType.Fish)
			{
				return Type == ItemType.Ore;
			}
			return true;
		}
	}

	public string ToolTipText => GetToolTip();

	public string RarityColor => GetRarityColor(Rarity);

	public string ChatRarityColor => GetChatRarityColor(Rarity);

	public bool HasStats
	{
		get
		{
			if (Type == ItemType.HouseItem || Type == ItemType.Map)
			{
				return false;
			}
			if (!(DisplayMaxHealth > 0f) && !(DisplayAttack > 0f) && !(DisplayArmor > 0f) && !(DisplayEvasion > 0f) && !(DisplayCrit > 0f))
			{
				return DisplayHaste > 0f;
			}
			return true;
		}
	}

	public string Icon
	{
		get
		{
			if (string.IsNullOrEmpty(icon))
			{
				return TypeIcon;
			}
			if (Icons.Length == 0)
			{
				return "";
			}
			return Icons[0];
		}
		set
		{
			icon = value;
		}
	}

	public string[] Icons => icon.Split(',');

	public string IconFg
	{
		get
		{
			if (string.IsNullOrEmpty(icon))
			{
				return "";
			}
			if (Icons.Length <= 1)
			{
				return "";
			}
			return Icons[1];
		}
	}

	public string TypeIcon => ItemIcons.GetTypeIcon(Type);

	public bool IsEquipType
	{
		get
		{
			if (base.EquipSlot != 0 && Type != ItemType.Map)
			{
				return Type != ItemType.HouseItem;
			}
			return false;
		}
	}

	public bool CanBeEquipped
	{
		get
		{
			if (IsEquipType && (IsCosmetic || Entities.Instance.me.Level >= Level) && (!base.IsTool || Entities.Instance.me.tradeSkillLevel[TradeSkillID] >= TradeSkillLevel))
			{
				if (IsGuardian)
				{
					return Session.MyPlayerData.IsGuardian();
				}
				return true;
			}
			return false;
		}
	}

	public bool HasPreview
	{
		get
		{
			if (!IsEquipType && TravelParams == null && Type != ItemType.HouseItem)
			{
				return Type == ItemType.Map;
			}
			return true;
		}
	}

	public bool IsUsable
	{
		get
		{
			if (SpellID > 0 || Action != null)
			{
				return base.EquipSlot != EquipItemSlot.Pet;
			}
			return false;
		}
	}

	public string ChestSprite => ItemIcons.GetChestIcon(Rarity);

	protected float RarityLevelDiff => ItemRarity.GetItemRarity(Rarity).levelDiff;

	public Item()
	{
	}

	public Item(Item item, int qty)
		: base(item)
	{
		Name = item.Name;
		Desc = item.Desc;
		Cost = item.Cost;
		Level = item.Level;
		Type = item.Type;
		Icon = item.Icon;
		Rarity = item.Rarity;
		MaxHealth = item.MaxHealth;
		Attack = item.Attack;
		Armor = item.Armor;
		Evasion = item.Evasion;
		Crit = item.Crit;
		Haste = item.Haste;
		MaxStack = item.MaxStack;
		SpellID = item.SpellID;
		IsMC = item.IsMC;
		Qty = qty;
		IsUnique = item.IsUnique;
		IsNontransferable = item.IsNontransferable;
		IsLootBoxItem = item.IsLootBoxItem;
		TravelParams = item.TravelParams;
		Action = item.Action;
		TradeSkillLevel = item.TradeSkillLevel;
		TradeSkillID = item.TradeSkillID;
		PowerOffset = item.PowerOffset;
		timesInfusable = item.timesInfusable;
		MaxHealthControl = item.MaxHealthControl;
		AttackControl = item.AttackControl;
		ArmorControl = item.ArmorControl;
		EvasionControl = item.EvasionControl;
		CritControl = item.CritControl;
		HasteControl = item.HasteControl;
		MapAssetID = item.MapAssetID;
		MapID = item.MapID;
		ImagePath = item.ImagePath;
	}

	public string GetToolTip(bool showStats = true)
	{
		string text = "";
		text = "[" + RarityColor + "]" + Name + "[-]";
		text = text + "\n" + GetTagline(showStats);
		if (showStats && HasStats)
		{
			text = text + "\n" + Session.MyPlayerData.GetComparisonStatText(this) + "[-]";
		}
		return text + "\n" + GetDescription();
	}

	public virtual string GetDescription()
	{
		string text = (string.IsNullOrEmpty(Desc) ? "" : ("[37200D]" + Desc + "[-]"));
		EffectTemplate.EffectGroup effectGroup = GetEffectGroup();
		if (effectGroup != 0)
		{
			text = text + "\n[000000]One " + effectGroup.ToString() + " effect can be active at a time.";
		}
		if (TravelParams != null && (Type == ItemType.Item || Type == ItemType.Consumable))
		{
			text = text + "\n[892800]Travel Speed: [-][D73D00]" + TravelParams.SpeedText + "[-]";
		}
		text += GetSpellDescription();
		text += "\n";
		if (base.IsTool)
		{
			string text2 = ((TradeSkillLevel > Entities.Instance.me.tradeSkillLevel[TradeSkillID]) ? "[ad0000]" : "[000000]");
			text = text + "\n" + text2 + "Required " + Enum.GetName(typeof(TradeSkillType), TradeSkillID) + " Level: " + TradeSkillLevel + "[-]";
		}
		else if (IsEquipType)
		{
			string text3 = ((DisplayLevel > Entities.Instance.me.Level) ? "[ad0000]" : "[000000]");
			text = text + "\n" + text3 + "Required Level: " + DisplayLevel + "[-]";
		}
		text += ((MaxStack > 1) ? ("\n[000000]Stack size: " + MaxStack + "[-]") : "");
		text += (IsGuardian ? "\n[007778]Guardian Only[-]" : "");
		text += (IsUnique ? "\n[007778]Unique Item[-]" : "");
		return text.Trim();
	}

	public virtual string GetDescriptionForGuildPower()
	{
		return string.Concat("" + GetSpellEffectsDescription(), "\n").Trim();
	}

	public virtual string GetDescriptionForGuildPowerConfirmation()
	{
		return (string.IsNullOrEmpty(Desc) ? "" : ("[37200D]" + Desc + "[-]")).Trim();
	}

	public string GetSpellDescription()
	{
		if (SpellID == 0)
		{
			return "";
		}
		SpellTemplate spell = GetSpell();
		if (spell == null)
		{
			return "";
		}
		if (Type != 0 && Type != ItemType.Consumable)
		{
			return "";
		}
		return spell.GetDescriptionForItem(Level);
	}

	public string GetSpellEffectsDescription()
	{
		if (SpellID == 0)
		{
			return "";
		}
		SpellTemplate spell = GetSpell();
		if (spell == null)
		{
			return "";
		}
		return spell.GetDescriptionForItemWithoutBaseSpellInfo(Level);
	}

	public SpellTemplate GetSpell()
	{
		return SpellTemplates.Get(SpellID, Entities.Instance.me.effects, Entities.Instance.me.ScaledClassRank, Entities.Instance.me.EquippedClassID, 0);
	}

	public EffectTemplate.EffectGroup GetEffectGroup()
	{
		return (from action in GetSpell()?.descSpellActions
			from effect in action.spellEffects
			where effect.effectT.effectGroup != EffectTemplate.EffectGroup.None
			select effect.effectT.effectGroup).FirstOrDefault() ?? EffectTemplate.EffectGroup.None;
	}

	public static string GetRarityColor(RarityType rarity)
	{
		if (RarityColorMap.ContainsKey(rarity))
		{
			return RarityColorMap[rarity];
		}
		return RarityColorMap[RarityType.Junk];
	}

	public static string GetChatRarityColor(RarityType rarity)
	{
		if (ChatRarityColorMap.ContainsKey(rarity))
		{
			return ChatRarityColorMap[rarity];
		}
		return ChatRarityColorMap[RarityType.Junk];
	}

	public int GetCombatPower()
	{
		if (!ItemSlots.IsPowerSlot(base.EquipSlot) || IsCosmetic)
		{
			return 0;
		}
		float num = Mathf.FloorToInt(((float)DisplayLevel + RarityLevelDiff) * 20f) + DisplayPowerOffset;
		if (!(num > 1f))
		{
			return 1;
		}
		return (int)num;
	}

	public int GetTradeSkillPower()
	{
		if (!base.IsTool || IsCosmetic)
		{
			return 0;
		}
		float num = Mathf.FloorToInt(((float)TradeSkillLevel + RarityLevelDiff) * 20f) + DisplayPowerOffset;
		if (!(num > 1f))
		{
			return 1;
		}
		return (int)num;
	}

	public string GetTypeText()
	{
		switch (Type)
		{
		case ItemType.Item:
			if (IsUsable)
			{
				if (TravelParams == null)
				{
					return "Reusable";
				}
				return "Travel Form";
			}
			return Type.ToString();
		case ItemType.Consumable:
		{
			if (TravelParams != null)
			{
				return "Consumable - Travel Form";
			}
			EffectTemplate.EffectGroup effectGroup = GetEffectGroup();
			if (effectGroup != 0)
			{
				return "Consumable - " + effectGroup;
			}
			return "Consumable";
		}
		case ItemType.QuestItem:
			return "Quest Item";
		case ItemType.ClassToken:
			return "Class Token";
		case ItemType.FishingRod:
			return "Fishing Rod";
		case ItemType.PickAxe:
			return "Pickaxe";
		case ItemType.Crystal:
			return "Travel Crystal";
		case ItemType.Sword:
			return "Melee Weapon";
		case ItemType.Pistol:
			return "Gun";
		case ItemType.HouseItem:
			return "House Item";
		case ItemType.Map:
			return "House Plan";
		default:
			return Type.ToString();
		}
	}

	public static string GetSlotText(EquipItemSlot slot)
	{
		return slot switch
		{
			EquipItemSlot.Weapon => "Melee Weapon", 
			EquipItemSlot.Pistol => "Gun", 
			_ => slot.ToString(), 
		};
	}

	public string GetTagline(bool showPower = true)
	{
		string text = "";
		text = (base.IsTool ? ((Entities.Instance.me.tradeSkillLevel[TradeSkillID] >= TradeSkillLevel) ? (text + "[000000]") : (text + "[ad0000]" + Enum.GetName(typeof(TradeSkillType), TradeSkillID) + " Level " + TradeSkillLevel + "[-][000000] ")) : ((!ItemSlots.IsPowerSlot(base.EquipSlot)) ? (text + "[000000]") : ((Entities.Instance.me.Level >= DisplayLevel) ? (text + "[000000]") : (text + "[ad0000]Level " + DisplayLevel + "[-][000000] "))));
		if (!showPower)
		{
			text = text + Rarity.ToString() + " ";
		}
		if (IsCosmetic)
		{
			text += "Cosmetic ";
		}
		else if (IsEquipType && base.EquipSlot != EquipItemSlot.Pet && showPower)
		{
			if (base.IsTool)
			{
				if (Entities.Instance.me.tradeSkillLevel[TradeSkillID] >= TradeSkillLevel)
				{
					text = text + "Power " + GetTradeSkillPower() + " ";
				}
			}
			else if (Entities.Instance.me.Level >= DisplayLevel)
			{
				text = text + "Power " + GetCombatPower() + " ";
			}
		}
		if (IsGuardian)
		{
			text += (Session.MyPlayerData.IsGuardian() ? "[000000]" : "[ad0000]");
			text += "Guardian [-]";
		}
		return text + "[000000]" + GetTypeText() + " [-]";
	}

	public static int GetLootBoxItemTokenCost(RarityType rarity)
	{
		return rarity switch
		{
			RarityType.Legendary => 5000, 
			RarityType.Epic => 1000, 
			RarityType.Rare => 300, 
			_ => 100, 
		};
	}

	public static int GetLootBoxItemTokenSellPrice(RarityType rarity)
	{
		return Mathf.FloorToInt((float)GetLootBoxItemTokenCost(rarity) / 5f);
	}

	public void RecalculateStats()
	{
		MaxHealth = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.MaxHealth, MaxHealthControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
		Attack = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.Attack, AttackControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
		Armor = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.Armor, ArmorControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
		Evasion = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.Evasion, EvasionControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
		Crit = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.Crit, CritControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
		Haste = (IsCosmetic ? 0f : GameCurves.GetRealItemStatValue(Stat.Haste, HasteControl, base.EquipSlot, (float)Level + RarityLevelDiff + (float)PowerOffset / 20f));
	}
}

using StatCurves;

public class ShopItem : Item
{
	public bool HideUnavailable;

	public string BitFlagName;

	public byte BitFlagIndex;

	public string RequiredBadgeName;

	public int QSIndex;

	public int QSValue;

	public string RequirementText;

	public int WarID;

	public float WarProgress;

	public int SortOrder;

	public int ClassID;

	public int ClassRank;

	public bool IsNotBuyable;

	public int TokenID;

	public int TokenQty;

	public bool IsScaling;

	public int MapLevel;

	public int MapMaxLevel;

	public int ScaleMapOverride;

	public int GloryLevel;

	public override int DisplayLevel
	{
		get
		{
			if (!IsScaling)
			{
				return Level;
			}
			return Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Entities.Instance.me.Level, MapLevel, MapMaxLevel), Level);
		}
	}

	public override float DisplayMaxHealth
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.MaxHealth, MaxHealthControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public override float DisplayAttack
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.Attack, AttackControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public override float DisplayArmor
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.Armor, ArmorControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public override float DisplayEvasion
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.Evasion, EvasionControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public override float DisplayCrit
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.Crit, CritControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public override float DisplayHaste
	{
		get
		{
			if (!IsCosmetic)
			{
				return GameCurves.GetRealItemStatValue(Stat.Haste, HasteControl, base.EquipSlot, (float)DisplayLevel + base.RarityLevelDiff + (float)DisplayPowerOffset / 20f);
			}
			return 0f;
		}
	}

	public virtual string CurrencyString
	{
		get
		{
			if (TokenID > 0)
			{
				Item item = Items.Get(TokenID);
				if (item != null)
				{
					return item.Name;
				}
				return "";
			}
			if (!IsMC)
			{
				return "Gold";
			}
			return "Dragon Crystals";
		}
	}

	public virtual int CurrencyCost
	{
		get
		{
			if (TokenID <= 0)
			{
				return Cost;
			}
			return TokenQty;
		}
	}

	public string CurrencyIcon
	{
		get
		{
			if (TokenID > 0)
			{
				Item item = Items.Get(TokenID);
				if (item != null)
				{
					return item.Icon;
				}
				return "";
			}
			if (!IsMC)
			{
				return "Coin";
			}
			return "DragonGem";
		}
	}

	public string CurrencyIconFg
	{
		get
		{
			if (TokenID > 0)
			{
				Item item = Items.Get(TokenID);
				if (item != null)
				{
					return item.IconFg;
				}
				return "";
			}
			return "";
		}
	}

	public bool IsAvailable()
	{
		if (Wars.HasKey(WarID) && Wars.Get(WarID).ProgressPercent < WarProgress)
		{
			return false;
		}
		if (!Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex))
		{
			return false;
		}
		if (Session.MyPlayerData.GetQSValue(QSIndex) < QSValue)
		{
			return false;
		}
		if (Session.MyPlayerData.GetClassRank(ClassID) < ClassRank)
		{
			return false;
		}
		if (Session.MyPlayerData.GloryLevel < GloryLevel)
		{
			return false;
		}
		return true;
	}

	public string GetLockInfo()
	{
		if (!string.IsNullOrEmpty(RequirementText))
		{
			return RequirementText;
		}
		if (Wars.HasKey(WarID) && Wars.Get(WarID).ProgressPercent < WarProgress)
		{
			return (int)(WarProgress * 100f) + "% progress required in " + Wars.Get(WarID).Name + " war to unlock this item.";
		}
		if (!Session.MyPlayerData.CheckBitFlag(BitFlagName, BitFlagIndex))
		{
			return RequiredBadgeName + " Badge required to unlock this item.";
		}
		if (Session.MyPlayerData.GetQSValue(QSIndex) < QSValue)
		{
			return "Quest requirement not met.";
		}
		if (Session.MyPlayerData.GetClassRank(ClassID) < ClassRank)
		{
			return "Requires Rank " + ClassRank + " " + CombatClass.GetClassByID(ClassID).Name + " to unlock this item.";
		}
		if (Session.MyPlayerData.GloryLevel < GloryLevel)
		{
			return "Requires Glory Level " + GloryLevel + " to unlock this item.";
		}
		return null;
	}

	public bool IsVisibleInStore()
	{
		if (!IsAvailable())
		{
			return !HideUnavailable;
		}
		return true;
	}
}

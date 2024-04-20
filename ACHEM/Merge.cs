using System;
using System.Collections.Generic;
using StatCurves;

public class Merge : ShopItem
{
	public int MergeID;

	public int ItemID;

	public float MergeMinutes;

	public int MergeCost;

	public bool IsCraftOnly;

	public bool IsSpeedUpDisabled;

	public int InvLevel;

	public List<MergeItem> MergeItems = new List<MergeItem>();

	public DateTime? TSComplete { get; set; }

	public override int DisplayLevel
	{
		get
		{
			if (InvLevel > 0)
			{
				return InvLevel;
			}
			if (!ItemTypes.IsScaling(Type))
			{
				return Level;
			}
			if (ScaleMapOverride > 0)
			{
				if (!IsScaling)
				{
					return Level;
				}
				return Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Level, MapLevel, MapMaxLevel), Level);
			}
			if (!Game.Instance.AreaData.DoRewardsScale)
			{
				return Level;
			}
			return Entities.Instance.me.GetRelativeLevel(Entities.Instance.me.ScaledLevel, Level);
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

	public override string GetDescription()
	{
		return base.GetDescription();
	}
}

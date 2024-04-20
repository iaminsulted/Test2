using StatCurves;

public class QuestRewardItem : Item
{
	public float Rate;

	public byte RewardType;

	public bool IsScaling;

	public int MapLevel;

	public int MapMaxLevel;

	public override int DisplayLevel
	{
		get
		{
			if (IsScaling)
			{
				return Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Entities.Instance.me.Level, MapLevel, MapMaxLevel), Level);
			}
			return Level;
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
}

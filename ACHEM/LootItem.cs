using StatCurves;

public class LootItem : Item
{
	public class LootItemAddress
	{
		public int LootBagID;

		public int LootItemID;

		public LootItemAddress()
		{
		}

		public LootItemAddress(int lootbagID, int lootItemID)
		{
			LootBagID = lootbagID;
			LootItemID = lootItemID;
		}
	}

	public int LootItemID;

	public int LootBagID;

	public int ItemID;

	public int LooterID;

	public int InvLevel;

	public int InvPowerOffset;

	public int RarityModifier;

	public override int DisplayLevel => InvLevel;

	public override int DisplayPowerOffset => InvPowerOffset;

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

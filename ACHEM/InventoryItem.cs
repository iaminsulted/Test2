using System;

public class InventoryItem : Item
{
	public enum Equip
	{
		None,
		Stat,
		Cosmetic
	}

	public DateTime DateAcquired;

	public float InvMaxHealth;

	public float InvAttack;

	public float InvEvasion;

	public float InvCrit;

	public float InvHaste;

	public float InvArmor;

	public int modifierID;

	public ItemModifier modifier;

	public int ShopID;

	public bool IsCollectionItem;

	public int CollectionBadgeID;

	public string ShopName;

	public bool IsEquipped => EquipID > Equip.None;

	public bool IsStatEquip => EquipID == Equip.Stat;

	public bool IsCosmeticEquip => EquipID == Equip.Cosmetic;

	public int CharItemID { get; set; }

	public int TransferID { get; set; }

	public int BankID { get; set; }

	public Equip EquipID { get; set; }

	public int InvLevel { get; set; }

	public int InvTimesInfusable { get; set; }

	public bool InInventory => BankID == 0;

	public string Tags { get; set; }

	public override int DisplayTimesInfusable => InvTimesInfusable;

	public override int DisplayLevel => InvLevel;

	public override float DisplayMaxHealth => InvMaxHealth;

	public override float DisplayAttack => InvAttack;

	public override float DisplayArmor => InvArmor;

	public override float DisplayEvasion => InvEvasion;

	public override float DisplayCrit => InvCrit;

	public override float DisplayHaste => InvHaste;

	public int SellPrice
	{
		get
		{
			if (IsCollectionItem)
			{
				return 0;
			}
			float num = (OverrideSellback ? SellbackMultiplier : 0.1f);
			return (int)((float)Cost * num);
		}
	}

	public override string GetDescription()
	{
		string description = base.GetDescription();
		description += ((base.IsRemovable && SellPrice > 0) ? ("\n[935C0F]Sell Price: " + SellPrice + " Gold[-]") : "");
		if (ShopID > 0)
		{
			if (CollectionBadgeID > 0)
			{
				return description + "\n[166d00]" + ShopName + " Item.[-]";
			}
			return description + (IsSeasonal ? ("\n[166d00]Obtained: " + DateAcquired.Year + "[-]") : "");
		}
		return description + (IsSeasonal ? ("\n[166d00]Obtained: " + DateAcquired.Year + "[-]") : "");
	}
}

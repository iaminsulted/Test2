using StatCurves;

namespace Assets.Scripts.Game;

public static class ItemIcons
{
	public static string GetCosmeticIconBySlot(EquipItemSlot equipSlot)
	{
		return equipSlot switch
		{
			EquipItemSlot.Weapon => "cosmetic_sword", 
			EquipItemSlot.Pistol => "cosmetic_gun", 
			EquipItemSlot.Bow => "cosmetic_bow", 
			EquipItemSlot.FishingRod => "cosmetic_fishingrod", 
			_ => "cosmetic_mask", 
		};
	}

	public static string GetTypeIcon(ItemType itemType)
	{
		switch (itemType)
		{
		case ItemType.Shoulders:
			return "Icon-Shoulders";
		case ItemType.Belt:
			return "Icon-Belt";
		case ItemType.Gloves:
			return "Icon-Gloves";
		case ItemType.Boots:
			return "Icon-Boots";
		case ItemType.Robe:
			return "Icon-Robe";
		case ItemType.Class:
		case ItemType.Armor:
			return "Icon-Armor";
		case ItemType.Helm:
			return "Icon-Helm";
		case ItemType.Sword:
			return "Icon-Weapon";
		case ItemType.Pistol:
			return "Icon-Pistol";
		case ItemType.Back:
			return "Icon-Cape";
		case ItemType.Item:
			return "Icon-Itembag";
		case ItemType.ClassToken:
			return "Icon-ClassToken";
		case ItemType.Pet:
			return "Icon-Pet";
		case ItemType.Bow:
			return "Icon-Bow";
		case ItemType.FishingRod:
			return "Icon-FishingRod";
		case ItemType.Fish:
			return "Icon-Fish";
		case ItemType.Bobber:
			return "Icon-Bobber";
		default:
			return "Icon-Itembag";
		}
	}

	public static string GetChestIcon(RarityType rarity)
	{
		return rarity switch
		{
			RarityType.Common => "CommonChestImage", 
			RarityType.Uncommon => "CommonChestImage", 
			RarityType.Rare => "RareChestImage", 
			RarityType.Epic => "EpicChestImage", 
			RarityType.Legendary => "EpicChestImage", 
			_ => "CommonChestImage", 
		};
	}
}

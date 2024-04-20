using System;

namespace StatCurves
{
	// Token: 0x0200000E RID: 14
	public class ItemTypes
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00003EF4 File Offset: 0x000020F4
		public static bool IsScaling(ItemType type)
		{
			return type == ItemType.Armor || type == ItemType.Robe || type == ItemType.Belt || type == ItemType.Bracers || type == ItemType.Gloves || type == ItemType.Boots || type == ItemType.Shoulders || type == ItemType.Back || type == ItemType.Helm || type == ItemType.Sword || type == ItemType.Pistol || type == ItemType.Bow;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003F40 File Offset: 0x00002140
		public static int GetSortOrder(ItemType type)
		{
			int result;
			switch (type)
			{
			case ItemType.Item:
				result = 20;
				break;
			case ItemType.QuestItem:
				result = 30;
				break;
			case ItemType.Class:
			case ItemType.Armor:
				result = 4;
				break;
			case ItemType.Robe:
				result = 6;
				break;
			case ItemType.Belt:
				result = 7;
				break;
			case ItemType.Bracers:
				result = 5;
				break;
			case ItemType.Gloves:
				result = 5;
				break;
			case ItemType.Boots:
				result = 8;
				break;
			case ItemType.Shoulders:
				result = 3;
				break;
			case ItemType.Back:
				result = 6;
				break;
			case ItemType.Helm:
				result = 2;
				break;
			case ItemType.Sword:
			case ItemType.Pistol:
			case ItemType.Bow:
				result = 1;
				break;
			case ItemType.Consumable:
				result = 21;
				break;
			case ItemType.Chest:
				result = 40;
				break;
			case ItemType.Token:
				result = 25;
				break;
			case ItemType.ClassToken:
				result = 26;
				break;
			case ItemType.Pet:
				result = 9;
				break;
			case ItemType.Crystal:
				result = 50;
				break;
			case ItemType.FishingRod:
				result = 10;
				break;
			case ItemType.PickAxe:
				result = 11;
				break;
			case ItemType.Fish:
				result = 22;
				break;
			case ItemType.Ore:
				result = 23;
				break;
			case ItemType.Bobber:
				result = 10;
				break;
			default:
				result = int.MaxValue;
				break;
			}
			return result;
		}
	}
}

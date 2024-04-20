using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StatCurves
{
	// Token: 0x0200000A RID: 10
	public class ItemSlots
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003A14 File Offset: 0x00001C14
		public static bool IsPowerSlot(EquipItemSlot slot)
		{
			return slot != EquipItemSlot.None && slot != EquipItemSlot.Pet && slot != EquipItemSlot.Class && !ItemSlots.IsToolSlot(slot);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003A40 File Offset: 0x00001C40
		public static bool IsWeaponSlot(EquipItemSlot slot)
		{
			return slot == EquipItemSlot.Weapon || slot == EquipItemSlot.Pistol || slot == EquipItemSlot.Bow;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003A64 File Offset: 0x00001C64
		public static bool IsToolSlot(EquipItemSlot slot)
		{
			return slot == EquipItemSlot.FishingRod || slot == EquipItemSlot.Pickaxe;
		}

		// Token: 0x04000042 RID: 66
		private static readonly IReadOnlyList<EquipItemSlot> EquipPowerSlots = new ReadOnlyCollection<EquipItemSlot>(new List<EquipItemSlot>
		{
			EquipItemSlot.Armor,
			EquipItemSlot.Belt,
			EquipItemSlot.Gloves,
			EquipItemSlot.Boots,
			EquipItemSlot.Shoulders,
			EquipItemSlot.Back,
			EquipItemSlot.Helm,
			EquipItemSlot.Weapon
		});

		// Token: 0x04000043 RID: 67
		public static readonly int TotalPowerSlots = ItemSlots.EquipPowerSlots.Count;
	}
}

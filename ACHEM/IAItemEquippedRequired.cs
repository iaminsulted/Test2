using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Item Equipped Required")]
public class IAItemEquippedRequired : InteractionRequirement
{
	public int ItemID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = myPlayerData.IsItemEquipped(ItemID);
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public void OnEnable()
	{
		Session.MyPlayerData.ItemEquipped += OnEquipUpdate;
		Session.MyPlayerData.ItemUnequipped += OnEquipUpdate;
	}

	private void OnEquipUpdate(InventoryItem iItem)
	{
		if (iItem.ID == ItemID)
		{
			OnRequirementUpdate();
		}
	}

	public void OnDisable()
	{
		Session.MyPlayerData.ItemEquipped -= OnEquipUpdate;
		Session.MyPlayerData.ItemUnequipped -= OnEquipUpdate;
	}
}

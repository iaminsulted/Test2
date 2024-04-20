public class IAItemEquippedRequiredCore : IARequiredCore
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

	public IAItemEquippedRequiredCore()
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

	~IAItemEquippedRequiredCore()
	{
		Session.MyPlayerData.ItemEquipped -= OnEquipUpdate;
		Session.MyPlayerData.ItemUnequipped -= OnEquipUpdate;
	}
}

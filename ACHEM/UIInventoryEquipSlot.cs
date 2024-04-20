public class UIInventoryEquipSlot : UIItem
{
	public UISprite IconEquipped;

	public UISprite IconCostume;

	public override void Init(Item item)
	{
		base.Init(item);
		if (item is InventoryItem)
		{
			InventoryItem inventoryItem = item as InventoryItem;
			ShowIcon(inventoryItem.EquipID);
		}
	}

	private void ShowIcon(InventoryItem.Equip equipId)
	{
		if (IconEquipped != null)
		{
			IconEquipped.gameObject.SetActive(equipId == InventoryItem.Equip.Stat);
		}
		if (IconCostume != null)
		{
			IconCostume.gameObject.SetActive(equipId == InventoryItem.Equip.Cosmetic);
		}
	}

	private void OnItemUpdate(InventoryItem item)
	{
		Init(item);
	}
}

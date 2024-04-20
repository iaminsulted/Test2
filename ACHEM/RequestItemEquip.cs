public class RequestItemEquip : Request
{
	public int CharItemID;

	public InventoryItem.Equip EquipID;

	public RequestItemEquip()
	{
		type = 10;
		cmd = 1;
	}
}

public class ItemActionTransfer : ItemAction
{
	public int MapID;

	public int CellID;

	public int SpawnID;

	public ItemActionTransfer(int mapID, int cellID, int spawnID)
		: base(ItemActionType.Transfer)
	{
		MapID = mapID;
		CellID = cellID;
		SpawnID = spawnID;
	}
}

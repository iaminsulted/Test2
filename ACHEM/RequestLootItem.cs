public class RequestLootItem : Request
{
	public int LootBagID;

	public int LootItemID;

	public RequestLootItem(int lootBagID, int lootItemID)
	{
		type = 16;
		cmd = 3;
		LootBagID = lootBagID;
		LootItemID = lootItemID;
	}
}

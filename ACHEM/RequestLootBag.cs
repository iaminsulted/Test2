public class RequestLootBag : Request
{
	public int LootBagID;

	public RequestLootBag(int lootBagID)
	{
		type = 16;
		cmd = 4;
		LootBagID = lootBagID;
	}
}

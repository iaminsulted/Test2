public class RequestDustLootBoxItem : Request
{
	public int CharItemID;

	public RequestDustLootBoxItem()
	{
		type = 10;
		cmd = 11;
	}

	public RequestDustLootBoxItem(int charItemID)
	{
		type = 10;
		cmd = 11;
		CharItemID = charItemID;
	}
}

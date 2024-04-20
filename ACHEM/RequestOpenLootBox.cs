public class RequestOpenLootBox : Request
{
	public int ItemID;

	public RequestOpenLootBox()
	{
		type = 10;
		cmd = 9;
	}

	public RequestOpenLootBox(int itemID)
	{
		type = 10;
		cmd = 9;
		ItemID = itemID;
	}
}

public class RequestItemUse : Request
{
	public int CharItemID;

	public RequestItemUse()
	{
		type = 10;
		cmd = 6;
	}

	public RequestItemUse(int charItemID)
	{
		type = 10;
		cmd = 6;
		CharItemID = charItemID;
	}
}

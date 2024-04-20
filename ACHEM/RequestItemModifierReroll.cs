internal class RequestItemModifierReroll : Request
{
	public int CharItemID;

	public RequestItemModifierReroll()
	{
		type = 10;
		cmd = 19;
	}

	public RequestItemModifierReroll(int charItemID)
	{
		type = 10;
		cmd = 19;
		CharItemID = charItemID;
	}
}

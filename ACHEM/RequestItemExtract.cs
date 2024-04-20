public class RequestItemExtract : Request
{
	public int CharItemID;

	public RequestItemExtract()
	{
		type = 10;
		cmd = 14;
	}

	public RequestItemExtract(int charItemID)
	{
		type = 10;
		cmd = 14;
		CharItemID = charItemID;
	}
}

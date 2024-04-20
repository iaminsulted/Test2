public class RequestItemInfuse : Request
{
	public int CharItemID;

	public RequestItemInfuse()
	{
		type = 10;
		cmd = 13;
	}

	public RequestItemInfuse(int charItemID)
	{
		type = 10;
		cmd = 13;
		CharItemID = charItemID;
	}
}

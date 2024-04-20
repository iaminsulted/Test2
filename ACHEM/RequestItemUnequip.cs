public class RequestItemUnequip : Request
{
	public int CharItemID;

	public RequestItemUnequip()
	{
		type = 10;
		cmd = 2;
	}
}

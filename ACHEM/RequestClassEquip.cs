public class RequestClassEquip : Request
{
	public int ClassID;

	public RequestClassEquip()
	{
		type = 26;
		cmd = 2;
	}
}

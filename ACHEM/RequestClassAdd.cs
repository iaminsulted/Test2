public class RequestClassAdd : Request
{
	public int ClassID;

	public RequestClassAdd()
	{
		type = 26;
		cmd = 1;
	}
}

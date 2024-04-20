public class RequestMailDelete : Request
{
	public int charID;

	public int mailID;

	public RequestMailDelete()
	{
		type = 51;
		cmd = 4;
	}
}

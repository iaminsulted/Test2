public class RequestGoto : Request
{
	public int charID;

	public string link;

	public RequestGoto()
	{
		type = 29;
		cmd = 6;
	}

	public RequestGoto(int charID, string code)
	{
		type = 29;
		cmd = 6;
		this.charID = charID;
		link = code;
	}
}

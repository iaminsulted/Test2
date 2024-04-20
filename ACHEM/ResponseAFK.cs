public class ResponseAFK : Response
{
	public int EntityID;

	public bool isAFK;

	public ResponseAFK(bool afk)
	{
		type = 17;
		cmd = 22;
		isAFK = afk;
	}
}

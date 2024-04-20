public class ResponseGoto : Response
{
	public int CharID;

	public bool isDungeon;

	public string AreaName;

	public string ToPlayerName;

	public bool isPrivate;

	public string Code;

	public ResponseGoto()
	{
		type = 29;
		cmd = 6;
	}
}

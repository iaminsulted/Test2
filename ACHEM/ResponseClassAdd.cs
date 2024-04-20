public class ResponseClassAdd : Response
{
	public int ClassID;

	public CharClass charClass;

	public ResponseClassAdd()
	{
		type = 26;
		cmd = 1;
	}
}

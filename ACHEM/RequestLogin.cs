public class RequestLogin : Request
{
	public int id;

	public string token;

	public RequestLogin()
	{
		type = 3;
		cmd = Com.CmdNone;
	}
}

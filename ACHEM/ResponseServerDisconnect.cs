internal class ResponseServerDisconnect : Response
{
	public string message;

	public ResponseServerDisconnect(string msg)
	{
		type = 17;
		cmd = 23;
		message = msg;
	}
}

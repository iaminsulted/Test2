public class ResponseOpenInAdmin : Response
{
	public string URL;

	public ResponseOpenInAdmin(string URL)
	{
		type = 46;
		cmd = 21;
		this.URL = URL;
	}
}

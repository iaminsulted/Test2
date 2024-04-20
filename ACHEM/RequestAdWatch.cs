public class RequestAdWatch : Request
{
	public string AdToken;

	public RequestAdWatch()
	{
		type = 35;
	}

	public RequestAdWatch(string adToken)
	{
		type = 35;
		AdToken = adToken;
	}
}

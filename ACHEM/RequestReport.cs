public class RequestReport : Request
{
	public string UserName;

	public string Report;

	public string Comment;

	public RequestReport()
	{
		type = 23;
	}
}

public class ResponseAdWatch : Response
{
	public bool Success;

	public ResponseAdWatch()
	{
		type = 35;
	}

	public ResponseAdWatch(bool success)
	{
		type = 35;
		Success = success;
	}
}

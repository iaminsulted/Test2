public class WebApiResponse
{
	public static int Error_Basic = 1;

	public int Error;

	public string Message;

	public virtual bool Success => Error == 0;

	public bool HasError(int checkError)
	{
		return Error == checkError;
	}
}

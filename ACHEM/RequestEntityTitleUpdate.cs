public class RequestEntityTitleUpdate : Request
{
	public int Title;

	public RequestEntityTitleUpdate()
	{
		type = 17;
		cmd = 17;
	}

	public RequestEntityTitleUpdate(int title)
	{
		type = 17;
		cmd = 17;
		Title = title;
	}
}

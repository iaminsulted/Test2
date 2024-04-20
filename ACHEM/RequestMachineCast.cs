public class RequestMachineCast : Request
{
	public int ID;

	public RequestMachineCast()
	{
		type = 19;
		cmd = 6;
	}

	public RequestMachineCast(int id)
	{
		type = 19;
		cmd = 6;
		ID = id;
	}
}

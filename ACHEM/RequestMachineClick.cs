public class RequestMachineClick : Request
{
	public int ID;

	public RequestMachineClick()
	{
		type = 19;
		cmd = 3;
	}

	public RequestMachineClick(int id)
	{
		type = 19;
		cmd = 3;
		ID = id;
	}
}

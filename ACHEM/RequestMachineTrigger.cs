public class RequestMachineTrigger : Request
{
	public int ID;

	public RequestMachineTrigger()
	{
		type = 19;
		cmd = 11;
	}

	public RequestMachineTrigger(int id)
	{
		type = 19;
		cmd = 11;
		ID = id;
	}
}

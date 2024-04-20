public class RequestMachineResourceTrigger : Request
{
	public int machineID;

	public RequestMachineResourceTrigger()
	{
		type = 19;
		cmd = 24;
	}

	public RequestMachineResourceTrigger(int machineID)
	{
		type = 19;
		cmd = 24;
		this.machineID = machineID;
	}
}

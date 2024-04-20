public class RequestMachineResourceDrop : Request
{
	public int MachineID;

	public RequestMachineResourceDrop()
	{
		type = 19;
		cmd = 20;
	}

	public RequestMachineResourceDrop(int machineID)
	{
		type = 19;
		cmd = 20;
		MachineID = machineID;
	}
}

public class RequestMachineResourceCollect : Request
{
	public int MachineID;

	public int PrimaryItemID;

	public int RareItemID;

	public RequestMachineResourceCollect()
	{
		type = 19;
		cmd = 18;
	}

	public RequestMachineResourceCollect(int machineID, int primaryItemID, int rareItemID)
	{
		type = 19;
		cmd = 18;
		MachineID = machineID;
		PrimaryItemID = primaryItemID;
		RareItemID = rareItemID;
	}
}

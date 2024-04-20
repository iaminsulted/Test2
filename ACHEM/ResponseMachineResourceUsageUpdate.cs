public class ResponseMachineResourceUsageUpdate : Response
{
	public int MachineID;

	public int Usages;

	public ResponseMachineResourceUsageUpdate(int machineID, int usages)
	{
		type = 19;
		cmd = 25;
		MachineID = machineID;
		Usages = usages;
	}
}

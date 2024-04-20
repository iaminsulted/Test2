public class ResponseMachineResourceInterrupt : Response
{
	public int MachineID;

	public ResponseMachineResourceInterrupt()
	{
		type = 19;
		cmd = 22;
	}

	public ResponseMachineResourceInterrupt(int machineID)
	{
		type = 19;
		cmd = 22;
		MachineID = machineID;
	}
}

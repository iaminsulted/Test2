public class ResponseMachineResourceDespawn : Response
{
	public int MachineID;

	public ResponseMachineResourceDespawn(int machineID)
	{
		type = 19;
		cmd = 19;
		MachineID = machineID;
	}
}

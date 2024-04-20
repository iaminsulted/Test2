public class ResponseMachineUpdate : Response
{
	public int MachineID;

	public byte State;

	public int EntityId;

	public ResponseMachineUpdate()
	{
		type = 19;
		cmd = 2;
	}

	public ResponseMachineUpdate(int machineID, byte state)
	{
		type = 19;
		cmd = 2;
		MachineID = machineID;
		State = state;
	}

	public ResponseMachineUpdate(int machineID, byte state, int id)
	{
		type = 19;
		cmd = 2;
		MachineID = machineID;
		State = state;
		EntityId = id;
	}
}

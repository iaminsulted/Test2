public class ResponseMachineHarpoonFire : Response
{
	public int machineID;

	public ResponseMachineHarpoonFire()
	{
		type = 19;
		cmd = 15;
	}

	public ResponseMachineHarpoonFire(int machineID)
	{
		type = 19;
		cmd = 15;
		this.machineID = machineID;
	}
}

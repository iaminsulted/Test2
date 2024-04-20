public class RequestMachineHarpoonFire : Request
{
	public int machineID;

	public RequestMachineHarpoonFire()
	{
		type = 19;
		cmd = 15;
	}

	public RequestMachineHarpoonFire(int machineID)
	{
		type = 19;
		cmd = 15;
		this.machineID = machineID;
	}
}

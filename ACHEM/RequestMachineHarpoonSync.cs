public class RequestMachineHarpoonSync : Request
{
	public int machineID;

	public float verticalRotation;

	public float horizontalRotation;

	public RequestMachineHarpoonSync()
	{
		type = 19;
		cmd = 14;
	}

	public RequestMachineHarpoonSync(int machineID, float verticalRotation, float horizontalRotation)
	{
		type = 19;
		cmd = 14;
		this.machineID = machineID;
		this.verticalRotation = verticalRotation;
		this.horizontalRotation = horizontalRotation;
	}
}

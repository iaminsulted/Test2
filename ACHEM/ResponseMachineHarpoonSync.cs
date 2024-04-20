public class ResponseMachineHarpoonSync : Response
{
	public int machineID;

	public float verticalRotation;

	public float horizontalRotation;

	public ResponseMachineHarpoonSync()
	{
		type = 19;
		cmd = 14;
	}

	public ResponseMachineHarpoonSync(int machineID, float verticalRotation, float horizontalRotation)
	{
		type = 19;
		cmd = 14;
		this.machineID = machineID;
		this.verticalRotation = verticalRotation;
		this.horizontalRotation = horizontalRotation;
	}
}

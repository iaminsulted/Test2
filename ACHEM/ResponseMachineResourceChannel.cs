public class ResponseMachineResourceChannel : Response
{
	public int machineID;

	public float time;

	public ResponseMachineResourceChannel(int machineID, float time)
	{
		type = 19;
		cmd = 17;
		this.machineID = machineID;
		this.time = time;
	}
}

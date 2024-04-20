public class RequestUpdateRequirements : Request
{
	public int SpawnID;

	public string Requirements;

	public RequestUpdateRequirements(int SpawnID, string Requirements)
	{
		type = 46;
		cmd = 23;
		this.SpawnID = SpawnID;
		this.Requirements = Requirements;
	}
}

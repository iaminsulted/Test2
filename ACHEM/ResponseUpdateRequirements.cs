public class ResponseUpdateRequirements : Response
{
	public int SpawnID;

	public string Requirements;

	public ResponseUpdateRequirements(int SpawnID, string Requirements)
	{
		type = 46;
		cmd = 24;
		this.SpawnID = SpawnID;
		this.Requirements = Requirements;
	}
}

public class RequestNPCDialogueStart : Request
{
	public int spawnID;

	public bool objective;

	public RequestNPCDialogueStart()
	{
		type = 18;
		cmd = 3;
	}

	public RequestNPCDialogueStart(int spawnID, bool objective)
	{
		type = 18;
		cmd = 3;
		this.spawnID = spawnID;
		this.objective = objective;
	}
}

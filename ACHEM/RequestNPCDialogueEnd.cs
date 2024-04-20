public class RequestNPCDialogueEnd : Request
{
	public int spawnID;

	public RequestNPCDialogueEnd()
	{
		type = 18;
		cmd = 4;
	}

	public RequestNPCDialogueEnd(int spawnID)
	{
		type = 18;
		cmd = 4;
		this.spawnID = spawnID;
	}
}

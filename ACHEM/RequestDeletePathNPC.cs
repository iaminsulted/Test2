public class RequestDeletePathNPC : Request
{
	public int SpawnID;

	public int PathID;

	public RequestDeletePathNPC(int SpawnID, int PathID)
	{
		type = 46;
		cmd = 10;
		this.SpawnID = SpawnID;
		this.PathID = PathID;
	}
}

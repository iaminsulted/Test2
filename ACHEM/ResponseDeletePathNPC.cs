public class ResponseDeletePathNPC : Response
{
	public int SpawnID;

	public int PathID;

	public ResponseDeletePathNPC(int SpawnID, int PathID)
	{
		type = 46;
		cmd = 19;
		this.SpawnID = SpawnID;
		this.PathID = PathID;
	}
}

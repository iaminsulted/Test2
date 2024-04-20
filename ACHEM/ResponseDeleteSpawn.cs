public class ResponseDeleteSpawn : Response
{
	public int SpawnID;

	public ResponseDeleteSpawn(int SpawnID)
	{
		type = 46;
		cmd = 14;
		this.SpawnID = SpawnID;
	}
}

public class RequestDeleteSpawn : Request
{
	public int SpawnID;

	public RequestDeleteSpawn(int SpawnID)
	{
		type = 46;
		cmd = 5;
		this.SpawnID = SpawnID;
	}
}

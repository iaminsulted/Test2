public class RequestAddSpawn : Request
{
	public int SpawnListNpcID;

	public int SpawnID;

	public int NpcID;

	public RequestAddSpawn()
	{
		type = 46;
		cmd = 4;
	}

	public RequestAddSpawn(int SpawnListNpcID, int SpawnID, int NpcID)
	{
		type = 46;
		cmd = 4;
		this.SpawnListNpcID = SpawnListNpcID;
		this.SpawnID = SpawnID;
		this.NpcID = NpcID;
	}
}

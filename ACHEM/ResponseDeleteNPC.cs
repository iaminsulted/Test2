public class ResponseDeleteNPC : Response
{
	public int SpawnListNpcID;

	public int SpawnID;

	public ResponseDeleteNPC(int SpawnListNpcID, int SpawnID)
	{
		type = 46;
		cmd = 16;
		this.SpawnListNpcID = SpawnListNpcID;
		this.SpawnID = SpawnID;
	}
}

public class RequestDeleteNPC : Request
{
	public int SpawnListNpcID;

	public int SpawnID;

	public RequestDeleteNPC(int SpawnListNpcID, int SpawnID)
	{
		type = 46;
		cmd = 7;
		this.SpawnListNpcID = SpawnListNpcID;
		this.SpawnID = SpawnID;
	}
}

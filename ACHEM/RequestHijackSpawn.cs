public class RequestHijackSpawn : Request
{
	public int SpawnID;

	public float x;

	public float y;

	public float z;

	public int rotY;

	public RequestHijackSpawn(int SpawnID, float x, float y, float z, int rotY)
	{
		type = 46;
		cmd = 3;
		this.SpawnID = SpawnID;
		this.x = x;
		this.y = y;
		this.z = z;
		this.rotY = rotY;
	}
}

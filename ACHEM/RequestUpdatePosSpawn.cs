public class RequestUpdatePosSpawn : Request
{
	public int SpawnID;

	public ComVector3 Pos;

	public int RotationY;

	public int PathID;

	public RequestUpdatePosSpawn(int SpawnID, ComVector3 Pos, int RotationY, int PathID)
	{
		type = 46;
		cmd = 2;
		this.SpawnID = SpawnID;
		this.Pos = Pos;
		this.RotationY = RotationY;
		this.PathID = PathID;
	}
}

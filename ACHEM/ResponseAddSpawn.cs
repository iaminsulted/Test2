public class ResponseAddSpawn : Response
{
	public int SpawnID;

	public int PathID;

	public ComVector3 Pos;

	public int RotationY;

	public ResponseAddSpawn(int SpawnID, int PathID, ComVector3 Pos, int RotationY)
	{
		type = 46;
		cmd = 13;
		this.SpawnID = SpawnID;
		this.PathID = PathID;
		this.Pos = Pos;
		this.RotationY = RotationY;
	}
}

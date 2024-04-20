public class RequestUpdatePathNPC : Request
{
	public int SpawnID;

	public int PathID;

	public ComVector3 Pos;

	public int RotationY;

	public RequestUpdatePathNPC(int SpawnID, int PathID, ComVector3 Pos, int RotationY)
	{
		type = 46;
		cmd = 9;
		this.SpawnID = SpawnID;
		this.PathID = PathID;
		this.Pos = Pos;
		this.RotationY = RotationY;
	}
}

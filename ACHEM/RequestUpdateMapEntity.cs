public class RequestUpdateMapEntity : Request
{
	public MapEntityTypes MType;

	public string Data;

	public int MapEntityID;

	public bool MoveToMe;

	public ComMapEntity entity;

	public RequestUpdateMapEntity(MapEntityTypes MType, string Data, int MapEntityID, bool MoveToMe, ComMapEntity entity = null)
	{
		type = 46;
		cmd = 29;
		this.MType = MType;
		this.Data = Data;
		this.MapEntityID = MapEntityID;
		this.MoveToMe = MoveToMe;
		this.entity = entity;
	}
}

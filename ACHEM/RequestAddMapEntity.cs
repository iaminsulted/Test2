public class RequestAddMapEntity : Request
{
	public MapEntityTypes MType;

	public string Data;

	public RequestAddMapEntity(MapEntityTypes MType, string Data)
	{
		type = 46;
		cmd = 25;
		this.MType = MType;
		this.Data = Data;
	}
}

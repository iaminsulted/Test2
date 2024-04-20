public class ResponseUpdateMapEntity : Response
{
	public ComMapEntity Entity;

	public ResponseUpdateMapEntity(ComMapEntity Entity)
	{
		type = 46;
		cmd = 30;
		this.Entity = Entity;
	}
}

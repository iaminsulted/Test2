public class ResponseDeleteMapEntity : Response
{
	public int ID;

	public ResponseDeleteMapEntity(int ID)
	{
		type = 46;
		cmd = 28;
		this.ID = ID;
	}
}

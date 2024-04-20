public class RequestDeleteMapEntity : Request
{
	public int ID;

	public RequestDeleteMapEntity(int ID)
	{
		type = 46;
		cmd = 27;
		this.ID = ID;
	}
}

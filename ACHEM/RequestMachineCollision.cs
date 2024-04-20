public class RequestMachineCollision : Request
{
	public int ID;

	public bool isDb;

	public RequestMachineCollision()
	{
		type = 19;
		cmd = 13;
	}

	public RequestMachineCollision(int id, bool isDb = false)
	{
		type = 19;
		cmd = 13;
		ID = id;
		this.isDb = isDb;
	}
}

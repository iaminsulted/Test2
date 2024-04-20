internal class RequestPing : Request
{
	public int ID;

	public RequestPing(int ID)
	{
		type = 37;
		cmd = 1;
		this.ID = ID;
	}
}

public class ResponsePathStop : Response
{
	public int ID;

	public ResponsePathStop()
	{
	}

	public ResponsePathStop(int id)
	{
		type = 2;
		cmd = 3;
		ID = id;
	}
}

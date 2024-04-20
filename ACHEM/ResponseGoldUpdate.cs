public class ResponseGoldUpdate : Response
{
	public int Gold;

	public ResponseGoldUpdate()
	{
		type = 17;
		cmd = 8;
	}

	public ResponseGoldUpdate(int gold)
	{
		type = 17;
		cmd = 8;
		Gold = gold;
	}
}

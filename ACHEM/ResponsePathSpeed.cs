public class ResponsePathSpeed : Response
{
	public int ID;

	public float Speed;

	public ResponsePathSpeed()
	{
	}

	public ResponsePathSpeed(int id, float speed)
	{
		type = 2;
		cmd = 2;
		ID = id;
		Speed = speed;
	}
}

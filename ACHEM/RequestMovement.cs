public class RequestMovement : Request
{
	public MovementState state;

	public float posX;

	public float posY;

	public float posZ;

	public float rotY;

	public float timeStamp;

	public RequestMovement()
	{
		type = 1;
	}
}

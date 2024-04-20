public class RequestTeleport : Request
{
	public float posX;

	public float posY;

	public float posZ;

	public int rotY;

	public float timeStamp;

	public RequestTeleport(float posX, float posY, float posZ, int rotY, float timeStamp)
	{
		type = 46;
		cmd = 1;
		this.posX = posX;
		this.posY = posY;
		this.posZ = posZ;
		this.rotY = rotY;
		this.timeStamp = timeStamp;
	}
}

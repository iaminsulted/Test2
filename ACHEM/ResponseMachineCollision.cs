public class ResponseMachineCollision : Response
{
	public CollisionMode collisionMode;

	public int machineID;

	public ResponseMachineCollision()
	{
		type = 19;
		cmd = 13;
	}

	public ResponseMachineCollision(int machineID, CollisionMode collisionMode)
	{
		type = 19;
		cmd = 13;
		this.collisionMode = collisionMode;
		this.machineID = machineID;
	}
}

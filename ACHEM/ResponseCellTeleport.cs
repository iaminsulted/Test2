public class ResponseCellTeleport : Response
{
	public int entityId;

	public Entity.Type entityType;

	public float posX;

	public float posY;

	public float posZ;

	public float rotY;

	public ResponseCellTeleport()
	{
		type = 8;
		cmd = 4;
	}

	public ResponseCellTeleport(int entityId, Entity.Type entityType, float posX, float posY, float posZ, float rotY)
	{
		type = 8;
		cmd = 4;
		this.entityId = entityId;
		this.entityType = entityType;
		this.posX = posX;
		this.posY = posY;
		this.posZ = posZ;
		this.rotY = rotY;
	}
}

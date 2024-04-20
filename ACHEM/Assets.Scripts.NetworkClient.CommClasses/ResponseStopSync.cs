namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseStopSync : Response
{
	public int entityId;

	public Entity.Type entityType;

	public float posX;

	public float posY;

	public float posZ;

	public float rotY;

	public ResponseStopSync(int entityID, Entity.Type entityType, float posX, float posY, float posZ, float rotY)
	{
		type = 2;
		cmd = 4;
		entityId = entityID;
		this.entityType = entityType;
		this.posX = posX;
		this.posY = posY;
		this.posZ = posZ;
		this.rotY = rotY;
	}
}

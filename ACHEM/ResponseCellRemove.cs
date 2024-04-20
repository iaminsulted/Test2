public class ResponseCellRemove : Response
{
	public int areaID;

	public int cellID;

	public Entity.Type entityType;

	public int entityID;

	public ResponseCellRemove(Entity.Type entityType, int entityID)
	{
		type = 8;
		cmd = 3;
		this.entityType = entityType;
		this.entityID = entityID;
	}
}

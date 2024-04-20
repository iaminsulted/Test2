public class ResponseAreaRemove : Response
{
	public int areaID;

	public int cellID;

	public Entity.Type entityType;

	public int entityID;

	public ResponseAreaRemove(Entity.Type entityType, int entityID)
	{
		type = 7;
		cmd = 2;
		this.entityType = entityType;
		this.entityID = entityID;
	}
}

public class ResponseEntityGender : Response
{
	public int EntityID;

	public Entity.Type etyp;

	public string gender;

	public ResponseEntityGender()
	{
		type = 17;
		cmd = 24;
	}

	public ResponseEntityGender(Entity entity)
	{
		type = 17;
		cmd = 24;
		EntityID = entity.ID;
		etyp = entity.type;
		gender = entity.baseAsset.gender;
	}
}

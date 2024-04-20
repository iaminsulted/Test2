public class ResponseClassEquip : Response
{
	public int EntityID;

	public Entity.Type etyp;

	public int ClassID;

	public int ClassXP;

	public ResponseClassEquip()
	{
		type = 26;
		cmd = 2;
	}

	public ResponseClassEquip(int classID, Entity entity)
	{
		type = 26;
		cmd = 2;
		ClassID = entity.EquippedClassID;
		ClassXP = entity.EquippedClassXP;
		EntityID = entity.ID;
		etyp = entity.type;
	}
}

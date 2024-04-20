public class ResponseEntityCustomize : Response
{
	public int EntityID;

	public Entity.Type etyp;

	public int haircolor;

	public int skincolor;

	public int eyecolor;

	public int lipcolor;

	public int hair;

	public int braid;

	public int stache;

	public int beard;

	public ResponseEntityCustomize()
	{
		type = 17;
		cmd = 11;
	}

	public ResponseEntityCustomize(Entity entity)
	{
		type = 17;
		cmd = 11;
		EntityID = entity.ID;
		etyp = entity.type;
		haircolor = entity.baseAsset.ColorHair;
		skincolor = entity.baseAsset.ColorSkin;
		eyecolor = entity.baseAsset.ColorEye;
		lipcolor = entity.baseAsset.ColorLip;
		hair = entity.baseAsset.Hair;
		braid = entity.baseAsset.Braid;
		stache = entity.baseAsset.Stache;
		beard = entity.baseAsset.Beard;
	}
}

public class ResponseCellAdd : Response
{
	public int areaID;

	public int cellID;

	public ComEntity entity;

	public ResponseCellAdd(ComEntity entity)
	{
		type = 8;
		cmd = 2;
		areaID = entity.areaID;
		cellID = entity.cellID;
		this.entity = entity;
	}
}

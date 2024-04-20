public class RequestEntityTransferMap : Request
{
	public int MapID;

	public int CellID;

	public int SpawnID;

	public RequestEntityTransferMap()
	{
		type = 17;
		cmd = 14;
	}

	public RequestEntityTransferMap(int mapID, int cellID, int spawnID)
	{
		type = 17;
		cmd = 14;
		MapID = mapID;
		CellID = cellID;
		SpawnID = spawnID;
	}
}

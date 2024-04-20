public class RequestIATransferPad : Request
{
	public string UniqueID;

	public int AreaID;

	public int CellID;

	public int SpawnID;

	public RequestIATransferPad()
	{
		type = 19;
		cmd = 1;
	}
}

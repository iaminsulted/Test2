internal class ResponseExtract : Response
{
	public InventoryItem cItem;

	public ResponseExtract(InventoryItem charItem)
	{
		cItem = charItem;
		type = 10;
		cmd = 17;
	}
}

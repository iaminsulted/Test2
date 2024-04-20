public class ResponseItemAdd : Response
{
	public InventoryItem Item;

	public ResponseItemAdd()
	{
		type = 10;
		cmd = 5;
	}

	public ResponseItemAdd(InventoryItem item)
	{
		type = 10;
		cmd = 5;
		Item = item;
	}
}

using System.Collections.Generic;

public class ResponseItemLoad : Response
{
	public List<Item> Items;

	public ResponseItemLoad()
	{
		type = 10;
		cmd = 7;
	}

	public ResponseItemLoad(List<Item> items)
	{
		type = 10;
		cmd = 7;
		Items = items;
	}
}

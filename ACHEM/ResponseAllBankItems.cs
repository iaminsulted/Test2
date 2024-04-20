using System.Collections.Generic;

public class ResponseAllBankItems : Response
{
	public Dictionary<int, List<InventoryItem>> allItems;
}

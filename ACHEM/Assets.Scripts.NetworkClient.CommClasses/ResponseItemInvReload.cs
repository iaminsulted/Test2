using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseItemInvReload : Response
{
	public Dictionary<int, List<InventoryItem>> InvItems;
}

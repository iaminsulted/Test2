using System.Collections.Generic;

public class RequestPvpActionEquip : Request
{
	public Dictionary<int, int> PvpActionIDs;

	public RequestPvpActionEquip(Dictionary<int, int> pvpActionIDs)
	{
		type = 17;
		cmd = 31;
		PvpActionIDs = pvpActionIDs;
	}
}

using System.Collections.Generic;

public class ResponsePvpActionEquip : Response
{
	public Dictionary<int, int> PvpActionIDs;

	public ResponsePvpActionEquip()
	{
		type = 17;
		cmd = 31;
	}

	public ResponsePvpActionEquip(Dictionary<int, int> pvpActionIDs)
	{
		type = 17;
		cmd = 31;
		PvpActionIDs = pvpActionIDs;
	}
}

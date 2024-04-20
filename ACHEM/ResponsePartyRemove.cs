using System.Collections.Generic;

public class ResponsePartyRemove : Response
{
	public Dictionary<int, string> PlayerData;

	public ResponsePartyRemove()
	{
		type = 31;
		cmd = 3;
	}
}

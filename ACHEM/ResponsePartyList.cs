using System.Collections.Generic;

public class ResponsePartyList : Response
{
	public Dictionary<int, PlayerPartyData> Data;

	public int LeaderID;

	public bool IsPrivate;

	public ResponsePartyList()
	{
		type = 31;
		cmd = 5;
	}
}

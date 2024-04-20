using System.Collections.Generic;

public class RequestAllBankItems : Request
{
	public List<int> requestedIDs;

	public RequestAllBankItems(List<int> requestedids)
	{
		type = 32;
		cmd = 4;
		requestedIDs = requestedids;
	}
}

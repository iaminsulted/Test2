using System.Collections.Generic;

public class RequestQuestLoad : Request
{
	public List<int> QuestIDs;

	public RequestQuestLoad()
	{
		type = 15;
		cmd = 1;
	}
}

using System.Collections.Generic;

public class RequestQuestComplete : Request
{
	public int QuestID;

	public int ChosenRewardID;

	public Queue<int> ChosenRewardIDs = new Queue<int>();

	public RequestQuestComplete()
	{
		type = 15;
		cmd = 4;
	}
}

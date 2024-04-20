public class RequestQuestAbandon : Request
{
	public int QuestID;

	public RequestQuestAbandon()
	{
		type = 15;
		cmd = 3;
	}
}

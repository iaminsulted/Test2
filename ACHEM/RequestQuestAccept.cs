public class RequestQuestAccept : Request
{
	public int QuestID;

	public RequestQuestAccept()
	{
		type = 15;
		cmd = 2;
	}
}

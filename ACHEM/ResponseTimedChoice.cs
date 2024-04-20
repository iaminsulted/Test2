public class ResponseTimedChoice : Response
{
	public enum ChoiceType
	{
		PartyKick,
		PartyDungeonJoin,
		QueueReadyCheck
	}

	public string title;

	public string description;

	public ChoiceType choiceType;

	public float time;

	public ResponseTimedChoice()
	{
		type = 36;
		cmd = 2;
	}

	public ResponseTimedChoice(string title, string description, ChoiceType choiceType, float time)
	{
		type = 36;
		cmd = 2;
		this.title = title;
		this.description = description;
		this.choiceType = choiceType;
		this.time = time;
	}
}

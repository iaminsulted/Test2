public class RequestVoteKickChoice : Request
{
	public bool choice;

	public RequestVoteKickChoice()
	{
		type = 31;
		cmd = 9;
	}

	public RequestVoteKickChoice(bool choice)
	{
		type = 31;
		cmd = 9;
		this.choice = choice;
	}
}

public class RequestTimedChoice : Request
{
	public bool choice;

	public RequestTimedChoice()
	{
		type = 36;
		cmd = 2;
	}

	public RequestTimedChoice(bool choice)
	{
		type = 36;
		cmd = 2;
		this.choice = choice;
	}
}

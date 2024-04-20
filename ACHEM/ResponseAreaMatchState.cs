public class ResponseAreaMatchState : Response
{
	public MatchState MatchState;

	public ResponseAreaMatchState()
	{
		type = 7;
		cmd = 5;
	}

	public ResponseAreaMatchState(MatchState state)
	{
		type = 7;
		cmd = 5;
		MatchState = state;
	}
}

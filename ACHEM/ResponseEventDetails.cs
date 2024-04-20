public class ResponseEventDetails : Response
{
	public float XPMultiplier;

	public float GoldMultiplier;

	public float CXPMultiplier;

	public float DailyQuestMultiplier;

	public float DailyTaskMultiplier;

	public ResponseEventDetails()
	{
		type = 27;
		cmd = 4;
	}
}

namespace Assets.Scripts.NetworkClient.CommClasses;

internal class ResponseDailyTaskInfo : Response
{
	public CharDailyTask charDaily;

	public ResponseDailyTaskInfo(CharDailyTask daily)
	{
		type = 41;
		cmd = 1;
		charDaily = daily;
	}
}

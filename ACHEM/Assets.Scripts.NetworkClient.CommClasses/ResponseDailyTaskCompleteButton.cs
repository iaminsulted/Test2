namespace Assets.Scripts.NetworkClient.CommClasses;

internal class ResponseDailyTaskCompleteButton : Request
{
	public int taskID;

	public ResponseDailyTaskCompleteButton(int id)
	{
		type = 41;
		cmd = 1;
		taskID = id;
	}
}

using System.Collections.Generic;

internal class ResponseServerDailyTaskInitialize : Response
{
	public List<DailyTask> dailyTasks;

	public List<CharDailyTask> charDailyTasks;

	public ResponseServerDailyTaskInitialize(List<DailyTask> tasks, List<CharDailyTask> charTasks)
	{
		type = 42;
		cmd = 1;
		dailyTasks = tasks;
		charDailyTasks = charTasks;
	}
}

public class CombinedDailyTask
{
	public int taskID { get; set; }

	public int curQty { get; set; }

	public bool collected { get; set; }

	public string name { get; set; }

	public string desc { get; set; }

	public int targetQty { get; set; }

	public int xpReward { get; set; }

	public int goldReward { get; set; }

	public bool completed { get; set; }

	public float completionPercent { get; set; }

	public bool visible { get; set; }

	public int category { get; set; }

	public bool metaTask { get; set; }

	public CombinedDailyTask(int ID, int cur, int target, string description, int xp, int gold, bool collect, bool vis, int cat, bool meta)
	{
		taskID = ID;
		curQty = cur;
		targetQty = target;
		desc = description;
		xpReward = xp;
		goldReward = gold;
		collected = collect;
		visible = vis;
		category = cat;
		metaTask = meta;
		completionPercent = (float)curQty / (float)targetQty;
		if (curQty >= targetQty)
		{
			completed = true;
			if (collected)
			{
				completionPercent = 0f;
			}
		}
		else
		{
			completed = false;
		}
	}
}

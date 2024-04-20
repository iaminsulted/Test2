using UnityEngine;

public class UIDailyRewardItem : UIItem
{
	[HideInInspector]
	public int Day;

	public void Init(Item item, int day)
	{
		base.Init(item);
		Day = day;
		InfoLabel.text = GetRewardText(item);
	}

	private string GetRewardText(Item item)
	{
		return "[5e5e5e]Day " + Day + "[-]";
	}
}

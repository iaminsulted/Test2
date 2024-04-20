using UnityEngine;

public class UIMapListItem : UIItem
{
	public AreaData area;

	public GameObject icon_lock;

	public GameObject icon_quest;

	public void Load(AreaData ad)
	{
		area = ad;
		NameLabel.text = area.displayName;
		icon_quest.SetActive(Session.MyPlayerData.CurrentlyTrackedQuest != null && Session.MyPlayerData.CurrentlyTrackedQuest.TargetMapId == ad.id);
		icon_lock.SetActive(!IsAvailable());
	}

	public bool IsAvailable()
	{
		return Session.MyPlayerData.CheckBitFlag(area.UnlockBitFlagName, (byte)area.UnlockBitFlagIndex);
	}
}

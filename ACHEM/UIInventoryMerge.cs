using System.Linq;

public class UIInventoryMerge : UIInventoryItem
{
	public Merge merge;

	public UIMergeTimer timer;

	public UISprite IconLock;

	private void OnEnable()
	{
		Session.MyPlayerData.MergeAdded += PlayerItemAdded;
		Session.MyPlayerData.MergeRemoved += PlayerItemAdded;
	}

	private void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.MergeAdded -= PlayerItemAdded;
			Session.MyPlayerData.MergeRemoved -= PlayerItemAdded;
		}
	}

	public void Init(Merge merge)
	{
		Init((Item)merge);
		this.merge = merge;
		ShowTimer(isOn: false);
		IconLock.gameObject.SetActive(!merge.IsAvailable());
		FindPlayerItem();
	}

	private void FindPlayerItem()
	{
		Merge merge = Session.MyPlayerData.merges.FirstOrDefault((Merge x) => x.MergeID == this.merge.MergeID);
		if (merge == null)
		{
			ShowTimer(isOn: false);
			return;
		}
		ShowTimer(isOn: true);
		timer.Init(merge);
	}

	public void PlayerItemAdded(Merge updatedMerge)
	{
		if (merge != null && merge.MergeID == updatedMerge.MergeID)
		{
			FindPlayerItem();
		}
	}

	private void ShowTimer(bool isOn)
	{
		timer.gameObject.SetActive(isOn);
		InfoLabel.enabled = !isOn;
	}
}

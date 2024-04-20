public class MergeItem : Item
{
	public int MergeID;

	public int ItemID;

	public int SortOrder;

	public MergeItem()
	{
	}

	public MergeItem(Item item, int mid, int iid, int qty, int sortorder)
		: base(item, qty)
	{
		MergeID = mid;
		ItemID = iid;
		Qty = qty;
		SortOrder = sortorder;
	}
}

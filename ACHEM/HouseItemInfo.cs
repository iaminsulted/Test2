public class HouseItemInfo
{
	public BundleInfo bInfo;

	public string AssetName;

	public int ItemID;

	public HouseItemInfo()
	{
	}

	public HouseItemInfo(InventoryItem iItem)
	{
		bInfo = iItem.bundle;
		AssetName = iItem.AssetName;
		ItemID = iItem.ID;
	}
}

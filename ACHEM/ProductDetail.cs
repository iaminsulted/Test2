using System;

public class ProductDetail
{
	public ProductID ID;

	public string StoreProductID;

	public long SteamProductID;

	public string Title;

	public string BitFlagName;

	public int BitFlagIndex;

	public DateTime startDate;

	public DateTime endDate;

	public bool HasBadge
	{
		get
		{
			if (!string.IsNullOrEmpty(BitFlagName))
			{
				return BitFlagIndex > 0;
			}
			return false;
		}
	}

	public bool IsDCPackage
	{
		get
		{
			if (ID != ProductID.CRYSTALS_200 && ID != ProductID.CRYSTALS_900 && ID != ProductID.CRYSTALS_2000 && ID != ProductID.CRYSTALS_5000)
			{
				return ID == ProductID.CRYSTALS_15000;
			}
			return true;
		}
	}

	public ProductDetail(ProductID id, string storeProductID, string title, string bitFlagName, int bitFlagIndex, DateTime start, DateTime end)
	{
		ID = id;
		StoreProductID = storeProductID;
		Title = title;
		BitFlagName = bitFlagName;
		BitFlagIndex = bitFlagIndex;
		startDate = start;
		endDate = end;
	}
}

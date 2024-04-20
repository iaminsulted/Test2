using System.Collections.Generic;

public class Shop
{
	public int ID;

	public ShopType Type;

	public string Name;

	public string BitFlagName;

	public byte BitFlagIndex;

	public int ScaleMapOverride;

	public bool IsCollectionPriceDC;

	public int CollectionPrice;

	public int CollectionBadgeID;

	public List<ShopItem> Items;
}

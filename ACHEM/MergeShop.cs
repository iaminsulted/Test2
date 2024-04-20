using System;
using System.Collections.Generic;

public class MergeShop
{
	public int MergeShopID;

	public string Name;

	public string BitFlagName;

	public byte BitFlagIndex;

	public int ScaleMapOverride;

	public DateTime? TS;

	public Dictionary<int, Merge> Merges = new Dictionary<int, Merge>();
}

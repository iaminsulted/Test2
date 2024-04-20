using System.Collections.Generic;
using Assets.Scripts.Housing;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ComHouseData
{
	public List<ComHouseItem> hItems;

	public Dictionary<int, HouseItemInfo> hItemInfos;

	public Dictionary<int, int> hItemCounts;

	public HouseData hData;
}

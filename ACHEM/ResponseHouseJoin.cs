using System.Collections.Generic;
using Assets.Scripts.Housing;

public class ResponseHouseJoin : Response
{
	public HouseData hData;

	public List<ComHouseItem> hItems;

	public Dictionary<int, HouseItemInfo> hItemInfos;

	public Dictionary<int, int> hItemCounts;
}

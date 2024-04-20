using System.Collections.Generic;
using Assets.Scripts.Housing;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseHouseData : Response
{
	public int ListVersion;

	public Dictionary<int, HouseData> HouseData;

	public HouseDataCategory Category;

	public int ServerListCount;

	public bool IsReversed;
}

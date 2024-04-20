using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseData : Request
{
	public int LowerBoundIndex;

	public int ListVersion;

	public List<int> ListIndices;

	public HouseDataCategory SortType;

	public bool IsReversed;

	public string QueryString;

	public RequestHouseData(int lowerBoundIndex, List<int> listIndices, HouseDataCategory sortType, int listVersion, bool isReversed = false, string queryString = null)
	{
		type = 50;
		cmd = 8;
		LowerBoundIndex = lowerBoundIndex;
		SortType = sortType;
		ListVersion = listVersion;
		ListIndices = listIndices;
		IsReversed = isReversed;
		QueryString = queryString;
	}
}

using System.Collections.Generic;

public class ResponseAreaList : Response
{
	public List<AreaData> areas;

	public List<RegionData> regions;

	public ResponseAreaList()
	{
		type = 7;
		cmd = 3;
	}
}

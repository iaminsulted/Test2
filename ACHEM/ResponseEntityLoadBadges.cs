using System.Collections.Generic;

public class ResponseEntityLoadBadges : Response
{
	public Dictionary<int, Badge> Badges;

	public Dictionary<int, BadgeTitleCategory> Categories;

	public ResponseEntityLoadBadges()
	{
		type = 17;
		cmd = 18;
	}
}

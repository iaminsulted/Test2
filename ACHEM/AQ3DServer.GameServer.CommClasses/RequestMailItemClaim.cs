using System.Collections.Generic;

namespace AQ3DServer.GameServer.CommClasses;

public class RequestMailItemClaim : Request
{
	public int charid;

	public List<RewardItem> rewards;

	public RequestMailItemClaim()
	{
		type = 51;
		cmd = 6;
	}
}

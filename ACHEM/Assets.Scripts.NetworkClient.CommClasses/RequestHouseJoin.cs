using Assets.Scripts.Housing;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseJoin : Request
{
	public HouseData HouseData;

	public RequestHouseJoin(HouseData houseData)
	{
		type = 50;
		cmd = 5;
		HouseData = houseData;
	}
}

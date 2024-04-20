using Assets.Scripts.Housing;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseUpdate : Request
{
	public HouseData HouseData;

	public RequestHouseUpdate(HouseData houseData)
	{
		type = 50;
		cmd = 10;
		HouseData = houseData;
	}
}

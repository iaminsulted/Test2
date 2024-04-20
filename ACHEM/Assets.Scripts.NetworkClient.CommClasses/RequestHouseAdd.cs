using Assets.Scripts.Housing;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseAdd : Request
{
	public HouseData HouseData;

	public RequestHouseAdd(HouseData houseData)
	{
		type = 50;
		cmd = 9;
		HouseData = houseData;
	}
}

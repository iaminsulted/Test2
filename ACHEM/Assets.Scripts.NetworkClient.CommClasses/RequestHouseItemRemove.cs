namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseItemRemove : Request
{
	public int HouseItemID;

	public RequestHouseItemRemove(int houseItemID)
	{
		type = 50;
		cmd = 3;
		HouseItemID = houseItemID;
	}
}

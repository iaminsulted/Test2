namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseCommand : Request
{
	public int HouseID { get; set; }

	public int ItemID { get; set; }

	public int HouseItemID { get; set; }

	public RequestHouseCommand(Com.CmdHousing command, int houseID = 0, int itemID = 0, int houseItemID = 0)
	{
		type = 50;
		cmd = (byte)command;
		HouseID = houseID;
		ItemID = itemID;
		HouseItemID = houseItemID;
	}
}

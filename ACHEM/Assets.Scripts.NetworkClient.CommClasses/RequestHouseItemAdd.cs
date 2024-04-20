namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseItemAdd : Request
{
	public ComHouseItem RequestedHouseItem;

	public RequestHouseItemAdd(ComHouseItem chItem)
	{
		type = 50;
		cmd = 1;
		RequestedHouseItem = chItem;
	}
}

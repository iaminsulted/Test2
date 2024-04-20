namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseItemClearAll : Request
{
	public RequestHouseItemClearAll()
	{
		type = 50;
		cmd = 4;
	}
}

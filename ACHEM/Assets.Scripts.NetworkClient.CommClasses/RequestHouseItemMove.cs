namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestHouseItemMove : Request
{
	public ComHouseItemMove ComMove;

	public RequestHouseItemMove(ComHouseItemMove comMove)
	{
		type = 50;
		cmd = 2;
		ComMove = comMove;
	}
}

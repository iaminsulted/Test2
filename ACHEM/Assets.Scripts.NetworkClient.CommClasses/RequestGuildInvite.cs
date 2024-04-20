namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestGuildInvite : Request
{
	public int FromID;

	public string To;

	public RequestGuildInvite()
	{
		type = 40;
		cmd = 4;
	}

	public RequestGuildInvite(int from, string to)
	{
		type = 40;
		cmd = 4;
		FromID = from;
		To = to;
	}
}

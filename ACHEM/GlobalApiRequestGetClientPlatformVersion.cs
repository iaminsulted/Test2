public class GlobalApiRequestGetClientPlatformVersion : GlobalApiRequest
{
	private int _clientID;

	public GlobalApiRequestGetClientPlatformVersion(int clientID)
	{
		_clientID = clientID;
	}

	public override string GetURL()
	{
		return Main.httpPrefix + "global.aq3d.com/api/Client/GetClientPlatformVersion2?ClientID=" + _clientID + "&BuildPlatform=" + (int)BuildPlatform.Get;
	}
}

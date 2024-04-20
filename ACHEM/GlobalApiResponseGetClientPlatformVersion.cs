public class GlobalApiResponseGetClientPlatformVersion : GlobalApiResponse
{
	public static int Error_ClientIdNotFound = 2;

	public ClientVersionRecord clientVersion;

	public string minRequiredVersion;
}

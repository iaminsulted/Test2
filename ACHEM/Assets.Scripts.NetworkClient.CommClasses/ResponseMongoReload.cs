namespace Assets.Scripts.NetworkClient.CommClasses;

internal class ResponseMongoReload : Response
{
	public ResponseMongoReload()
	{
		type = 34;
		cmd = 6;
	}
}

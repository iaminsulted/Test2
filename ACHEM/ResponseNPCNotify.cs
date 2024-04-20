public class ResponseNPCNotify : Response
{
	public int spawnID;

	public string message;

	public GameNotificationType NotificationType = GameNotificationType.Both;

	public ResponseNPCNotify(int spawnID, string message, GameNotificationType notificationType = GameNotificationType.Both)
	{
		type = 18;
		cmd = 10;
		this.spawnID = spawnID;
		this.message = message;
		NotificationType = notificationType;
	}
}

public class ResponseNotify : Response
{
	public string Message;

	public GameNotificationType NotificationType = GameNotificationType.Both;

	public ResponseNotify(string message, GameNotificationType notificationType = GameNotificationType.Both)
	{
		type = 27;
		cmd = 1;
		Message = message;
		NotificationType = notificationType;
	}
}

public class ResponseBitFlagUpdate : Response
{
	public bool Value;

	public Badge Badge;

	public bool Notify;

	public bool ShowInChat;

	public ResponseBitFlagUpdate()
	{
		type = 17;
		cmd = 9;
	}

	public ResponseBitFlagUpdate(Badge badge, bool value, bool notify)
	{
		type = 17;
		cmd = 9;
		Badge = badge;
		Value = value;
		Notify = notify;
	}
}

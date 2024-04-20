public class RequestPingSession : Request
{
	public float sessionPing;

	public RequestPingSession(float ping)
	{
		sessionPing = ping;
		type = 37;
		cmd = 2;
	}
}

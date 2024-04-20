public class RequestChannelJoin : Request
{
	public int channelID;

	public RequestChannelJoin()
	{
		type = 6;
		cmd = 1;
	}
}

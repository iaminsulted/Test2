public class ResponsePartyPrivacy : Response
{
	public bool IsPrivate;

	public ResponsePartyPrivacy()
	{
		type = 31;
		cmd = 11;
	}
}

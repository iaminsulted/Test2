public class RequestMailSeen : Request
{
	public MailMessage mail;

	public RequestMailSeen()
	{
		type = 51;
		cmd = 7;
	}
}

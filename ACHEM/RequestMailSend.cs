public class RequestMailSend : Request
{
	public MailMessage mail;

	public string recipient;

	public RequestMailSend()
	{
		type = 51;
		cmd = 2;
	}
}

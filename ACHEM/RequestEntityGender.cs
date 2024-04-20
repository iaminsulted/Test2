public class RequestEntityGender : Request
{
	public string gender;

	public RequestEntityGender()
	{
		type = 17;
		cmd = 24;
	}
}

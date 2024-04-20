public class ResponseMachineAreaFlag : Response
{
	public string key;

	public string value;

	public ResponseMachineAreaFlag(string key, string value)
	{
		type = 19;
		cmd = 10;
		this.key = key;
		this.value = value;
	}
}

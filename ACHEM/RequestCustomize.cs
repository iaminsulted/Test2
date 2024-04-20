public class RequestCustomize : Request
{
	public int haircolor;

	public int skincolor;

	public int eyecolor;

	public int lipcolor;

	public int hair;

	public int braid;

	public int stache;

	public int beard;

	public RequestCustomize()
	{
		type = 17;
		cmd = 11;
	}
}

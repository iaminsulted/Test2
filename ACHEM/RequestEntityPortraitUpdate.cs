public class RequestEntityPortraitUpdate : Request
{
	public int Portrait;

	public RequestEntityPortraitUpdate()
	{
		type = 17;
		cmd = 16;
	}

	public RequestEntityPortraitUpdate(int portrait)
	{
		type = 17;
		cmd = 16;
		Portrait = portrait;
	}
}

public class ResponseEntityPortraitUpdate : Response
{
	public int EntityID;

	public int Portrait;

	public ResponseEntityPortraitUpdate()
	{
		type = 17;
		cmd = 16;
	}

	public ResponseEntityPortraitUpdate(int entityID, int portrait)
	{
		type = 17;
		cmd = 16;
		EntityID = entityID;
		Portrait = portrait;
	}
}

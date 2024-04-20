public class RequestAreaJoin : Request
{
	public int areaID;

	public bool IsPrivate;

	public bool EnterScaled;

	public RequestAreaJoin()
	{
		type = 7;
		cmd = 1;
	}

	public RequestAreaJoin(int id, bool isPrivate, bool enterScaled)
	{
		type = 7;
		cmd = 1;
		IsPrivate = isPrivate;
		EnterScaled = enterScaled;
		areaID = id;
	}
}

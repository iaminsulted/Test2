public class RequestEffectRemove : Request
{
	public int effectID;

	public RequestEffectRemove()
	{
		type = 12;
		cmd = 9;
	}

	public RequestEffectRemove(int effectID)
	{
		type = 12;
		cmd = 9;
		this.effectID = effectID;
	}
}

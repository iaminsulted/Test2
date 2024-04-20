public class RequestEmote : Request
{
	public StateEmote em;

	public RequestEmote()
	{
		type = 21;
	}

	public RequestEmote(StateEmote em)
	{
		type = 21;
		this.em = em;
	}
}

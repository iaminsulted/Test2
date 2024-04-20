public class ResponseEmote : Response
{
	public int ID;

	public StateEmote em;

	public bool loop;

	public ResponseEmote()
	{
		type = 21;
	}

	public ResponseEmote(int ID, StateEmote em, bool loop = false)
	{
		type = 21;
		this.ID = ID;
		this.em = em;
		this.loop = loop;
	}
}

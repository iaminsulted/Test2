public class RequestGuildInfo : Request
{
	public int GuildID;

	public RequestGuildInfo(int GuildID)
	{
		this.GuildID = GuildID;
		type = 40;
		cmd = 2;
	}
}

public class RequestGuildBuyPower : Request
{
	public int GuildID;

	public int ItemID;

	public RequestGuildBuyPower()
	{
		type = 40;
		cmd = 12;
	}
}

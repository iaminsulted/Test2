public class RequestUpdateGuildTax : Request
{
	public int newTax;

	public int guildID;

	public RequestUpdateGuildTax(int newTax, int guildID)
	{
		type = 40;
		cmd = 14;
		this.newTax = newTax;
		this.guildID = guildID;
	}
}

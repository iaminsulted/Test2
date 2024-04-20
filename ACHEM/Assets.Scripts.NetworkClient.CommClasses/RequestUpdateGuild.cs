namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestUpdateGuild : Request
{
	private int guildID;

	public RequestUpdateGuild(int guildID)
	{
		type = 40;
		cmd = 2;
		this.guildID = guildID;
	}
}

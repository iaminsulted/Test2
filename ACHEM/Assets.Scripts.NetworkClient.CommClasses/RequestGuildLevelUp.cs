namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestGuildLevelUp : Request
{
	public int guildID;

	public RequestGuildLevelUp(int guildID)
	{
		type = 40;
		cmd = 11;
		this.guildID = guildID;
	}
}

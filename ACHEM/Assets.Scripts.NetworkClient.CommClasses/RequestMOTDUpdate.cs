namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestMOTDUpdate : Request
{
	public string motd;

	public int guildID;

	public RequestMOTDUpdate(string motd, int guildID)
	{
		type = 40;
		cmd = 10;
		this.motd = motd;
		this.guildID = guildID;
	}
}

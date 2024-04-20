namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseMOTDUpdate : Response
{
	public string motd;

	public int guildID;

	public ResponseMOTDUpdate(string motd, int guildID)
	{
		type = 40;
		cmd = 10;
		this.motd = motd;
		this.guildID = guildID;
	}
}

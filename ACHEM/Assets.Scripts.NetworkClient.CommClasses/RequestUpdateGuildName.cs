namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestUpdateGuildName : Request
{
	public string newName;

	public int guildID;

	public RequestUpdateGuildName(string newName, int guildID)
	{
		type = 40;
		cmd = 8;
		this.newName = newName;
		this.guildID = guildID;
	}
}

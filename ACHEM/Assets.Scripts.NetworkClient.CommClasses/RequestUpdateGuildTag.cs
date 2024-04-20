namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestUpdateGuildTag : Request
{
	public string newTag;

	public int guildID;

	public RequestUpdateGuildTag(string newTag, int guildID)
	{
		type = 40;
		cmd = 9;
		this.newTag = newTag;
		this.guildID = guildID;
	}
}

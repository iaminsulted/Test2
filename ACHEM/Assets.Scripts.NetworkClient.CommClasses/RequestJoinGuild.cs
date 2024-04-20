namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestJoinGuild : Request
{
	public int charID;

	public int guildID;

	public byte role;

	public string guildName;

	public string tag;

	public GuildMember guildMember;

	public RequestJoinGuild(string guildName, string tag)
	{
		guildID = -1;
		role = 2;
		this.tag = tag;
		this.guildName = guildName;
		type = 40;
		cmd = 1;
	}

	public RequestJoinGuild(int charID, int guildID, byte role)
	{
		this.charID = charID;
		this.guildID = guildID;
		this.role = role;
		type = 40;
		cmd = 1;
	}
}

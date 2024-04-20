namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestGuildChangeRole : Request
{
	public int requesterID;

	public int guildMemberID;

	public int guildID;

	public GuildRole newRole;

	public bool swapRole;

	public RequestGuildChangeRole(int requesterID, int guildMemberID, int guildID, GuildRole newRole, bool swapRole)
	{
		type = 40;
		cmd = 6;
		this.requesterID = requesterID;
		this.guildID = guildID;
		this.swapRole = swapRole;
		this.guildMemberID = guildMemberID;
		this.newRole = newRole;
	}
}

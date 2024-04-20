namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseGuildMemberStatus : Response
{
	public int ID;

	public GuildMember guildMember;

	public bool isNewPlayer;
}

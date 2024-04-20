namespace Assets.Scripts.NetworkClient.CommClasses;

public class RequestLeaveGuild : Request
{
	public int memberIDLeaving;

	public int kicker;

	public int guildID;

	public RequestLeaveGuild(int memberLeavingID, int guildID)
	{
		type = 40;
		cmd = 3;
		memberIDLeaving = memberLeavingID;
		this.guildID = guildID;
		kicker = -1;
	}

	public RequestLeaveGuild(int memberIDLeaving, int kicker, int guildID)
	{
		type = 40;
		cmd = 3;
		this.memberIDLeaving = memberIDLeaving;
		this.kicker = kicker;
		this.guildID = guildID;
	}
}

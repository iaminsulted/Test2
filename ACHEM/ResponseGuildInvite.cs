public class ResponseGuildInvite : Response
{
	public int LeaderID;

	public string Message;

	public string LeaderName;

	public int guildID;

	public ResponseGuildInvite()
	{
		type = 40;
		cmd = 4;
	}

	public int GetID()
	{
		return LeaderID;
	}

	public string GetMessage()
	{
		return Message;
	}

	public string GetName()
	{
		return LeaderName;
	}
}

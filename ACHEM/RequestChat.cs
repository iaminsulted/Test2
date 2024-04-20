public class RequestChat : Request
{
	public string msg;

	public int channelID;

	public string recipient;

	public int guildID;

	public string sender;

	public RequestChat()
	{
		type = 4;
	}

	public RequestChat(string r, string m)
	{
		recipient = r;
		msg = m;
		type = 4;
	}

	public void SetPublicMessage(int channelID, string msg)
	{
		cmd = 1;
		this.channelID = channelID;
		this.msg = msg;
	}

	public void SetWhisper(string recipient, string msg)
	{
		cmd = 2;
		this.recipient = recipient;
		this.msg = msg;
	}
}

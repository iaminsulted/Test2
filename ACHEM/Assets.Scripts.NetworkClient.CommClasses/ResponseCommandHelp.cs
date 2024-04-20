namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseCommandHelp : Response
{
	public ChatCommand chatCmd;

	public ResponseCommandHelp()
	{
		type = 9;
		cmd = 4;
	}

	public ResponseCommandHelp(ChatCommand chatCmd)
	{
		type = 9;
		cmd = 4;
		this.chatCmd = chatCmd;
	}
}

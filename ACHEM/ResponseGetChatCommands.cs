using System.Collections.Generic;

public class ResponseGetChatCommands : Response
{
	public List<ChatCommand> commands;

	public ResponseGetChatCommands()
	{
		type = 9;
		cmd = 5;
	}
}

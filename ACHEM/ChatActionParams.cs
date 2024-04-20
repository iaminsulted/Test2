using System.Collections.Generic;

public class ChatActionParams
{
	public readonly List<string> args;

	public ChatActionParams(List<string> args)
	{
		this.args = args ?? new List<string>();
	}
}

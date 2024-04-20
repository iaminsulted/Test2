using System;
using System.Collections.Generic;
using System.Linq;

public class ChatCommand
{
	public string command;

	public string description;

	public List<string> aliases;

	public ChatCommands.Role permission;

	public Action<ChatActionParams> method;

	public List<ChatParam> args;

	public int ArgsRequired => args.Count((ChatParam param) => !param.isOptional);

	public string HelpMessage
	{
		get
		{
			string text = "Command: /" + command;
			string text2 = "\nDescription: " + description;
			string text3 = "";
			string text4 = "";
			if (args.Count > 0)
			{
				text4 = "\nParams:";
				text3 = "\nExample: /" + command;
				foreach (ChatParam arg in args)
				{
					text3 = text3 + " " + arg.example;
					text4 = ((!arg.isOptional) ? (text4 + "\n  " + arg.name) : (text4 + "\n  " + arg.name + " (default=" + arg.defaultValue + ")"));
				}
			}
			string text5 = "";
			if (aliases.Count > 0)
			{
				text5 = "\nAliases: ";
				foreach (string alias in aliases)
				{
					text5 = text5 + "/" + alias;
					if (alias != aliases.Last())
					{
						text5 += ", ";
					}
				}
			}
			return text + text2 + text4 + text3 + text5;
		}
	}

	public ChatCommand(string command, string description, ChatCommands.Role permission, Action<ChatActionParams> method, List<string> aliases = null, List<ChatParam> args = null)
	{
		this.command = command.ToLower();
		this.description = description;
		this.permission = permission;
		this.method = method;
		this.aliases = aliases ?? new List<string>();
		this.args = args ?? new List<ChatParam>();
	}
}

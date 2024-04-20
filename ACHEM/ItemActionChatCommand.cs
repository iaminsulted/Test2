public class ItemActionChatCommand : ItemAction
{
	public string Command;

	public ItemActionChatCommand(string command)
		: base(ItemActionType.ChatCommand)
	{
		Command = command;
	}
}

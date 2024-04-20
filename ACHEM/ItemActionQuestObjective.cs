public class ItemActionQuestObjective : ItemAction
{
	public int QuestID;

	public int QOID;

	public ItemActionQuestObjective(int questid, int qoid)
		: base(ItemActionType.QuestObjective)
	{
		QuestID = questid;
		QOID = qoid;
	}
}

public class NotificationQuestTurnIn : NotificationQuest
{
	public UILabel labelName;

	public void Init(Quest quest)
	{
		labelName.text = quest.Name;
	}
}

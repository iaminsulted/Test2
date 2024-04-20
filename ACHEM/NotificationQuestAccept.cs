public class NotificationQuestAccept : NotificationQuest
{
	public UILabel labelName;

	public void Init(Quest quest)
	{
		labelName.text = quest.Name;
	}

	public override void Activate()
	{
		base.Activate();
		AudioManager.Play2DSFX("sfx_engine_questaccept");
	}
}

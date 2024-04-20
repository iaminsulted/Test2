public class NotificationQuestComplete : NotificationQuest
{
	public UILabel labelName;

	public UILabel labelGold;

	public UILabel labelXP;

	public void Init(Quest quest)
	{
		labelName.text = quest.Name;
		labelGold.text = quest.GoldReward().ToString();
		labelXP.text = quest.XPReward().ToString();
	}

	public override void Activate()
	{
		base.Activate();
		AudioManager.Play2DSFX("sfx_engine_questcomplete");
	}
}

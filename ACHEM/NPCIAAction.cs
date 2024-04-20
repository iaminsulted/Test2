public class NPCIAAction : NPCIA
{
	public ClientTriggerActionCore Action;

	public string icon;

	public override string Icon
	{
		get
		{
			if (!string.IsNullOrEmpty(icon))
			{
				return icon;
			}
			return "";
		}
	}

	public override ApopState ApopState
	{
		get
		{
			if (Action is CTAQuestLoadCore)
			{
				CTAQuestLoadCore cTAQuestLoadCore = Action as CTAQuestLoadCore;
				foreach (int turnInQuestID in cTAQuestLoadCore.TurnInQuestIDs)
				{
					if (Session.MyPlayerData.IsQuestComplete(turnInQuestID))
					{
						if (IsAvailable())
						{
							return ApopState.QuestTurnin;
						}
						if (DontHideWhenLocked)
						{
							return ApopState.LockedQuestTurnin;
						}
					}
				}
				foreach (int acceptQuestID in cTAQuestLoadCore.AcceptQuestIDs)
				{
					if (!Session.MyPlayerData.HasQuest(acceptQuestID) && Session.MyPlayerData.IsQuestAcceptable(Quests.Get(acceptQuestID)))
					{
						if (IsAvailable())
						{
							return ApopState.QuestAvailable;
						}
						if (DontHideWhenLocked)
						{
							return ApopState.LockedQuestAvailable;
						}
					}
				}
			}
			return ApopState.Talk;
		}
	}

	public override Quest TurnInQuest
	{
		get
		{
			if (Action is CTAQuestLoadCore)
			{
				foreach (int turnInQuestID in (Action as CTAQuestLoadCore).TurnInQuestIDs)
				{
					Quest result = Quests.Get(turnInQuestID);
					if (Session.MyPlayerData.IsQuestComplete(turnInQuestID))
					{
						return result;
					}
				}
			}
			return null;
		}
	}

	public override bool IsAvailable()
	{
		if (Action is CTAQuestLoadCore)
		{
			if (base.IsAvailable())
			{
				return ((CTAQuestLoadCore)Action).AreQuestAvailable();
			}
			return false;
		}
		return base.IsAvailable();
	}

	protected override void Init()
	{
		base.Init();
	}
}

using System.Collections.Generic;

public class NPCIAQuest : NPCIA
{
	public List<int> QuestIDs;

	public override string Icon
	{
		get
		{
			if (ApopState != ApopState.QuestTurnin)
			{
				return "Icon-Quest";
			}
			return "Icon-QuestTurnin";
		}
	}

	public override string CurrentLabel
	{
		get
		{
			if (ApopState != ApopState.QuestTurnin)
			{
				return "Quest";
			}
			return "Turn in Quest";
		}
	}

	public override ApopState ApopState
	{
		get
		{
			ApopState apopState = ApopState.Talk;
			foreach (int questID in QuestIDs)
			{
				if (Session.MyPlayerData.IsQuestComplete(questID))
				{
					if (IsAvailable())
					{
						if (ApopState.QuestTurnin > apopState)
						{
							apopState = ApopState.QuestTurnin;
						}
					}
					else if (DontHideWhenLocked && ApopState.LockedQuestTurnin > apopState)
					{
						apopState = ApopState.LockedQuestTurnin;
					}
				}
				else
				{
					if (Session.MyPlayerData.HasQuest(questID) || !Session.MyPlayerData.IsQuestAcceptable(Quests.Get(questID)))
					{
						continue;
					}
					if (IsAvailable())
					{
						if (ApopState.QuestAvailable > apopState)
						{
							apopState = ApopState.QuestAvailable;
						}
					}
					else if (DontHideWhenLocked && ApopState.LockedQuestAvailable > apopState)
					{
						apopState = ApopState.LockedQuestAvailable;
					}
				}
			}
			return apopState;
		}
	}

	public override Quest TurnInQuest
	{
		get
		{
			foreach (int questID in QuestIDs)
			{
				Quest result = Quests.Get(questID);
				if (Session.MyPlayerData.IsQuestComplete(questID))
				{
					return result;
				}
			}
			return null;
		}
	}

	public bool AreQuestAvailable()
	{
		foreach (int questID in QuestIDs)
		{
			Quest quest = Quests.Get(questID);
			if (quest == null || Session.MyPlayerData.IsQuestAcceptable(quest))
			{
				return true;
			}
		}
		return false;
	}

	public override bool IsAvailable()
	{
		if (base.IsAvailable())
		{
			return AreQuestAvailable();
		}
		return false;
	}

	protected override void Init()
	{
		base.Init();
	}
}

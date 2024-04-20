public class NPCIATalk : NPCIA
{
	public string Text;

	public string icon;

	public override string Icon
	{
		get
		{
			if (!string.IsNullOrEmpty(icon))
			{
				return icon;
			}
			return "Icon-Chat";
		}
	}

	public override ApopState ApopState
	{
		get
		{
			ApopState apopState = ApopState.Talk;
			foreach (NPCIA child in children)
			{
				ApopState apopState2 = child.ApopState;
				if (IsAvailable())
				{
					if (apopState2 > apopState)
					{
						apopState = apopState2;
					}
				}
				else if (DontHideWhenLocked)
				{
					switch (apopState2)
					{
					case ApopState.QuestAvailable:
						apopState2 = ApopState.LockedQuestAvailable;
						break;
					case ApopState.QuestTurnin:
						apopState2 = ApopState.LockedQuestTurnin;
						break;
					case ApopState.QuestObjective:
						apopState2 = ApopState.LockedQuestObjective;
						break;
					}
					if (apopState2 > apopState)
					{
						apopState = apopState2;
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
			foreach (NPCIA child in children)
			{
				if (child.TurnInQuest != null)
				{
					return child.TurnInQuest;
				}
			}
			return null;
		}
	}

	protected override void Init()
	{
		base.Init();
	}
}

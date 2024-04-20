using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class Quest
{
	public static Color DefaultQuestColor = new Color32(91, 67, 6, byte.MaxValue);

	public static Color AutoQuestColor = new Color32(0, 0, 0, byte.MaxValue);

	public static Color DailyQuestColor = new Color32(130, 70, 20, byte.MaxValue);

	public int ID;

	public string Name;

	public string StartNPCName;

	public string StartText;

	public string EndNPCName;

	public string EndText;

	public int Level;

	public int MapLevel;

	public int MapMaxLevel;

	public int XP;

	public int Gold;

	public bool Auto;

	public int QSIndex;

	public int QSValue;

	public int MapID;

	public string MapName;

	public int MapEndID;

	public string EndMapName;

	public int TurnInNpcCellId;

	public int TurnInNpcSpawnId;

	public bool IsRepeatable;

	public bool IsScaling;

	public string BitFlagNameRequired;

	public byte BitFlagIndexRequired;

	public string BitFlagNameAwarded;

	public byte BitFlagIndexAwarded;

	public string RequiredBadgeName;

	public string AwardedBadgeName;

	public int ClassUnlockableRequirement;

	public string TurninText;

	public QuestTurnInType TurnInType;

	public int EndDialogID;

	public bool IsSagaQuest;

	public float fXP;

	public float fGold;

	public float fClassXP;

	public List<IAQuestStringRequiredCore> QSRequirements = new List<IAQuestStringRequiredCore>();

	public bool IsDaily;

	public bool bStaff;

	public int SagaDialogID;

	public bool HideTurnIn;

	public QuestRewardType QuestRewardType;

	public Queue<int> ChosenRewardIDs = new Queue<int>();

	public int QuestRewardOptionQty;

	public List<QuestObjective> Objectives = new List<QuestObjective>();

	public List<QuestRewardItem> Rewards = new List<QuestRewardItem>();

	public int RequiredLevel => Mathf.Max(1, Level - 3);

	public string DisplayName
	{
		get
		{
			if (IsDaily)
			{
				return Name + " (Daily)";
			}
			if (IsRepeatable || (!HasBitFlagAwarded && !IsOnQuestString))
			{
				return Name + " (Repeatable)";
			}
			return Name;
		}
	}

	public bool HasBitFlagAwarded
	{
		get
		{
			if (!string.IsNullOrEmpty(BitFlagNameAwarded))
			{
				return BitFlagIndexAwarded > 0;
			}
			return false;
		}
	}

	public bool IsOnQuestString => QSIndex > 0;

	public Color DisplayColor
	{
		get
		{
			if (IsDaily)
			{
				return DailyQuestColor;
			}
			if (IsSagaQuest)
			{
				return AutoQuestColor;
			}
			return DefaultQuestColor;
		}
	}

	public int TargetMapId
	{
		get
		{
			if (!Session.MyPlayerData.HasQuest(ID))
			{
				return MapID;
			}
			if (Session.MyPlayerData.IsQuestComplete(ID))
			{
				return MapEndID;
			}
			foreach (QuestObjective objective in Objectives)
			{
				if (!Session.MyPlayerData.IsQuestObjectiveCompleted(ID, objective.ID))
				{
					return objective.MapID;
				}
			}
			return -1;
		}
	}

	public string TargetMapName
	{
		get
		{
			if (!Session.MyPlayerData.HasQuest(ID))
			{
				return MapName;
			}
			if (Session.MyPlayerData.IsQuestComplete(ID))
			{
				return EndMapName;
			}
			foreach (QuestObjective objective in Objectives)
			{
				if (!Session.MyPlayerData.IsQuestObjectiveCompleted(ID, objective.ID))
				{
					return objective.MapName;
				}
			}
			return "";
		}
	}

	public int XPReward()
	{
		int relativeLevel = Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Entities.Instance.me.Level, MapLevel, MapMaxLevel), Level);
		if (IsScaling && relativeLevel > Level)
		{
			float num = (float)Levels.GetBaseQuestXP(relativeLevel) * fXP;
			if (IsRepeatable || (!HasBitFlagAwarded && !IsOnQuestString))
			{
				num *= 0.1f;
			}
			return Mathf.CeilToInt(num);
		}
		float num2 = 1f;
		num2 = ((!IsDaily) ? Session.MyPlayerData.XPMultiplier : Session.MyPlayerData.DailyQuestMultiplier);
		return Mathf.CeilToInt((float)XP * num2);
	}

	public int ClassXPReward(int playerLevel, int classRank)
	{
		int levelDifference = playerLevel - Level;
		float num = (float)ClassRanks.GetBaseQuestXP(classRank, levelDifference) * fClassXP * Session.MyPlayerData.CXPMultiplier;
		if (IsRepeatable || (!HasBitFlagAwarded && !IsOnQuestString))
		{
			num *= 0.1f;
		}
		return Mathf.CeilToInt(num);
	}

	public int GoldReward()
	{
		int relativeLevel = Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Entities.Instance.me.Level, MapLevel, MapMaxLevel), Level);
		if (IsScaling && relativeLevel > Level)
		{
			int num = Mathf.CeilToInt((float)StatCurves.Gold.GetBaseQuestGold(relativeLevel) * fGold * Session.MyPlayerData.GoldMultiplier);
			if (IsRepeatable || (!HasBitFlagAwarded && !IsOnQuestString))
			{
				num = Mathf.CeilToInt((float)num * 0.1f);
			}
			return num;
		}
		return Gold;
	}

	public bool IsInCategory(QuestCategory category)
	{
		switch (category)
		{
		case QuestCategory.Daily:
			return IsDaily;
		case QuestCategory.Saga:
			return IsSagaQuest;
		default:
			if (!IsDaily)
			{
				return !IsSagaQuest;
			}
			return false;
		}
	}
}

using System;
using UnityEngine;

[Serializable]
[AddComponentMenu("Interactivity/Requirements/IA Quest String Required")]
public class IAQuestStringRequired : InteractionRequirement
{
	public int QSIndex;

	public int QSValue;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Comparison == ComparisonType.GreaterThan)
		{
			return myPlayerData.GetQSValue(QSIndex) > QSValue;
		}
		if (Comparison == ComparisonType.GreaterThanOrEqual)
		{
			return myPlayerData.GetQSValue(QSIndex) >= QSValue;
		}
		if (Comparison == ComparisonType.Equal)
		{
			return myPlayerData.GetQSValue(QSIndex) == QSValue;
		}
		if (Comparison == ComparisonType.LessThanOrEqual)
		{
			return myPlayerData.GetQSValue(QSIndex) <= QSValue;
		}
		if (Comparison == ComparisonType.LessThan)
		{
			return myPlayerData.GetQSValue(QSIndex) < QSValue;
		}
		return false;
	}

	public void OnEnable()
	{
		Session.MyPlayerData.QuestStringUpdated += OnQuestStringUpdated;
	}

	private void OnQuestStringUpdated(int index, int value)
	{
		if (QSIndex == index)
		{
			OnRequirementUpdate();
		}
	}

	public void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestStringUpdated -= OnQuestStringUpdated;
		}
	}
}

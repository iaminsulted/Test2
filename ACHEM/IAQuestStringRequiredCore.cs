public class IAQuestStringRequiredCore : IARequiredCore
{
	public int QSIndex;

	public int QSValue;

	public ComparisonType Comparison;

	public IAQuestStringRequiredCore()
	{
		Session.MyPlayerData.QuestStringUpdated += OnQuestStringUpdated;
	}

	~IAQuestStringRequiredCore()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestStringUpdated -= OnQuestStringUpdated;
		}
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Comparison switch
		{
			ComparisonType.GreaterThanOrEqual => myPlayerData.GetQSValue(QSIndex) >= QSValue, 
			ComparisonType.GreaterThan => myPlayerData.GetQSValue(QSIndex) > QSValue, 
			ComparisonType.Equal => myPlayerData.GetQSValue(QSIndex) == QSValue, 
			ComparisonType.LessThan => myPlayerData.GetQSValue(QSIndex) < QSValue, 
			ComparisonType.LessThanOrEqual => myPlayerData.GetQSValue(QSIndex) <= QSValue, 
			ComparisonType.NotEqual => myPlayerData.GetQSValue(QSIndex) != QSValue, 
			_ => false, 
		};
	}

	private void OnQuestStringUpdated(int index, int value)
	{
		if (QSIndex == index)
		{
			OnRequirementUpdate();
		}
	}
}

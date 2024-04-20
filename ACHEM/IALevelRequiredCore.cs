public class IALevelRequiredCore : IARequiredCore
{
	public int Level;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Comparison == ComparisonType.GreaterThan)
		{
			return Entities.Instance.me.Level > Level;
		}
		if (Comparison == ComparisonType.GreaterThanOrEqual)
		{
			return Entities.Instance.me.Level >= Level;
		}
		if (Comparison == ComparisonType.Equal)
		{
			return Entities.Instance.me.Level == Level;
		}
		if (Comparison == ComparisonType.LessThanOrEqual)
		{
			return Entities.Instance.me.Level <= Level;
		}
		if (Comparison == ComparisonType.LessThan)
		{
			return Entities.Instance.me.Level < Level;
		}
		return false;
	}

	public IALevelRequiredCore()
	{
		Entities.Instance.me.LevelUpdated += CheckRequirement;
	}

	private void CheckRequirement()
	{
		OnRequirementUpdate();
	}

	~IALevelRequiredCore()
	{
		Entities.Instance.me.LevelUpdated -= CheckRequirement;
	}
}

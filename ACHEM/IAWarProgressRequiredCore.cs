public class IAWarProgressRequiredCore : IARequiredCore
{
	public int WarID;

	public float WarProgress;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (!Wars.HasKey(WarID))
		{
			return false;
		}
		if (Comparison == ComparisonType.GreaterThan)
		{
			return Wars.Get(WarID).ProgressPercent > WarProgress;
		}
		if (Comparison == ComparisonType.GreaterThanOrEqual)
		{
			return Wars.Get(WarID).ProgressPercent >= WarProgress;
		}
		if (Comparison == ComparisonType.Equal)
		{
			return Wars.Get(WarID).ProgressPercent == WarProgress;
		}
		if (Comparison == ComparisonType.LessThanOrEqual)
		{
			return Wars.Get(WarID).ProgressPercent <= WarProgress;
		}
		if (Comparison == ComparisonType.LessThan)
		{
			return Wars.Get(WarID).ProgressPercent < WarProgress;
		}
		return false;
	}

	public IAWarProgressRequiredCore()
	{
		Wars.WarsUpdated += base.OnRequirementUpdate;
	}

	~IAWarProgressRequiredCore()
	{
		Wars.WarsUpdated -= base.OnRequirementUpdate;
	}
}

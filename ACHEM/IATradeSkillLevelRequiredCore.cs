using StatCurves;

public class IATradeSkillLevelRequiredCore : IARequiredCore
{
	public int level;

	public TradeSkillType tradeSkillType;

	public ComparisonType comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return comparison switch
		{
			ComparisonType.GreaterThanOrEqual => Entities.Instance.me.tradeSkillLevel[tradeSkillType] >= level, 
			ComparisonType.GreaterThan => Entities.Instance.me.tradeSkillLevel[tradeSkillType] > level, 
			ComparisonType.Equal => Entities.Instance.me.tradeSkillLevel[tradeSkillType] == level, 
			ComparisonType.LessThan => Entities.Instance.me.tradeSkillLevel[tradeSkillType] < level, 
			ComparisonType.LessThanOrEqual => Entities.Instance.me.tradeSkillLevel[tradeSkillType] <= level, 
			_ => false, 
		};
	}

	public IATradeSkillLevelRequiredCore()
	{
		Entities.Instance.me.TradeSkillLevelUpdated += CheckRequirement;
	}

	private void CheckRequirement(TradeSkillType type, int level)
	{
		OnRequirementUpdate();
	}

	~IATradeSkillLevelRequiredCore()
	{
		Entities.Instance.me.TradeSkillLevelUpdated -= CheckRequirement;
	}
}

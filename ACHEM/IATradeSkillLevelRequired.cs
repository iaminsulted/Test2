using StatCurves;
using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Trade Skill Level Required")]
public class IATradeSkillLevelRequired : InteractionRequirement
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

	private void OnTradeSkillLevelUpdated(TradeSkillType type, int level)
	{
		OnRequirementUpdate();
	}

	public void OnEnable()
	{
		Entities.Instance.me.TradeSkillLevelUpdated += OnTradeSkillLevelUpdated;
	}

	public void OnDisable()
	{
		if (Entities.Instance != null && Entities.Instance.me != null)
		{
			Entities.Instance.me.TradeSkillLevelUpdated -= OnTradeSkillLevelUpdated;
		}
	}
}

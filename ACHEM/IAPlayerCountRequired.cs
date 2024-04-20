using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Player Count Required")]
public class IAPlayerCountRequired : InteractionRequirement
{
	public int TargetCount;

	public ComparisonType Comparison;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		int playerCountByCellId = Entities.Instance.GetPlayerCountByCellId(Game.Instance.CurrentCell.CellID);
		switch (Comparison)
		{
		case ComparisonType.GreaterThanOrEqual:
			if (playerCountByCellId >= TargetCount)
			{
				return true;
			}
			break;
		case ComparisonType.GreaterThan:
			if (playerCountByCellId > TargetCount)
			{
				return true;
			}
			break;
		case ComparisonType.Equal:
			if (playerCountByCellId == TargetCount)
			{
				return true;
			}
			break;
		case ComparisonType.LessThan:
			if (playerCountByCellId < TargetCount)
			{
				return true;
			}
			break;
		case ComparisonType.LessThanOrEqual:
			if (playerCountByCellId <= TargetCount)
			{
				return true;
			}
			break;
		default:
			return false;
		}
		return false;
	}
}

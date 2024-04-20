using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Level Required")]
public class IALevelRequired : InteractionRequirement
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

	public void OnEnable()
	{
		Entities.Instance.me.LevelUpdated += OnLevelUpdated;
	}

	private void OnLevelUpdated()
	{
		OnRequirementUpdate();
	}

	public void OnDisable()
	{
		if (Entities.Instance != null && Entities.Instance.me != null)
		{
			Entities.Instance.me.LevelUpdated -= OnLevelUpdated;
		}
	}
}

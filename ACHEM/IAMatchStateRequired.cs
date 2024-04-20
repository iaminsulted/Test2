using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Match State Required")]
public class IAMatchStateRequired : InteractionRequirement
{
	public MatchState MatchState;

	public ComparisonType Comparison;

	public void Start()
	{
		Game.Instance.MatchStateUpdated += OnMatchStateUpdated;
	}

	public void OnDestroy()
	{
		if (Game.Instance != null)
		{
			Game.Instance.MatchStateUpdated -= OnMatchStateUpdated;
		}
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Comparison switch
		{
			ComparisonType.GreaterThanOrEqual => Game.Instance.AreaData.MatchState >= MatchState, 
			ComparisonType.GreaterThan => Game.Instance.AreaData.MatchState > MatchState, 
			ComparisonType.Equal => Game.Instance.AreaData.MatchState == MatchState, 
			ComparisonType.LessThan => Game.Instance.AreaData.MatchState < MatchState, 
			ComparisonType.LessThanOrEqual => Game.Instance.AreaData.MatchState <= MatchState, 
			_ => false, 
		};
	}

	private void OnMatchStateUpdated()
	{
		OnRequirementUpdate();
	}
}

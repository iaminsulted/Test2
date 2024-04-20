using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Target Score Required")]
public class IATargetScoreRequired : InteractionRequirement
{
	public int teamID;

	public int targetScore;

	public void Start()
	{
		Scoreboard.ScoreUpdated += OnScoreUpdated;
		OnScoreUpdated();
	}

	public void OnDestroy()
	{
		Scoreboard.ScoreUpdated -= OnScoreUpdated;
	}

	private void OnScoreUpdated()
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Scoreboard.GetTeamScore(teamID) == targetScore;
	}
}

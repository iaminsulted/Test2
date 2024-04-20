using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Client Trigger State Required")]
public class IAClientTriggerStateRequired : InteractionRequirement
{
	public ClientTrigger ClientTrigger;

	public byte State;

	public void Awake()
	{
		ClientTrigger.StateUpdated += OnClientTriggerStateUpdated;
	}

	private void OnClientTriggerStateUpdated(byte state)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return ClientTrigger.State == State;
	}

	public void OnDestroy()
	{
		ClientTrigger.StateUpdated -= OnClientTriggerStateUpdated;
	}
}

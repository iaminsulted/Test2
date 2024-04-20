using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Client Trigger Active Required")]
public class IAClientTriggerActiveRequired : InteractionRequirement
{
	public ClientTrigger ClientTrigger;

	public bool Active;

	public void Awake()
	{
		ClientTrigger.ActiveUpdated += OnClientTriggerActiveUpdated;
	}

	private void OnClientTriggerActiveUpdated(bool active)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return ClientTrigger.IsActive == Active;
	}

	public void OnDestroy()
	{
		ClientTrigger.ActiveUpdated -= OnClientTriggerActiveUpdated;
	}
}

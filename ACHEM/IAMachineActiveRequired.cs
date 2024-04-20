using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Machine Active Required")]
public class IAMachineActiveRequired : InteractionRequirement
{
	public BaseMachine Machine;

	public bool Active;

	public void Awake()
	{
		Machine.ActiveUpdated += OnMachineActiveUpdated;
	}

	private void OnMachineActiveUpdated(bool obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		return Machine.IsActive == Active;
	}

	public void OnDestroy()
	{
		Machine.ActiveUpdated -= OnMachineActiveUpdated;
	}
}

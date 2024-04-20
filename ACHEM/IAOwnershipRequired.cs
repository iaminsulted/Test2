public class IAOwnershipRequired : InteractionRequirement
{
	public OwnerMachine ownerMachine;

	public void Awake()
	{
		ownerMachine.Initialized += OnMachineStateUpdated;
		ownerMachine.StateUpdated += OnMachineStateUpdated;
	}

	public void OnDestroy()
	{
		ownerMachine.Initialized -= OnMachineStateUpdated;
		ownerMachine.StateUpdated -= OnMachineStateUpdated;
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Entities.Instance.me == ownerMachine.Owner)
		{
			return true;
		}
		return false;
	}

	private void OnMachineStateUpdated(byte state)
	{
		OnRequirementUpdate();
	}
}

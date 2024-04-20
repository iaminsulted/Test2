public class IAMachineStateRequiredCore : IARequiredCore
{
	public BaseMachine Machine;

	public byte State;

	public bool Not;

	public IAMachineStateRequiredCore(BaseMachine machine)
	{
		Machine = machine;
		Machine.Initialized += OnMachineStateUpdated;
		Machine.StateUpdated += OnMachineStateUpdated;
	}

	private void OnMachineStateUpdated(byte state)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Machine.State == State;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	~IAMachineStateRequiredCore()
	{
		Machine.Initialized -= OnMachineStateUpdated;
		Machine.StateUpdated -= OnMachineStateUpdated;
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA Machine State Required")]
public class IAMachineStateRequired : InteractionRequirement
{
	public BaseMachine Machine;

	public byte State;

	public bool Not;

	public int MachineID = -1;

	public void Awake()
	{
		if (Machine != null)
		{
			Machine.Initialized += OnMachineStateUpdated;
			Machine.StateUpdated += OnMachineStateUpdated;
		}
	}

	private void OnMachineStateUpdated(byte state)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Machine == null)
		{
			Machine = BaseMachine.GetMachineByMachineID(MachineID);
			if (Machine == null)
			{
				return false;
			}
			Machine.Initialized += OnMachineStateUpdated;
			Machine.StateUpdated += OnMachineStateUpdated;
		}
		bool flag = Machine.State == State;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public void OnDestroy()
	{
		if (Machine != null)
		{
			Machine.Initialized -= OnMachineStateUpdated;
			Machine.StateUpdated -= OnMachineStateUpdated;
		}
	}

	public void SetMachine(BaseMachine machine)
	{
		Machine = machine;
		Machine.Initialized += OnMachineStateUpdated;
		Machine.StateUpdated += OnMachineStateUpdated;
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Trigger Machine")]
public class CTATriggerMachineByID : ClientTriggerAction
{
	public int machineID;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestMachineTrigger(machineID));
		UIWindow.ClearWindows();
	}
}

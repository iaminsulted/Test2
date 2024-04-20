using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Trigger Machine")]
public class CTATriggerMachine : ClientTriggerAction
{
	public BaseMachine Machine;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestMachineTrigger(Machine.ID));
	}
}

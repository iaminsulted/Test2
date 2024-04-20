using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Set Client Trigger State")]
public class CTASetClientTriggerState : ClientTriggerAction
{
	public ClientTrigger ClientTrigger;

	public byte State;

	protected override void OnExecute()
	{
		ClientTrigger.SetState(State, Entities.Instance.me.ID);
	}
}

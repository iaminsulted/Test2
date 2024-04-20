using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Trigger Client Trigger")]
public class CTATriggerClientTrigger : ClientTriggerAction
{
	public ClientTrigger clientTrigger;

	public bool checkRequirements;

	protected override void OnExecute()
	{
		clientTrigger.Trigger(checkRequirements);
	}
}

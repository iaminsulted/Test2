using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Bank")]
public class CTABank : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UIBankManager.Show();
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Bank")]
public class CTABankCore : ClientTriggerActionCore
{
	protected override void OnExecute()
	{
		UIBankManager.Show();
	}
}

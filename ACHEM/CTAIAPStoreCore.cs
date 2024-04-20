using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA IAP Store")]
public class CTAIAPStoreCore : ClientTriggerActionCore
{
	protected override void OnExecute()
	{
		UIIAPStore.Show();
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA IAP Store")]
public class CTAIAPStore : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UIIAPStore.Show();
	}
}

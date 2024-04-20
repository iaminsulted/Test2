using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Shop")]
public class CTADailyChest : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UIPreviewLoot.LoadShop();
	}
}

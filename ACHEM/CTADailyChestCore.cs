using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Shop")]
public class CTADailyChestCore : ClientTriggerActionCore
{
	protected override void OnExecute()
	{
		UIPreviewLoot.LoadShop();
	}
}

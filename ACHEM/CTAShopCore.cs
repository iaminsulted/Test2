using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Shop")]
public class CTAShopCore : ClientTriggerActionCore
{
	public int ID;

	protected override void OnExecute()
	{
		UIShop.LoadShop(ID);
	}
}

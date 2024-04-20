using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Craft Shop")]
public class CTACraftShopCore : ClientTriggerActionCore
{
	public int ID;

	protected override void OnExecute()
	{
		UIMerge.Load(ID);
	}
}

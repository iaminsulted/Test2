using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Craft Shop")]
public class CTACraftShop : ClientTriggerAction
{
	public int ID;

	protected override void OnExecute()
	{
		UIMerge.Load(ID);
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Infusion")]
public class CTAAugmentReroll : ClientTriggerAction
{
	public int ID;

	protected override void OnExecute()
	{
		UIAugmentReroll.Load();
	}
}

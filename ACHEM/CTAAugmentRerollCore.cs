using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/Core/CTA AugmentReroll Core")]
public class CTAAugmentRerollCore : ClientTriggerActionCore
{
	public int ID;

	protected override void OnExecute()
	{
		UIAugmentReroll.Load();
	}
}

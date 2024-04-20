using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Infusion")]
public class CTAInfusionCore : ClientTriggerActionCore
{
	public int ID;

	protected override void OnExecute()
	{
		UIInfusion.Load();
	}
}

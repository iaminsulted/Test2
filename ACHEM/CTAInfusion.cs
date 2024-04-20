using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Infusion")]
public class CTAInfusion : ClientTriggerAction
{
	public int ID;

	protected override void OnExecute()
	{
		UIInfusion.Load();
	}
}

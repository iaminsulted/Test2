using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Gender")]
public class CTAGenderCore : ClientTriggerActionCore
{
	protected override void OnExecute()
	{
		UICharGender.Show();
	}
}

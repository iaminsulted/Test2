using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Gender")]
public class CTAGender : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UICharGender.Show();
	}
}

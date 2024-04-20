using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Customize")]
public class CTACustomize : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UICharCustomize.Show();
	}
}

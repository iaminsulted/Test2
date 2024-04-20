using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Image Window Close")]
public class CTAImageWindowClose : ClientTriggerAction
{
	protected override void OnExecute()
	{
		UIImageWindow.CloseWindow();
	}
}

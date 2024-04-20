using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Close Window")]
public class CTACloseWindow : ClientTriggerAction
{
	public UIWindow window;

	protected override void OnExecute()
	{
		window.Close();
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Fancy Notify Player")]
public class CTAFancyNotifyPlayer : ClientTriggerAction
{
	public string Title;

	public string Message;

	protected override void OnExecute()
	{
		FancyNotification.ShowText(Title, Message);
	}
}

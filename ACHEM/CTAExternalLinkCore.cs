using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA External Link")]
public class CTAExternalLinkCore : ClientTriggerActionCore
{
	public string Url;

	protected override void OnExecute()
	{
		Confirmation.OpenUrl(Url);
	}
}

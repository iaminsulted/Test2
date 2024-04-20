using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Video")]
public class CTAVideoCore : ClientTriggerActionCore
{
	public string URL_Youtube;

	public string URL;

	public List<ClientTriggerAction> OnCompleteActions;

	protected override void OnExecute()
	{
	}

	private void OnComplete()
	{
		foreach (ClientTriggerAction onCompleteAction in OnCompleteActions)
		{
			onCompleteAction.Execute();
		}
	}
}

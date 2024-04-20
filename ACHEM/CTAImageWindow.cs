using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Image Window")]
public class CTAImageWindow : ClientTriggerAction
{
	public string URL;

	public List<ClientTriggerAction> Actions;

	public List<string> Labels;

	protected override void OnExecute()
	{
		UIImageWindow.LoadImageWindow(URL, Actions, Labels);
	}
}

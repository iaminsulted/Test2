using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Chat Command")]
public class CTAChatCommandCore : ClientTriggerActionCore
{
	public string CommandText;

	protected override void OnExecute()
	{
		ChatCommands.RunCommand("/" + CommandText);
	}
}

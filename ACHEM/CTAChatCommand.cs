using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Chat Command")]
public class CTAChatCommand : ClientTriggerAction
{
	public string CommandText;

	protected override void OnExecute()
	{
		string[] array = CommandText.Split('|');
		foreach (string text in array)
		{
			ChatCommands.RunCommand("/" + text);
		}
	}
}

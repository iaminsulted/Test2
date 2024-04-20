using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Private Queue")]
public class CTAPrivateQueue : ClientTriggerAction
{
	public int queueID;

	protected override void OnExecute()
	{
		PrivateQueueUI.Show(queueID);
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Private Queue")]
public class CTAPrivateQueueCore : ClientTriggerActionCore
{
	public int queueID;

	protected override void OnExecute()
	{
		PrivateQueueUI.Show(queueID);
	}
}

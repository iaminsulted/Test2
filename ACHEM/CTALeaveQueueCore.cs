using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Leave Queue")]
public class CTALeaveQueueCore : ClientTriggerActionCore
{
	public int queueID;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestLeaveQueue());
	}
}

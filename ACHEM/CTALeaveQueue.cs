using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Leave Queue")]
public class CTALeaveQueue : ClientTriggerAction
{
	public int queueID;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestLeaveQueue());
	}
}

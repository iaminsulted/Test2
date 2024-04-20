using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Enter Queue")]
public class CTAEnterQueueCore : ClientTriggerActionCore
{
	public int queueID;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestJoinQueue(queueID));
	}
}

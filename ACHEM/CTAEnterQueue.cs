using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Enter Queue")]
public class CTAEnterQueue : ClientTriggerAction
{
	public int queueID;

	protected override void OnExecute()
	{
		AEC.getInstance().sendRequest(new RequestJoinQueue(queueID));
	}
}

using System.Collections.Generic;

public class CTAAsync : ClientTriggerAction
{
	public List<ClientTriggerAction> OnCompleteActions = new List<ClientTriggerAction>();

	protected void OnComplete()
	{
		if (OnCompleteActions == null)
		{
			return;
		}
		foreach (ClientTriggerAction onCompleteAction in OnCompleteActions)
		{
			onCompleteAction.Execute();
		}
	}
}

using System.Collections.Generic;

public class CTAAsyncCore : ClientTriggerActionCore
{
	public List<ClientTriggerActionCore> OnCompleteActions = new List<ClientTriggerActionCore>();

	protected void OnComplete()
	{
		if (OnCompleteActions == null)
		{
			return;
		}
		foreach (ClientTriggerActionCore onCompleteAction in OnCompleteActions)
		{
			onCompleteAction.Execute();
		}
	}
}

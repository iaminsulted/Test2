using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Open Apop")]
public class CTAOpenApopCore : ClientTriggerActionCore
{
	public int ApopID;

	protected override void OnExecute()
	{
		ApopViewer.Show(ApopID);
	}
}

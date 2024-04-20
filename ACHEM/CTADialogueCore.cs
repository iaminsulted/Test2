using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Dialogue")]
public class CTADialogueCore : CTAAsyncCore
{
	public int ID;

	public bool SkipCompleteAction;

	protected override void OnExecute()
	{
		DialogueSlotManager.Show(ID, base.OnComplete, SkipCompleteAction);
	}
}

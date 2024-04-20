using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Random Dialogue")]
public class CTARandomDialogueCore : CTAAsyncCore
{
	public List<int> IDs = new List<int>();

	public bool SkipCompleteAction;

	protected override void OnExecute()
	{
		DialogueSlotManager.Show(IDs[Random.Range(0, IDs.Count)], base.OnComplete, SkipCompleteAction);
	}
}

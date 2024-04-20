using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Cinematic")]
public class CTACinematicCore : CTAAsyncCore
{
	public string Name;

	protected override void OnExecute()
	{
		DialogueSlotManager.Show(Name, base.OnComplete);
	}
}

using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Cinematic")]
public class CTACinematic : CTAAsync
{
	public string Name;

	protected override void OnExecute()
	{
		DialogueSlotManager.Show(Name, base.OnComplete);
	}
}

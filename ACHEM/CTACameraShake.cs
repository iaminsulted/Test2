using UnityEngine;

public class CTACameraShake : ClientTriggerAction
{
	public SpellCamShake.Style style;

	[Range(0.1f, 10f)]
	public float shakeMultiplier = 0.1f;

	[Range(0.1f, 10f)]
	public float duration = 0.1f;

	protected override void OnExecute()
	{
		Game.Instance.camController.PlayCameraShake(style, shakeMultiplier, duration);
	}
}

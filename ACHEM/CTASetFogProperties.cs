using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Set Fog Properties")]
public class CTASetFogProperties : ClientTriggerAction
{
	public float LerpTime = 1f;

	public bool Fog;

	public FogMode FogMode;

	public Color FogColor;

	public float FogDensity;

	public float FogStartDistance;

	public float FogEndDistance;

	private Camera main;

	private void Start()
	{
		main = Camera.main;
	}

	protected override void OnExecute()
	{
		RenderSettings.fog = Fog;
		RenderSettings.fogMode = FogMode;
		StartCoroutine(LerpFogProperties());
	}

	private IEnumerator LerpFogProperties()
	{
		Color startFogColor = RenderSettings.fogColor;
		Color endFogColor = FogColor;
		float startFogDensity = RenderSettings.fogDensity;
		float endFogDensity = FogDensity;
		float startFogStartDistance = RenderSettings.fogStartDistance;
		float endFogStartDistance = FogStartDistance;
		float startFogEndDistance = RenderSettings.fogEndDistance;
		float endFogEndDistance = FogEndDistance;
		float timeElapsed = 0f;
		while (timeElapsed < LerpTime)
		{
			timeElapsed += Time.deltaTime;
			float t = timeElapsed / LerpTime;
			RenderSettings.fogColor = Color.Lerp(startFogColor, endFogColor, t);
			RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, endFogDensity, t);
			RenderSettings.fogStartDistance = Mathf.Lerp(startFogStartDistance, endFogStartDistance, t);
			RenderSettings.fogEndDistance = Mathf.Lerp(startFogEndDistance, endFogEndDistance, t);
			main.backgroundColor = RenderSettings.fogColor;
			yield return new WaitForEndOfFrame();
		}
		RenderSettings.fogColor = endFogColor;
		RenderSettings.fogDensity = endFogDensity;
		RenderSettings.fogStartDistance = endFogStartDistance;
		RenderSettings.fogEndDistance = endFogEndDistance;
		main.backgroundColor = RenderSettings.fogColor;
	}
}

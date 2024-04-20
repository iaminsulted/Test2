using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Set Light Properties")]
public class CTASetLightProperties : ClientTriggerAction
{
	public string TargetID;

	public Light TargetLight;

	public float LerpTime = 1f;

	public bool Disable;

	public Color Color;

	public float Intensity;

	public float IndirectMultiplier;

	public LightShadows ShadowType;

	public bool DrawHalo;

	public Flare Flare;

	public LightRenderMode RenderMode;

	public LayerMask CullingMask;

	public Vector3 LocalRotation;

	public Light GetTargetLight
	{
		get
		{
			if (TargetLight == null)
			{
				TargetLight = UniqueID.Get(TargetID).GetComponent<Light>();
			}
			return TargetLight;
		}
	}

	protected override void OnExecute()
	{
		Light getTargetLight = GetTargetLight;
		getTargetLight.enabled = !Disable;
		getTargetLight.shadows = ShadowType;
		getTargetLight.flare = Flare;
		getTargetLight.renderMode = RenderMode;
		getTargetLight.cullingMask = CullingMask;
		StartCoroutine(LerpFogProperties());
	}

	private IEnumerator LerpFogProperties()
	{
		Light light = GetTargetLight;
		Color startColor = light.color;
		Color endColor = Color;
		float startIntensity = light.intensity;
		float endIntensity = Intensity;
		float startIndirectMultiplier = light.bounceIntensity;
		float endIndirectMultiplier = IndirectMultiplier;
		Quaternion startRotation = light.transform.localRotation;
		Quaternion endRotation = Quaternion.Euler(LocalRotation);
		float timeElapsed = 0f;
		while (timeElapsed < LerpTime)
		{
			timeElapsed += Time.deltaTime;
			float t = timeElapsed / LerpTime;
			light.color = Color.Lerp(startColor, endColor, t);
			light.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
			light.bounceIntensity = Mathf.Lerp(startIndirectMultiplier, endIndirectMultiplier, t);
			light.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
			yield return new WaitForEndOfFrame();
		}
		light.color = endColor;
		light.intensity = endIntensity;
		light.bounceIntensity = endIndirectMultiplier;
		light.transform.localRotation = endRotation;
	}
}

using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraEffects : MonoBehaviour
{
	private DepthOfField dof;

	private BloomOptimized bloom;

	public Transform focusTransform;

	public void Start()
	{
		SetDoF((bool)SettingsManager.UseDepthOfField && focusTransform != null);
		SetBloom(SettingsManager.UseBloom);
	}

	public void SetDoF(bool value)
	{
		if (value)
		{
			if (dof == null)
			{
				dof = base.gameObject.AddComponent<DepthOfField>();
				dof.focalSize = 0.2f;
				dof.aperture = 0.3f;
				dof.highResolution = true;
				dof.dofHdrShader = Shader.Find("Hidden/Dof/DepthOfFieldHdr");
			}
			dof.focalTransform = focusTransform;
		}
		else if (dof != null)
		{
			Object.Destroy(dof);
		}
	}

	public void SetBloom(bool value)
	{
		if (value)
		{
			if (bloom == null)
			{
				bloom = base.gameObject.AddComponent<BloomOptimized>();
				bloom.intensity = 0.4f;
				bloom.fastBloomShader = Shader.Find("Hidden/FastBloom");
			}
		}
		else if (bloom != null)
		{
			Object.Destroy(bloom);
		}
	}
}

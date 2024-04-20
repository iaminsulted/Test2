using System;
using System.Collections;
using Assets.Scripts.Game;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class NPCCamController : MonoBehaviour
{
	public static int MASK_BASE;

	public Camera npccam;

	public static NPCCamController Instance;

	public float Duration;

	private Camera maincam;

	private DepthOfField dof;

	private BloomOptimized bloom;

	private Transform targetTransform;

	private Transform focusTransform;

	public static event Action<bool> ActiveUpdated;

	static NPCCamController()
	{
		MASK_BASE |= 1 << Layers.DEFAULT;
		MASK_BASE |= 1 << Layers.WATER;
		MASK_BASE |= 1 << Layers.TERRAIN;
		MASK_BASE |= 1 << Layers.PHYSICS;
		MASK_BASE |= 1 << Layers.FAR;
		MASK_BASE |= 1 << Layers.MIDDLE;
		MASK_BASE |= 1 << Layers.MIDDLEFAR;
		MASK_BASE |= 1 << Layers.CLOSE;
		MASK_BASE |= 1 << Layers.BG;
		MASK_BASE |= 1 << Layers.SNOW;
	}

	private void Awake()
	{
		Instance = this;
		npccam.enabled = false;
	}

	public static void BeginNPCPush(Transform camT, Transform focusT, int mask)
	{
		if (!(camT == null))
		{
			NPCCamController.ActiveUpdated?.Invoke(obj: true);
			Instance.Init(camT, focusT, mask);
		}
	}

	public static void Close()
	{
		if (Instance != null)
		{
			Instance.Cancel();
		}
	}

	private void Init(Transform camT, Transform focusT, int mask)
	{
		if (maincam == null)
		{
			maincam = Camera.main;
		}
		maincam.enabled = false;
		npccam.enabled = true;
		npccam.cullingMask = mask;
		npccam.farClipPlane = Game.Instance.CurrentCell.CameraFarPlane;
		CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
		targetTransform = camT;
		focusTransform = focusT;
		SetDoF((bool)SettingsManager.UseDepthOfField && focusT != null);
		SetBloom(SettingsManager.UseBloom);
		StartCoroutine("PushIn");
	}

	private void Cancel()
	{
		StopCoroutine("PushIn");
		if (maincam == null)
		{
			maincam = Camera.main;
		}
		maincam.enabled = true;
		npccam.enabled = false;
		NPCCamController.ActiveUpdated?.Invoke(obj: false);
	}

	private IEnumerator PushIn()
	{
		float currentTime = 0f;
		Vector3 startingPos = maincam.transform.position;
		Quaternion startingRot = maincam.transform.rotation;
		base.transform.position = startingPos;
		base.transform.rotation = startingRot;
		Vector3 targetPosition = targetTransform.position;
		Quaternion targetRotation = targetTransform.rotation;
		while (currentTime < Duration)
		{
			yield return null;
			currentTime += Time.deltaTime;
			float num = Mathf.Clamp01(currentTime / Duration);
			float t = Mathf.Sin(num * MathF.PI * 0.5f);
			float t2 = Mathf.Pow(num, 1.5f);
			if (targetTransform == null)
			{
				Cancel();
				break;
			}
			base.transform.SetPositionAndRotation(Vector3.Lerp(startingPos, targetPosition, t), Quaternion.Slerp(startingRot, targetRotation, t2));
			if (dof != null)
			{
				dof.aperture = Mathf.Lerp(0.3f, 0.625f, num);
			}
		}
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
			UnityEngine.Object.Destroy(dof);
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
			UnityEngine.Object.Destroy(bloom);
		}
	}
}

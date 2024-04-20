using System;
using System.Collections;
using UnityEngine;

namespace AQ3D.DialogueSystem;

public class DialogueCameraSlot : MonoBehaviour
{
	public enum LightingType
	{
		morning,
		afternoon,
		evening,
		gloomy,
		night,
		campfire,
		scene
	}

	private LightProbes sceneprobes;

	private SavedLightProbeData lpData;

	public Transform ActiveCam;

	public LightingType CurrentType;

	private int probeId;

	private string probeName;

	public GameObject ProbesMorning;

	public GameObject ProbesAfternoon;

	public GameObject ProbesEvening;

	public GameObject ProbesNight;

	public GameObject ProbesGloomy;

	public GameObject ProbesCamp;

	private void Awake()
	{
		sceneprobes = LightmapSettings.lightProbes;
	}

	public void InitCam(Vector3 camPos, Vector3 camRotation)
	{
		if (ActiveCam != null)
		{
			ActiveCam.localEulerAngles = camRotation;
			ActiveCam.localPosition = camPos;
			if (!ActiveCam.gameObject.activeInHierarchy)
			{
				ActiveCam.gameObject.SetActive(value: true);
			}
		}
	}

	private void SetProbeData()
	{
		if (lpData == null)
		{
			lpData = Resources.Load<SavedLightProbeData>("DialogueLightProbeData");
		}
		lpData.CreateProbeData(probeId, probeName, LightmapSettings.lightProbes);
	}

	public void InitLighting(LightingType lt)
	{
		sceneprobes = LightmapSettings.lightProbes;
		if (lpData == null)
		{
			lpData = Resources.Load<SavedLightProbeData>("DialogueLightProbeData");
		}
		if (lt != LightingType.scene)
		{
			CurrentType = lt;
			switch (lt)
			{
			case LightingType.morning:
				LightmapSettings.lightProbes = ProbesMorning.GetComponent<BakeProbes>().bakedProbes;
				break;
			case LightingType.afternoon:
				LightmapSettings.lightProbes = ProbesAfternoon.GetComponent<BakeProbes>().bakedProbes;
				break;
			case LightingType.evening:
				LightmapSettings.lightProbes = ProbesEvening.GetComponent<BakeProbes>().bakedProbes;
				break;
			case LightingType.night:
				LightmapSettings.lightProbes = ProbesNight.GetComponent<BakeProbes>().bakedProbes;
				break;
			case LightingType.gloomy:
				LightmapSettings.lightProbes = ProbesGloomy.GetComponent<BakeProbes>().bakedProbes;
				break;
			case LightingType.campfire:
				LightmapSettings.lightProbes = ProbesCamp.GetComponent<BakeProbes>().bakedProbes;
				break;
			}
		}
	}

	public void ResetLighting()
	{
		LightmapSettings.lightProbes = sceneprobes;
	}

	public void MoveCam(DialogueFrameTransition trans, SlotPosition slotpos)
	{
		StartCoroutine(movecam(trans, slotpos));
	}

	private IEnumerator movecam(DialogueFrameTransition trans, SlotPosition pos)
	{
		Vector3 spos = pos.Position;
		Vector3 epos = trans.Position;
		Vector3 srot = pos.Rotation;
		Vector3 erot = trans.Rotation;
		yield return new WaitForSeconds(trans.Delay);
		float elapsedtime = 0f;
		while (elapsedtime < trans.TransitionTime)
		{
			elapsedtime += Time.deltaTime;
			float num = elapsedtime / trans.TransitionTime;
			if (trans.Easing != 0)
			{
				switch (trans.Easing)
				{
				case 1:
					num = 1f - Mathf.Cos(num * MathF.PI * 0.5f);
					break;
				case 2:
					num = Mathf.Sin(num * MathF.PI * 0.5f);
					break;
				case 3:
					num *= num;
					break;
				case 4:
					num = num * num * (3f - 2f * num);
					break;
				}
			}
			ActiveCam.localPosition = Vector3.Lerp(spos, epos, num);
			ActiveCam.localEulerAngles = Vector3.Lerp(srot, erot, num);
			yield return null;
		}
		ActiveCam.localPosition = epos;
		ActiveCam.localEulerAngles = erot;
	}
}

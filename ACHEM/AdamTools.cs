using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AdamTools : MonoBehaviour
{
	private class FlyCamSettings
	{
		public string Name;

		public float MainSpeed;

		public float ShiftAdd;

		public float MaxShift;
	}

	private const bool log = true;

	public const string helpText = "Y - Resubmit chat command\nC - Toggle character\nMouse Button 4/F2 - Take screenshot\nControl + Mouse Button 4/F2 - Take multiple hires screenshot\nG - Toggle green screen\nH - Cycle through camera transforms\nJ - Toggle name plate\nV - Toggle animators\nF - Toggle between look and player camera\nN - Toggle GUI\n/getanimationnames\n/setanimation <animationName> <time> <speed>\n/setentityface <eye x> <eye y> <mouth x> <mouth y>\n/getmyanimationnames\n/setmyanimation <animationName> <time> <speed>\n/resetmyanimations use this to reenable your characters animations\n/setmyface <eye x> <eye y> <mouth x> <mouth y>\n/movetome\n/greenscreendistance <distance>\n";

	private int currentCameraPositionIndex = -1;

	private bool isGreenScreenOn;

	private bool areAnimatorsOn = true;

	private bool isCheatsEnabled;

	private bool isFlyCamOn;

	private GameObject gui;

	private UIPanel namePlate;

	private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

	private List<Animator> animators = new List<Animator>();

	private DataPrefab dataPrefab;

	private Camera mainCamera;

	private Transform cameraParent;

	private LayerMask layerMask;

	private static GetCubeMap myGetCubeMap;

	private Vector3[] cameraPositions = new Vector3[6]
	{
		new Vector3(0f, 2.58f, -6.2f),
		new Vector3(0.0556f, 1.2393f, 4f),
		new Vector3(0.0556f, 1.2393f, 4f),
		new Vector3(0.7027f, 1.2607f, 3.3043f),
		new Vector3(0.313f, 1.2304f, 2.5294f),
		new Vector3(0.1f, 1.64f, 1.15f)
	};

	private readonly Vector3[] cameraRotations = new Vector3[6]
	{
		new Vector3(7.7157f, 6.0234f, 0f),
		new Vector3(2f, -179.0546f, 0f),
		new Vector3(2f, -167.27f, 0f),
		new Vector3(2f, -153.3f, 0f),
		new Vector3(-3.1f, -159.1395f, 0f),
		new Vector3(-9.2331f, -174.8226f, 0f)
	};

	private int flyCamSettingsIndex = 2;

	private readonly FlyCamSettings[] flyCamSettings = new FlyCamSettings[5]
	{
		new FlyCamSettings
		{
			Name = "Very slow",
			MainSpeed = 0.5f,
			ShiftAdd = 1f,
			MaxShift = 2f
		},
		new FlyCamSettings
		{
			Name = "Slow",
			MainSpeed = 2.5f,
			ShiftAdd = 5f,
			MaxShift = 10f
		},
		new FlyCamSettings
		{
			Name = "Normal",
			MainSpeed = 5f,
			ShiftAdd = 10f,
			MaxShift = 20f
		},
		new FlyCamSettings
		{
			Name = "Fast",
			MainSpeed = 10f,
			ShiftAdd = 20f,
			MaxShift = 40f
		},
		new FlyCamSettings
		{
			Name = "Very Fast",
			MainSpeed = 40f,
			ShiftAdd = 80f,
			MaxShift = 160f
		}
	};

	private static readonly Dictionary<string, AnimationClip> stringClip = new Dictionary<string, AnimationClip>();

	private static RuntimeAnimatorController playerruntimeAnimatorController;

	private int charRenderToggleState;

	private bool namePlatesActive = true;

	private static Entity SelectedEntity => Entities.Instance.me.Target;

	private FlyCamSettings currentFlyCamSettings => flyCamSettings[flyCamSettingsIndex];

	public static void GetEntityAnimationNames()
	{
		if (SelectedEntity == null)
		{
			Chat.Notify("No Entity Selected");
			return;
		}
		string text = "";
		Animator componentInChildren = SelectedEntity.wrapper.GetComponentInChildren<Animator>();
		RuntimeAnimatorController runtimeAnimatorController = componentInChildren.runtimeAnimatorController;
		if (!componentInChildren.gameObject.GetComponent<AnimationOverrides>())
		{
			List<AnimationClip> list = new List<AnimationClip>();
			AnimationClip[] animationClips = runtimeAnimatorController.animationClips;
			foreach (AnimationClip animationClip in animationClips)
			{
				if (!list.Contains(animationClip))
				{
					list.Add(animationClip);
					if (!stringClip.ContainsKey(SelectedEntity.name + animationClip.name.ToLower()))
					{
						stringClip.Add(SelectedEntity.name + animationClip.name.ToLower(), animationClip);
					}
				}
				text = text + "Name: " + animationClip.name + " FrameCount: " + animationClip.length * animationClip.frameRate + "\n";
			}
			componentInChildren.gameObject.AddComponent<AnimationOverrides>().Animations = list.ToArray();
		}
		else
		{
			AnimationClip[] animationClips = componentInChildren.gameObject.GetComponent<AnimationOverrides>().Animations;
			foreach (AnimationClip animationClip2 in animationClips)
			{
				text = text + "Name: " + animationClip2.name + " FrameCount: " + animationClip2.length * animationClip2.frameRate + "\n";
			}
		}
		Chat.Notify(text);
	}

	public static void GetMyAnimationNames()
	{
		if (Entities.Instance.me == null)
		{
			Chat.Notify("You don't exist somehow !?!?!?");
			return;
		}
		string text = "";
		Animator componentInChildren = Entities.Instance.me.wrapper.GetComponentInChildren<Animator>();
		RuntimeAnimatorController runtimeAnimatorController = componentInChildren.runtimeAnimatorController;
		if (!componentInChildren.gameObject.GetComponent<AnimationOverrides>())
		{
			List<AnimationClip> list = new List<AnimationClip>();
			AnimationClip[] animationClips = runtimeAnimatorController.animationClips;
			foreach (AnimationClip animationClip in animationClips)
			{
				if (!list.Contains(animationClip))
				{
					list.Add(animationClip);
					if (!stringClip.ContainsKey("Player" + animationClip.name.ToLower()))
					{
						stringClip.Add("Player" + animationClip.name.ToLower(), animationClip);
					}
				}
				text = text + "Name: " + animationClip.name + " FrameCount: " + animationClip.length * animationClip.frameRate + "\n";
			}
			componentInChildren.gameObject.AddComponent<AnimationOverrides>().Animations = list.ToArray();
		}
		else
		{
			AnimationClip[] animationClips = componentInChildren.gameObject.GetComponent<AnimationOverrides>().Animations;
			foreach (AnimationClip animationClip2 in animationClips)
			{
				text = text + "Name: " + animationClip2.name + " FrameCount: " + animationClip2.length * animationClip2.frameRate + "\n";
			}
		}
		Chat.Notify(text);
	}

	public static void SetMyAnimation(string animationName, float time, float speed)
	{
		if (Entities.Instance.me == null)
		{
			Chat.Notify("You don't exist somehow !?!?!?");
			return;
		}
		Animator componentInChildren = Entities.Instance.me.wrapper.GetComponentInChildren<Animator>();
		if (!componentInChildren.gameObject.GetComponent<AnimationOverrides>())
		{
			RuntimeAnimatorController runtimeAnimatorController = componentInChildren.runtimeAnimatorController;
			List<AnimationClip> list = new List<AnimationClip>();
			AnimationClip[] animationClips = runtimeAnimatorController.animationClips;
			foreach (AnimationClip animationClip in animationClips)
			{
				if (!list.Contains(animationClip))
				{
					list.Add(animationClip);
					if (!stringClip.ContainsKey("Player" + animationClip.name.ToLower()))
					{
						stringClip.Add("Player" + animationClip.name.ToLower(), animationClip);
					}
				}
			}
			componentInChildren.gameObject.AddComponent<AnimationOverrides>().Animations = list.ToArray();
		}
		if (!stringClip.ContainsKey("Player" + animationName.ToLower()))
		{
			Chat.Notify("Can't find animation named " + animationName + " in Player");
			return;
		}
		if (componentInChildren.runtimeAnimatorController != (AnimatorOverrideController)Resources.Load("AdamToolsOverride"))
		{
			playerruntimeAnimatorController = componentInChildren.runtimeAnimatorController;
			componentInChildren.runtimeAnimatorController = UnityEngine.Object.Instantiate((AnimatorOverrideController)Resources.Load("AdamToolsOverride"));
		}
		AnimationClip animationClip3 = ((componentInChildren.runtimeAnimatorController as AnimatorOverrideController)["AdamToolsMainState"] = stringClip["Player" + animationName.ToLower()]);
		float normalizedTime = ((!(time > animationClip3.length * animationClip3.frameRate)) ? (1f / (animationClip3.length * animationClip3.frameRate) * time) : 1f);
		componentInChildren.Play("AdamToolsMainState", 0, normalizedTime);
		componentInChildren.speed = speed;
	}

	public void CheatsOn()
	{
		isCheatsEnabled = !isCheatsEnabled;
		ToggleNamePlates(!isCheatsEnabled);
		Log(isCheatsEnabled ? "Cheats enabled." : "Cheats disabled.");
		if (isCheatsEnabled)
		{
			Chat.Notify("Y - Resubmit chat command\nC - Toggle character\nMouse Button 4/F2 - Take screenshot\nControl + Mouse Button 4/F2 - Take multiple hires screenshot\nG - Toggle green screen\nH - Cycle through camera transforms\nJ - Toggle name plate\nV - Toggle animators\nF - Toggle between look and player camera\nN - Toggle GUI\n/getanimationnames\n/setanimation <animationName> <time> <speed>\n/setentityface <eye x> <eye y> <mouth x> <mouth y>\n/getmyanimationnames\n/setmyanimation <animationName> <time> <speed>\n/resetmyanimations use this to reenable your characters animations\n/setmyface <eye x> <eye y> <mouth x> <mouth y>\n/movetome\n/greenscreendistance <distance>\n", "[ffffff]");
		}
		_ = isCheatsEnabled;
	}

	public static void ResetMyController()
	{
		Animator componentInChildren = Entities.Instance.me.wrapper.GetComponentInChildren<Animator>();
		componentInChildren.runtimeAnimatorController = playerruntimeAnimatorController;
		componentInChildren.speed = 1f;
	}

	public static void SetAllEntityAnimations(string animationName, float time, float speed)
	{
		SetMyAnimation(animationName, time, speed);
		foreach (NPC activeNPC in Entities.Instance.GetActiveNPCs())
		{
			Animator componentInChildren = activeNPC.wrapper.GetComponentInChildren<Animator>();
			if (!componentInChildren.gameObject.GetComponent<AnimationOverrides>())
			{
				RuntimeAnimatorController runtimeAnimatorController = componentInChildren.runtimeAnimatorController;
				List<AnimationClip> list = new List<AnimationClip>();
				AnimationClip[] animationClips = runtimeAnimatorController.animationClips;
				foreach (AnimationClip animationClip in animationClips)
				{
					if (!list.Contains(animationClip))
					{
						list.Add(animationClip);
						if (!stringClip.ContainsKey(activeNPC.name + animationClip.name.ToLower()))
						{
							stringClip.Add(activeNPC.name + animationClip.name.ToLower(), animationClip);
						}
					}
				}
				componentInChildren.gameObject.AddComponent<AnimationOverrides>().Animations = list.ToArray();
			}
			if (!stringClip.ContainsKey(activeNPC.name + animationName.ToLower()))
			{
				Chat.Notify("Can't find animation named " + animationName + " in " + activeNPC.name);
				break;
			}
			if (componentInChildren.runtimeAnimatorController != (AnimatorOverrideController)Resources.Load("AdamToolsOverride"))
			{
				componentInChildren.runtimeAnimatorController = UnityEngine.Object.Instantiate((AnimatorOverrideController)Resources.Load("AdamToolsOverride"));
			}
			AnimationClip animationClip3 = ((componentInChildren.runtimeAnimatorController as AnimatorOverrideController)["AdamToolsMainState"] = stringClip[activeNPC.name + animationName.ToLower()]);
			float normalizedTime = ((!(time > animationClip3.length * animationClip3.frameRate)) ? (1f / (animationClip3.length * animationClip3.frameRate) * time) : 1f);
			componentInChildren.Play("AdamToolsMainState", 0, normalizedTime);
			componentInChildren.speed = speed;
		}
	}

	public static void SetEntityAnimation(string animationName, float time, float speed)
	{
		if (SelectedEntity == null)
		{
			Chat.Notify("No Entity Selected");
			return;
		}
		Animator componentInChildren = SelectedEntity.wrapper.GetComponentInChildren<Animator>();
		if (!componentInChildren.gameObject.GetComponent<AnimationOverrides>())
		{
			RuntimeAnimatorController runtimeAnimatorController = componentInChildren.runtimeAnimatorController;
			List<AnimationClip> list = new List<AnimationClip>();
			AnimationClip[] animationClips = runtimeAnimatorController.animationClips;
			foreach (AnimationClip animationClip in animationClips)
			{
				if (!list.Contains(animationClip))
				{
					list.Add(animationClip);
					if (!stringClip.ContainsKey(SelectedEntity.name + animationClip.name.ToLower()))
					{
						stringClip.Add(SelectedEntity.name + animationClip.name.ToLower(), animationClip);
					}
				}
			}
			componentInChildren.gameObject.AddComponent<AnimationOverrides>().Animations = list.ToArray();
		}
		if (!stringClip.ContainsKey(SelectedEntity.name + animationName.ToLower()))
		{
			Chat.Notify("Can't find animation named " + animationName + " in " + SelectedEntity.name);
			return;
		}
		if (componentInChildren.runtimeAnimatorController != (AnimatorOverrideController)Resources.Load("AdamToolsOverride"))
		{
			componentInChildren.runtimeAnimatorController = UnityEngine.Object.Instantiate((AnimatorOverrideController)Resources.Load("AdamToolsOverride"));
		}
		AnimationClip animationClip3 = ((componentInChildren.runtimeAnimatorController as AnimatorOverrideController)["AdamToolsMainState"] = stringClip[SelectedEntity.name + animationName.ToLower()]);
		float normalizedTime = ((!(time > animationClip3.length * animationClip3.frameRate)) ? (1f / (animationClip3.length * animationClip3.frameRate) * time) : 1f);
		componentInChildren.Play("AdamToolsMainState", 0, normalizedTime);
		componentInChildren.speed = speed;
	}

	public static void SetEntityFace(Vector4 faceOffset)
	{
		if (SelectedEntity == null)
		{
			Chat.Notify("No Entity Selected");
			return;
		}
		if (SelectedEntity.assetController.GetType() == typeof(PlayerAssetController))
		{
			PlayerAssetController obj = SelectedEntity.assetController as PlayerAssetController;
			obj.StopBlink();
			obj.SetFaceOffset(faceOffset);
			return;
		}
		Renderer[] componentsInChildren = SelectedEntity.assetController.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if ((bool)renderer.material && renderer.material.HasProperty("_FaceOffset"))
			{
				renderer.material.SetVector("_FaceOffset", faceOffset);
			}
		}
	}

	public static void SetMyFace(Vector4 faceOffset)
	{
		PlayerAssetController obj = Entities.Instance.me.assetController as PlayerAssetController;
		obj.StopBlink();
		obj.SetFaceOffset(faceOffset);
	}

	public static void MoveEntity()
	{
		if (SelectedEntity == null)
		{
			Chat.Notify("No Entity Selected");
			return;
		}
		Vector3 position = Entities.Instance.me.position;
		float y = Entities.Instance.me.rotation.eulerAngles.y;
		SelectedEntity.Teleport(position.x, position.y, position.z, y);
	}

	public static void CreateDialogueBg(string imageType)
	{
		if (!Directory.Exists("C:/Screenshots"))
		{
			Directory.CreateDirectory("C:/Screenshots");
		}
		if (Camera.main.GetComponent<GetCubeMap>() == null)
		{
			Camera.main.gameObject.AddComponent<GetCubeMap>();
			myGetCubeMap = Camera.main.gameObject.GetComponent<GetCubeMap>();
		}
		myGetCubeMap.Location = "C:/Screenshots/";
		myGetCubeMap.Name = "DBG_" + Game.Instance.AreaData.displayName.Replace(" ", "") + "_" + DateTime.UtcNow.ToString("HHmm");
		myGetCubeMap.CreateDialogueCube(imageType);
	}

	public static void GreenScreenDistance(float value)
	{
		Color32 color = ((value == 0f) ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) : new Color32(31, byte.MaxValue, 46, byte.MaxValue));
		if (value == 0f)
		{
			value = 2500f;
		}
		GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = color;
		GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = value;
	}

	public static bool HasAccess()
	{
		if (Session.MyPlayerData != null)
		{
			if (Session.MyPlayerData.AccessLevel < 100)
			{
				return Session.MyPlayerData.CheckBitFlag("iu1", 52);
			}
			return true;
		}
		return false;
	}

	public void Awake()
	{
		Reset();
	}

	private void Reset()
	{
		dataPrefab = UnityEngine.Object.FindObjectOfType<DataPrefab>();
		currentCameraPositionIndex = -1;
		isGreenScreenOn = false;
		areAnimatorsOn = true;
		isCheatsEnabled = false;
		isFlyCamOn = false;
		gui = null;
		namePlate = null;
		meshRenderers.Clear();
		animators.Clear();
		flyCamSettingsIndex = 2;
		if (!(dataPrefab == null))
		{
			mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
			layerMask = mainCamera.cullingMask;
			if (!(mainCamera == null) && !(mainCamera.GetComponent<FlyCam>() == null))
			{
				cameraParent = mainCamera.transform.parent;
				mainCamera.GetComponent<FlyCam>().mainSpeed = currentFlyCamSettings.MainSpeed;
				mainCamera.GetComponent<FlyCam>().shiftAdd = currentFlyCamSettings.ShiftAdd;
				mainCamera.GetComponent<FlyCam>().maxShift = currentFlyCamSettings.MaxShift;
			}
		}
	}

	private void LateUpdate()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if (dataPrefab == null)
		{
			Reset();
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F1))
		{
			isCheatsEnabled = !isCheatsEnabled;
			ToggleNamePlates(!isCheatsEnabled);
			Log(isCheatsEnabled ? "Cheats enabled." : "Cheats disabled.");
			if (isCheatsEnabled)
			{
				Chat.Notify("Y - Resubmit chat command\nC - Toggle character\nMouse Button 4/F2 - Take screenshot\nControl + Mouse Button 4/F2 - Take multiple hires screenshot\nG - Toggle green screen\nH - Cycle through camera transforms\nJ - Toggle name plate\nV - Toggle animators\nF - Toggle between look and player camera\nN - Toggle GUI\n/getanimationnames\n/setanimation <animationName> <time> <speed>\n/setentityface <eye x> <eye y> <mouth x> <mouth y>\n/getmyanimationnames\n/setmyanimation <animationName> <time> <speed>\n/resetmyanimations use this to reenable your characters animations\n/setmyface <eye x> <eye y> <mouth x> <mouth y>\n/movetome\n/greenscreendistance <distance>\n", "[ffffff]");
			}
		}
		if (!isCheatsEnabled)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			Chat.Instance.ResubmitLastCommand();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (charRenderToggleState == 0 || charRenderToggleState == 1)
			{
				Renderer[] componentsInChildren = Entities.Instance.me.wrapper.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer obj in componentsInChildren)
				{
					obj.enabled = !obj.enabled;
				}
			}
			if (charRenderToggleState == 1 || charRenderToggleState == 2)
			{
				Renderer component = Entities.Instance.me.wrapper.transform.Find("charGO/Model").GetComponent<Renderer>();
				component.enabled = !component.enabled;
				Entities.Instance.me.wrapper.GetComponent<PlayerAssetController>().ToggleGloves(component.enabled);
			}
			charRenderToggleState = (charRenderToggleState + 1) % 3;
		}
		if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.Mouse4))
		{
			UIRoot component2 = UnityEngine.Object.FindObjectOfType<NamePlates>().GetComponent<UIRoot>();
			component2.maximumHeight = 5000;
			component2.adjustByDPI = true;
			bool allowDynamicResolution = mainCamera.allowDynamicResolution;
			bool allowMSAA = mainCamera.allowMSAA;
			int width = Screen.width;
			int height = Screen.height;
			float num = 2160f / (float)height;
			int num2 = Mathf.RoundToInt((float)width * num);
			int num3 = 2160;
			if (!Directory.Exists("C:/Screenshots"))
			{
				Directory.CreateDirectory("C:/Screenshots");
			}
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftMeta) && !Input.GetKey(KeyCode.LeftShift))
			{
				Camera camera = new GameObject("HiResCamera").AddComponent<Camera>();
				camera.transform.parent = mainCamera.transform;
				camera.transform.localPosition = Vector3.zero;
				camera.transform.localRotation = Quaternion.identity;
				camera.fieldOfView = mainCamera.fieldOfView;
				camera.cullingMask = mainCamera.cullingMask;
				camera.farClipPlane = mainCamera.farClipPlane;
				camera.backgroundColor = mainCamera.backgroundColor;
				camera.allowMSAA = true;
				camera.allowDynamicResolution = true;
				RenderTexture renderTexture = new RenderTexture(num2, num3, 32);
				renderTexture.antiAliasing = 8;
				renderTexture.anisoLevel = 16;
				camera.targetTexture = renderTexture;
				Texture2D texture2D = new Texture2D(num2, num3, TextureFormat.RGB24, mipChain: false);
				camera.Render();
				RenderTexture.active = renderTexture;
				texture2D.ReadPixels(new Rect(0f, 0f, num2, num3), 0, 0);
				camera.targetTexture = null;
				RenderTexture.active = null;
				UnityEngine.Object.Destroy(renderTexture);
				byte[] bytes = texture2D.EncodeToPNG();
				string text = string.Format("C:/Screenshots/HiResMainCamera_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), num2, num3);
				File.WriteAllBytes(text, bytes);
				Log("Hires Screenshot saved: " + text);
				UnityEngine.Object.Destroy(camera.gameObject);
			}
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftMeta))
			{
				Camera camera2 = new GameObject("HiResCamera").AddComponent<Camera>();
				camera2.transform.parent = mainCamera.transform;
				camera2.transform.localPosition = Vector3.zero;
				camera2.transform.localRotation = Quaternion.identity;
				camera2.fieldOfView = mainCamera.fieldOfView;
				camera2.cullingMask = mainCamera.cullingMask;
				camera2.farClipPlane = mainCamera.farClipPlane;
				camera2.backgroundColor = mainCamera.backgroundColor;
				camera2.allowMSAA = true;
				camera2.allowDynamicResolution = true;
				RenderTexture renderTexture3 = (camera2.targetTexture = new RenderTexture(num2, num3, 32));
				Texture2D texture2D2 = new Texture2D(num2, num3, TextureFormat.RGB24, mipChain: false);
				camera2.Render();
				RenderTexture.active = renderTexture3;
				texture2D2.ReadPixels(new Rect(0f, 0f, num2, num3), 0, 0);
				camera2.targetTexture = null;
				RenderTexture.active = null;
				UnityEngine.Object.Destroy(renderTexture3);
				byte[] bytes2 = texture2D2.EncodeToPNG();
				string text2 = string.Format("C:/Screenshots/HiResMainCamera_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), num2, num3);
				File.WriteAllBytes(text2, bytes2);
				Log("Hires Screenshot saved: " + text2);
				mainCamera.allowDynamicResolution = true;
				mainCamera.allowMSAA = true;
				text2 = string.Format("C:/Screenshots/Screenshot_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), width, height);
				ScreenCapture.CaptureScreenshot(text2, 1);
				mainCamera.allowDynamicResolution = allowDynamicResolution;
				mainCamera.allowMSAA = allowMSAA;
				Log("Multiple Screenshots saved: " + text2);
				Camera component3 = GameObject.Find("UICamera").GetComponent<Camera>();
				renderTexture3 = (component3.targetTexture = new RenderTexture(num2, num3, 32));
				Texture2D texture2D3 = new Texture2D(num2, num3);
				component3.Render();
				RenderTexture.active = renderTexture3;
				texture2D3.ReadPixels(new Rect(0f, 0f, num2, num3), 0, 0);
				component3.targetTexture = null;
				RenderTexture.active = null;
				UnityEngine.Object.Destroy(renderTexture3);
				byte[] bytes3 = texture2D3.EncodeToPNG();
				text2 = string.Format("C:/Screenshots/HiResUICamera_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), num2, num3);
				File.WriteAllBytes(text2, bytes3);
				Log("Multiple Screenshots saved: " + text2);
				Camera camera3 = new GameObject("AlphaCam").AddComponent<Camera>();
				camera3.transform.parent = mainCamera.transform;
				camera3.transform.localPosition = Vector3.zero;
				camera3.transform.localRotation = Quaternion.identity;
				camera3.fieldOfView = mainCamera.fieldOfView;
				camera3.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(Layers.PLAYER_ME), LayerMask.LayerToName(Layers.OTHER_PLAYERS), LayerMask.LayerToName(Layers.NPCS));
				camera3.backgroundColor = new Color(0f, 0f, 0f, 0f);
				camera3.allowMSAA = true;
				camera3.allowDynamicResolution = true;
				renderTexture3 = (camera3.targetTexture = new RenderTexture(num2, num3, 32));
				Texture2D texture2D4 = new Texture2D(num2, num3);
				camera3.Render();
				RenderTexture.active = renderTexture3;
				texture2D4.ReadPixels(new Rect(0f, 0f, num2, num3), 0, 0);
				camera3.targetTexture = null;
				RenderTexture.active = null;
				UnityEngine.Object.Destroy(renderTexture3);
				byte[] bytes4 = texture2D4.EncodeToPNG();
				text2 = string.Format("C:/Screenshots/TransparentCharacters_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), num2, num3);
				File.WriteAllBytes(text2, bytes4);
				UnityEngine.Object.Destroy(camera3.gameObject);
				UnityEngine.Object.Destroy(camera2.gameObject);
				mainCamera.allowDynamicResolution = allowDynamicResolution;
				mainCamera.allowMSAA = allowMSAA;
				Log("Multiple Screenshots saved: " + text2);
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				TakeTransparentScreenshot();
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			isGreenScreenOn = !isGreenScreenOn;
			Log(isGreenScreenOn ? "Green Screen enabled." : "Green Screen disabled.");
			if (isGreenScreenOn)
			{
				mainCamera.backgroundColor = new Color32(31, byte.MaxValue, 46, byte.MaxValue);
				mainCamera.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(Layers.PLAYER_ME), LayerMask.LayerToName(Layers.OTHER_PLAYERS), LayerMask.LayerToName(Layers.NPCS));
			}
			else
			{
				mainCamera.cullingMask = layerMask;
			}
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			currentCameraPositionIndex = (currentCameraPositionIndex + 1) % cameraPositions.Length;
			Transform obj2 = mainCamera.transform;
			obj2.localPosition = cameraPositions[currentCameraPositionIndex];
			obj2.localRotation = new Quaternion
			{
				eulerAngles = cameraRotations[currentCameraPositionIndex]
			};
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			namePlatesActive = !namePlatesActive;
			ToggleNamePlates(namePlatesActive);
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			areAnimatorsOn = !areAnimatorsOn;
			if (animators.Count == 0)
			{
				animators = UnityEngine.Object.FindObjectsOfType<Animator>().ToList();
			}
			foreach (Animator animator in animators)
			{
				if (animator != null)
				{
					animator.enabled = areAnimatorsOn;
				}
			}
			Log(areAnimatorsOn ? "Animators enabled." : "Animators disabled.");
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (isFlyCamOn)
			{
				mainCamera.transform.parent = cameraParent;
				Entities.Instance.me.IsMovementDisabled = false;
				mainCamera.GetComponentInParent<CameraController>().enabled = true;
				mainCamera.GetComponent<FlyCam>().enabled = false;
				currentCameraPositionIndex = 0;
				mainCamera.transform.localPosition = Vector3.zero;
				mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			else
			{
				Entities.Instance.me.IsMovementDisabled = true;
				mainCamera.GetComponentInParent<CameraController>().enabled = false;
				mainCamera.GetComponent<FlyCam>().enabled = true;
				mainCamera.transform.parent = null;
			}
			isFlyCamOn = !isFlyCamOn;
			Log(isFlyCamOn ? "Flycam enabled." : "Flycam disabled.");
		}
		if (isFlyCamOn)
		{
			bool flag = false;
			if (Input.GetKeyDown(KeyCode.Equals))
			{
				flag = true;
				flyCamSettingsIndex++;
			}
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				flag = true;
				flyCamSettingsIndex--;
			}
			if (flag)
			{
				flyCamSettingsIndex = Mathf.Clamp(flyCamSettingsIndex, 0, flyCamSettings.Length - 1);
				mainCamera.GetComponentInParent<FlyCam>().mainSpeed = currentFlyCamSettings.MainSpeed;
				mainCamera.GetComponentInParent<FlyCam>().shiftAdd = currentFlyCamSettings.ShiftAdd;
				mainCamera.GetComponentInParent<FlyCam>().maxShift = currentFlyCamSettings.MaxShift;
				Log("Flycam speed changed to: " + currentFlyCamSettings.Name);
			}
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			if (gui == null)
			{
				gui = GameObject.Find("UI Root (2D) - Game");
			}
			gui.SetActive(!gui.activeSelf);
			Log(gui.activeSelf ? "GUI enabled." : "GUI disabled.");
		}
	}

	private void ToggleNamePlates(bool active)
	{
		if (namePlate == null)
		{
			namePlate = UnityEngine.Object.FindObjectOfType<NamePlates>().GetComponent<UIPanel>();
		}
		namePlate.alpha = (active ? 1 : 0);
		NPCIATrigger[] array = UnityEngine.Object.FindObjectsOfType<NPCIATrigger>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Renderer>().enabled = namePlate.alpha != 0f;
		}
	}

	public static void TakeTransparentScreenshot()
	{
		Camera component = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		UIRoot component2 = UnityEngine.Object.FindObjectOfType<NamePlates>().GetComponent<UIRoot>();
		component2.maximumHeight = 5000;
		component2.adjustByDPI = true;
		bool allowDynamicResolution = component.allowDynamicResolution;
		bool allowMSAA = component.allowMSAA;
		int width = Screen.width;
		int height = Screen.height;
		float num = 2160f / (float)height;
		int num2 = Mathf.RoundToInt((float)width * num);
		int num3 = 2160;
		if (!Directory.Exists("C:/Screenshots"))
		{
			Directory.CreateDirectory("C:/Screenshots");
		}
		Camera camera = UnityEngine.Object.Instantiate(component);
		camera.name = "AlphaCam";
		camera.transform.parent = component.transform.parent;
		camera.transform.position = component.transform.position;
		camera.transform.rotation = component.transform.rotation;
		camera.allowMSAA = true;
		camera.allowDynamicResolution = true;
		camera.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(Layers.PLAYER_ME), LayerMask.LayerToName(Layers.OTHER_PLAYERS), LayerMask.LayerToName(Layers.NPCS), LayerMask.LayerToName(Layers.SNOW));
		RenderTexture targetTexture = camera.targetTexture;
		CameraClearFlags clearFlags = camera.clearFlags;
		RenderTexture active = RenderTexture.active;
		Texture2D texture2D = new Texture2D(num2, num3, TextureFormat.ARGB32, mipChain: false);
		Texture2D texture2D2 = new Texture2D(num2, num3, TextureFormat.ARGB32, mipChain: false);
		Texture2D texture2D3 = new Texture2D(num2, num3, TextureFormat.ARGB32, mipChain: false);
		RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 24, RenderTextureFormat.ARGB32);
		Rect source = new Rect(0f, 0f, num2, num3);
		RenderTexture.active = temporary;
		camera.targetTexture = temporary;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.black;
		camera.Render();
		texture2D2.ReadPixels(source, 0, 0);
		texture2D2.Apply();
		camera.backgroundColor = Color.white;
		camera.Render();
		texture2D.ReadPixels(source, 0, 0);
		texture2D.Apply();
		for (int i = 0; i < texture2D3.height; i++)
		{
			for (int j = 0; j < texture2D3.width; j++)
			{
				float num4 = texture2D.GetPixel(j, i).r - texture2D2.GetPixel(j, i).r;
				num4 = 1f - num4;
				Color color = ((num4 != 0f) ? (texture2D2.GetPixel(j, i) / num4) : Color.clear);
				color.a = num4;
				texture2D3.SetPixel(j, i, color);
			}
		}
		byte[] bytes = texture2D3.EncodeToPNG();
		string text = string.Format("C:/Screenshots/TransparentCharacters_{0}_{1}x{2}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), num2, num3);
		File.WriteAllBytes(text, bytes);
		camera.clearFlags = clearFlags;
		camera.targetTexture = targetTexture;
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		UnityEngine.Object.Destroy(texture2D2);
		UnityEngine.Object.Destroy(texture2D);
		UnityEngine.Object.Destroy(texture2D3);
		UnityEngine.Object.Destroy(camera.gameObject);
		Camera.main.allowDynamicResolution = allowDynamicResolution;
		Camera.main.allowMSAA = allowMSAA;
		Chat.Notify("Saved Transparent Image: " + text, "[ffffff]");
	}

	private void Log(string message)
	{
		Debug.LogError(message);
	}
}

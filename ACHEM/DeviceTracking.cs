using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeviceTracking : MonoBehaviour
{
	public enum DeviceEvent
	{
		Main_SettingsInitialized = 1,
		Main_FBManagerInitialized = 2,
		Main_LoginManagerInitialized = 3,
		Main_LoginStateLoadedOk = 4,
		LoginState_StartedOk = 10,
		LoginState_VersionAndEnvironmentTextDisplayedOk = 11,
		LoginState_DisplayGuestButtonOk = 12,
		LoginState_AttemptAutoLoginAQ3D = 13,
		LoginState_AttemptAutoLoginGuest = 14,
		LoginState_Complete = 15,
		LoginState_OnAccountCreateClick = 20,
		LoginState_OnLoginUsernamePasswordClick = 21,
		LoginState_OnLoginFBClick = 22,
		LoginState_OnLoginGuestClick = 23,
		LoginManager_AttemptLoginAQ3D = 30,
		LoginManager_ResponseSuccessLoginAQ3D = 31,
		LoginManager_AttemptLoginFacebook = 32,
		LoginManager_ResponseSuccessLoginFacebook = 33,
		LoginManager_AttemptLoginGuest = 34,
		LoginManager_ResponseSuccessLoginGuest = 35,
		LoginManager_AttemptLoginApple = 36,
		LoginManager_ResponseSuccessLoginApple = 37,
		AccountCreate_InitOk = 40,
		AccountCreate_CreateNewLoginAQ3DClick = 41,
		AccountCreate_AttemptCreateNewLoginAQ3D = 42,
		AccountCreate_NewLoginAQ3DCreatedOk = 43,
		AccountCreate_LinkNewLoginAQ3DToExistingGuest = 44,
		AccountCreate_DiscardGuestCreateNewLoginAQ3D = 45
	}

	private static bool processingList = false;

	private static List<WWWForm> wwwList = new List<WWWForm>();

	public long EventBitTracker;

	private static DeviceTracking instance;

	public static DeviceTracking Instance => instance;

	public void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (wwwList.Count > 0 && !processingList)
		{
			processingList = true;
			List<WWWForm> listClone = new List<WWWForm>(wwwList);
			wwwList.Clear();
			StartCoroutine(ProcessList(listClone));
		}
	}

	private IEnumerator ProcessList(List<WWWForm> listClone)
	{
		foreach (WWWForm item in listClone)
		{
			using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/RecordDeviceEvent", item);
			yield return www.SendWebRequest();
		}
		processingList = false;
	}

	public void RecordDeviceEvent(DeviceEvent deviceEvent)
	{
		if (!Main.EnvironmentsInitialized)
		{
			Debug.Log("Environments must be initialized to call RecordDeviceEvent");
		}
		else if (!EventComplete(deviceEvent))
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
			wWWForm.AddField("DeviceEvent", (int)deviceEvent);
			wwwList.Add(wWWForm);
		}
	}

	public UnityWebRequest RecordDeviceData(string AppsFlyerID)
	{
		if (!Main.EnvironmentsInitialized)
		{
			Debug.LogError("Environments must be initialized to call RecordDeviceData");
			return null;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
		wWWForm.AddField("DeviceType", SystemInfo.deviceType.ToString());
		wWWForm.AddField("DeviceModel", TrimBrackets(SystemInfo.deviceModel));
		wWWForm.AddField("OperatingSystem", TrimBrackets(SystemInfo.operatingSystem));
		wWWForm.AddField("ProcessorType", SystemInfo.processorType);
		wWWForm.AddField("ProcessorCount", SystemInfo.processorCount);
		wWWForm.AddField("ProcessorFrequency", SystemInfo.processorFrequency);
		wWWForm.AddField("MaxTextureSize", SystemInfo.maxTextureSize);
		wWWForm.AddField("SystemMemorySize", SystemInfo.systemMemorySize);
		wWWForm.AddField("GraphicsMemorySize", SystemInfo.graphicsMemorySize);
		wWWForm.AddField("GraphicsDeviceID", SystemInfo.graphicsDeviceID);
		wWWForm.AddField("GraphicsDeviceName", SystemInfo.graphicsDeviceName);
		wWWForm.AddField("GraphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
		wWWForm.AddField("GraphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
		wWWForm.AddField("NpotSupport", SystemInfo.npotSupport.ToString());
		wWWForm.AddField("Supports2DArrayTextures", Convert.ToInt32(SystemInfo.supports2DArrayTextures));
		wWWForm.AddField("Supports3DTextures", Convert.ToInt32(SystemInfo.supports3DTextures));
		wWWForm.AddField("SupportsAudio", Convert.ToInt32(SystemInfo.supportsAudio));
		wWWForm.AddField("SupportsComputeShaders", Convert.ToInt32(SystemInfo.supportsComputeShaders));
		wWWForm.AddField("SupportsCubemapArrayTextures", Convert.ToInt32(SystemInfo.supportsCubemapArrayTextures));
		wWWForm.AddField("SupportsImageEffects", 1);
		wWWForm.AddField("SupportsLocationService", Convert.ToInt32(SystemInfo.supportsLocationService));
		wWWForm.AddField("SupportsMotionVectors", Convert.ToInt32(SystemInfo.supportsMotionVectors));
		wWWForm.AddField("SupportsRawShadowDepthSampling", Convert.ToInt32(SystemInfo.supportsRawShadowDepthSampling));
		wWWForm.AddField("SupportsSparseTextures", Convert.ToInt32(SystemInfo.supportsSparseTextures));
		wWWForm.AddField("AppsFlyerID", AppsFlyerID);
		return UnityWebRequest.Post(Main.WebServiceURL + "/Game/RecordDeviceData", wWWForm);
	}

	private string TrimBrackets(string msg)
	{
		return msg.Replace("<", "").Replace(">", "");
	}

	private bool EventComplete(DeviceEvent deviceEvent)
	{
		return Util.BitGet(EventBitTracker, (int)deviceEvent);
	}
}

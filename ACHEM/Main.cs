using System;
using System.Collections;
using AppsFlyerSDK;
using CodeStage.AdvancedFPSCounter;
using IngameDebugConsole;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
	public const string GLOBAL_BUILD_VERSION_PATH_EDITOR = "http://global.aq3d.com/api/Client/GetClientVersion?key=7qNbrUHnRrpg&clientID=";

	public static int ClientID;

	public static string ClientDisplayVersion;

	public static int DevBuild;

	public static Environment Environment;

	public static bool IsPTR;

	public static bool LocalhostWebAPI;

	public static EditorAutoLogin EditorAutoLogin;

	public static string APPLICATION_PATH;

	public static string TextureURL;

	public static string AssetBundleURL;

	public static string WebServiceURL;

	public static string WebServiceURL_ForceSecure;

	public static string WebAccountURL;

	public static string FBRedirectURL_ForceSecure;

	public static string BundlePath;

	public static string TermsURL = "https://www.artix.com/policy-terms";

	public static string BugReportURL = "https://bugs.artix.com/tracker/category?appID=11";

	public static string ContactSupportURL = "https://support.artix.com/hc/en-us/requests/new?ticket_form_id=360000070392";

	public static string StatusTwitterURL = "https://twitter.com/aq3dstatus";

	public static bool EnvironmentsInitialized;

	public static string httpPrefix = "https://";

	public static string SupportURL = "http://support.artix.com";

	public int clientID;

	public int devBuild;

	public bool localhostWebAPI;

	public bool PTR;

	public EditorAutoLogin editorAutoLogin;

	public static string pubMinReq;

	public static string pubCurrentVersion;

	[ReadOnly]
	public ClientVersion clientVersion;

	public ControlScheme controlScheme;

	[Tooltip("If checked the editor will display ALL messages (Log+), unchecked only Warning+ will be displayed")]
	public bool EnableLog;

	public bool ShowMessages;

	private bool delayShowVersionUpdateRequired;

	private string delayRequiredClientVersion;

	private void Awake()
	{
		IsPTR = PTR;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Screen.sleepTimeout = -1;
		if (Platform.IsEditor)
		{
			UIGame.ControlScheme = controlScheme;
		}
		httpPrefix = (Platform.IsMobile ? "https://" : "http://");
		Debug.unityLogger.filterLogType = (EnableLog ? LogType.Log : LogType.Warning);
		if (PlayerPrefs.HasKey("PASSWORD") && !PlayerPrefs.HasKey("PASSWORDENCODE"))
		{
			string value = Util.Base64Encode(PlayerPrefs.GetString("PASSWORD"));
			PlayerPrefs.DeleteKey("PASSWORD");
			PlayerPrefs.SetString("PASSWORDENCODE", value);
			PlayerPrefs.Save();
		}
		base.gameObject.AddComponent<SteamManager>();
		base.gameObject.AddComponent<SteamSystem>();
	}

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		NotificationManager.Instance.CancelAdventureAwaitsNotification();
		TextureCompositor.Init(base.transform);
		SettingsManager.ToggleFPSMonitorUpdated += ToggleFPSMonitor;
		ToggleFPSMonitor(SettingsManager.FPSMonitor);
		SettingsManager.ToggleDebugOutputLogUpdated += ToggleDebugOutputLogUpdated;
		ToggleDebugOutputLogUpdated(SettingsManager.DebugOutputLog);
		AudioManager.Init(GetComponent<SFXPlayer>());
		AEC.IsLogEnabled = ShowMessages;
		StateManager.Instance.LoadState("scene.initialize");
		Initialize();
	}

	private void ToggleDebugOutputLogUpdated(bool enabled)
	{
		UnityEngine.Object.FindObjectOfType<DebugLogManager>()?.gameObject.SetActive(enabled);
	}

	private void Initialize()
	{
		StartCoroutine(InitializeRoutine());
	}

	private IEnumerator InitializeRoutine()
	{
		GlobalApiRequestGetClientPlatformVersion globalApiRequestGetClientPlatformVersion = new GlobalApiRequestGetClientPlatformVersion(clientID);
		using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(globalApiRequestGetClientPlatformVersion.GetURL()))
		{
			string errorTitle = "Unable to load version data";
			string friendlyMsg = "Please check your internet connection and press OK to try again!";
			string customContext = unityWebRequest.url;
			yield return unityWebRequest.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(unityWebRequest, customContext);
			if (unityWebRequest.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, unityWebRequest.error, unityWebRequest.responseCode, null, customContext, null, showMessageBox: true, Initialize);
				yield break;
			}
			if (unityWebRequest.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, unityWebRequest.error, null, customContext, null, showMessageBox: true, Initialize);
				yield break;
			}
			if (unityWebRequest.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, unityWebRequest.error, null, customContext, null, showMessageBox: true, Initialize);
				yield break;
			}
			try
			{
				GlobalApiResponseGetClientPlatformVersion globalApiResponseGetClientPlatformVersion = JsonConvert.DeserializeObject<GlobalApiResponseGetClientPlatformVersion>(unityWebRequest.downloadHandler.text);
				string text = globalApiResponseGetClientPlatformVersion.minRequiredVersion;
				if (text == null)
				{
					text = "0.0.0";
				}
				string currentVersion = (pubCurrentVersion = Application.version);
				pubMinReq = text;
				if (globalApiResponseGetClientPlatformVersion == null)
				{
					ShowGenericUpdateRequired();
					yield break;
				}
				if (globalApiResponseGetClientPlatformVersion.Error == GlobalApiResponseGetClientPlatformVersion.Error_ClientIdNotFound)
				{
					ShowGenericUpdateRequired();
					yield break;
				}
				if (globalApiResponseGetClientPlatformVersion.clientVersion == null)
				{
					errorTitle = "Client version record was null";
					friendlyMsg = "Please check your internet connection and try again!";
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, unityWebRequest.downloadHandler.text, null, customContext, null, showMessageBox: false);
					ShowGenericUpdateRequired();
					yield break;
				}
				ClientID = globalApiResponseGetClientPlatformVersion.clientVersion.clientID;
				ClientDisplayVersion = Application.version;
				BundlePath = globalApiResponseGetClientPlatformVersion.clientVersion.bundlePath;
				Environment = globalApiResponseGetClientPlatformVersion.clientVersion.environment;
				DevBuild = devBuild;
				LocalhostWebAPI = localhostWebAPI;
				if (Platform.IsEditor)
				{
					EditorAutoLogin = editorAutoLogin;
				}
				else
				{
					EditorAutoLogin = new EditorAutoLogin();
				}
				if (!CheckVersion(currentVersion, text))
				{
					delayShowVersionUpdateRequired = true;
					delayRequiredClientVersion = globalApiResponseGetClientPlatformVersion.minRequiredVersion;
					PlayerPrefs.SetInt("DisableAutoLogin", 1);
					PlayerPrefs.Save();
				}
				if (PlayerPrefs.GetInt("Environment") != (int)Environment)
				{
					PlayerPrefs.DeleteKey("CurrentServerID");
					PlayerPrefs.SetInt("Environment", (int)Environment);
					PlayerPrefs.Save();
					Caching.ClearCache();
				}
				InitEnvironmentURLs();
			}
			catch (Exception ex)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, null, showMessageBox: true, Initialize);
				yield break;
			}
		}
		using UnityWebRequest unityWebRequest = DeviceTracking.Instance.RecordDeviceData(AppsFlyer.getAppsFlyerId());
		yield return unityWebRequest.SendWebRequest();
		long.TryParse(unityWebRequest.downloadHandler.text, out DeviceTracking.Instance.EventBitTracker);
		SettingsManager.Init();
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.Main_SettingsInitialized);
		FBManager.Initialize();
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.Main_FBManagerInitialized);
		LoginManager.Initialize();
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.Main_LoginManagerInitialized);
		StateManager.Instance.LoadState("scene.login");
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.Main_LoginStateLoadedOk);
		if (IsPTR)
		{
			UIManager.Instance.AddPTRWatermark(devBuild);
		}
		NotificationManager.Init();
		if (delayShowVersionUpdateRequired && delayRequiredClientVersion != null)
		{
			ShowVersionSpecificUpdateRequired(delayRequiredClientVersion);
		}
	}

	public static void OpenAppleAppStore()
	{
		Application.OpenURL("https://itunes.apple.com/us/app/adventurequest-3d/id968076300");
		Application.Quit();
	}

	public static void OpenGooglePlayStore()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.battleon.aq3d");
		Application.Quit();
	}

	public static void CloseApplication()
	{
		Application.Quit();
	}

	public static void InitEnvironmentURLs()
	{
		if (IsPTR)
		{
			BugReportURL = "https://bugs.artix.com/tracker/category?appID=17";
			Environment = Environment.PTR;
		}
		switch (Environment)
		{
		case Environment.Live:
			APPLICATION_PATH = httpPrefix + "cdn.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://game.aq3d.com/api";
			WebServiceURL = httpPrefix + "game.aq3d.com/api";
			WebAccountURL = httpPrefix + "game.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://game.aq3d.com";
			break;
		case Environment.LiveBypassCDN:
			APPLICATION_PATH = httpPrefix + "game.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://game.aq3d.com/api";
			WebServiceURL = httpPrefix + "game.aq3d.com/api";
			WebAccountURL = httpPrefix + "game.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://game.aq3d.com";
			break;
		case Environment.Stage:
			APPLICATION_PATH = httpPrefix + "stage.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://stage.aq3d.com/api";
			WebServiceURL = httpPrefix + "stage.aq3d.com/api";
			WebAccountURL = httpPrefix + "stage.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://stage.aq3d.com";
			break;
		case Environment.Content:
			APPLICATION_PATH = httpPrefix + "content.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://content.aq3d.com/api";
			WebServiceURL = httpPrefix + "content.aq3d.com/api";
			WebAccountURL = httpPrefix + "content.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://content.aq3d.com";
			break;
		case Environment.Code:
			APPLICATION_PATH = httpPrefix + "code.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://code.aq3d.com/api";
			WebServiceURL = httpPrefix + "code.aq3d.com/api";
			WebAccountURL = httpPrefix + "code.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://code.aq3d.com";
			break;
		case Environment.Code2:
			APPLICATION_PATH = httpPrefix + "code2.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://code2.aq3d.com/api";
			WebServiceURL = httpPrefix + "code2.aq3d.com/api";
			WebAccountURL = httpPrefix + "code2.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://code2.aq3d.com";
			break;
		case Environment.PTR:
			APPLICATION_PATH = httpPrefix + "ptr.aq3d.com/game";
			WebServiceURL_ForceSecure = "https://ptr.aq3d.com/api";
			WebServiceURL = httpPrefix + "ptr.aq3d.com/api";
			WebAccountURL = httpPrefix + "ptr.aq3d.com/account";
			FBRedirectURL_ForceSecure = "https://ptr.aq3d.com";
			break;
		}
		if (LocalhostWebAPI)
		{
			WebServiceURL = "http://localhost:51232";
			WebServiceURL_ForceSecure = "http://localhost:51232";
		}
		AssetBundleURL = GetBundlePath();
		TextureURL = APPLICATION_PATH + "/gamefiles/textures/";
		Debug.Log("APPLICATION_PATH : '" + APPLICATION_PATH + "'");
		Debug.Log("WebServiceURL_ForceSecure: " + WebServiceURL_ForceSecure);
		Debug.Log("WebServiceURL: " + WebServiceURL);
		Debug.Log("WebAccountURL: " + WebAccountURL);
		Debug.Log("FacebookRedirectPrefix: " + FBRedirectURL_ForceSecure);
		Debug.Log("TextureURL: " + TextureURL);
		EnvironmentsInitialized = true;
	}

	private static string GetBundlePath()
	{
		string text = "/web/";
		text = "/pc/";
		return APPLICATION_PATH + "/gamefiles/" + BundlePath + text;
	}

	public void OnApplicationQuit()
	{
		Debug.Log(" > OnApplicationQuit");
		if (AEC.getInstance() != null)
		{
			AEC.getInstance().close();
		}
		NotificationManager.Instance.ScheduleAdventureAwaitsNotification();
	}

	public static void ShowGenericUpdateRequired()
	{
		MessageBox.Show("Update Required", "Close and reopen Steam to install the latest AQ3D update to play!", "Exit", delegate
		{
			CloseApplication();
		});
	}

	public static void ShowVersionSpecificUpdateRequired(string minRequiredVersion)
	{
		MessageBox.Show("Update Required", "Close and reopen Steam to install AQ3D version " + minRequiredVersion + "!", "Exit", delegate
		{
			CloseApplication();
		});
	}

	private void ToggleFPSMonitor(bool newValue)
	{
		AFPSCounter.Instance.OperationMode = (newValue ? OperationMode.Normal : OperationMode.Disabled);
	}

	private void OnDisable()
	{
		SettingsManager.ToggleFPSMonitorUpdated -= ToggleFPSMonitor;
		SettingsManager.ToggleDebugOutputLogUpdated -= ToggleDebugOutputLogUpdated;
	}

	public static bool CheckVersion(string currentVersion, string requiredVersion)
	{
		int num = int.Parse(requiredVersion.Substring(0, 1));
		int num2 = int.Parse(requiredVersion.Substring(2, requiredVersion.Substring(2).LastIndexOf(".")));
		int num3 = int.Parse(requiredVersion.Substring(requiredVersion.LastIndexOf(".") + 1));
		int num4 = int.Parse(currentVersion.Substring(0, 1));
		int num5 = int.Parse(currentVersion.Substring(2, currentVersion.Substring(2).LastIndexOf(".")));
		int num6 = int.Parse(currentVersion.Substring(currentVersion.LastIndexOf(".") + 1));
		if (num == num4)
		{
			if (num2 == num5)
			{
				return num6 >= num3;
			}
			return num5 > num2;
		}
		return num4 > num;
	}
}

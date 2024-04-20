using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppsFlyerSDK;
using Assets.Scripts.Game;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.CrashReportHandler;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
	public static bool firstTimeLoadingLoginState = true;

	public static bool firstGuestSession = false;

	private string cachedAppleID;

	private static LoginManager instance;

	public static LoginManager Instance => instance;

	public static void Initialize()
	{
		instance = new GameObject("LoginManager").AddComponent<LoginManager>();
	}

	public static void LoginAQ3D(string email, string password)
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_AttemptLoginAQ3D);
		instance.StartCoroutine(instance.LoginAQ3DRoutine(email, password));
	}

	private IEnumerator LoginAQ3DRoutine(string email, string password)
	{
		Loader.show("Logging in with AQ3D Account...", 0f);
		WebApiRequestLoginAQ3D request = new WebApiRequestLoginAQ3D(email, password);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/LoginAQ3D", request.GetWWWForm());
		string errorTitle = "Login Error";
		string friendlyMsg = "There was an error with login. Please check your connection or try again later.";
		string customContext = "Email: " + email;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			friendlyMsg = "The login server is currently experiencing issues. Please try again later.";
			LoginFailed();
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			friendlyMsg = "There was an error connecting to the login server. The login server may currently be offline. Please check your connection or try again later.";
			LoginFailed();
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		try
		{
			WebApiResponseLoginAQ3D webApiResponseLoginAQ3D = JsonConvert.DeserializeObject<WebApiResponseLoginAQ3D>(www.downloadHandler.text);
			if (webApiResponseLoginAQ3D.HasStandardErrorActions())
			{
				yield break;
			}
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_ResponseSuccessLoginAQ3D);
			PlayerPrefs.SetInt("HasAQ3DAccount", webApiResponseLoginAQ3D.Account.UserID);
			PlayerPrefs.SetInt("LastLoginMethod", 1);
			PlayerPrefs.Save();
			Session.Account = webApiResponseLoginAQ3D.Account;
			Session.IsGuest = false;
			Session.IsReconnectable = true;
			LoginSuccess();
		}
		catch (Exception ex)
		{
			LoginFailed();
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
	}

	public static void LoginFacebook()
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_AttemptLoginFacebook);
		instance.StartCoroutine(instance.LoginFacebookRoutine(0));
	}

	private IEnumerator LoginFacebookRoutine(int acceptedTerms)
	{
		Loader.show("Logging in with Facebook...", 0f);
		string guestToken = (HasGuestGuid() ? GetGuestGuid() : "");
		string linkSource = "";
		if (PlayerPrefs.HasKey("LINKSOURCE"))
		{
			linkSource = PlayerPrefs.GetString("LINKSOURCE");
		}
		WebApiRequestLoginFacebook request = new WebApiRequestLoginFacebook(guestToken, FBManager.FacebookAccessToken, GetAQ3DUserID(), FBManager.AQ3DIntention, FBManager.VerifiedAQ3DEmail, FBManager.VerifiedAQ3DPassword, linkSource, acceptedTerms);
		FBManager.AQ3DIntention = 0;
		FBManager.VerifiedAQ3DEmail = "";
		FBManager.VerifiedAQ3DPassword = "";
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/LoginFacebook", request.GetWWWForm());
		string errorTitle = "Login Facebook Failed!";
		string friendlyMsg = "Unable to login with Facebook. Please try again or contact support at " + Main.SupportURL;
		string customContext = "UserID: " + GetAQ3DUserID();
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		try
		{
			WebApiResponseLoginFacebook webApiResponseLoginFacebook = JsonConvert.DeserializeObject<WebApiResponseLoginFacebook>(www.downloadHandler.text);
			if (webApiResponseLoginFacebook.HasStandardErrorActions())
			{
				yield break;
			}
			if (webApiResponseLoginFacebook.HasError(WebApiResponseLoginFacebook.FacebookError_InvalidAccessToken))
			{
				BusyDialog.Close();
				Loader.close();
				FBManager.Logout();
				FBManager.Login();
				yield break;
			}
			if (webApiResponseLoginFacebook.HasError(WebApiResponseLoginFacebook.FacebookError_GraphDidNotReturnValidEmail))
			{
				BusyDialog.Close();
				Loader.close();
				FBManager.Logout();
				FBManager.FacebookEmailPermissionFailedOnLogin();
				yield break;
			}
			if (webApiResponseLoginFacebook.HasError(WebApiResponseLoginFacebook.FacebookError_AskUserIntentionForAQ3DAccount))
			{
				BusyDialog.Close();
				Loader.close();
				FBManager.AskIntentionForAQ3DAccount(webApiResponseLoginFacebook.CharacterName);
			}
			else if (webApiResponseLoginFacebook.HasError(WebApiResponseLoginFacebook.FacebookError_HasNotAcceptedTOS))
			{
				UIAcceptTerms.Show(acceptedTermsFB);
				Loader.close();
				yield break;
			}
			if (!PlayerPrefs.HasKey("LastLoginMethod"))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("af_registration_method", "facebook");
				AppsFlyer.sendEvent("af_complete_registration", dictionary);
			}
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_ResponseSuccessLoginFacebook);
			PlayerPrefs.SetInt("LastLoginMethod", 2);
			PlayerPrefs.Save();
			Session.Account = webApiResponseLoginFacebook.Account;
			Session.IsGuest = false;
			Session.IsReconnectable = true;
			LoginSuccess();
		}
		catch (Exception ex)
		{
			LoginFailed();
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, request.GetWWWForm(), customContext, ex);
		}
	}

	private void acceptedTermsFB()
	{
		instance.StartCoroutine(instance.LoginFacebookRoutine(1));
	}

	public static void LoginGuest()
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_AttemptLoginGuest);
		instance.StartCoroutine(instance.LoginGuestRoutine(0));
	}

	private IEnumerator LoginGuestRoutine(int acceptedTerms)
	{
		Loader.show("Logging in as Guest...", 0f);
		string linkSource = "";
		if (PlayerPrefs.HasKey("LINKSOURCE"))
		{
			linkSource = PlayerPrefs.GetString("LINKSOURCE");
		}
		WebApiRequestLoginGuest request = new WebApiRequestLoginGuest(GetGuestGuid(), linkSource, acceptedTerms);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/LoginGuest", request.GetWWWForm());
		string errorTitle = "Login Guest Failed!";
		string friendlyMsg = "Unable to login as a Guest. Please try again or contact support at " + Main.SupportURL;
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			LoginFailed();
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		try
		{
			WebApiResponseLoginGuest webApiResponseLoginGuest = JsonConvert.DeserializeObject<WebApiResponseLoginGuest>(www.downloadHandler.text);
			if (webApiResponseLoginGuest.HasStandardErrorActions())
			{
				yield break;
			}
			if (webApiResponseLoginGuest.HasError(WebApiResponseLoginGuest.GuestError_HasNotAcceptedTOS))
			{
				UIAcceptTerms.Show(acceptedTermsGuest);
				Loader.close();
				yield break;
			}
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_ResponseSuccessLoginGuest);
			PlayerPrefs.SetInt("LastLoginMethod", 3);
			PlayerPrefs.Save();
			Session.Account = webApiResponseLoginGuest.Account;
			Session.IsGuest = true;
			Session.IsReconnectable = true;
			LoginSuccess();
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
			LoginFailed();
		}
	}

	private void acceptedTermsGuest()
	{
		instance.StartCoroutine(instance.LoginGuestRoutine(1));
	}

	public static void LoginApple(string idToken)
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_AttemptLoginApple);
		instance.StartCoroutine(instance.LoginAppleRoutine(idToken, 0));
	}

	private IEnumerator LoginAppleRoutine(string idToken, int acceptedTerms)
	{
		Loader.show("Logging in with Apple...", 0f);
		string text = "";
		cachedAppleID = idToken;
		if (PlayerPrefs.HasKey("LINKSOURCE"))
		{
			text = PlayerPrefs.GetString("LINKSOURCE");
		}
		Debug.Log("Login idToken: " + idToken + " linkSource: " + text);
		WebApiRequestLoginApple request = new WebApiRequestLoginApple(idToken, text, acceptedTerms);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/LoginApple", request.GetWWWForm());
		string errorTitle = "Login Apple Failed!";
		string friendlyMsg = "Unable to login with Apple. Please try again or contact support at " + Main.SupportURL;
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			Debug.Log("login failed");
			LoginFailed();
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			Debug.Log("login failed");
			LoginFailed();
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			Debug.Log("login failed: " + www.error);
			LoginFailed();
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		try
		{
			WebApiResponseLoginApple webApiResponseLoginApple = JsonConvert.DeserializeObject<WebApiResponseLoginApple>(www.downloadHandler.text);
			if (webApiResponseLoginApple.HasStandardErrorActions())
			{
				yield break;
			}
			if (webApiResponseLoginApple.HasError(WebApiResponseLoginApple.AppleError_HasNotAcceptedTOS))
			{
				UIAcceptTerms.Show(acceptedTermsApple);
				Loader.close();
				yield break;
			}
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginManager_ResponseSuccessLoginApple);
			PlayerPrefs.SetInt("LastLoginMethod", 4);
			PlayerPrefs.Save();
			Session.Account = webApiResponseLoginApple.Account;
			Session.AppleUserHasAQ3DLogin = webApiResponseLoginApple.HasLoginAQ3D;
			Session.IsGuest = false;
			Session.IsReconnectable = true;
			LoginSuccess();
		}
		catch (Exception ex)
		{
			Debug.Log("login failed: " + ex.Message + " www.txt: " + www.downloadHandler.text);
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
			LoginFailed();
		}
	}

	private void acceptedTermsApple()
	{
		instance.StartCoroutine(instance.LoginAppleRoutine(cachedAppleID, 1));
	}

	public void Reset()
	{
		StopAllCoroutines();
	}

	public static string GetGuestGuid()
	{
		if (PlayerPrefs.HasKey("GUESTGUID"))
		{
			return PlayerPrefs.GetString("GUESTGUID");
		}
		string text = Guid.NewGuid().ToString();
		PlayerPrefs.SetString("GUESTGUID", text);
		PlayerPrefs.Save();
		firstGuestSession = true;
		return text;
	}

	public static bool HasGuestGuid()
	{
		return PlayerPrefs.HasKey("GUESTGUID");
	}

	public static bool HasAQ3DAccount()
	{
		return PlayerPrefs.HasKey("HasAQ3DAccount");
	}

	public static int GetAQ3DUserID()
	{
		if (HasAQ3DAccount())
		{
			return PlayerPrefs.GetInt("HasAQ3DAccount");
		}
		return 0;
	}

	public static bool HasRealAccount()
	{
		if ((!PlayerPrefs.HasKey("USERNAME") || !PlayerPrefs.HasKey("PASSWORDENCODE")) && !PlayerPrefs.HasKey("GUESTGUIDCONVERTED"))
		{
			return PlayerPrefs.HasKey("FBAT");
		}
		return true;
	}

	public static void LoginSuccess()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("af_customer_user_id", Session.Account.UserID.ToString());
		AppsFlyer.sendEvent("af_login", dictionary);
		AppsFlyer.setCustomerUserId(Session.Account.UserID.ToString());
		CrashReportHandler.SetUserMetadata("aq3dUserId", Session.Account.UserID.ToString());
		instance.StartCoroutine(instance.DownloadData());
	}

	public static void LoginFailed()
	{
		BusyDialog.Close();
		Loader.close();
	}

	private IEnumerator DownloadData()
	{
		if (Session.Account == null)
		{
			yield break;
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Login_Success);
		Loader.show("Loading Spells...", 0f);
		yield return WebAPI.Request("/Game/GetEffectTemplates", "Failed to load effect templates.", delegate(Dictionary<int, List<EffectVersion>> callback)
		{
			EffectTemplates.Init(callback);
		}, delegate(float progress)
		{
			Loader.show("Loading Spells...", 0.33f + progress / 3f);
		});
		yield return WebAPI.Request("/Game/GetEntitySpellTemplates", "Failed to load entity spell templates.", delegate(Dictionary<int, List<SpellVersion>> callback)
		{
			SpellTemplates.Init(callback);
		}, delegate(float progress)
		{
			Loader.show("Loading Spells...", 0.33f + progress / 3f);
		});
		yield return WebAPI.Request("/Game/GetMachineSpellTemplates", "Failed to load machine spell templates.", delegate(List<MachineSpellTemplate> callback)
		{
			MachineSpellTemplates.Init(callback);
		}, delegate(float progress)
		{
			Loader.show("Loading Spells...", 0.66f + progress / 3f);
		});
		yield return WebAPI.Request("/Game/GetStyleCollection", "Failed to load style collection.", delegate(StyleCollection callback)
		{
			StylesUtil.Init(callback);
		});
		yield return WebAPI.Request("/Game/GetSheathTypes", "Failed to retrieve sheath types.", delegate(List<SheathType> callback)
		{
			Sheathing.Instance.AddSheatheTypes(callback);
		});
		yield return WebAPI.Request("/Game/GetDailyRewards", "Failed to retrieve daily rewards.", delegate(Dictionary<int, Item> callback)
		{
			DailyRewards.Init(callback);
		});
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Login_TemplatesDownloaded);
		Loader.show("Loading Assets...", 0f);
		AssetBundleManager.UnloadAll();
		foreach (KeyValuePair<string, BundleInfo> bundle in Session.Account.bundles)
		{
			string name = bundle.Key;
			BundleInfo value = bundle.Value;
			AssetBundleLoader requestBundle = AssetBundleManager.LoadAssetBundle(value);
			while (!requestBundle.isDone)
			{
				Loader.show("Loading " + name + "...", requestBundle.GetProgress());
				yield return null;
			}
			if (!string.IsNullOrEmpty(requestBundle.Error))
			{
				MessageBoxWithDetails.Show("Load Failure", "Unable to load " + name + " asset bundle.", MessageBoxWithDetails.Type.Status, "URL: " + requestBundle.FullUrl + "\n\nError: " + requestBundle.Error);
				Loader.close();
				yield break;
			}
			requestBundle.Dispose();
		}
		Dictionary<string, MixerTrack> mixerTracks = new Dictionary<string, MixerTrack>();
		AssetBundleLoader requestGameBundle = AssetBundleManager.LoadAssetBundle(Session.Account.GameAssets_Bundle);
		yield return requestGameBundle;
		AssetBundleRequest abr = requestGameBundle.Asset.LoadAssetAsync<GameObject>("GameAudio");
		yield return abr;
		GameObject gameObject = abr.asset as GameObject;
		if (gameObject != null)
		{
			foreach (MixerTrack item in gameObject.GetComponent<SFXPlayer>().MixerTracks.Where((MixerTrack x) => x.Clip != null))
			{
				if (!mixerTracks.ContainsKey(item.Name))
				{
					mixerTracks[item.Name] = item;
				}
			}
		}
		GameObject gameObject2 = GameObject.Find("GameAudio");
		if (gameObject2 != null)
		{
			foreach (MixerTrack mixerTrack in gameObject2.GetComponent<SFXPlayer>().MixerTracks)
			{
				if (!mixerTracks.ContainsKey(mixerTrack.Name))
				{
					mixerTracks[mixerTrack.Name] = mixerTrack;
				}
			}
		}
		UnityEngine.Object.Destroy(gameObject2);
		AudioManager.AddTracks(mixerTracks.Values.ToList());
		Loader.close();
		if (TextureCompositor.Instance is TextureCompositorOrtho)
		{
			Shader.globalMaximumLOD = 200;
		}
		else
		{
			Shader.globalMaximumLOD = int.MaxValue;
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Login_BundlesDownloaded);
		if (Session.Account.chars.Count > 0)
		{
			StateManager.Instance.LoadState("scene.charselect");
		}
		else
		{
			StateManager.Instance.LoadState("scene.charcreate");
		}
	}

	public static void RefreshMongoData()
	{
		instance.StartCoroutine(instance.RefreshMongoRoutine());
	}

	private IEnumerator RefreshMongoRoutine()
	{
		yield return WebAPI.Request("/Game/GetEffectTemplates", "Failed to load effect templates.", delegate(Dictionary<int, List<EffectVersion>> callback)
		{
			EffectTemplates.Init(callback);
		});
		yield return WebAPI.Request("/Game/GetEntitySpellTemplates", "Failed to load entity spell templates.", delegate(Dictionary<int, List<SpellVersion>> callback)
		{
			SpellTemplates.Init(callback);
		});
		yield return WebAPI.Request("/Game/GetMachineSpellTemplates", "Failed to load machine spell templates.", delegate(List<MachineSpellTemplate> callback)
		{
			MachineSpellTemplates.Init(callback);
		});
		yield return WebAPI.Request("/Game/GetStyleCollection", "Failed to load style collection.", delegate(StyleCollection callback)
		{
			StylesUtil.Init(callback);
		});
		Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + "Mongo client data refreshed[-]");
	}
}

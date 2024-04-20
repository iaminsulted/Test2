using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class FBStandalone : FBManager
{
	private string FB_REDIRECT = Main.FBRedirectURL_ForceSecure + "/fbconnect/Aq3dFB2.html";

	private string FBConnectState;

	private IEnumerator ActivePollForAccessTokenRoutine;

	private bool FBConnectCreatedOk;

	private bool isPolling;

	private bool newTokenReceived;

	protected override string FacebookAccessTokenPlatform
	{
		get
		{
			if (PlayerPrefs.HasKey("FBAT"))
			{
				return PlayerPrefs.GetString("FBAT");
			}
			return "";
		}
	}

	protected override void LoginPlatform()
	{
		if (FBManager.HasFacebookAccessToken())
		{
			LoginManager.LoginFacebook();
		}
		else
		{
			FBManager.RequestReadPermission();
		}
	}

	protected override void LogoutPlatform()
	{
		DeleteFacebookAccessToken();
	}

	private IEnumerator CreateFBConnectRecordRoutine()
	{
		FBConnectState = SystemInfo.deviceUniqueIdentifier + "_" + Util.TokenGenerator();
		WWWForm form = new WWWForm();
		form.AddField("State", FBConnectState);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/RequestFBconnect", form);
		string errorTitle = "Login Facebook Failed!";
		string friendlyMsg = "Failed to connect with Facebook.";
		string customContext = "FBConnectState: " + FBConnectState;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		Loader.close();
		if (www.isHttpError)
		{
			friendlyMsg = "Failed to connect with Facebook. Please try again later.";
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			friendlyMsg = "Failed to connect with Facebook. Please check your connection or try again later.";
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
	}

	private string GetFacebookOauthUrl(bool rerequest, string permissions)
	{
		string text = "https://www.facebook.com/dialog/oauth?client_id=516395781857744&state=" + FBConnectState + "&redirect_uri=" + FB_REDIRECT + "&scope=" + permissions;
		if (rerequest)
		{
			text += "&auth_type=rerequest";
		}
		return text;
	}

	private void PollForAccessToken()
	{
		newTokenReceived = false;
		isPolling = true;
		ActivePollForAccessTokenRoutine = PollForAccessTokenRoutine();
		StartCoroutine(ActivePollForAccessTokenRoutine);
	}

	private IEnumerator PollForAccessTokenRoutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("state", FBConnectState);
		using (UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL_ForceSecure + "/Game/PollFBConnect", form))
		{
			string errorTitle = "Login Facebook Failed";
			string friendlyMsg = "Failed to obtain AccessToken.";
			string customContext = "FBConnectState: " + FBConnectState;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
				yield break;
			}
			if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
				yield break;
			}
			if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
				yield break;
			}
			try
			{
				APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
				if (aPIResponse.Error != null)
				{
					isPolling = false;
					ActivePollForAccessTokenRoutine = null;
					yield break;
				}
				if (!string.IsNullOrEmpty(aPIResponse.Message))
				{
					if (aPIResponse.Message != "ERROR")
					{
						newTokenReceived = true;
						SaveFacebookAccessToken(aPIResponse.Message);
						BusyDialog.Close();
						FBManager.rerequest = false;
					}
					isPolling = false;
					ActivePollForAccessTokenRoutine = null;
					yield break;
				}
			}
			catch (Exception)
			{
				isPolling = false;
				ActivePollForAccessTokenRoutine = null;
				yield break;
			}
		}
		yield return new WaitForSeconds(2.2f);
		PollForAccessToken();
	}

	private void StopPollingForAccessToken()
	{
		if (ActivePollForAccessTokenRoutine != null)
		{
			StopCoroutine(ActivePollForAccessTokenRoutine);
			ActivePollForAccessTokenRoutine = null;
		}
	}

	private void SaveFacebookAccessToken(string accessToken)
	{
		PlayerPrefs.SetString("FBAT", accessToken);
		PlayerPrefs.Save();
	}

	private void DeleteFacebookAccessToken()
	{
		if (PlayerPrefs.HasKey("FBAT"))
		{
			PlayerPrefs.DeleteKey("FBAT");
			PlayerPrefs.Save();
		}
	}

	protected override void RequestReadPermissionPlatform()
	{
		StartCoroutine(RequestReadPermissionRoutine());
	}

	private IEnumerator RequestReadPermissionRoutine()
	{
		yield return StartCoroutine(CreateFBConnectRecordRoutine());
		BusyDialog.Show("Authenticating Facebook", closable: true);
		BusyDialog.Instance.OnClose += StopPollingForAccessToken;
		Application.OpenURL(GetFacebookOauthUrl(FBManager.rerequest, "email"));
		PollForAccessToken();
		while (isPolling)
		{
			yield return null;
		}
		if (FBManager.HasFacebookAccessToken())
		{
			FBManager.OnReadPermission(arg1: true, "");
		}
		else
		{
			FBManager.OnReadPermission(arg1: false, "Unable to obtain Facebook access token");
		}
	}

	protected override void RequestPublishPermissionPlatform()
	{
		StartCoroutine(RequestPublishActionPermissionRoutine());
	}

	private IEnumerator RequestPublishActionPermissionRoutine()
	{
		yield return StartCoroutine(CreateFBConnectRecordRoutine());
		BusyDialog.Show("Requesting Permission", closable: true);
		BusyDialog.Instance.OnClose += StopPollingForAccessToken;
		Application.OpenURL(GetFacebookOauthUrl(FBManager.rerequest, "publish_actions"));
		newTokenReceived = false;
		PollForAccessToken();
		while (isPolling)
		{
			yield return null;
		}
		if (newTokenReceived)
		{
			FBManager.OnPublishPermission(arg1: true, "");
		}
		else
		{
			FBManager.OnPublishPermission(arg1: false, "Unable to obtain Facebook publish permission");
		}
	}

	protected override void PostScreenshotPlatform(string message, byte[] screenshotData)
	{
		StartCoroutine(PostScreenshotRoutine(message, screenshotData));
	}

	private IEnumerator PostScreenshotRoutine(string message, byte[] screenshotData)
	{
		if (!FBManager.HasFacebookAccessToken())
		{
			FBManager.OnScreenshotResponse(obj: false);
			MessageBox.Show("Facebook Required", "You must first link your account to Facebook in order to use the selfie cam!");
			yield break;
		}
		BusyDialog.Show("Validating Permission");
		using (UnityWebRequest unityWebRequest = UnityWebRequest.Get("https://graph.facebook.com/me/permissions?access_token=" + FBManager.FacebookAccessToken))
		{
			yield return unityWebRequest;
			if (unityWebRequest.error != null)
			{
				FBManager.OnScreenshotResponse(obj: false);
				MessageBox.Show("Connection Problem", "Failed to communicate with Facebook, please try again!");
				yield break;
			}
			if (!unityWebRequest.downloadHandler.text.Contains("publish_actions"))
			{
				FBManager.OnScreenshotResponse(obj: false);
				FBManager.ShowPublishPermissionConfirmation();
				yield break;
			}
		}
		BusyDialog.Show("Posting to Facebook");
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddBinaryData("image", screenshotData, "Screenshot.png");
		wwwForm.AddField("name", message);
		using UnityWebRequest unityWebRequest = UnityWebRequest.Post("https://graph.facebook.com/me/photos?access_token=" + FBManager.FacebookAccessToken, wwwForm);
		string errorTitle = "Failed to Post Screenshot";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "URL: " + unityWebRequest.url;
		yield return unityWebRequest.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(unityWebRequest, customContext);
		if (unityWebRequest.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, unityWebRequest.error, unityWebRequest.responseCode, wwwForm, customContext);
			FBManager.OnScreenshotResponse(obj: false);
			yield break;
		}
		if (unityWebRequest.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, unityWebRequest.error, wwwForm, customContext);
			FBManager.OnScreenshotResponse(obj: false);
			yield break;
		}
		if (unityWebRequest.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, unityWebRequest.error, wwwForm, customContext);
			yield break;
		}
		if (unityWebRequest.downloadHandler.text.Contains("publish_actions"))
		{
			FBManager.OnScreenshotResponse(obj: false);
			FBManager.ShowPublishPermissionConfirmation();
			yield break;
		}
		FBManager.OnScreenshotResponse(obj: true);
	}
}

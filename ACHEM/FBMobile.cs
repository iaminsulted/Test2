using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using UnityEngine;

public class FBMobile : FBManager
{
	protected override string FacebookAccessTokenPlatform
	{
		get
		{
			if (!FB.IsInitialized || !FB.IsLoggedIn)
			{
				return "";
			}
			return AccessToken.CurrentAccessToken.TokenString;
		}
	}

	protected override void LoginPlatform()
	{
		FBManager.RequestReadPermission();
	}

	protected override void LogoutPlatform()
	{
		if (FB.IsLoggedIn)
		{
			if (PlayerPrefs.HasKey("FBAT"))
			{
				PlayerPrefs.DeleteKey("FBAT");
				PlayerPrefs.Save();
			}
			FB.LogOut();
		}
	}

	protected override void RequestReadPermissionPlatform()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(OnInitComplete);
		}
		else
		{
			OnFBInitialized();
		}
	}

	private void OnInitComplete()
	{
		if (!FB.IsInitialized)
		{
			Debug.LogError("Failed to Initialize the Facebook SDK");
		}
		else
		{
			OnFBInitialized();
		}
	}

	private void OnFBInitialized()
	{
		if (FB.IsLoggedIn)
		{
			OnFBLoggedIn();
			return;
		}
		FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, RequestReadPermissionsCallback);
	}

	private void RequestReadPermissionsCallback(ILoginResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError("FBSDK: Read Permission Error - " + result.Error);
			LogoutPlatform();
			FBManager.OnReadPermission(arg1: false, result.Error);
		}
		else if (!FB.IsLoggedIn || result.Cancelled)
		{
			Debug.LogError("FBSDK: Read Permission Error - Request cancelled by Player");
			LogoutPlatform();
			FBManager.OnReadPermission(arg1: false, "Request cancelled by Player");
		}
		else
		{
			OnFBLoggedIn();
		}
	}

	private void OnFBLoggedIn()
	{
		PlayerPrefs.SetString("FBAT", FBManager.FacebookAccessToken);
		PlayerPrefs.Save();
		FBManager.OnReadPermission(arg1: true, "");
	}

	protected override void RequestPublishPermissionPlatform()
	{
		FB.LogInWithPublishPermissions(new List<string> { "publish_actions" }, RequestPublishPermissionCallback);
	}

	private void RequestPublishPermissionCallback(ILoginResult result)
	{
		if (result.AccessToken.Permissions.Contains("publish_actions"))
		{
			PlayerPrefs.SetString("FBAT", FBManager.FacebookAccessToken);
			PlayerPrefs.Save();
			FBManager.OnPublishPermission(arg1: true, "");
		}
		else
		{
			FBManager.OnPublishPermission(arg1: false, "Unable to obtain Facebook publish permission");
		}
	}

	protected override void PostScreenshotPlatform(string message, byte[] screenshotData)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddBinaryData("image", screenshotData, "Screenshot.png");
		wWWForm.AddField("name", message);
		FB.API("me/photos", HttpMethod.POST, PostScreenshotCallback, wWWForm);
	}

	private void PostScreenshotCallback(IGraphResult result)
	{
		if (result.Error == null)
		{
			FBManager.OnScreenshotResponse(obj: true);
			return;
		}
		FBManager.OnScreenshotResponse(obj: false);
		if (result.RawResult.Contains("publish_actions"))
		{
			FBManager.ShowPublishPermissionConfirmation();
		}
		else if (result.RawResult.Contains("Error validating access token") || result.RawResult.Contains("active access token"))
		{
			FBManager.RequestReadPermission();
		}
		else
		{
			MessageBox.Show("Screenshot Problem", result.Error);
		}
	}
}

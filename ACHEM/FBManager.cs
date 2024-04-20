using System;
using UnityEngine;

public abstract class FBManager : MonoBehaviour
{
	public enum AQ3DIntentions
	{
		IgnoreCreateNew = 1,
		LinkToExisting
	}

	protected const string FacebookAppID = "516395781857744";

	public static bool rerequest;

	public static bool alreadyAskedAboutEmailPermission = false;

	private static FBManager instance;

	public static Action<bool> OnScreenshotResponse;

	public static Action<bool, string> OnReadPermission;

	public static Action<bool, string> OnPublishPermission;

	public static int AQ3DIntention = 0;

	public static string VerifiedAQ3DEmail = "";

	public static string VerifiedAQ3DPassword = "";

	public static string FacebookAccessToken => instance.FacebookAccessTokenPlatform;

	protected abstract string FacebookAccessTokenPlatform { get; }

	public static void Initialize()
	{
		instance = new GameObject("FBManager").AddComponent<FBStandalone>();
	}

	public static void Login()
	{
		instance.LoginPlatform();
	}

	public static void Logout()
	{
		instance.LogoutPlatform();
	}

	public static void PostScreenshot(string message, byte[] screenshotData)
	{
		instance.PostScreenshotPlatform(message, screenshotData);
	}

	public static bool HasFacebookAccessToken()
	{
		if (FacebookAccessToken != null && FacebookAccessToken != "")
		{
			return FacebookAccessToken != "ERROR";
		}
		return false;
	}

	public static void RequestReadPermission()
	{
		instance.RequestReadPermissionPlatform();
	}

	public static void RequestPublishActionPermission()
	{
		instance.RequestPublishPermissionPlatform();
	}

	public static void CreateLoginFacebookForGuest()
	{
		RequestReadPermission();
	}

	protected abstract void RequestReadPermissionPlatform();

	protected abstract void RequestPublishPermissionPlatform();

	protected abstract void LoginPlatform();

	protected abstract void LogoutPlatform();

	protected abstract void PostScreenshotPlatform(string message, byte[] screenshotData);

	public static void ShowPublishPermissionConfirmation()
	{
		Confirmation.Show("Permission Required", "To use the selfie camera and share images you must accept the Facebook publish permission.", "Open Facebook", "Cancel", delegate(bool ok)
		{
			if (ok)
			{
				RequestPublishActionPermission();
			}
		}, Closable: false, enableCollider: true, large: true);
	}

	public static void FacebookEmailPermissionFailedOnGuestLink()
	{
		Confirmation.Show("Email Required", "You must accept the Facebook email permission so we can create a unique account for you.  Would you like to try again?", delegate(bool b)
		{
			if (b)
			{
				alreadyAskedAboutEmailPermission = true;
				rerequest = true;
				CreateLoginFacebookForGuest();
			}
		});
	}

	public static void FacebookEmailPermissionFailedOnLogin()
	{
		if (alreadyAskedAboutEmailPermission)
		{
			MessageBox.Show("Facebook Error", "We did not receive your email address from Facebook, let's create an AQ3D login instead!", "Ok", delegate
			{
				UIAccountCreate.Instance.ShowAQ3DCreate();
			});
			return;
		}
		Confirmation.Show("Email Required", "You must accept the Facebook email permission so we can create a unique account for you.  Would you like to try again?", delegate(bool b)
		{
			if (b)
			{
				alreadyAskedAboutEmailPermission = true;
				rerequest = true;
				Login();
			}
		});
	}

	public static void AskIntentionForAQ3DAccount(string charName)
	{
		Confirmation.Show("You already have an AQ3D login!", "Do you want to link Facebook to your existing character '" + charName + "', or create a new character?", "Link to '" + charName + "'", "Create New Character", delegate(bool conf)
		{
			ConfirmActionForExistingUser(conf);
		}, Closable: false, enableCollider: true, large: true);
	}

	private static void ConfirmActionForExistingUser(bool linkToExisting)
	{
		if (linkToExisting)
		{
			UIVerifyAQ3D.Load();
			return;
		}
		AQ3DIntention = 1;
		LoginManager.LoginFacebook();
	}
}

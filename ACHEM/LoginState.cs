using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;
using UnityEngine.SignInWithApple;

public class LoginState : State
{
	public UIButtonColor btnLoginAQ3D;

	public UIButtonColor btnLoginFacebook;

	public UIButtonColor btnGuest;

	public UIButtonColor btnLoginApple;

	public UILabel lblVersion;

	public UILabel lblEnvironment;

	public UILabel lblBtnGuest;

	public GameObject OrLine;

	public AudioClip loginTrack;

	private void Start()
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_StartedOk);
		if (Session.pendingGoto != null)
		{
			StateManager.Instance.LoadState("scene.charselect");
		}
		if (Session.pendingServer != null)
		{
			StateManager.Instance.LoadState("scene.charselect");
		}
		btnLoginApple.gameObject.SetActive(value: false);
		lblVersion.text = "Version ";
		if (Main.IsPTR)
		{
			UILabel uILabel = lblVersion;
			uILabel.text = uILabel.text + "PTRb" + Main.DevBuild;
			lblEnvironment.text = "";
		}
		else if (Main.Environment == Environment.PTR || Main.Environment == Environment.Code2 || Main.Environment == Environment.Code || Main.Environment == Environment.Content || Main.Environment == Environment.Stage)
		{
			UILabel uILabel2 = lblVersion;
			uILabel2.text = uILabel2.text + Main.ClientDisplayVersion + "b" + Main.DevBuild;
			lblEnvironment.text = Main.Environment.ToString() + " Environment";
		}
		else
		{
			lblEnvironment.text = Main.ClientDisplayVersion;
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_VersionAndEnvironmentTextDisplayedOk);
		UIEventListener uIEventListener = UIEventListener.Get(btnLoginAQ3D.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick_LoginAQ3D));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLoginFacebook.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClick_LoginFB));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnGuest.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClick_LoginGuest));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnLoginApple.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClick_LoginApple));
		FBManager.OnReadPermission = (Action<bool, string>)Delegate.Combine(FBManager.OnReadPermission, new Action<bool, string>(HandleReadPermission));
		bool active = !PlayerPrefs.HasKey("USERNAME") && !PlayerPrefs.HasKey("PASSWORDENCODE") && !PlayerPrefs.HasKey("GUESTGUIDCONVERTED") && !PlayerPrefs.HasKey("FBAT") && !PlayerPrefs.HasKey("JWT");
		btnGuest.gameObject.SetActive(active);
		OrLine.SetActive(active);
		if (PlayerPrefs.HasKey("GUESTGUID") && PlayerPrefs.HasKey("GuestGuidCharName"))
		{
			lblBtnGuest.text = "Continue as Guest";
		}
		else
		{
			lblBtnGuest.text = "Start as New Guest";
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_DisplayGuestButtonOk);
		if (Main.IsPTR)
		{
			MessageBox.Show("Public Test Realm (PTR)", "This is a special testing realm which uses a copy of your character. If your account is new, you may not be able to log in. The server may reset often. Please go to options and clear your cache when switching between this and the Live Game.");
		}
		else if (LoginManager.firstTimeLoadingLoginState && PlayerPrefs.GetInt("DisableAutoLogin") == 0)
		{
			LoginManager.firstTimeLoadingLoginState = false;
			if (PlayerPrefs.HasKey("LastLoginMethod"))
			{
				switch (PlayerPrefs.GetInt("LastLoginMethod"))
				{
				case 1:
					if (PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORDENCODE"))
					{
						DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_AttemptAutoLoginAQ3D);
						LoginManager.LoginAQ3D(PlayerPrefs.GetString("USERNAME"), Util.Base64Decode(PlayerPrefs.GetString("PASSWORDENCODE")));
					}
					break;
				case 2:
					if (!PlayerPrefs.HasKey("FBAT"))
					{
					}
					break;
				case 3:
					if (PlayerPrefs.HasKey("GUESTGUID") && PlayerPrefs.HasKey("GuestGuidCharName") && !PlayerPrefs.HasKey("GUESTGUIDCONVERTED"))
					{
						DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_AttemptAutoLoginGuest);
						LoginManager.LoginGuest();
					}
					break;
				}
			}
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_Complete);
		if (Main.EditorAutoLogin.LogIn)
		{
			StartCoroutine(AutoLogin());
		}
		else
		{
			AudioManager.PlayBGMClip(loginTrack);
		}
	}

	private IEnumerator AutoLogin()
	{
		yield return null;
		OnClick_LoginAQ3D(null);
	}

	private void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnLoginAQ3D.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick_LoginAQ3D));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLoginFacebook.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClick_LoginFB));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnGuest.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClick_LoginGuest));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnLoginApple.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClick_LoginApple));
		FBManager.OnReadPermission = (Action<bool, string>)Delegate.Remove(FBManager.OnReadPermission, new Action<bool, string>(HandleReadPermission));
	}

	private void OnClick_LoginGuest(GameObject go)
	{
		if (!Main.CheckVersion(Main.pubCurrentVersion, Main.pubMinReq))
		{
			Main.ShowVersionSpecificUpdateRequired(Main.pubMinReq);
			return;
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_OnLoginGuestClick);
		LoginManager.LoginGuest();
	}

	private void OnClick_LoginFB(GameObject go)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			AssetBundleManager.UnloadAll();
			if (Caching.ClearCache())
			{
				Debug.LogWarning("Cache cleared!");
			}
			else
			{
				Debug.LogWarning("Could not clear cache!");
			}
		}
		if (!Main.CheckVersion(Main.pubCurrentVersion, Main.pubMinReq))
		{
			Main.ShowVersionSpecificUpdateRequired(Main.pubMinReq);
			return;
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_OnLoginFBClick);
		FBManager.Login();
	}

	private void HandleReadPermission(bool success, string error)
	{
		if (success)
		{
			LoginManager.LoginFacebook();
			return;
		}
		LoginManager.LoginFailed();
		MessageBox.Show("Facebook Login Failed", error);
	}

	private void OnClick_LoginAQ3D(GameObject go)
	{
		if (Input.GetKey(KeyCode.LeftShift) || Main.EditorAutoLogin.ClearCache)
		{
			AssetBundleManager.UnloadAll();
			if (Caching.ClearCache())
			{
				Debug.LogWarning("Cache cleared!");
			}
			else
			{
				Debug.LogWarning("Could not clear cache!");
			}
		}
		if (!Main.CheckVersion(Main.pubCurrentVersion, Main.pubMinReq))
		{
			Main.ShowVersionSpecificUpdateRequired(Main.pubMinReq);
			return;
		}
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.LoginState_OnLoginUsernamePasswordClick);
		UIAccountCreate.Instance.ShowAQ3DLoginOrCreate();
	}

	private void OnClick_LoginApple(GameObject go)
	{
		base.gameObject.GetComponent<SignInWithApple>().Login(OnLogin);
	}

	private void OnLogin(SignInWithApple.CallbackArgs args)
	{
		if (!string.IsNullOrWhiteSpace(args.error))
		{
			LoginManager.LoginFailed();
			if (!args.error.Contains("1001") && !args.error.Contains("1000"))
			{
				MessageBox.Show("Apple Login Failed", args.error);
			}
			return;
		}
		if (string.IsNullOrEmpty(args.userInfo.idToken))
		{
			LoginManager.LoginFailed();
			MessageBox.Show("Apple Login Failed", "Invalid Token");
			return;
		}
		if (args.userInfo.idToken.Split('.').Length != 3)
		{
			LoginManager.LoginFailed();
			MessageBox.Show("Apple Login Failed", "Invalid Token");
			return;
		}
		if (!PlayerPrefs.HasKey("JWT"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("af_registration_method", "apple");
			AppsFlyer.sendEvent("af_complete_registration", dictionary);
		}
		PlayerPrefs.SetString("JWT", args.userInfo.idToken);
		PlayerPrefs.Save();
		if (!Main.CheckVersion(Main.pubCurrentVersion, Main.pubMinReq))
		{
			Main.ShowVersionSpecificUpdateRequired(Main.pubMinReq);
		}
		else
		{
			LoginManager.LoginApple(args.userInfo.idToken);
		}
	}

	public void ShowOptions()
	{
		UIOptions.Show();
	}

	private void HideButtons()
	{
		btnLoginAQ3D.gameObject.SetActive(value: false);
		btnLoginFacebook.gameObject.SetActive(value: false);
		btnGuest.gameObject.SetActive(value: false);
	}

	private void ShowButtons()
	{
		btnLoginAQ3D.gameObject.SetActive(value: true);
		btnLoginFacebook.gameObject.SetActive(value: true);
		btnGuest.gameObject.SetActive(value: true);
	}

	public override void Close()
	{
		base.Close();
		LoginManager.Instance.Reset();
	}
}

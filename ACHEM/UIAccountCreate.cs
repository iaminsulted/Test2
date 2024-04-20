using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppsFlyerSDK;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class UIAccountCreate : UIWindow
{
	public const string MatchEmailPattern = "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$";

	[Header("Shared UI")]
	public GameObject SharedPanel;

	public UILabel TitleLabel;

	public UILabel SubtitleLabel;

	public UIButtonColor btnBack;

	public UIButtonColor btnClose;

	[Header("Login AQ3D")]
	public UIInput LoginUsername;

	public UIInput LoginPassword;

	public UIToggle RememberLogin;

	public UIButtonColor btnLoginAQ3D;

	public UIButtonColor btnOpenCreateAccount;

	public UIButtonColor btnForgotPassword;

	[Header("Create AQ3D")]
	public UIInput CreateAQ3DUsername;

	public UIInput CreateAQ3DPassword;

	public UIToggle CreateAQ3DToggleTerms;

	public UIToggle CreateAQ3DNewsLetterOptIn;

	public UIButtonColor btnCreateAQ3D;

	public UIButton btnShowTerms;

	[Header("Add AQ3D")]
	public UIInput AddAQ3DUsername;

	public UIInput AddAQ3DPassword;

	public UIToggle AddAQ3DToggleTerms;

	public UIToggle AddAQ3DNewsLetterOptIn;

	public UIButtonColor btnAddAQ3D;

	public UIButton btnAddAQ3DShowTerms;

	[Header("Convert Guest")]
	public UIInput ConvertGuestUsername;

	public UIInput ConvertGuestPassword;

	public UIToggle ConvertGuestToggleTerms;

	public UIToggle ConvertGuestAQ3DNewsLetterOptIn;

	public UIButtonColor btnConvertGuestShowTerms;

	public UIButtonColor btnConvertGuestCreateAQ3D;

	public GameObject ConvertOptions;

	public GameObject ConvertToAQ3D;

	public UIButtonColor btnConvertToAQ3D;

	public UIButtonColor btnConvertToFacebook;

	[Header("Recover Password")]
	public UILabel ExistingEmailLabel;

	public UIButtonColor btnResetPassword;

	public UIButtonColor btnReturnToLogin;

	[Header("States")]
	public GameObject StateLogin;

	public GameObject StateCreateNewAQ3D;

	public GameObject StateResetPassword;

	public GameObject StateConvertGuest;

	public GameObject StateAddLoginAQ3D;

	public static Action ConvertGuestSuccess;

	public static Action AddLoginAQ3DSuccess;

	private string _title;

	private string _subtitle;

	private static UIAccountCreate instance;

	public static UIAccountCreate Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/AccountCreate"), UIManager.Instance.transform).GetComponent<UIAccountCreate>();
				instance.Init();
			}
			return instance;
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnForgotPassword.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnForgetPasswordClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLoginAQ3D.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnLoginAQ3DClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnOpenCreateAccount.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCreateNewAccountClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnCreateAQ3D.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnCreateAQ3DClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnShowTerms.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnTermsClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnAddAQ3D.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnAddLoginAQ3DClick));
		UIEventListener uIEventListener7 = UIEventListener.Get(btnConvertGuestShowTerms.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnTermsClick));
		UIEventListener uIEventListener8 = UIEventListener.Get(btnConvertGuestCreateAQ3D.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnConnectGuestToLoginAQ3DClick));
		UIEventListener uIEventListener9 = UIEventListener.Get(btnConvertToAQ3D.gameObject);
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnConvertToAQ3DClick));
		UIEventListener uIEventListener10 = UIEventListener.Get(btnConvertToFacebook.gameObject);
		uIEventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener10.onClick, new UIEventListener.VoidDelegate(OnConvertToFacebookClick));
		UIEventListener uIEventListener11 = UIEventListener.Get(btnResetPassword.gameObject);
		uIEventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener11.onClick, new UIEventListener.VoidDelegate(OnResetPasswordClick));
		UIEventListener uIEventListener12 = UIEventListener.Get(btnReturnToLogin.gameObject);
		uIEventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener12.onClick, new UIEventListener.VoidDelegate(OnReturnToLoginClick));
		UIEventListener uIEventListener13 = UIEventListener.Get(btnBack.gameObject);
		uIEventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener13.onClick, new UIEventListener.VoidDelegate(OnBackClick));
		UIEventListener uIEventListener14 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener14.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener14.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		FBManager.OnReadPermission = (Action<bool, string>)Delegate.Combine(FBManager.OnReadPermission, new Action<bool, string>(HandleReadPermission));
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_InitOk);
	}

	private void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnForgotPassword.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnForgetPasswordClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnLoginAQ3D.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnLoginAQ3DClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnOpenCreateAccount.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCreateNewAccountClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnCreateAQ3D.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnCreateAQ3DClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnConvertGuestShowTerms.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnTermsClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnResetPassword.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnResetPasswordClick));
		UIEventListener uIEventListener7 = UIEventListener.Get(btnReturnToLogin.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener8 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener9 = UIEventListener.Get(btnBack.gameObject);
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnBackClick));
		FBManager.OnReadPermission = (Action<bool, string>)Delegate.Remove(FBManager.OnReadPermission, new Action<bool, string>(HandleReadPermission));
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnTermsClick(GameObject go)
	{
		Confirmation.OpenUrl(Main.TermsURL);
	}

	private void HideAllStates()
	{
		StateLogin.SetActive(value: false);
		StateCreateNewAQ3D.SetActive(value: false);
		StateConvertGuest.SetActive(value: false);
		StateResetPassword.SetActive(value: false);
		StateAddLoginAQ3D.SetActive(value: false);
	}

	private void OnReturnToLoginClick(GameObject go)
	{
		ShowAQ3DLoginOrCreate();
	}

	private bool VerifyEmailPasswordTerms(string email, string password, bool termsChecked)
	{
		if (!IsValidEmail(email))
		{
			MessageBox.Show("Invalid Email!", "Please enter a valid email address.");
			return false;
		}
		if (email == "Username" || email == "" || password == "")
		{
			MessageBox.Show("Incomplete!", "Please enter Email and Password");
			return false;
		}
		if (!termsChecked)
		{
			MessageBox.Show("Agree", "You must agree to the Terms and Conditions");
			return false;
		}
		return true;
	}

	public static bool IsValidEmail(string email)
	{
		if (email != null)
		{
			return Regex.IsMatch(email, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$");
		}
		return false;
	}

	private void OnBackClick(GameObject go)
	{
		if (StateResetPassword.activeSelf)
		{
			ShowAQ3DCreate();
		}
		else if (StateCreateNewAQ3D.activeSelf)
		{
			ShowAQ3DLoginOrCreate();
		}
		else if (StateConvertGuest.activeSelf)
		{
			if (ConvertToAQ3D.activeSelf)
			{
				DrawConvertGuest();
			}
			else
			{
				Close();
			}
		}
	}

	public void ShowAQ3DLoginOrCreate()
	{
		TitleLabel.text = "Login";
		SubtitleLabel.text = "";
		bool flag = false;
		if (PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORDENCODE"))
		{
			LoginUsername.value = PlayerPrefs.GetString("USERNAME");
			LoginPassword.value = Util.Base64Decode(PlayerPrefs.GetString("PASSWORDENCODE"));
			RememberLogin.value = true;
			flag = Main.EditorAutoLogin.LogIn;
		}
		HideAllStates();
		SharedPanel.SetActive(value: true);
		StateLogin.SetActive(value: true);
		btnBack.gameObject.SetActive(value: false);
		if (flag)
		{
			OnLoginAQ3DClick(null);
		}
	}

	private void OnForgetPasswordClick(GameObject go)
	{
		Confirmation.OpenUrl(Main.WebAccountURL + "/ResetPassword");
	}

	private void OnCreateNewAccountClick(GameObject go)
	{
		ShowAQ3DCreate();
	}

	private void OnLoginAQ3DClick(GameObject go)
	{
		string value = LoginUsername.value;
		string value2 = LoginPassword.value;
		Loader.show("Log In", 0f);
		btnLoginAQ3D.enabled = false;
		if (value == "Username" || value == "" || value2 == "")
		{
			btnLoginAQ3D.enabled = true;
			Loader.close();
			return;
		}
		if (RememberLogin.value)
		{
			PlayerPrefs.SetString("USERNAME", value);
			PlayerPrefs.SetString("PASSWORDENCODE", Util.Base64Encode(value2));
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.DeleteKey("USERNAME");
			PlayerPrefs.DeleteKey("PASSWORD");
			PlayerPrefs.DeleteKey("PASSWORDENCODE");
		}
		LoginManager.LoginAQ3D(value, value2);
	}

	public void ShowRecover()
	{
		TitleLabel.text = "Account Exists";
		SubtitleLabel.text = "";
		HideAllStates();
		SharedPanel.SetActive(value: true);
		StateResetPassword.SetActive(value: true);
		btnBack.gameObject.SetActive(value: true);
	}

	private void OnResetPasswordClick(GameObject go)
	{
		Confirmation.OpenUrl(Main.WebAccountURL + "/ResetPassword");
	}

	public void ShowAQ3DCreate()
	{
		TitleLabel.text = "New Account";
		SubtitleLabel.text = "";
		HideAllStates();
		SharedPanel.SetActive(value: true);
		StateCreateNewAQ3D.SetActive(value: true);
		btnBack.gameObject.SetActive(value: true);
	}

	private void OnCreateAQ3DClick(GameObject go)
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_CreateNewLoginAQ3DClick);
		CreateAQ3DLogin();
	}

	private void CreateAQ3DLogin(int guestAction = 0)
	{
		if (!VerifyEmailPasswordTerms(CreateAQ3DUsername.value, CreateAQ3DPassword.value, CreateAQ3DToggleTerms.value))
		{
			return;
		}
		if (LoginManager.HasGuestGuid() && !LoginManager.HasRealAccount() && guestAction == 0)
		{
			if (PlayerPrefs.HasKey("GuestGuidCharName"))
			{
				string @string = PlayerPrefs.GetString("GuestGuidCharName");
				Confirmation.Show("You have a guest character!", "Do you want to link your existing guest character '" + @string + "' to this new login, or create a new character?\n\n[ff0000]WARNING: If you create a new character your guest character '" + @string + "' will be permanently discarded![-]", "Link to '" + @string + "'", "Create New Character", delegate(bool conf)
				{
					ConfirmActionForExistingGuest(conf);
				}, Closable: false, enableCollider: true, large: true);
			}
			else
			{
				CreateAQ3DLogin(2);
			}
		}
		else
		{
			StartCoroutine(CreateUserLoginAQ3DRoutine(CreateAQ3DUsername.value, CreateAQ3DPassword.value, guestAction));
		}
	}

	private void ConfirmActionForExistingGuest(bool linkToExisting)
	{
		if (linkToExisting)
		{
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_LinkNewLoginAQ3DToExistingGuest);
			CreateAQ3DLogin(1);
		}
		else
		{
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_DiscardGuestCreateNewLoginAQ3D);
			CreateAQ3DLogin(2);
		}
	}

	private IEnumerator CreateUserLoginAQ3DRoutine(string email, string password, int guestAction)
	{
		DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_AttemptCreateNewLoginAQ3D);
		Loader.show("Connecting...", 0f);
		string linkSource = "";
		if (PlayerPrefs.HasKey("LINKSOURCE"))
		{
			linkSource = PlayerPrefs.GetString("LINKSOURCE");
		}
		WebApiRequestCreateUserLoginAQ3D request = new WebApiRequestCreateUserLoginAQ3D(email, password, guestAction, CreateAQ3DNewsLetterOptIn.value, linkSource);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/CreateUserLoginAQ3D", request.GetWWWForm());
		string errorTitle = "Failed to create AQ3D login";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = "Email: " + email + " GuestAction: " + guestAction;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		Loader.close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		WebApiResponseCreateUserLoginAQ3D webApiResponseCreateUserLoginAQ3D;
		try
		{
			webApiResponseCreateUserLoginAQ3D = JsonConvert.DeserializeObject<WebApiResponseCreateUserLoginAQ3D>(www.downloadHandler.text);
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, "Failed to process response from the server", ex.Message, request.GetWWWForm(), customContext, ex);
			yield break;
		}
		if (webApiResponseCreateUserLoginAQ3D.Success)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("af_registration_method", "aq3d");
			AppsFlyer.sendEvent("af_complete_registration", dictionary);
			DeviceTracking.Instance.RecordDeviceEvent(DeviceTracking.DeviceEvent.AccountCreate_NewLoginAQ3DCreatedOk);
			PlayerPrefs.SetString("USERNAME", CreateAQ3DUsername.value);
			PlayerPrefs.SetString("PASSWORDENCODE", Util.Base64Encode(CreateAQ3DPassword.value));
			PlayerPrefs.Save();
			if (LoginManager.HasGuestGuid())
			{
				PlayerPrefs.SetInt("GUESTGUIDCONVERTED", 1);
			}
			LoginManager.LoginAQ3D(CreateAQ3DUsername.value, CreateAQ3DPassword.value);
			Close();
		}
		else if (webApiResponseCreateUserLoginAQ3D.HasError(WebApiResponseCreateUserLoginAQ3D.Error_UserAccountAlreadyExists))
		{
			ExistingEmailLabel.text = CreateAQ3DUsername.value;
			ShowRecover();
		}
		else if (!webApiResponseCreateUserLoginAQ3D.Success)
		{
			MessageBox.Show("Error!", webApiResponseCreateUserLoginAQ3D.Message);
		}
	}

	public void ShowAddLoginAQ3D()
	{
		TitleLabel.text = "Create Login";
		SubtitleLabel.text = "Add Cross Platform Email Login";
		HideAllStates();
		SharedPanel.SetActive(value: true);
		StateAddLoginAQ3D.SetActive(value: true);
		btnBack.gameObject.SetActive(value: false);
	}

	private void OnAddLoginAQ3DClick(GameObject go)
	{
		AddAQ3DLogin();
	}

	private void AddAQ3DLogin()
	{
		if (VerifyEmailPasswordTerms(AddAQ3DUsername.value, AddAQ3DPassword.value, AddAQ3DToggleTerms.value))
		{
			if (!PlayerPrefs.HasKey("JWT"))
			{
				MessageBox.Show("Error!", "Unable to locate Apple Web Token");
				return;
			}
			string @string = PlayerPrefs.GetString("JWT");
			StartCoroutine(AddLoginAQ3DRoutine(AddAQ3DUsername.value, AddAQ3DPassword.value, @string));
		}
	}

	private IEnumerator AddLoginAQ3DRoutine(string email, string password, string appleJWT)
	{
		Loader.show("Connecting...", 0f);
		WebApiRequestAddLoginAQ3D request = new WebApiRequestAddLoginAQ3D(email, password, appleJWT);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/AddLoginAQ3D", request.GetWWWForm());
		string errorTitle = "Failed to add AQ3D login";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = "Email: " + email;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		Loader.close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		WebApiResponseAddLoginAQ3D webApiResponseAddLoginAQ3D;
		try
		{
			webApiResponseAddLoginAQ3D = JsonConvert.DeserializeObject<WebApiResponseAddLoginAQ3D>(www.downloadHandler.text);
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, "Failed to process response from the server", ex.Message, request.GetWWWForm(), customContext, ex);
			yield break;
		}
		if (webApiResponseAddLoginAQ3D.Success)
		{
			PlayerPrefs.SetString("USERNAME", AddAQ3DUsername.value);
			PlayerPrefs.SetString("PASSWORDENCODE", Util.Base64Encode(AddAQ3DPassword.value));
			PlayerPrefs.Save();
			Session.AppleUserHasAQ3DLogin = true;
			OnAddLoginAQ3DSuccess();
			MessageBox.Show("Success!", "You have added an AQ3D login that can be used to play from any device!");
			Close();
		}
		else if (webApiResponseAddLoginAQ3D.HasError(WebApiResponseAddLoginAQ3D.Error_UserAccountAlreadyExists))
		{
			MessageBox.Show("Error!", "That email is already linked to another user.");
		}
		else if (!webApiResponseAddLoginAQ3D.Success)
		{
			MessageBox.Show("Error!", webApiResponseAddLoginAQ3D.Message);
		}
	}

	public void ShowConvertGuest(string title = "", string subtitle = "", bool delayOpen = false)
	{
		_title = title;
		_subtitle = subtitle;
		if (delayOpen)
		{
			SharedPanel.SetActive(value: false);
			StartCoroutine(DelayDrawConvertGuest());
		}
		else
		{
			DrawConvertGuest();
		}
	}

	private IEnumerator DelayDrawConvertGuest()
	{
		yield return new WaitForSeconds(2.5f);
		DrawConvertGuest();
	}

	private void DrawConvertGuest()
	{
		TitleLabel.text = ((_title != "") ? _title : "Create Login");
		SubtitleLabel.text = ((_subtitle != "") ? _subtitle : "Create a login to save your progress\nand play from any device!");
		HideAllStates();
		SharedPanel.SetActive(value: true);
		StateConvertGuest.SetActive(value: true);
		ConvertOptions.SetActive(value: true);
		ConvertToAQ3D.SetActive(value: false);
		btnBack.gameObject.SetActive(value: false);
	}

	private void OnConvertToAQ3DClick(GameObject go)
	{
		ConvertOptions.SetActive(value: false);
		ConvertToAQ3D.SetActive(value: true);
		btnBack.gameObject.SetActive(value: true);
	}

	private void OnConvertToFacebookClick(GameObject go)
	{
		FBManager.CreateLoginFacebookForGuest();
	}

	private void OnConnectGuestToLoginAQ3DClick(GameObject go)
	{
		ConnectGuestToLoginAQ3D();
	}

	private void ConnectGuestToLoginAQ3D()
	{
		if (VerifyEmailPasswordTerms(ConvertGuestUsername.value, ConvertGuestPassword.value, ConvertGuestToggleTerms.value))
		{
			StartCoroutine(ConnectGuestToLoginAQ3DRoutine(ConvertGuestUsername.value, ConvertGuestPassword.value));
		}
	}

	private IEnumerator ConnectGuestToLoginAQ3DRoutine(string email, string password)
	{
		BusyDialog.Show("Connecting...");
		if (!LoginManager.HasGuestGuid())
		{
			MessageBox.Show("Error!", "Failed to convert guest to AQ3D account!");
			yield break;
		}
		WebApiRequestConnectGuestToLoginAQ3D request = new WebApiRequestConnectGuestToLoginAQ3D(email, password, ConvertGuestAQ3DNewsLetterOptIn.value, LoginManager.GetGuestGuid());
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/ConnectGuestToLoginAQ3D", request.GetWWWForm());
		string errorTitle = "Failed to connect guest to AQ3D login";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = "Email: " + email;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		BusyDialog.Close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		try
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			WebApiResponseConnectGuestToLoginAQ3D webApiResponseConnectGuestToLoginAQ3D = JsonConvert.DeserializeObject<WebApiResponseConnectGuestToLoginAQ3D>(www.downloadHandler.text);
			if (webApiResponseConnectGuestToLoginAQ3D.Success)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("af_registration_method", "guestConvertToAQ3D");
				AppsFlyer.sendEvent("af_complete_registration", dictionary);
				Session.IsGuest = false;
				PlayerPrefs.SetInt("GUESTGUIDCONVERTED", 1);
				PlayerPrefs.SetString("USERNAME", email);
				PlayerPrefs.SetString("PASSWORDENCODE", Util.Base64Encode(password));
				PlayerPrefs.Save();
				OnConvertGuestSuccess();
				MessageBox.Show("Success!", "You have created a new AQ3D login that can be used to play from any device!");
				Close();
			}
			else
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, webApiResponseConnectGuestToLoginAQ3D.Message, request.GetWWWForm(), customContext);
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, request.GetWWWForm(), customContext, ex);
		}
	}

	public void ConnectGuestToLoginFB()
	{
		StartCoroutine(ConnectGuestToLoginFBRoutine());
	}

	private IEnumerator ConnectGuestToLoginFBRoutine()
	{
		BusyDialog.Show("Connecting...");
		if (!LoginManager.HasGuestGuid())
		{
			MessageBox.Show("Error!", "Failed to convert guest to Facebook account!");
			yield break;
		}
		string customContext = "FAT: " + FBManager.FacebookAccessToken + " GuestGuid: " + LoginManager.GetGuestGuid();
		WebApiRequestConnectGuestToLoginFB request = new WebApiRequestConnectGuestToLoginFB(FBManager.FacebookAccessToken, LoginManager.GetGuestGuid());
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/ConnectGuestToLoginFB", request.GetWWWForm());
		string errorTitle = "Failed to connect guest to Facebook login!";
		string friendlyMsg = "Unable to communicated with the server.";
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		BusyDialog.Close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		WebApiResponseConnectGuestToLoginFB webApiResponseConnectGuestToLoginFB = JsonConvert.DeserializeObject<WebApiResponseConnectGuestToLoginFB>(www.downloadHandler.text);
		if (webApiResponseConnectGuestToLoginFB.Success)
		{
			Session.IsGuest = false;
			PlayerPrefs.SetInt("GUESTGUIDCONVERTED", 1);
			PlayerPrefs.Save();
			OnConvertGuestSuccess();
			MessageBox.Show("Success!", "You have created a new Facebook login that can be used to play from any device!");
			Close();
		}
		else if (webApiResponseConnectGuestToLoginFB.HasError(WebApiResponseConnectGuestToLoginFB.FacebookError_GraphDidNotReturnValidEmail))
		{
			FBManager.Logout();
			FBManager.FacebookEmailPermissionFailedOnGuestLink();
		}
		else
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, "Unable to process response from the server.", webApiResponseConnectGuestToLoginFB.Message, request.GetWWWForm(), customContext);
		}
	}

	public void OnConvertGuestSuccess()
	{
		if (ConvertGuestSuccess != null)
		{
			ConvertGuestSuccess();
		}
	}

	public void OnAddLoginAQ3DSuccess()
	{
		if (AddLoginAQ3DSuccess != null)
		{
			AddLoginAQ3DSuccess();
		}
	}

	private void HandleReadPermission(bool success, string error)
	{
		if (success)
		{
			ConnectGuestToLoginFB();
			return;
		}
		LoginManager.LoginFailed();
		MessageBox.Show("Permission Failed", error);
	}
}

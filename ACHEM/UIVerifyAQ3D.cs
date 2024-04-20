using System;
using UnityEngine;

public class UIVerifyAQ3D : UIWindow
{
	[Header("Shared UI")]
	public UILabel TitleLabel;

	public UILabel SubtitleLabel;

	public UIButtonColor btnClose;

	[Header("Verify AQ3D")]
	public UIInput LoginUsername;

	public UIInput LoginPassword;

	public UIButtonColor btnVerifyAQ3D;

	[Header("States")]
	public GameObject StateVerify;

	public static Action ConvertGuestSuccess;

	private static UIVerifyAQ3D instance;

	public static void Load()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/VerifyAQ3D"), UIManager.Instance.transform).GetComponent<UIVerifyAQ3D>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnVerifyAQ3D.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnVerifyAQ3DClick));
		TitleLabel.text = "Verify Login";
		SubtitleLabel.text = "Verify AQ3D login to link Facebook";
	}

	private void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnVerifyAQ3D.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnVerifyAQ3DClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnVerifyAQ3DClick(GameObject go)
	{
		string value = LoginUsername.value;
		string value2 = LoginPassword.value;
		if (!(value == "Username") && !(value == "") && !(value2 == ""))
		{
			FBManager.AQ3DIntention = 2;
			FBManager.VerifiedAQ3DEmail = value;
			FBManager.VerifiedAQ3DPassword = value2;
			LoginManager.LoginFacebook();
		}
	}
}

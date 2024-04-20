using System;
using UnityEngine;

public class PrivateQueueUI : ModalWindow
{
	private static PrivateQueueUI instance;

	public UIButton BtnCreate;

	public UIButton BtnJoin;

	public UIButton BtnClose;

	public UIInput InputPassword;

	public UILabel DefaultText;

	private int queueID;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnCreate.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCreateClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnJoin.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnJoinClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public static void Show(int queueID)
	{
		if (instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIPrivateQueue"), UIManager.Instance.transform);
			obj.name = "UIPrivateQueue";
			instance = obj.GetComponent<PrivateQueueUI>();
			instance.Init();
		}
		instance.queueID = queueID;
	}

	private void OnCreateClick(GameObject go)
	{
		if (ValidatePassword(InputPassword.value))
		{
			AEC.getInstance().sendRequest(new RequestJoinQueue(queueID, InputPassword.value, createPrivate: true));
			Close();
		}
	}

	private void OnJoinClick(GameObject go)
	{
		if (ValidatePassword(InputPassword.value))
		{
			AEC.getInstance().sendRequest(new RequestJoinQueue(queueID, InputPassword.value));
			Close();
		}
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	public void OnPasswordChange()
	{
		DefaultText.gameObject.SetActive(string.IsNullOrEmpty(InputPassword.value));
	}

	private bool ValidatePassword(string password)
	{
		if (string.IsNullOrEmpty(password))
		{
			Notification.ShowWarning("Must provide a password");
			return false;
		}
		if (password.Length > 20)
		{
			Notification.ShowWarning("Please use a shorter password");
			return false;
		}
		return true;
	}
}

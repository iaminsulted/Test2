using System;
using UnityEngine;

public class Confirmation : ModalWindow
{
	protected static Confirmation mInstance;

	protected Action<bool> callback;

	public BoxCollider2D blocker;

	public UILabel Title;

	public UILabel Message;

	public UIButton BtnYes;

	public UIButton BtnNo;

	public UIButton BtnClose;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnYes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onYesClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnNo.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onNoClick));
		if (BtnClose != null)
		{
			UIEventListener uIEventListener3 = UIEventListener.Get(BtnClose.gameObject);
			uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onCloseClick));
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			callback(obj: true);
			Close();
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
		{
			Close();
		}
	}

	protected void ShowConfirmation(string title, string message, Action<bool> callback)
	{
		Title.text = title;
		Message.text = message;
		this.callback = callback;
	}

	protected void onYesClick(GameObject go)
	{
		callback(obj: true);
		Close();
	}

	protected void onNoClick(GameObject go)
	{
		callback(obj: false);
		Close();
	}

	protected void onCloseClick(GameObject go)
	{
		Close();
	}

	protected override void Close()
	{
		callback = null;
		base.Close();
	}

	public static void Show(string title, string message, string yesLabel, string noLabel, Action<bool> callback, bool Closable = false, bool enableCollider = true, bool large = false)
	{
		Show(title, message, callback, Closable, enableCollider, large);
		SetLabels(yesLabel, noLabel);
	}

	public static void Show(string title, string message, Action<bool> callback, bool isClosable = false, bool enableCollider = true, bool isLarge = false)
	{
		if (mInstance == null)
		{
			mInstance = CreateInstance(isClosable, enableCollider, isLarge);
		}
		mInstance.ShowConfirmation(title, message, callback);
	}

	private static Confirmation CreateInstance(bool isClosable, bool enableCollider, bool isLarge)
	{
		string path = (isLarge ? "ConfirmationLarge" : "Confirmation");
		if (isClosable)
		{
			path = "ConfirmationClosable";
		}
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(path), UIManager.Instance.transform);
		obj.name = "Confirmation";
		Confirmation component = obj.GetComponent<Confirmation>();
		component.blocker.enabled = enableCollider;
		component.Init();
		return component;
	}

	public static void SetLabels(string yesLabel, string noLabel)
	{
		if (!(mInstance == null))
		{
			UILabel componentInChildren = mInstance.BtnYes.GetComponentInChildren<UILabel>();
			UILabel componentInChildren2 = mInstance.BtnNo.GetComponentInChildren<UILabel>();
			if (componentInChildren != null)
			{
				componentInChildren.text = yesLabel;
			}
			if (componentInChildren2 != null)
			{
				componentInChildren2.text = noLabel;
			}
		}
	}

	public static void OpenUrl(string url)
	{
		Show("Open Web Page", "This will open a website in your browser, continue?", delegate(bool b)
		{
			if (b)
			{
				Application.OpenURL(url);
			}
		});
	}
}

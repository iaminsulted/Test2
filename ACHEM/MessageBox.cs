using System;
using UnityEngine;

public class MessageBox : ModalWindow
{
	protected Action callback;

	[Header("Normal Box")]
	public UILabel Title;

	public UILabel Message;

	public UILabel ButtonText;

	public UIButton ButtonClose;

	public static MessageBox Instance { get; protected set; }

	protected virtual void Awake()
	{
		Instance = this;
	}

	protected virtual void OnDestroy()
	{
		Instance = null;
	}

	protected virtual void Start()
	{
		if (ButtonClose != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(ButtonClose.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onCloseClick));
		}
	}

	protected void onCloseClick(GameObject go)
	{
		if (callback != null)
		{
			callback();
		}
		Close();
	}

	protected override void Close()
	{
		callback = null;
		base.Close();
	}

	public static void Show(string title, string message, string buttonText, Action callback = null)
	{
		if (Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MessageBox"), UIManager.Instance.transform);
			obj.name = "MessageBox";
			Instance = obj.GetComponent<MessageBox>();
			Instance.Init();
		}
		Instance.callback = callback;
		Instance.Title.text = title;
		Instance.Message.text = message;
		Instance.ButtonText.text = buttonText;
	}

	public static void Show(string title, string message, Action callback = null)
	{
		Show(title, message, "OK", callback);
	}
}

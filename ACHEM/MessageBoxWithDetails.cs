using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MessageBoxWithDetails : MessageBox
{
	public enum Type
	{
		Support,
		Status
	}

	[Header("Friendly Box")]
	public GameObject friendlyBox;

	public UILabel TitleFriendly;

	public UILabel MessageFriendly;

	public UILabel ButtonTextFriendly;

	public UIButton ButtonCloseFriendly;

	public UIButton ButtonDetails;

	[Header("Details Box")]
	public GameObject detailsBox;

	public UILabel TitleDetails;

	public UILabel MessageDetails;

	public UILabel ButtonTextDetails;

	public UILabel ButtonActionLabel;

	public UIButton ButtonCloseDetails;

	[FormerlySerializedAs("ButtonContactSupport")]
	public UIButton ActionButton;

	private Type type;

	protected override void Start()
	{
		base.Start();
		if (ButtonCloseFriendly != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(ButtonCloseFriendly.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(base.onCloseClick));
		}
		if (ButtonCloseDetails != null)
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(ButtonCloseDetails.gameObject);
			uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(base.onCloseClick));
		}
		if (ButtonDetails != null)
		{
			UIEventListener uIEventListener3 = UIEventListener.Get(ButtonDetails.gameObject);
			uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onDetailsClick));
		}
		if (ActionButton != null)
		{
			UIEventListener uIEventListener4 = UIEventListener.Get(ActionButton.gameObject);
			uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(onActionClick));
		}
	}

	private void onActionClick(GameObject go)
	{
		Type type = this.type;
		if (type == Type.Support || type != Type.Status)
		{
			Confirmation.OpenUrl(Main.ContactSupportURL);
		}
		else
		{
			Confirmation.OpenUrl(Main.StatusTwitterURL);
		}
	}

	private void onDetailsClick(GameObject go)
	{
		((MessageBoxWithDetails)MessageBox.Instance).friendlyBox.SetActive(value: false);
		((MessageBoxWithDetails)MessageBox.Instance).detailsBox.SetActive(value: true);
	}

	public static void Show(string title, string friendlyMessage, string buttonText, string fullMessage, Type type, Action callback = null)
	{
		if (MessageBox.Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MessageBoxWithDetails"), UIManager.Instance.transform);
			obj.name = "MessageBox";
			MessageBox.Instance = obj.GetComponent<MessageBoxWithDetails>();
			((MessageBoxWithDetails)MessageBox.Instance).Init();
			((MessageBoxWithDetails)MessageBox.Instance).detailsBox.SetActive(value: false);
			((MessageBoxWithDetails)MessageBox.Instance).friendlyBox.SetActive(value: true);
		}
		((MessageBoxWithDetails)MessageBox.Instance).type = type;
		((MessageBoxWithDetails)MessageBox.Instance).callback = callback;
		((MessageBoxWithDetails)MessageBox.Instance).TitleFriendly.text = title;
		((MessageBoxWithDetails)MessageBox.Instance).MessageFriendly.text = friendlyMessage;
		((MessageBoxWithDetails)MessageBox.Instance).ButtonTextFriendly.text = buttonText;
		((MessageBoxWithDetails)MessageBox.Instance).TitleDetails.text = title;
		((MessageBoxWithDetails)MessageBox.Instance).MessageDetails.text = fullMessage;
		((MessageBoxWithDetails)MessageBox.Instance).ButtonTextDetails.text = buttonText;
		((MessageBoxWithDetails)MessageBox.Instance).ButtonActionLabel.text = GetActionTextByType(type);
	}

	public static void Show(string title, string friendlyMessage, Type type, string fullMessage = "", Action callback = null)
	{
		Show(title, friendlyMessage, "OK", fullMessage, type, callback);
	}

	public static string GetActionTextByType(Type type)
	{
		if (type == Type.Support || type != Type.Status)
		{
			return "CONTACT SUPPORT";
		}
		return "CHECK SERVER STATUS";
	}
}

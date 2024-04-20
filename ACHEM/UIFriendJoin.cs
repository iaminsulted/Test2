using System;
using UnityEngine;

public class UIFriendJoin : UIWindow
{
	private static UIFriendJoin instance;

	public UIButton btnClose;

	public UIButton btnSummonJoin;

	public UIInput input;

	public Action<string> callback;

	public static void Show(Action<string> callback)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/EnterSummonCode"), UIManager.Instance.transform).GetComponent<UIFriendJoin>();
		}
		instance.callback = callback;
		instance.Init();
	}

	public void Start()
	{
		Init();
	}

	protected override void Init()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnSummonJoin.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnJoinClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClose));
		input.isSelected = true;
	}

	public void OnJoinClicked(GameObject go)
	{
		if (input.value.Length != 0)
		{
			if (callback != null)
			{
				callback(input.value.Trim(' '));
			}
			Destroy();
		}
	}

	private void OnClose(GameObject go)
	{
		if (callback != null)
		{
			callback(null);
		}
		Destroy();
	}

	protected override void Destroy()
	{
		base.Destroy();
		UIEventListener uIEventListener = UIEventListener.Get(btnSummonJoin.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnJoinClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClose));
	}
}

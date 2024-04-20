using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFriendsList : UIMenuWindow
{
	public UIButton btnSummon;

	public UIButton btnSummonJoin;

	public UIButton btnFriendRequests;

	public UILabel BtnFriendRequestsLabel;

	public UILabel ServerStatus;

	public UISprite BtnFriendRequestsSprite;

	public UISprite BtnFriendRequestsSprite2;

	private static UIFriendsList instance;

	public UIFriendDetail detail;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIInventoryFriend> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	public UIGrid grid;

	private UIInventoryFriend selected;

	public static void Toggle()
	{
		if (instance == null)
		{
			Show();
		}
		else
		{
			instance.Close();
		}
	}

	public static void Show()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UIFriendsList"), UIManager.Instance.transform).GetComponent<UIFriendsList>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnSummon.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSummonClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnFriendRequests.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnRequestsClick));
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryFriend>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		detail.gameObject.SetActive(value: false);
		btnFriendRequests.isEnabled = Session.MyPlayerData.friendRequests.Count > 0;
		if (PlayerPrefs.HasKey("CurrentServer"))
		{
			string @string = PlayerPrefs.GetString("CurrentServer");
			if (!string.IsNullOrEmpty(@string))
			{
				ServerStatus.text = "Your Server - " + @string;
			}
		}
	}

	private void OnEnable()
	{
		Session.MyPlayerData.FriendsUpdated += UpdatedFriends;
		Game.Instance.SendFriendListRequest();
		if (btnFriendRequests.GetComponent<UINotificationIcon>() != null)
		{
			btnFriendRequests.GetComponent<UINotificationIcon>().ShouldIBeOn();
		}
	}

	private void OnDisable()
	{
		Session.MyPlayerData.FriendsUpdated -= UpdatedFriends;
	}

	public void UpdatedFriends()
	{
		if (selected != null && Session.MyPlayerData.friendsList.IndexOf(selected.Friend) < 0)
		{
			selected = null;
		}
		refresh();
	}

	public void refresh()
	{
		foreach (UIInventoryFriend itemGO in itemGOs)
		{
			itemGO.gameObject.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnFriendClicked;
		}
		itemGOs.Clear();
		foreach (FriendData item in Session.MyPlayerData.friendsList.OrderByDescending((FriendData x) => x.ServerID).ThenBy((FriendData y) => y.strName).ToList())
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryFriend component = obj.GetComponent<UIInventoryFriend>();
			component.Init(item);
			component.Clicked += OnFriendClicked;
			itemGOs.Add(component);
		}
		grid.Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnFriendClicked(UIItem si)
	{
		if (selected != null)
		{
			selected.Selected = false;
		}
		selectFriend(si as UIInventoryFriend);
	}

	private void selectFriend(UIInventoryFriend f)
	{
		selected = f;
		selected.Selected = true;
		detail.gameObject.SetActive(value: true);
		detail.Init(f.Friend);
		f.Selected = true;
	}

	public void OnSummonClick(GameObject go)
	{
		UISummonWord.Show();
	}

	public void OnRequestsClick(GameObject go)
	{
		UIFriendRequests.Show();
	}
}

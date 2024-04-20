using System;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIHouseSlot : MonoBehaviour
{
	public UILabel HouseName;

	public UILabel PublicPrivate;

	public UILabel MapName;

	public UIButton ShowDetailBtn;

	public UIButton EnterHouseBtn;

	public UITexture BackgroundTexture;

	private HouseData houseData;

	private UIHouseChooser houseChooser;

	private AssetLoader<Texture2D> imageDownloader;

	public void Init(HouseData hData, UIHouseChooser hChooser, AssetLoader<Texture2D> imgDownloader)
	{
		houseData = hData;
		houseChooser = hChooser;
		imageDownloader = imgDownloader;
		RefreshFromHouseData();
	}

	public void RefreshFromHouseData()
	{
		HouseName.text = houseData.Name;
		switch (houseData.Visibility)
		{
		case HouseVisibility.Public:
		{
			ServerInfo serverInfo = ServerInfo.Servers.Where((ServerInfo x) => x.ID == houseData.ServerID).First();
			PublicPrivate.text = "Public - Server: " + serverInfo.Name;
			break;
		}
		case HouseVisibility.Private:
			PublicPrivate.text = "Private";
			break;
		case HouseVisibility.FriendsOnly:
			PublicPrivate.text = "Friends Only";
			break;
		case HouseVisibility.GuildOnly:
			PublicPrivate.text = "Guild Only";
			break;
		}
		if (houseChooser.MyMaps.TryGetValue(houseData.MapItemID, out var value))
		{
			MapName.text = value.Name;
			SetImage(value);
		}
	}

	public void SetImage(Item item)
	{
		imageDownloader.Get(item.AssetName, item.bundle, delegate(Texture2D tex)
		{
			BackgroundTexture.mainTexture = tex;
		});
	}

	private void WrongServerMessage()
	{
		ServerInfo serverInfo = ServerInfo.Servers.Where((ServerInfo x) => x.ID == houseData.ServerID).First();
		MessageBox.Show("House is on " + serverInfo.Name + "!", "You cannot edit or join a public house from a different server. Please join " + serverInfo.Name + " if you'd like to enter or make edits!");
	}

	private void OnShowDetailClicked(GameObject go)
	{
		if (houseData.Visibility == HouseVisibility.Public && AEC.getInstance().ServerID != houseData.ServerID)
		{
			WrongServerMessage();
		}
		else
		{
			UIHouseChooser.Instance.ShowDetail(houseData, this);
		}
	}

	private void OnEnterHouseClicked(GameObject go)
	{
		if (houseData.Visibility == HouseVisibility.Public && AEC.getInstance().ServerID != houseData.ServerID)
		{
			WrongServerMessage();
		}
		else
		{
			AEC.getInstance().sendRequest(new RequestHouseJoin(houseData));
		}
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ShowDetailBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnShowDetailClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(EnterHouseBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnEnterHouseClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ShowDetailBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnShowDetailClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(EnterHouseBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnEnterHouseClicked));
	}
}

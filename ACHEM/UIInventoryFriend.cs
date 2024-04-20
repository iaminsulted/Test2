using UnityEngine;

public class UIInventoryFriend : UIItem
{
	public UISprite Check;

	public FriendData Friend;

	public UILabel Level;

	public UISprite Online;

	public UISprite OnlineRing;

	private readonly Color32 Offline_Color = new Color32(90, 90, 90, 155);

	public void Init(FriendData friend)
	{
		Friend = friend;
		UpdateStatus();
		Check.gameObject.SetActive(value: false);
		NameLabel.text = Friend.strName;
		InfoLabel.text = Friend.MapDisplayName;
		Level.text = "Level " + Friend.Level;
	}

	private void UpdateStatus()
	{
		if (!Friend.IsOnline)
		{
			Online.color = new Color32(111, 111, 111, byte.MaxValue);
			OnlineRing.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			NameLabel.color = Offline_Color;
			Level.color = Offline_Color;
			InfoLabel.color = Offline_Color;
		}
		if (!Friend.OnSameServer)
		{
			UISprite online = Online;
			int width = (Online.height = 15);
			online.width = width;
			UISprite onlineRing = OnlineRing;
			width = (OnlineRing.height = 16);
			onlineRing.width = width;
		}
	}
}

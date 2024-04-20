using System;
using Assets.Scripts.Game;
using UnityEngine;

public class UIMapItem : MonoBehaviour
{
	public UILabel MapNameLabel;

	public UIButton mainBtn;

	public UISprite checkmarkSprite;

	public UITexture BackgroundTexture;

	private UIHouseSlotDetail houseSlotDetail;

	private AssetLoader<Texture2D> imageDownloader;

	public InventoryItem MapInventoryItem { get; private set; }

	public void Init(InventoryItem iItem, UIHouseSlotDetail hItemDetail, AssetLoader<Texture2D> imgDownloader)
	{
		MapInventoryItem = iItem;
		MapNameLabel.text = iItem.Name;
		houseSlotDetail = hItemDetail;
		imageDownloader = imgDownloader;
		imageDownloader.Get(iItem.AssetName, iItem.bundle, delegate(Texture2D tex)
		{
			BackgroundTexture.mainTexture = tex;
		});
	}

	public void EnableCheckmark()
	{
		checkmarkSprite.gameObject.SetActive(value: true);
	}

	public void DisableCheckmark()
	{
		checkmarkSprite.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(mainBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnMainBtnClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(mainBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnMainBtnClicked));
	}

	private void OnMainBtnClicked(GameObject go)
	{
		houseSlotDetail.SelectMap(this, MapInventoryItem);
		EnableCheckmark();
	}
}

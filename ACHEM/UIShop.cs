using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIShop : UIMenuWindow
{
	private enum ShopMode
	{
		Buy,
		Sell
	}

	private ShopMode mode;

	private static UIShop instance;

	public UIButton btnBuy;

	public UIButton btnSell;

	public UILabel lblTitle;

	public UILabel LabelTokenQty;

	private static int ShopID;

	private static ShopType ShopType;

	private static int TokenID;

	public IEnumerable<Item> shopitems;

	public IEnumerable<Item> curitems;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIShopItem> itemGOs;

	private UIShopItem selectedItem;

	private string shopName;

	private ObjectPool<GameObject> itemGOpool;

	private Shop shop;

	public UIShopItemDetail uiShopItemDetail;

	public UISprite BackgroundSprite;

	public GameObject BuyCollectionGroup;

	public UIButton BuyCollectionButton;

	public UILabel CollectionPriceLabel;

	public GameObject CollectionPriceGroup;

	public UILabel PurchasedLabel;

	private bool isCollectionPurchased;

	private ShopMode Mode
	{
		get
		{
			return mode;
		}
		set
		{
			if (value != mode)
			{
				mode = value;
				if (selectedItem != null)
				{
					selectedItem.Selected = false;
				}
				if (mode == ShopMode.Buy)
				{
					lblTitle.text = shopName;
					curitems = shopitems;
				}
				else
				{
					lblTitle.text = "Sell";
					curitems = InventoryItems;
				}
				refresh();
			}
		}
	}

	public List<Item> InventoryItems => (from p in Session.MyPlayerData.items
		where p.IsRemovable
		orderby p.CharItemID descending
		select p).Cast<Item>().ToList();

	private bool IsCollection => shop.CollectionBadgeID > 0;

	public static void LoadShop(int shopID, ShopType shopType = ShopType.Shop, int tokenID = 0)
	{
		if (ShopID != shopID || ShopType != shopType)
		{
			ShopID = shopID;
			ShopType = shopType;
			TokenID = tokenID;
			if (Shops.Get(ShopID, shopType) != null)
			{
				Load(Shops.Get(ShopID, shopType));
				return;
			}
			Game.Instance.SendShopLoadRequest(shopID, shopType);
			Game.Instance.ShopLoaded += ShopDataLoaded;
		}
	}

	public static void ShopDataLoaded(Shop shop, string message)
	{
		Game.Instance.ShopLoaded -= ShopDataLoaded;
		if (shop != null)
		{
			Load(shop);
			return;
		}
		ShopID = 0;
		MessageBox.Show("Restricted Access", message);
	}

	private static void Load(Shop shop)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Shop"), UIManager.Instance.transform).GetComponent<UIShop>();
			instance.Init();
		}
		instance.LoadShop(shop);
	}

	private void LoadShop(Shop shop)
	{
		lblTitle.text = shop.Name;
		shopName = shop.Name;
		this.shop = shop;
		shopitems = shop.Items.Where((ShopItem p) => p.IsVisibleInStore()).Cast<Item>();
		lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		curitems = shopitems;
		if (IsCollection)
		{
			BuyCollectionGroup.gameObject.SetActive(value: true);
			BackgroundSprite.height -= 63;
			CollectionPriceLabel.text = shop.CollectionPrice.ToString("n0");
			if (Session.MyPlayerData.HasBadgeID(shop.CollectionBadgeID))
			{
				ShowCollectionAsPurchased();
			}
		}
		refresh();
		uiShopItemDetail.Visible = false;
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIShopItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(BuyCollectionButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBuyCollectionClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnBuy.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBuyClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnSell.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSellClick));
		if (TokenID == 0)
		{
			lblGold.transform.parent.gameObject.SetActive(value: true);
			LabelTokenQty.transform.parent.gameObject.SetActive(value: false);
		}
		else
		{
			lblGold.transform.parent.gameObject.SetActive(value: false);
			LabelTokenQty.transform.parent.gameObject.SetActive(value: true);
			LabelTokenQty.parent.GetComponent<UIItemTooltip>().SetItem(Items.Get(TokenID));
			UpdateTokenQty();
		}
		Session.MyPlayerData.ItemAdded += OnItemAdded;
		Session.MyPlayerData.ItemRemoved += OnItemRemoved;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
		Session.MyPlayerData.BitFlagUpdated += OnBitFlagUpdated;
		uiShopItemDetail.Visible = false;
	}

	private void UpdateTokenQty()
	{
		LabelTokenQty.text = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == TokenID).Sum((InventoryItem x) => x.Qty).ToString();
	}

	protected void OnBuyCollectionClick(GameObject go)
	{
		Badge badge = Badges.Get(shop.CollectionBadgeID);
		if (shop.IsCollectionPriceDC && Session.MyPlayerData.MC < shop.CollectionPrice)
		{
			ConfirmationSpend.Show("Insufficient Funds!", "Would you like to purchase more DCs?", "gems", shop.CollectionPrice, "Buy DCs", delegate(bool confirm)
			{
				if (confirm)
				{
					UIIAPStore.Show(showDCPage: true);
				}
			}, IsCollection);
			return;
		}
		ConfirmationSpend.Show("Buy Collection", "Buy this Collection - '" + shop.Name + "'? Permanently unlock every item in the Collection and get the Badge '" + badge.Name + "' and in-game Title '" + badge.Title + "'!\n\nAccess all of your Collections at Hootenheim in Battleon Bank.", "gems", shop.CollectionPrice, "Buy", delegate(bool confirm)
		{
			if (confirm)
			{
				AEC.getInstance().sendRequest(new RequestBuyCollection(shop.ID));
			}
		}, IsCollection);
	}

	protected void OnBuyClick(GameObject go)
	{
		Mode = ShopMode.Buy;
	}

	protected void OnSellClick(GameObject go)
	{
		Mode = ShopMode.Sell;
	}

	private void OnBitFlagUpdated(string name, byte index, bool value)
	{
		if (Session.MyPlayerData.HasBadgeID(shop.CollectionBadgeID))
		{
			ShowCollectionAsPurchased();
			refresh();
		}
	}

	private void ShowCollectionAsPurchased()
	{
		if (isCollectionPurchased)
		{
			return;
		}
		isCollectionPurchased = true;
		BuyCollectionButton.isEnabled = false;
		PurchasedLabel.gameObject.SetActive(value: true);
		CollectionPriceGroup.gameObject.SetActive(value: false);
		foreach (Item curitem in curitems)
		{
			curitem.Cost = 0;
		}
	}

	private void OnItemAdded(InventoryItem iItem)
	{
		if (Mode == ShopMode.Sell)
		{
			curitems = InventoryItems;
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIShopItem component = obj.GetComponent<UIShopItem>();
			component.Init(iItem);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
			container.GetComponent<UIGrid>().Reposition();
		}
		UpdateTokenQty();
	}

	private void OnItemRemoved(InventoryItem iItem)
	{
		if (Mode == ShopMode.Sell)
		{
			if (iItem == uiShopItemDetail.item)
			{
				uiShopItemDetail.Close();
			}
			curitems = InventoryItems;
			for (int num = itemGOs.Count - 1; num >= 0; num--)
			{
				UIShopItem uIShopItem = itemGOs[num];
				if (uIShopItem.Item == iItem)
				{
					if (selectedItem == uIShopItem)
					{
						selectedItem.Selected = false;
					}
					uIShopItem.gameObject.transform.SetAsLastSibling();
					itemGOpool.Release(uIShopItem.gameObject);
					uIShopItem.Clicked -= OnItemClicked;
					itemGOs.RemoveAt(num);
					break;
				}
			}
			container.GetComponent<UIGrid>().Reposition();
			container.parent.GetComponent<UIScrollView>().InvalidateBounds();
			container.parent.GetComponent<UIPanel>().SetDirty();
		}
		UpdateTokenQty();
	}

	private void OnItemUpdated(InventoryItem iItem)
	{
		if (Mode == ShopMode.Sell)
		{
			for (int num = itemGOs.Count - 1; num >= 0; num--)
			{
				UIShopItem uIShopItem = itemGOs[num];
				if (uIShopItem.Item == iItem)
				{
					uIShopItem.Init(iItem);
					break;
				}
			}
			container.GetComponent<UIGrid>().Reposition();
			container.parent.GetComponent<UIScrollView>().InvalidateBounds();
			container.parent.GetComponent<UIPanel>().SetDirty();
		}
		UpdateTokenQty();
	}

	public void refresh()
	{
		foreach (UIShopItem itemGO in itemGOs)
		{
			itemGO.gameObject.SetActive(value: false);
			itemGO.gameObject.transform.SetAsLastSibling();
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		IEnumerable<Item> enumerable = curitems.Where((Item x) => x.Type != ItemType.Map);
		enumerable = ((mode != ShopMode.Sell) ? curitems : curitems.Where((Item x) => x.Type != ItemType.Map));
		foreach (Item item in enumerable)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIShopItem component = obj.GetComponent<UIShopItem>();
			component.Init(item);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnItemClicked(UIItem si)
	{
		if (selectedItem != null)
		{
			if (selectedItem == si)
			{
				if (!uiShopItemDetail.Visible)
				{
					ShowDetail(selectedItem.Item);
				}
				else if (si.Item.HasPreview)
				{
					UIPreview.Show(selectedItem.Item);
				}
				return;
			}
			selectedItem.Selected = false;
		}
		selectedItem = si as UIShopItem;
		selectedItem.Selected = true;
		ShowDetail(selectedItem.Item);
	}

	private void ShowDetail(Item item)
	{
		if (Mode == ShopMode.Buy)
		{
			uiShopItemDetail.LoadBuyItem((ShopItem)item, shop, ShopType, TokenID);
		}
		else
		{
			uiShopItemDetail.LoadSellItem((InventoryItem)item);
		}
	}

	protected override void Destroy()
	{
		ShopID = 0;
		TokenID = 0;
		ShopType = ShopType.Shop;
		Session.MyPlayerData.ItemAdded -= OnItemAdded;
		Session.MyPlayerData.ItemRemoved -= OnItemRemoved;
		Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
		Session.MyPlayerData.BitFlagUpdated -= OnBitFlagUpdated;
		base.Destroy();
	}
}

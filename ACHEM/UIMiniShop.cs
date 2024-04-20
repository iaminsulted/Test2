using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMiniShop : UIMenuWindow
{
	private enum ShopMode
	{
		Buy,
		Sell
	}

	private ShopMode mode;

	public static string SpriteDefault = "BlackbuttonWhiteStroked36x36";

	public static string SpriteSelected = "WhiteButton27x27";

	private static UIMiniShop instance;

	private static int ShopID;

	private static Shop shop;

	private static ShopType ShopType;

	private static int TokenID;

	public IEnumerable<Item> shopitems;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIShopItem> itemGOs;

	private UIShopItem selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	public UIShopItemDetail uiShopItemDetail;

	public UILabel lblTitle;

	public UILabel LabelTokenQty;

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

	public static void TogglePotionShop()
	{
		if (instance == null)
		{
			LoadShop(12);
		}
		else
		{
			instance.Close();
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
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/ShopMini"), UIManager.Instance.transform).GetComponent<UIMiniShop>();
			instance.Init();
		}
		UIMiniShop.shop = shop;
		instance.LoadShop(shop);
	}

	private void LoadShop(Shop shop)
	{
		lblTitle.text = shop.Name;
		shopitems = shop.Items.Where((ShopItem p) => p.IsVisibleInStore()).Cast<Item>().ToList();
		lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		lblMC.text = Session.MyPlayerData.MC.ToString("n0");
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
		uiShopItemDetail.Visible = false;
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
		Session.MyPlayerData.ItemAdded += OnItemUpdated;
		Session.MyPlayerData.ItemRemoved += OnItemUpdated;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
	}

	private void OnItemUpdated(InventoryItem obj)
	{
		UpdateTokenQty();
	}

	private void UpdateTokenQty()
	{
		LabelTokenQty.text = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == TokenID).Sum((InventoryItem x) => x.Qty).ToString();
	}

	public void refresh()
	{
		foreach (UIShopItem itemGO in itemGOs)
		{
			itemGO.gameObject.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		foreach (Item shopitem in shopitems)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIShopItem component = obj.GetComponent<UIShopItem>();
			component.Init(shopitem);
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
		uiShopItemDetail.LoadBuyItem((ShopItem)item, shop, ShopType, TokenID);
	}

	protected override void Destroy()
	{
		ShopID = 0;
		TokenID = 0;
		Session.MyPlayerData.ItemAdded -= OnItemUpdated;
		Session.MyPlayerData.ItemRemoved -= OnItemUpdated;
		Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
		base.Destroy();
	}
}

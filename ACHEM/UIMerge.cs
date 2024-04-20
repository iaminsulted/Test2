using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMerge : UIMenuWindow
{
	private enum ShopMode
	{
		Shop,
		Inventory
	}

	private ShopMode mode;

	public static Color colorbtndefault = new Color(40f / 51f, 40f / 51f, 40f / 51f);

	public static Color colorbtnselected = new Color(13f / 15f, 26f / 51f, 0.08627451f);

	private static UIMerge instance;

	public static int currentShopID;

	public static MergeShop currentShop;

	public List<Merge> curmerges;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIInventoryMerge> itemGOs;

	private UIInventoryMerge selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	public UIMergeDetail mergeDetail;

	public UILabel lblTitle;

	public static void Load(int msid)
	{
		if (currentShopID != msid)
		{
			currentShopID = msid;
			if (MergeShops.Get(msid) == null)
			{
				Game.Instance.MergeShopLoaded += MergeShopDataLoaded;
				Game.Instance.SendMergeShopsRequest(msid);
			}
			else
			{
				loadedMergeShops(MergeShops.Get(msid));
			}
		}
	}

	public static void MergeShopDataLoaded(MergeShop mergeshop, string message)
	{
		Game.Instance.MergeShopLoaded -= MergeShopDataLoaded;
		if (mergeshop != null)
		{
			loadedMergeShops(mergeshop);
			return;
		}
		currentShopID = 0;
		MessageBox.Show("Restricted Access", message);
	}

	public static void loadedMergeShops(MergeShop mergeshop)
	{
		currentShop = MergeShops.Get(currentShopID);
		if (instance == null)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/MergeUI"), UIManager.Instance.transform).GetComponent<UIMerge>();
			instance.Init();
		}
		instance.mode = ShopMode.Shop;
		instance.lblTitle.text = currentShop.Name;
		instance.curmerges = (from p in currentShop.Merges.Values
			where p.IsVisibleInStore()
			orderby p.SortOrder
			select p).ToList();
		instance.refresh();
	}

	public static void LoadMergeInventory()
	{
		if (instance == null)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/MergeUI"), UIManager.Instance.transform).GetComponent<UIMerge>();
			instance.Init();
		}
		instance.mode = ShopMode.Inventory;
		instance.lblTitle.text = "Crafting";
		instance.curmerges = Session.MyPlayerData.merges;
		instance.refresh();
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryMerge>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		Session.MyPlayerData.MergeAdded += MergeAdded;
		Session.MyPlayerData.MergeRemoved += MergeRemoved;
		mergeDetail.Visible = false;
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			LoadMergeInventory();
		}
		else
		{
			instance.Close();
		}
	}

	public void MergeAdded(Merge merge)
	{
		if (mode == ShopMode.Inventory)
		{
			curmerges = Session.MyPlayerData.merges;
			refresh();
		}
		if (mergeDetail.merge.MergeID == merge.MergeID)
		{
			mergeDetail.LoadMerge(merge, currentShopID);
		}
	}

	private void MergeRemoved(Merge merge)
	{
		MessageBox.Show("Crafted Item", merge.Name + " crafted successfully!");
		if (mode == ShopMode.Inventory)
		{
			curmerges = Session.MyPlayerData.merges;
			refresh();
		}
		if (mergeDetail.merge.MergeID != merge.MergeID)
		{
			return;
		}
		if (mode == ShopMode.Inventory)
		{
			if (curmerges.Count == 0)
			{
				Close();
			}
			else
			{
				mergeDetail.LoadMerge(curmerges[0], currentShopID);
			}
		}
		else
		{
			Merge m = curmerges.Where((Merge x) => x.MergeID == merge.MergeID).FirstOrDefault();
			mergeDetail.LoadMerge(m, currentShopID);
		}
	}

	public void refresh()
	{
		foreach (UIInventoryMerge itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		foreach (Merge curmerge in curmerges)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryMerge component = obj.GetComponent<UIInventoryMerge>();
			component.Init(curmerge);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		refreshDetail();
	}

	private void OnItemClicked(UIItem si)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
		selectedItem = si as UIInventoryMerge;
		selectedItem.Selected = true;
		refreshDetail();
	}

	private void refreshDetail()
	{
		if (mergeDetail != null && selectedItem != null)
		{
			Merge merge = Session.MyPlayerData.merges.Where((Merge x) => x.MergeID == selectedItem.merge.MergeID).FirstOrDefault();
			if (merge == null)
			{
				mergeDetail.LoadMerge(selectedItem.merge, currentShopID);
			}
			else
			{
				mergeDetail.LoadMerge(merge, currentShopID);
			}
		}
	}

	protected override void Destroy()
	{
		if ((bool)instance)
		{
			instance = null;
		}
		currentShopID = 0;
		recordCompletes();
		Session.MyPlayerData.MergeAdded -= MergeAdded;
		Session.MyPlayerData.MergeRemoved -= MergeRemoved;
		base.Destroy();
	}

	private void recordCompletes()
	{
		int num = 0;
		for (int i = 0; i < Session.MyPlayerData.merges.Count; i++)
		{
			if (Session.MyPlayerData.merges[i].MergeMinutes <= 0f || (Session.MyPlayerData.merges[i].TSComplete.Value - GameTime.ServerTime).TotalSeconds <= 0.0)
			{
				num++;
			}
		}
		PlayerPrefs.SetInt("recordedMergeTotal", num);
	}
}

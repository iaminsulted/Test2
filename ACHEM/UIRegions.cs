using System.Collections.Generic;
using UnityEngine;

public class UIRegions : UIMenuWindow
{
	private const int Level_Restriction = 3;

	public UIScrollView scrollview;

	public UITable table;

	public UIGrid grid;

	public UILabel title;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIRegionListItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	private static UIRegions instance;

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
		if (CheckAccess())
		{
			if (instance == null)
			{
				Session.MyPlayerData.LoadQuestAreas();
				instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/Regions"), UIManager.Instance.transform).GetComponent<UIRegions>();
				instance.Init();
				WorldMap.Show();
			}
			instance.refresh();
		}
	}

	private static bool CheckAccess()
	{
		if (Entities.Instance.me.Level < 3)
		{
			MessageBox.Show("Travel", "Travel is available starting at Level " + 3 + "!");
			return false;
		}
		return true;
	}

	protected override void Init()
	{
		base.Init();
		itemGOs = new List<UIRegionListItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		container = itemGOprefab.transform.parent;
		Session.MyPlayerData.AreasReceived += MyPlayerData_AreasReceived;
	}

	private void MyPlayerData_AreasReceived()
	{
		refresh();
	}

	private void refresh()
	{
		if (Session.MyPlayerData.regionList == null)
		{
			Game.Instance.SendMapsListRequest();
			return;
		}
		foreach (UIRegionListItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (RegionData region in Session.MyPlayerData.regionList)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIRegionListItem component = obj.GetComponent<UIRegionListItem>();
			component.Load(region);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		base.gameObject.SetActive(value: true);
	}

	private void OnItemClicked(UIItem item)
	{
		UIQuickTravel.FilterRegion = ((UIRegionListItem)item).region.ID;
		UIQuickTravel.Show();
	}

	protected override void Destroy()
	{
		if (WorldMap.instance != null)
		{
			Object.Destroy(WorldMap.instance.gameObject);
		}
		Session.MyPlayerData.AreasReceived -= MyPlayerData_AreasReceived;
		base.Destroy();
		instance = null;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQuickTravel : UIMenuWindow
{
	public UIScrollView scrollview;

	public UITable table;

	public UIGrid grid;

	public UILabel title;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	private static UIQuickTravel instance;

	public static int FilterRegion = 1;

	public static void Show()
	{
		if (instance == null)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/Maps"), UIManager.Instance.transform).GetComponent<UIQuickTravel>();
			instance.Init();
		}
		RegionData regionData = Session.MyPlayerData.regionList.Where((RegionData x) => x.ID == FilterRegion).FirstOrDefault();
		instance.title.text = ((regionData != null) ? regionData.Name : "Battleon");
		instance.InitComplete();
	}

	public void InitComplete()
	{
		itemGOs = new List<UIItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		container = itemGOprefab.transform.parent;
		refresh();
	}

	private void refresh()
	{
		foreach (UIMapListItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (AreaData item in Session.MyPlayerData.areaList.Where((AreaData x) => x.regionID == FilterRegion).ToList())
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIMapListItem component = obj.GetComponent<UIMapListItem>();
			component.Load(item);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		base.gameObject.SetActive(value: true);
	}

	private void OnItemClicked(UIItem item)
	{
		UIMapListItem uIMapListItem = (UIMapListItem)item;
		if (uIMapListItem.IsAvailable())
		{
			AudioManager.Play2DSFX("UI_RespawnOrTeleport");
			Game.Instance.SendAreaJoinRequest(uIMapListItem.area.id);
		}
		else
		{
			Notification.ShowText("Explore the area to unlock it in travel menu.");
		}
	}
}

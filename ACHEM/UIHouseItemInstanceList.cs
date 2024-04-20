using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIHouseItemInstanceList : MonoBehaviour
{
	public UIViewport Viewport;

	public UIButton btnClose;

	public Transform ObjectMount;

	public Vector3 initialPosition;

	public UILabel title;

	public GameObject RowTemplate;

	public UIGrid ItemInstanceGrid;

	public UIScrollView scrollView;

	private InventoryItem iItem;

	private UIHouseItem hItem;

	private List<ComHouseItemListData> dataList = new List<ComHouseItemListData>();

	public void OnCloseClick(GameObject go)
	{
		Clear();
		base.gameObject.SetActive(value: false);
	}

	public void Clear()
	{
		if (dataList != null)
		{
			dataList.Clear();
		}
		ItemInstanceGrid.transform.DestroyChildren();
		ObjectMount.DestroyChildren();
	}

	private void OnDestroy()
	{
		Clear();
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		Session.MyPlayerData.OnHouseItemAdded += OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved += OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll += OnHouseItemClearAll;
		HousingManager.onHouseItemListRecieved = (Action<List<ComHouseItemListData>, int>)Delegate.Combine(HousingManager.onHouseItemListRecieved, new Action<List<ComHouseItemListData>, int>(OnHouseItemListResponse));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		Session.MyPlayerData.OnHouseItemAdded -= OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved -= OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll -= OnHouseItemClearAll;
		HousingManager.onHouseItemListRecieved = (Action<List<ComHouseItemListData>, int>)Delegate.Remove(HousingManager.onHouseItemListRecieved, new Action<List<ComHouseItemListData>, int>(OnHouseItemListResponse));
	}

	private void refresh()
	{
		ItemInstanceGrid.transform.DestroyChildren();
		List<ComHouseItemListData> list = new List<ComHouseItemListData>();
		int houseID = HousingManager.houseInstance.houseData.HouseID;
		list.AddRange(from x in dataList
			where x.HouseID == houseID
			orderby x.ID descending
			select x);
		list.AddRange(from x in dataList
			where x.HouseID != houseID
			orderby x.HouseID descending, x.ID
			select x);
		foreach (ComHouseItemListData item in list)
		{
			UIHouseItemRow component = UnityEngine.Object.Instantiate(RowTemplate).GetComponent<UIHouseItemRow>();
			component.Init(item);
			component.transform.SetParent(ItemInstanceGrid.transform, worldPositionStays: false);
		}
		ItemInstanceGrid.Reposition();
		scrollView.RestrictWithinBounds(instant: true);
	}

	private void OnHouseItemListResponse(List<ComHouseItemListData> listData, int itemID)
	{
		if (listData != null && itemID == iItem.ID)
		{
			dataList = listData;
			refresh();
		}
	}

	private void OnHouseItemAdded(ComHouseItem hItem)
	{
		if (iItem.ID == hItem.ItemID)
		{
			ComHouseItemListData comHouseItemListData = new ComHouseItemListData();
			comHouseItemListData.ID = hItem.ID;
			House houseInstance = HousingManager.houseInstance;
			comHouseItemListData.HouseID = houseInstance.houseData.HouseID;
			comHouseItemListData.ItemID = hItem.ItemID;
			dataList.Add(comHouseItemListData);
			refresh();
		}
	}

	private void OnHouseItemRemoved(int itemID, int houseItemID, int houseID)
	{
		if (iItem.ID == itemID)
		{
			ComHouseItemListData comHouseItemListData = dataList.Where((ComHouseItemListData x) => x.ID == houseItemID && x.HouseID == houseID).FirstOrDefault();
			if (comHouseItemListData != null)
			{
				dataList.Remove(comHouseItemListData);
				refresh();
			}
		}
	}

	private void OnHouseItemClearAll()
	{
		ItemCountUpdate();
	}

	private void ItemCountUpdate()
	{
	}

	public void SetAsset(GameObject asset)
	{
		GameObject obj = UnityEngine.Object.Instantiate(asset);
		obj.transform.SetParent(ObjectMount, worldPositionStays: false);
		obj.transform.rotation = asset.transform.rotation;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale *= 1.5f;
	}

	public void Show(UIHouseItem hItem)
	{
		Clear();
		iItem = hItem.iItem;
		this.hItem = hItem;
		AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseItemList, 0, iItem.ID));
		Viewport.sourceCamera = UIManager.Instance.uiCamera;
		title.text = "[" + iItem.RarityColor + "] " + iItem.Name;
		base.gameObject.SetActive(value: true);
	}
}

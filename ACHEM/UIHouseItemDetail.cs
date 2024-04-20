using System;
using UnityEngine;

public class UIHouseItemDetail : MonoBehaviour
{
	public InventoryItem iItem;

	public UIHouseItem hItem;

	public UIViewport Viewport;

	public UIButton btnClose;

	public UIButton btnPlace;

	public UIButton btnDragPlace;

	public UIButton btnInstanceList;

	public GameObject detailAnchor;

	public Transform ObjectMount;

	public Vector3 initialPosition;

	public UILabel title;

	public UILabel description;

	public UIHouseItemList inventory;

	public UIHouseItemInstanceList instanceList;

	public void OnInstanceListClick(GameObject obj)
	{
		instanceList.Show(hItem);
		base.gameObject.SetActive(value: false);
	}

	public void OnCloseClick(GameObject go)
	{
		Clear();
		base.gameObject.SetActive(value: false);
		detailAnchor.SetActive(value: false);
	}

	public void OnPlaceClick(GameObject go)
	{
		if (!HousingManager.houseInstance.IsEditing)
		{
			HousingManager.houseInstance.EnterEditMode();
		}
		if (iItem != null)
		{
			HousingManager.houseInstance.RequestAddStartPlacing(iItem.ID);
		}
		base.gameObject.SetActive(value: false);
	}

	private void OnDragPlaceStart(GameObject go)
	{
		if (!HousingManager.houseInstance.IsEditing)
		{
			HousingManager.houseInstance.EnterEditMode();
		}
		if (iItem != null)
		{
			HousingManager.houseInstance.StopPlacing();
			HousingManager.houseInstance.EnableDrag();
			HousingManager.houseInstance.RequestAddStartPlacing(iItem.ID);
		}
		base.gameObject.SetActive(value: false);
	}

	public void Clear()
	{
		instanceList.Clear();
		ObjectMount.DestroyChildren();
		instanceList.gameObject.SetActive(value: false);
		instanceList.ItemInstanceGrid.transform.DestroyChildren();
	}

	private void OnDestroy()
	{
		Clear();
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnPlace.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnPlaceClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnDragPlace.gameObject);
		uIEventListener3.onDragStart = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onDragStart, new UIEventListener.VoidDelegate(OnDragPlaceStart));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnInstanceList.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnInstanceListClick));
		Session.MyPlayerData.OnHouseItemAdded += OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved += OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll += OnHouseItemClearAll;
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnPlace.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnPlaceClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnDragPlace.gameObject);
		uIEventListener3.onDragStart = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onDragStart, new UIEventListener.VoidDelegate(OnDragPlaceStart));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnInstanceList.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnInstanceListClick));
		Session.MyPlayerData.OnHouseItemAdded -= OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved -= OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll -= OnHouseItemClearAll;
	}

	private void OnHouseItemAdded(ComHouseItem hItem)
	{
		if (iItem.ID == hItem.ItemID)
		{
			ItemCountUpdate();
		}
	}

	private void OnHouseItemRemoved(int itemID, int houseItemID, int houseID)
	{
		if (iItem.ID == itemID)
		{
			ItemCountUpdate();
		}
	}

	private void OnHouseItemClearAll()
	{
		ItemCountUpdate();
	}

	private void ItemCountUpdate()
	{
		int value;
		if (HousingManager.houseInstance == null || HousingManager.houseInstance.houseData.OwnerID != Entities.Instance.me.ID)
		{
			btnPlace.isEnabled = false;
			btnDragPlace.isEnabled = false;
			btnInstanceList.isEnabled = false;
		}
		else if (Session.MyPlayerData.HouseItemCounts.TryGetValue(iItem.ID, out value))
		{
			if (value >= iItem.Qty)
			{
				btnPlace.isEnabled = false;
				btnDragPlace.isEnabled = false;
			}
			else
			{
				btnPlace.isEnabled = true;
				btnDragPlace.isEnabled = true;
			}
		}
		else
		{
			btnPlace.isEnabled = true;
			btnDragPlace.isEnabled = true;
		}
	}

	public void SetAsset(GameObject asset)
	{
		GameObject obj = UnityEngine.Object.Instantiate(asset);
		obj.transform.SetParent(ObjectMount, worldPositionStays: false);
		obj.transform.rotation = asset.transform.rotation;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale *= 1.5f;
	}

	public void Show(UIHouseItem hItem, float yOffset, UIHouseItemList inventory)
	{
		Clear();
		this.inventory = inventory;
		iItem = hItem.iItem;
		this.hItem = hItem;
		ItemCountUpdate();
		Viewport.sourceCamera = UIManager.Instance.uiCamera;
		title.text = "[" + iItem.RarityColor + "] " + iItem.Name;
		description.text = iItem.GetDescription();
		detailAnchor.SetActive(value: true);
		base.gameObject.SetActive(value: true);
	}
}

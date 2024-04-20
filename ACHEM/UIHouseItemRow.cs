using System;
using System.Linq;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIHouseItemRow : MonoBehaviour
{
	public UILabel ItemText;

	public UILabel ServerName;

	public UIButton SelectBtn;

	public UIButton ItemRemoveBtn;

	private ComHouseItemListData rowData;

	public int HouseItemID => rowData.ID;

	public void Init(ComHouseItemListData rowData)
	{
		this.rowData = rowData;
		refresh();
		base.gameObject.SetActive(value: true);
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(SelectBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSelectBtnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(ItemRemoveBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnItemRemoveBtnClick));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(SelectBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSelectBtnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(ItemRemoveBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnItemRemoveBtnClick));
	}

	private void refresh()
	{
		HouseData houseData = Session.MyPlayerData.PersonalHouseData.Where((HouseData house) => house.HouseID == rowData.HouseID).First();
		ItemText.text = houseData.Name;
		ServerInfo serverInfo = ServerInfo.Servers.Where((ServerInfo server) => server.ID == houseData.ServerID).First();
		ServerName.text = serverInfo.Name;
		if (serverInfo.ID != AEC.getInstance().ServerID)
		{
			ItemRemoveBtn.gameObject.SetActive(value: false);
			ServerName.gameObject.SetActive(value: true);
		}
		else
		{
			ItemRemoveBtn.gameObject.SetActive(value: true);
			ServerName.gameObject.SetActive(value: false);
		}
	}

	private void OnSelectBtnClick(GameObject obj)
	{
		House houseInstance = HousingManager.houseInstance;
		if (houseInstance != null && houseInstance.houseData.HouseID == rowData.HouseID)
		{
			houseInstance.SelectItem(rowData.ID);
		}
	}

	private void OnItemRemoveBtnClick(GameObject obj)
	{
		AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseItemForceRemove, houseItemID: rowData.ID, houseID: rowData.HouseID, itemID: rowData.ItemID));
	}
}

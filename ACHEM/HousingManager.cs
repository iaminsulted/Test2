using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class HousingManager
{
	public static Action<List<ComHouseItemListData>, int> onHouseItemListRecieved;

	public static House houseInstance;

	public static IEnumerator HandleHouseJoin(List<ComHouseItem> hItems, Dictionary<int, HouseItemInfo> hItemInfos, Dictionary<int, int> hItemCounts, HouseData hData)
	{
		SelectionManager.Instance.DeselectAll();
		Clear();
		houseInstance = new GameObject("House").AddComponent<House>();
		houseInstance.Show(hData);
		if (hItems != null)
		{
			yield return houseInstance.AddItems(hItems, hItemInfos);
		}
		Session.MyPlayerData.SetHouseItemCounts(hItemCounts);
	}

	public static void Clear()
	{
		if (houseInstance != null)
		{
			UnityEngine.Object.Destroy(houseInstance.gameObject);
			houseInstance = null;
		}
	}

	public static void HandleHousingResponse(Response response)
	{
		switch ((Com.CmdHousing)response.cmd)
		{
		case Com.CmdHousing.HouseItemList:
		{
			ResponseHouseItemList responseHouseItemList = response as ResponseHouseItemList;
			onHouseItemListRecieved?.Invoke(responseHouseItemList.hItems, responseHouseItemList.ItemID);
			break;
		}
		case Com.CmdHousing.HouseExit:
			Confirmation.Show("Save House?", "There are unsaved modifications, would you like to save your house before exiting?", delegate(bool isYes)
			{
				if (isYes)
				{
					AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseSaveExit));
				}
				else
				{
					AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseForceExit));
				}
			});
			break;
		case Com.CmdHousing.HouseAdd:
		{
			ResponseHouseAdd responseHouseAdd = response as ResponseHouseAdd;
			Session.MyPlayerData.AddPersonalHouseData(new List<HouseData> { responseHouseAdd.HouseData });
			break;
		}
		case Com.CmdHousing.HouseData:
		{
			ResponseHouseData responseHouseData = response as ResponseHouseData;
			if (responseHouseData.Category == HouseDataCategory.PersonalHouseList)
			{
				if (responseHouseData.HouseData != null)
				{
					Session.MyPlayerData.AddPersonalHouseData(responseHouseData.HouseData.Values.ToList());
				}
				else
				{
					Session.MyPlayerData.AddPersonalHouseData(new List<HouseData>());
				}
			}
			else
			{
				Session.MyPlayerData.AddPublicHouseData(responseHouseData.HouseData, responseHouseData.Category, responseHouseData.ListVersion, responseHouseData.ServerListCount, responseHouseData.IsReversed);
			}
			break;
		}
		case Com.CmdHousing.HouseJoin:
		{
			ResponseHouseJoin responseHouseJoin = response as ResponseHouseJoin;
			HandleHouseJoin(responseHouseJoin.hItems, responseHouseJoin.hItemInfos, responseHouseJoin.hItemCounts, responseHouseJoin.hData);
			break;
		}
		case Com.CmdHousing.HouseItemAdd:
		{
			ResponseHouseItemAdd responseHouseItemAdd = response as ResponseHouseItemAdd;
			houseInstance?.AddItem(responseHouseItemAdd.hItem, responseHouseItemAdd.hItemInfo);
			Session.MyPlayerData.HouseItemAdd(responseHouseItemAdd.hItem);
			break;
		}
		case Com.CmdHousing.HouseItemMove:
		{
			ResponseHouseItemMove responseHouseItemMove = response as ResponseHouseItemMove;
			houseInstance?.MoveItem(responseHouseItemMove.ComMove);
			break;
		}
		case Com.CmdHousing.HouseItemRemove:
		{
			ResponseHouseItemRemove responseHouseItemRemove = response as ResponseHouseItemRemove;
			houseInstance?.RemoveItem(responseHouseItemRemove.HouseItemID, responseHouseItemRemove.HouseID);
			Session.MyPlayerData.HouseItemRemove(responseHouseItemRemove.ItemID, responseHouseItemRemove.HouseItemID, responseHouseItemRemove.HouseID);
			break;
		}
		case Com.CmdHousing.HouseItemClearAll:
		{
			ResponseHouseItemClearAll responseHouseItemClearAll = response as ResponseHouseItemClearAll;
			houseInstance?.Clear();
			Session.MyPlayerData.HouseItemClearAll(responseHouseItemClearAll.HouseItemCounts);
			break;
		}
		case Com.CmdHousing.HouseSave:
		case Com.CmdHousing.HouseUpdate:
		case Com.CmdHousing.HouseSaveExit:
		case Com.CmdHousing.HouseForceExit:
		case Com.CmdHousing.HouseQuest:
			break;
		}
	}
}

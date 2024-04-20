using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class UIDungeons : UIMenuWindow
{
	public GameObject itemGOprefab;

	public UIDungeonDetail detail;

	private static UIDungeons instance;

	private Transform container;

	private List<UIInventoryDungeon> itemGOs;

	private UIInventoryDungeon selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	public UIGrid grid;

	private List<int> DungeonIDs;

	public static void Load(List<int> dids)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/DungeonUI"), UIManager.Instance.transform).GetComponent<UIDungeons>();
			instance.Init();
		}
		instance.LoadDungeons(dids);
	}

	private void LoadDungeons(List<int> dids)
	{
		DungeonIDs = dids;
		List<int> list = DungeonIDs.Where((int p) => Dungeons.Get(p) == null).ToList();
		if (list.Count > 0)
		{
			StartCoroutine(LoadList(list));
		}
		else
		{
			refresh();
		}
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryDungeon>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		detail.Hide();
	}

	private IEnumerator LoadList(List<int> list)
	{
		string text = "";
		foreach (int item in list)
		{
			text = text + item + ",";
		}
		text = text.Substring(0, text.Length - 1);
		WWWForm form = new WWWForm();
		form.AddField("IDs", text);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/GetDungeons", form);
		string errorTitle = "Failed to load Dungeon List!";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "IDs: " + text;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
		}
		else if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
		}
		else if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
		}
		else
		{
			Debug.Log("WWW Ok!: " + www.downloadHandler.text);
			try
			{
				foreach (DungeonData value in JsonConvert.DeserializeObject<Dictionary<int, DungeonData>>(www.downloadHandler.text).Values)
				{
					Dungeons.Add(value);
				}
			}
			catch (Exception ex)
			{
				friendlyMsg = "Unable to process response from the server.";
				customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
			}
		}
		refresh();
	}

	public void refresh()
	{
		foreach (UIInventoryDungeon itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		foreach (DungeonData item in (from p in DungeonIDs
			select Dungeons.Get(p) into p
			where p != null
			select p).ToList())
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryDungeon component = obj.GetComponent<UIInventoryDungeon>();
			component.Init(item);
			component.Clicked += OnItemClicked;
			component.questIconOn(item);
			itemGOs.Add(component);
		}
		grid.repositionNow = true;
		if (itemGOs.Count > 0)
		{
			OnItemClicked(itemGOs[0]);
		}
	}

	private void RefreshDetail()
	{
		detail.Init(selectedItem.dungeon);
	}

	private void OnItemClicked(UIInventoryDungeon dungeon)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
		selectedItem = dungeon;
		selectedItem.Selected = true;
		RefreshDetail();
	}
}

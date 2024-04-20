using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class GearDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public GameObject GearPrefab;

	public void Load(Entity entity)
	{
		Clear();
		if (entity.isMe)
		{
			BuildList(((IEnumerable<Item>)Session.MyPlayerData.items.Where((InventoryItem x) => x.IsEquipped)).ToList());
		}
		else
		{
			if (!(entity is Player))
			{
				return;
			}
			List<EquipItem> list = entity.baseAsset.equips.Values.ToList();
			List<int> list2 = new List<int>();
			foreach (EquipItem item in list)
			{
				list2.Add(item.ID);
			}
			StartCoroutine(Load(list2));
		}
	}

	public void OnButtonClick()
	{
		isOpen = !isOpen;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		UpdateIcons();
		Grid.Reposition();
	}

	public void Clear()
	{
		foreach (GameObject content in Contents)
		{
			content.SetActive(value: false);
			UnityEngine.Object.Destroy(content.gameObject);
		}
		Contents.Clear();
		Grid.Reposition();
	}

	private void UpdateIcons()
	{
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
	}

	private void BuildList(List<Item> items)
	{
		items.Sort(delegate(Item item, Item item1)
		{
			int sortOrder = ItemTypes.GetSortOrder(item.Type);
			return ItemTypes.GetSortOrder(item1.Type).CompareTo(sortOrder);
		});
		foreach (Item item in items)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(GearPrefab, base.gameObject.transform.parent);
			gameObject.SetActive(value: true);
			gameObject.transform.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
			gameObject.GetComponent<UIInventoryItem>().Init(item);
			Contents.Add(gameObject);
		}
		Grid.Reposition();
	}

	private IEnumerator Load(List<int> ItemIds)
	{
		if (ItemIds.All((int item) => Items.Get(item) != null))
		{
			List<Item> items = ItemIds.Select(Items.Get).ToList();
			BuildList(items);
			yield break;
		}
		WWWForm form = new WWWForm();
		string[] array = new string[ItemIds.Count];
		for (int i = 0; i < ItemIds.Count; i++)
		{
			array[i] = ItemIds[i].ToString();
		}
		string text = string.Join(",", array);
		form.AddField("IDs", text);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/GetItems", form);
		string errorTitle = "Failed to load Char Item List!";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "ItemIds: " + text;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
		try
		{
			List<Item> list = JsonConvert.DeserializeObject<List<Item>>(www.downloadHandler.text);
			foreach (Item item in list)
			{
				Items.Add(item);
			}
			BuildList(list);
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
		}
	}
}

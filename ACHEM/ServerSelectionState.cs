using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSelectionState : State
{
	public GameObject itemGOprefab;

	public UIInput input;

	private Transform container;

	private List<UIItemServerInfo> itemGOs;

	public override void Init()
	{
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIItemServerInfo>();
		itemGOprefab.SetActive(value: false);
		LoadServerList();
	}

	private void LoadServerList()
	{
		Debug.Log("LoadServerList");
		BusyDialog.Show("Retrieving Server Information...");
		StartCoroutine(WaitForWebRequest());
	}

	private IEnumerator WaitForWebRequest()
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetServerList");
		string errorTitle = "Server Login Failed";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		BusyDialog.Close();
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		try
		{
			Debug.Log("WWW Ok!: " + www.downloadHandler.text);
			WebApiResponseServerList webApiResponseServerList = JsonConvert.DeserializeObject<WebApiResponseServerList>(www.downloadHandler.text);
			if (webApiResponseServerList.Success)
			{
				InitServerList(webApiResponseServerList.Servers);
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
	}

	private void InitServerList(List<ServerInfo> servers)
	{
		foreach (UIItemServerInfo itemGO in itemGOs)
		{
			itemGO.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (ServerInfo server in servers)
		{
			if (server.AccessLevel <= Session.Account.AccessLevel)
			{
				GameObject obj = UnityEngine.Object.Instantiate(itemGOprefab);
				obj.transform.SetParent(container, worldPositionStays: false);
				obj.SetActive(value: true);
				UIItemServerInfo component = obj.GetComponent<UIItemServerInfo>();
				component.Init(server);
				component.Clicked += OnItemClicked;
				itemGOs.Add(component);
			}
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnItemClicked(UIItemServerInfo si)
	{
		if (Main.EditorAutoLogin.ServerID > 0)
		{
			Main.EditorAutoLogin.ServerID = si.server.ID;
		}
		si.server.Save();
		OnBackClick();
	}

	public void OnBackClick()
	{
		StateManager.Instance.LoadState("scene.charselect");
	}
}

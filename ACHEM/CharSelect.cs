using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CharSelect : State
{
	public UILabel lblName;

	public UILabel CharacterLevel;

	public UILabel gold;

	public UILabel TotalClassRankNumber;

	public UISprite CharacterLevelSprite;

	public UIButton playBtn;

	public UIButton settings;

	public UIButton mc;

	public UIButton becomeGuardian;

	public UIButton charOptions;

	public UIButton serverSelect;

	public UIButton goToFriend;

	public UIButton deleteBtn;

	public UIButton backClick;

	public UIButton visitWebsite;

	public UIButton closeCharMenu;

	public UIButton reconnectYes;

	public UIButton reconnectNo;

	public UIButton news;

	private Player player;

	private PlayerAssetController assetController;

	public GameObject characterMenu;

	public GameObject reconnectPanel;

	public UILabel becomeGuardianLabel;

	public UILabel serverSelectLabel;

	public GameObject[] levelStars;

	public Color[] totalRankColors;

	private AEC aec;

	private ServerInfo selectedServer;

	private int hostNameIndex;

	private string[] hostNames;

	public override void Init()
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_StartedOk);
		base.Init();
		player = Session.Account.chars[0];
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_PlayerObjectFound);
		lblName.text = player.name;
		CharacterLevel.text = player.Level.ToString();
		UpdateRankIcon();
		if (player.TotalClassRank % 100 == 0)
		{
			TotalClassRankNumber.text = 100.ToString();
		}
		else
		{
			TotalClassRankNumber.text = (player.TotalClassRank % 100).ToString();
		}
		CharacterLevelSprite.spriteName = ((player.Level < Session.Account.LevelCap) ? "Icon-CircleGray" : "Icon-CircleGold");
		if (Session.IsGuest)
		{
			PlayerPrefs.SetString("GuestGuidCharName", player.name);
		}
		StartCoroutine(Load());
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(backClick.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBackClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(settings.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnSettingsClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(playBtn.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnPlayClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(charOptions.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnCharPageClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(visitWebsite.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnVisitWebsiteClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(closeCharMenu.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnCloseCharMenu));
		UIEventListener uIEventListener7 = UIEventListener.Get(serverSelect.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnServerSelect));
		UIEventListener uIEventListener8 = UIEventListener.Get(deleteBtn.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnDeleteClick));
		UIEventListener uIEventListener9 = UIEventListener.Get(goToFriend.gameObject);
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnGoToFriend));
		UIEventListener uIEventListener10 = UIEventListener.Get(becomeGuardian.gameObject);
		uIEventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener10.onClick, new UIEventListener.VoidDelegate(OnBecomeGuardianClick));
		UIEventListener uIEventListener11 = UIEventListener.Get(reconnectYes.gameObject);
		uIEventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener11.onClick, new UIEventListener.VoidDelegate(OnReconnectYes));
		UIEventListener uIEventListener12 = UIEventListener.Get(reconnectNo.gameObject);
		uIEventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener12.onClick, new UIEventListener.VoidDelegate(OnReconnectNo));
		UIEventListener uIEventListener13 = UIEventListener.Get(news.gameObject);
		uIEventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener13.onClick, new UIEventListener.VoidDelegate(OnNewsletterClick));
		aec = AEC.getInstance();
		aec.OnConnect += OnConnect;
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(backClick.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBackClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(settings.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnSettingsClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(playBtn.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnPlayClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(charOptions.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnCharPageClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(visitWebsite.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnVisitWebsiteClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(closeCharMenu.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnCloseCharMenu));
		UIEventListener uIEventListener7 = UIEventListener.Get(serverSelect.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnServerSelect));
		UIEventListener uIEventListener8 = UIEventListener.Get(deleteBtn.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnDeleteClick));
		UIEventListener uIEventListener9 = UIEventListener.Get(goToFriend.gameObject);
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnGoToFriend));
		UIEventListener uIEventListener10 = UIEventListener.Get(becomeGuardian.gameObject);
		uIEventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener10.onClick, new UIEventListener.VoidDelegate(OnBecomeGuardianClick));
		UIEventListener uIEventListener11 = UIEventListener.Get(reconnectYes.gameObject);
		uIEventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener11.onClick, new UIEventListener.VoidDelegate(OnReconnectYes));
		UIEventListener uIEventListener12 = UIEventListener.Get(reconnectNo.gameObject);
		uIEventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener12.onClick, new UIEventListener.VoidDelegate(OnReconnectNo));
		UIEventListener uIEventListener13 = UIEventListener.Get(news.gameObject);
		uIEventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener13.onClick, new UIEventListener.VoidDelegate(OnNewsletterClick));
		aec.OnConnect -= OnConnect;
	}

	private IEnumerator Load()
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_LoadServerListStarted);
		Loader.show("Retrieving Server Information...", 0f);
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetServerList");
		string errorTitle = "Loading Error";
		string friendlyMsg = "Failed to load server list.";
		string customContext = "URL : " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_Error_FailedToLoadedServerList);
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_Error_FailedToLoadedServerList);
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		if (www.error != null)
		{
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_Error_FailedToLoadedServerList);
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			yield break;
		}
		try
		{
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_LoadServerListCompleted);
			WebApiResponseServerList webApiResponseServerList = JsonConvert.DeserializeObject<WebApiResponseServerList>(www.downloadHandler.text);
			if (webApiResponseServerList.Success)
			{
				InitServerList(webApiResponseServerList.Servers);
			}
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_InitServerListOk);
			if ((Session.IsGuest || Session.AutoConnectServer) && selectedServer != null && selectedServer.State && !selectedServer.IsLocalhost)
			{
				OnPlayClick(null);
				yield break;
			}
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
		Loader.show("Loading scene...", 1f);
		AssetBundleLoader requestBundle = AssetBundleManager.LoadAssetBundle(Session.Account.CharSelectData.BundleInfo);
		yield return requestBundle;
		GameObject gameObject = requestBundle.Asset.LoadAsset<GameObject>(Session.Account.CharSelectData.EnvPrefab);
		if (gameObject != null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(gameObject, base.transform);
			obj.transform.localPosition = new Vector3(-0.75f, -1.05f, -6.75f);
			obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
			obj.AddComponent<GrassToggle>();
		}
		requestBundle.Dispose();
		ServerInfo serverInfo = ServerInfo.Servers.Where((ServerInfo p) => p.ID == Session.Account.chars[0].LastServerID).FirstOrDefault();
		if (serverInfo != null && serverInfo.State && Session.IsReconnectable && GameTime.ServerTime < Session.Account.chars[0].DisconnectTS)
		{
			ShowReconnectPanel();
		}
		Loader.close();
		GameObject gameObject2 = new GameObject("Player");
		gameObject2.transform.localPosition = new Vector3(-0.75f, -1.05f, -6.75f);
		gameObject2.transform.localRotation = Quaternion.Euler(0f, 150f, 0f);
		gameObject2.transform.SetParent(base.transform, worldPositionStays: false);
		gameObject2.AddComponent<DragRotateSlowDown>();
		gameObject2.layer = Layers.PLAYER_ME;
		assetController = gameObject2.AddComponent<PlayerAssetController>();
		assetController.Init(player.baseAsset);
		assetController.Load();
		assetController.AssetUpdated += AssetReady;
		gameObject2.transform.Find("orbGO/FlameParticle").GetComponent<Renderer>().material.renderQueue++;
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_PlayerAssetControllerLoadStarted);
		HandleAutoLogin();
	}

	private void AssetReady(GameObject _)
	{
	}

	private void InitServerList(List<ServerInfo> servers)
	{
		ServerInfo.Servers = servers;
		int serverID = ((Main.EditorAutoLogin.ServerID > 0) ? Main.EditorAutoLogin.ServerID : PlayerPrefs.GetInt("CurrentServerID"));
		selectedServer = servers.FirstOrDefault((ServerInfo p) => p.ID == serverID);
		if (selectedServer == null || !selectedServer.State || selectedServer.UserCount > selectedServer.MaxUsers || selectedServer.AccessLevel > Session.Account.AccessLevel)
		{
			PlayerPrefs.SetInt("CurrentServerID", 0);
			selectedServer = null;
			List<ServerInfo> list = servers.Where((ServerInfo p) => p.State && p.UserCount < p.MaxUsers && p.AccessLevel <= Session.Account.AccessLevel && p.IsLocalhost).ToList();
			selectedServer = list.Except(list.Where((ServerInfo p) => p.IsLocalhost)).FirstOrDefault((ServerInfo p) => p.Region == Session.Account.strRegion);
			if (selectedServer == null)
			{
				selectedServer = list.FirstOrDefault();
			}
		}
		if (selectedServer != null)
		{
			serverSelectLabel.text = selectedServer.DisplayName;
		}
		if (Session.pendingGoto != null)
		{
			Loader.show("Joining Friend's Server", 100f);
			GoSummon(Session.pendingGoto.Code);
		}
		if (Session.pendingServer != null)
		{
			GoServer(Session.pendingServer);
			Session.pendingServer = null;
		}
	}

	private void OnPlayClick(GameObject go)
	{
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_PlayClicked);
		if (selectedServer == null)
		{
			StateManager.Instance.LoadState("scene.serverselect");
		}
		else
		{
			Connect(selectedServer);
		}
	}

	private void OnReconnectYes(GameObject go)
	{
		reconnectPanel.SetActive(value: false);
		AudioManager.Play2DSFX("SFX_UI_Play");
		Connect(ServerInfo.Servers.FirstOrDefault((ServerInfo p) => p.ID == Session.Account.chars[0].LastServerID));
	}

	private void OnReconnectNo(GameObject go)
	{
		StopCoroutine(HideReconnectPanel());
		reconnectPanel.SetActive(value: false);
		Session.IsReconnectable = false;
		StartCoroutine(StopReconnect());
	}

	public void Connect(ServerInfo targetServer)
	{
		Loader.show("Connecting to server...", 0f);
		selectedServer = targetServer;
		selectedServer.Save();
		hostNameIndex = 0;
		hostNames = selectedServer.HostName.Split(',');
		ConnectToNextHost("Invalid HostName. Please try again later!");
	}

	private void ConnectToNextHost(string message)
	{
		if (hostNameIndex < hostNames.Length)
		{
			aec.connect(hostNames[hostNameIndex++], selectedServer.ServerPort, selectedServer.ID);
			return;
		}
		Loader.close();
		MessageBox.Show("Connection Failed!", message);
		Session.pendingGoto = null;
		Main.EditorAutoLogin.ServerID = 0;
	}

	public void OnConnect(string message)
	{
		if (message == "Success")
		{
			Loader.close();
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.CharSelect_ConnectOkAttemptLoadSceneGame);
			StateManager.Instance.LoadState("scene.game");
		}
		else
		{
			ConnectToNextHost(message);
		}
	}

	private void OnBecomeGuardianClick(GameObject go)
	{
		UIIAPStore.Show();
	}

	private void OnCharPageClick(GameObject go)
	{
		characterMenu.SetActive(value: true);
	}

	private void OnVisitWebsiteClick(GameObject go)
	{
		Confirmation.OpenUrl(Main.WebAccountURL + "/Character?id=" + player.name);
	}

	private void OnCloseCharMenu(GameObject go)
	{
		characterMenu.SetActive(value: false);
	}

	private void OnServerSelect(GameObject go)
	{
		StateManager.Instance.LoadState("scene.serverselect");
	}

	private void OnGoToFriend(GameObject go)
	{
		UIFriendJoin.Show(GoSummon);
	}

	private void OnDeleteClick(GameObject go)
	{
		DeleteCharacter();
	}

	private void DeleteCharacter()
	{
		ConfirmationTextField.Show("Confirmation", "Deleting the character is permanent and the current character name will not be available for a month. Are you sure you want to delete your character? Type DELETE below.", onDeleteConfirmed, "DELETE");
	}

	private void onDeleteConfirmed(bool b)
	{
		if (b)
		{
			Loader.show("Connecting to server...", 0f);
			StartCoroutine(DeleteCharacterCoroutine());
		}
	}

	private IEnumerator DeleteCharacterCoroutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("charid", Session.Account.chars[0].ID);
		form.AddField("strToken", Session.Account.strToken);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/CharDelete", form);
		string errorTitle = "Failed to Delete Character";
		string friendlyMsg = "Unable to communicate with the server. Please try again or contact support at " + Main.SupportURL;
		string customContext = "URL:" + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		Loader.close();
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
		friendlyMsg = "Unable to process response from the server. Please try again or contact support at " + Main.SupportURL;
		try
		{
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			if (aPIResponse.Error == null)
			{
				if (!int.TryParse(aPIResponse.Message, out var result))
				{
					yield break;
				}
				foreach (Player @char in Session.Account.chars)
				{
					if (@char.ID == result)
					{
						Session.Account.chars.Remove(@char);
						break;
					}
				}
				StateManager.Instance.LoadState("scene.charcreate");
				yield break;
			}
			if (aPIResponse.Error.Code == 1)
			{
				ErrorReporting.Instance.ReportError(errorTitle, aPIResponse.Error.Message, aPIResponse.Error.Message, null, customContext);
			}
			else
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, aPIResponse.Error.Message, null, customContext);
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext, ex);
		}
	}

	private void OnBackClick(GameObject go)
	{
		StateManager.Instance.LoadState("scene.login");
	}

	private void OnSettingsClick(GameObject go)
	{
		UISettings.Show();
	}

	private void GoSummon(string word)
	{
		if (!string.IsNullOrEmpty(word))
		{
			StartCoroutine(GoFriend(word));
		}
	}

	private IEnumerator GoFriend(string word)
	{
		Loader.show("Connecting to Friend's server", 0f);
		WWWForm form = new WWWForm();
		form.AddField("q", word);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/FindSummon", form);
		string errorTitle = "Failed to go to Friend";
		string friendlyMsg = "Your friend may be offline. Please try again later.";
		string customContext = "Word: " + word;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		Loader.close();
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
		try
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			if (aPIResponse.Error != null)
			{
				yield break;
			}
			string[] array = aPIResponse.Message.Split(',');
			if (array.Length > 1)
			{
				int.TryParse(array[0], out var result);
				int.TryParse(array[1], out var serverID);
				ServerInfo serverInfo = ServerInfo.Servers.FirstOrDefault((ServerInfo x) => x.ID == serverID || (serverID == -1 && x.IsLocalhost));
				if (serverInfo != null)
				{
					Session.pendingGoto = new GotoInfo(word);
					Session.pendingGoto.CharID = result;
					Connect(serverInfo);
					AudioManager.Play2DSFX("SFX_UI_GoToFriend");
				}
				else
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, "Invalid ServerID - " + serverID, form, customContext);
				}
			}
			else
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, "Invalid Server Response", form, customContext);
			}
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext);
		}
	}

	private void GoServer(ServerInfo si)
	{
		if (si != null)
		{
			Connect(si);
		}
	}

	public void UpdateRankIcon()
	{
		int num = player.TotalClassRank / 500;
		if (player.TotalClassRank % 500 < 101)
		{
			num--;
		}
		int num2 = (player.TotalClassRank - 100) / 100 % 5 + 1;
		if ((player.TotalClassRank - 100) / 100 % 5 == 0 && (player.TotalClassRank - 100) % 500 == 0)
		{
			num2 = 5;
		}
		if (player.TotalClassRank <= 100)
		{
			num2 = 0;
		}
		switch (num2)
		{
		case 1:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 2:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 3:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 4:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		case 5:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		default:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		}
		for (int i = 0; i < 5; i++)
		{
			if (levelStars[i].activeSelf)
			{
				levelStars[i].GetComponent<UISprite>().color = totalRankColors[num];
			}
		}
	}

	private void ShowReconnectPanel()
	{
		reconnectPanel.SetActive(value: true);
		StartCoroutine(HideReconnectPanel());
	}

	private IEnumerator HideReconnectPanel()
	{
		while (!(GameTime.ServerTime > Session.Account.chars[0].DisconnectTS))
		{
			yield return null;
		}
		OnReconnectNo(null);
	}

	private IEnumerator StopReconnect()
	{
		WebApiRequestStopReconnect request = new WebApiRequestStopReconnect(Session.Account.chars[0].ID, Session.Account.strToken);
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/StopReconnect", request.GetWWWForm());
		string errorTitle = "Failed to stop reconnect";
		string friendlyMsg = "Remote server could not be reached!";
		string customContext = "User ID: " + Session.Account.UserID;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, request.GetWWWForm(), customContext);
			yield break;
		}
		Debug.Log("WWW Ok!: " + www.downloadHandler.text);
	}

	private void HandleAutoLogin()
	{
		if (reconnectPanel.activeSelf)
		{
			if (Main.EditorAutoLogin.AutoReconnect)
			{
				OnReconnectYes(null);
			}
			else if (Main.EditorAutoLogin.SelectChar)
			{
				OnReconnectNo(null);
				OnPlayClick(null);
			}
		}
		else if (Main.EditorAutoLogin.SelectChar)
		{
			OnPlayClick(null);
		}
	}

	private void OnNewsletterClick(GameObject go)
	{
		WebView.OpenURL("https://aq3d.com/news/");
	}
}

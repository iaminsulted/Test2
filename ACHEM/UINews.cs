using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class UINews : UIFullscreenWindow
{
	public GameObject newsOptionGO;

	public UINewsListItem Instance;

	public UITexture apopImage;

	public UIButton btnClose;

	public UINewsAction newsAction;

	public UITable actionTable;

	public Transform container;

	private List<GameObject> itemGOs;

	private NPCIA rootNPCIA;

	private ObjectPool<GameObject> itemGOpool;

	private static UINews instance;

	private UINewsOption currentOption;

	public static string Title = "News";

	public static int NewsID;

	public static NewsState NewsState;

	public static int NewsVersion;

	public static byte NewsCooldown;

	public static NPCIA CurrentNPCIA;

	public UIScrollView Promos;

	public UISprite Alert;

	public UIScrollBar ScrollBar;

	public GameObject Spacer;

	public bool shownNews = true;

	public static void LoadNewsApop()
	{
		if (NewsID > 0 && CurrentNPCIA == null)
		{
			DialogueSlotManager.Initialized += CancelOrClose;
			ApopDownloader.GetApops(new List<int> { NewsID }, OnLoadEndHandler);
		}
	}

	public static void OnApopMapCleared()
	{
		CurrentNPCIA = null;
		LoadNewsApop();
	}

	private static void CancelOrClose()
	{
		DialogueSlotManager.Initialized -= CancelOrClose;
		if (instance != null)
		{
			instance.Close();
		}
	}

	private static void OnLoadEndHandler(List<NPCIA> loadedApops)
	{
		if (loadedApops == null || loadedApops.Count == 0)
		{
			Loader.close();
			return;
		}
		NPCIA apop = ApopMap.GetApop(NewsID);
		if (loadedApops[0].ID == NewsID && apop != null)
		{
			CurrentNPCIA = apop;
			if (CanShowNews() && IsAvailable())
			{
				UpdateNewsPrefs();
				LoadCurrentNews();
			}
			else
			{
				Loader.close();
			}
		}
	}

	public static bool CanShowNews()
	{
		if (!PlayerPrefs.HasKey("NewsVersion") || !PlayerPrefs.HasKey("NewsExpire"))
		{
			return true;
		}
		if (PlayerPrefs.GetInt("NewsVersion") < NewsVersion)
		{
			return true;
		}
		if (JsonConvert.DeserializeObject<DateTime>(PlayerPrefs.GetString("NewsExpire")) < DateTime.Now)
		{
			return true;
		}
		return false;
	}

	public static void UpdateNewsPrefs()
	{
		string value = JsonConvert.SerializeObject(DateTime.Now.AddHours((int)NewsCooldown));
		PlayerPrefs.SetString("NewsExpire", value);
		PlayerPrefs.SetInt("NewsVersion", NewsVersion);
		PlayerPrefs.Save();
	}

	public static bool IsAvailable()
	{
		if (NewsState == NewsState.Hidden || NewsID == 0 || CurrentNPCIA == null || !CurrentNPCIA.IsAvailable())
		{
			return false;
		}
		return CurrentNPCIA.children.Any((NPCIA npcia) => npcia.IsAvailable() || npcia.DontHideWhenLocked);
	}

	public static void LoadCurrentNews()
	{
		if (Loader.IsVisible)
		{
			Loader.close();
		}
		if (IsAvailable())
		{
			if (NewsState == NewsState.NewsUI)
			{
				LoadNews(CurrentNPCIA);
				return;
			}
			UINPCDialog.Load(new List<NPCIA> { CurrentNPCIA }, Title, null, null, clearWindows: false);
		}
	}

	public static void LoadNews(NPCIA Apop)
	{
		if (Apop != null)
		{
			foreach (NPCIA child in Apop.children)
			{
				if (child.IsAvailable() || child.DontHideWhenLocked)
				{
					UIWindow.ClearWindows();
					Load(Apop);
					return;
				}
			}
		}
		Loader.close();
	}

	public static void Load(NPCIA npcia)
	{
		NPCIA nPCIA = npcia.children.Where((NPCIA p) => p.IsTriggerAvailable()).FirstOrDefault();
		if (nPCIA != null)
		{
			if (nPCIA is NPCIAAction && ((NPCIAAction)nPCIA).Action is CTAAsyncCore)
			{
				CTAAsyncCore cTAAsyncCore = ((NPCIAAction)nPCIA).Action as CTAAsyncCore;
				if (cTAAsyncCore.OnCompleteActions != null && cTAAsyncCore.OnCompleteActions.Count == 0)
				{
					CTANPCIACore cTANPCIACore = new CTANPCIACore();
					npcia.Actions.Add(cTANPCIACore);
					cTANPCIACore.Title = Title;
					cTANPCIACore.Apops = new List<NPCIA> { npcia };
					cTAAsyncCore.OnCompleteActions.Add(cTANPCIACore);
				}
			}
			Session.Set(nPCIA.GetSessionDataID(), 0f);
			Load(nPCIA);
			return;
		}
		foreach (IARequiredCore requirement in npcia.Requirements)
		{
			if (!requirement.IsRequirementMet(Session.MyPlayerData))
			{
				return;
			}
		}
		if (npcia is NPCIAAction)
		{
			((NPCIAAction)npcia).Action.Execute();
		}
		else if (npcia is NPCIAShop)
		{
			UIShop.LoadShop(((NPCIAShop)npcia).ShopID);
		}
		else if (npcia is NPCIAQuest)
		{
			UIQuest.ShowQuests(((NPCIAQuest)npcia).QuestIDs, ((NPCIAQuest)npcia).QuestIDs);
		}
		else if (npcia is NPCIATalk)
		{
			LoadDialog((NPCIATalk)npcia);
		}
	}

	private static void LoadDialog(NPCIATalk npciatalk)
	{
		if (instance == null)
		{
			CreateInstance();
		}
		instance.LoadTalk(npciatalk);
	}

	private static void CreateInstance()
	{
		instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/NewsUI"), UIManager.Instance.transform).GetComponent<UINews>();
		instance.Init();
	}

	protected override void Init()
	{
		base.Init();
		container = newsOptionGO.transform.parent;
		itemGOs = new List<GameObject>();
		itemGOpool = new ObjectPool<GameObject>(newsOptionGO);
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		Session.MyPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnQuestObjectiveUpdated(int questID, int qoID)
	{
		List<NPCIA> list = rootNPCIA.children.Where((NPCIA p) => p.IsAvailable()).ToList();
		if (list.Count > 0 && list[0].ApopState == ApopState.QuestTurnin)
		{
			Load(list[0]);
		}
		else
		{
			refresh();
		}
	}

	protected override void Destroy()
	{
		NPCCamController.Close();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
		DialogueSlotManager.Initialized -= CancelOrClose;
		base.Destroy();
	}

	private void LoadTalk(NPCIATalk npciatalk)
	{
		if (rootNPCIA != npciatalk)
		{
			rootNPCIA = npciatalk;
			List<NPCIA> list = npciatalk.children.Where((NPCIA p) => p.IsAvailable()).ToList();
			if (list.Count > 0 && list[0].ApopState == ApopState.QuestTurnin)
			{
				Load(list[0]);
			}
			else
			{
				refresh();
			}
		}
	}

	protected override void Resume()
	{
		base.Resume();
		refresh();
	}

	private void refresh()
	{
		btnClose.gameObject.SetActive(value: true);
		apopImage.gameObject.SetActive(value: false);
		newsAction.gameObject.SetActive(value: false);
		currentOption = null;
		foreach (GameObject itemGO in itemGOs)
		{
			itemGO.GetComponent<UINewsOption>().Clicked -= OnOptionClicked;
			itemGO.GetComponent<UINewsOption>().Clicked -= OnOptionWithImageClicked;
			itemGO.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO);
		}
		itemGOs.Clear();
		InstanceObjects();
		EventDelegate.Add(ScrollBar.onChange, CheckAlertVis);
	}

	private void CheckAlertVis()
	{
		if (ScrollBar.value > 0.85f)
		{
			Alert.gameObject.SetActive(value: false);
		}
		else
		{
			Alert.gameObject.SetActive(value: true);
		}
	}

	private void OnChildActionClicked(NPCIA npcia)
	{
		Load(npcia);
		Close();
	}

	private void OnOptionClicked(UINewsOption si)
	{
		Load(si.npcia);
		Close();
	}

	private void OnOptionWithImageClicked(UINewsOption si)
	{
		if (currentOption != null)
		{
			currentOption.label.color = Color.white;
			currentOption.selected.SetActive(value: false);
		}
		si.selected.SetActive(value: true);
		si.label.color = new Color32(250, 222, 138, byte.MaxValue);
		StartCoroutine(ShowApopImage(si));
	}

	private IEnumerator ShowApopImage(UINewsOption si)
	{
		yield return null;
		if (currentOption == si)
		{
			yield break;
		}
		currentOption = si;
		if (!Loader.IsVisible)
		{
			BusyDialog.Show("Downloading image...");
		}
		if (si.npcia.ImageUrl != apopImage.name)
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + si.npcia.ImageUrl);
			string errorTitle = "Load Error";
			string friendlyMsg = "Failed to load news image: " + si.npcia.ImageUrl;
			yield return www.SendWebRequest();
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error);
			}
			else
			{
				try
				{
					Texture content = DownloadHandlerTexture.GetContent(www);
					UnityEngine.Object.DestroyImmediate(apopImage.mainTexture, allowDestroyingAssets: true);
					apopImage.width = Mathf.CeilToInt((float)apopImage.height * (float)content.width / (float)content.height);
					apopImage.mainTexture = content;
					apopImage.name = si.npcia.ImageUrl;
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message);
				}
			}
		}
		if (BusyDialog.IsVisible)
		{
			BusyDialog.Close();
		}
		newsAction.gameObject.SetActive(value: false);
		int i = 1;
		for (int childCount = actionTable.transform.childCount; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(actionTable.transform.GetChild(i).gameObject);
		}
		if (si.npcia is NPCIATalk)
		{
			foreach (NPCIA child in si.npcia.children)
			{
				bool active = true;
				if (!child.DontHideWhenLocked)
				{
					foreach (IARequiredCore requirement in child.Requirements)
					{
						if (!requirement.IsRequirementMet(Session.MyPlayerData))
						{
							active = false;
							break;
						}
					}
				}
				UINewsAction component = UnityEngine.Object.Instantiate(newsAction, newsAction.transform.parent).GetComponent<UINewsAction>();
				component.Label.text = child.Label;
				component.Button.GetComponent<UISprite>().UpdateAnchors();
				component.Button.onClick.Clear();
				component.Button.onClick.Add(new EventDelegate(delegate
				{
					OnChildActionClicked(child);
				}));
				component.gameObject.SetActive(active);
			}
		}
		else
		{
			newsAction.Label.text = si.npcia.Label;
			newsAction.Button.GetComponent<UISprite>().UpdateAnchors();
			newsAction.Button.onClick.Clear();
			newsAction.Button.onClick.Add(new EventDelegate(delegate
			{
				OnOptionClicked(si);
			}));
			newsAction.gameObject.SetActive(value: true);
		}
		container.gameObject.SetActive(value: true);
		btnClose.gameObject.SetActive(value: true);
		apopImage.gameObject.SetActive(value: true);
		actionTable.Reposition();
		yield return null;
		Loader.close();
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(apopImage.mainTexture);
	}

	private IEnumerator ShowApopImageNew(NPCIA npcia, UITexture apopImage)
	{
		if (npcia.ImageUrl != apopImage.name)
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + npcia.ImageUrl);
			string errorTitle = "Load Error";
			string friendlyMsg = "Failed to load news image: " + npcia.ImageUrl;
			yield return www.SendWebRequest();
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error);
			}
			else
			{
				try
				{
					Texture content = DownloadHandlerTexture.GetContent(www);
					UnityEngine.Object.DestroyImmediate(apopImage.mainTexture, allowDestroyingAssets: true);
					apopImage.width = Mathf.CeilToInt((float)apopImage.height * (float)content.width / (float)content.height);
					apopImage.mainTexture = content;
					apopImage.name = npcia.ImageUrl;
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message);
				}
			}
		}
		if (BusyDialog.IsVisible)
		{
			BusyDialog.Close();
		}
		int i = 1;
		for (int childCount = actionTable.transform.childCount; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(actionTable.transform.GetChild(i).gameObject);
		}
		if (npcia is NPCIATalk)
		{
			foreach (NPCIA child in npcia.children)
			{
				if (child.DontHideWhenLocked)
				{
					continue;
				}
				using List<IARequiredCore>.Enumerator enumerator2 = child.Requirements.GetEnumerator();
				while (enumerator2.MoveNext() && enumerator2.Current.IsRequirementMet(Session.MyPlayerData))
				{
				}
			}
		}
		container.gameObject.SetActive(value: true);
		btnClose.gameObject.SetActive(value: true);
		apopImage.gameObject.SetActive(value: true);
		actionTable.Reposition();
		yield return null;
		Loader.close();
	}

	public void InstanceObjects()
	{
		int i = 0;
		for (int count = rootNPCIA.children.Count; i < count; i++)
		{
			NPCIA nPCIA = rootNPCIA.children[i];
			if (nPCIA is NPCIATalk)
			{
				((NPCIATalk)nPCIA).parent = rootNPCIA;
			}
			if (nPCIA.IsAvailable() || nPCIA.DontHideWhenLocked)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(newsOptionGO, container);
				Instance = gameObject.GetComponent<UINewsListItem>();
				Instance.init(nPCIA);
			}
		}
		newsOptionGO.SetActive(value: false);
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		container.GetComponent<UIGrid>().Reposition();
		UnityEngine.Object.Instantiate(Spacer, container);
		Spacer.SetActive(value: false);
	}
}

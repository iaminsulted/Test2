using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class UINPCDialog : UIMenuWindow
{
	public UILabel lblDialog;

	public UILabel lblTitle;

	public UILabel lblSubtitle;

	public GameObject editableButton;

	public GameObject editableInputField;

	public GameObject saveButton;

	public GameObject dialogArrow;

	public GameObject itemGOprefab;

	public UITexture apopImage;

	public UINewsAction newsAction;

	public UIButton btnCloseImage;

	public GameObject progressBar;

	public UILabel lblImageTitle;

	public UILabel lblImageDesc;

	public UILabel lblProgress;

	public GameObject loader;

	public UITable imagePanelTable;

	public static Action<NPCIA> OnLoad;

	private Transform container;

	private List<GameObject> itemGOs;

	private string defaultTitle;

	private ObjectPool<GameObject> itemGOpool;

	private Transform camT;

	private Transform focusT;

	private UINPCDialogOption currentOption;

	private Texture2D ApopImage;

	private bool editable;

	private NPCIATalk currentTalk { get; set; }

	private NPCIATalk rootTalk { get; set; }

	public static UINPCDialog Instance { get; private set; }

	public static void Load(List<NPCIA> Apops, string title, Transform camT, Transform focusT, bool clearWindows = true)
	{
		if (Apops == null || Apops.Count <= 0)
		{
			return;
		}
		foreach (NPCIA Apop in Apops)
		{
			if (!Apop.IsAvailable())
			{
				continue;
			}
			if (clearWindows)
			{
				UIWindow.ClearWindows();
			}
			NPCIA nPCIA = Apop.children.FirstOrDefault((NPCIA p) => p.IsTriggerAvailable());
			if (nPCIA != null)
			{
				CTAAsyncCore cTAAsyncCore = (nPCIA as NPCIAAction)?.Action as CTAAsyncCore;
				if (cTAAsyncCore?.OnCompleteActions != null && cTAAsyncCore.OnCompleteActions.Count == 0)
				{
					CTANPCIACore cTANPCIACore = new CTANPCIACore();
					Apop.Actions.Add(cTANPCIACore);
					cTANPCIACore.CameraSpot = camT;
					cTANPCIACore.FocusSpot = focusT;
					cTANPCIACore.Title = title;
					cTANPCIACore.Apops = new List<NPCIA> { Apop };
					cTAAsyncCore.OnCompleteActions.Add(cTANPCIACore);
				}
				Session.Set(nPCIA.GetSessionDataID(), 0f);
				Load(nPCIA, title, camT, focusT);
			}
			else
			{
				Load(Apop, title, camT, focusT);
			}
			break;
		}
	}

	private static void Load(NPCIA npcia, string title, Transform camT, Transform focusT)
	{
		OnLoad?.Invoke(npcia);
		ApopMap.Cleared += closeInstance;
		if (npcia is NPCIAAction nPCIAAction)
		{
			if (nPCIAAction.Action is CTATransferMapCore cTATransferMapCore && cTATransferMapCore.MapID == Game.Instance.AreaData.id)
			{
				Instance.Close();
			}
			nPCIAAction.Action.Execute();
		}
		else if (npcia is NPCIAShop nPCIAShop)
		{
			UIShop.LoadShop(nPCIAShop.ShopID);
		}
		else if (npcia is NPCIAQuest nPCIAQuest)
		{
			UIQuest.ShowQuests(nPCIAQuest.QuestIDs, nPCIAQuest.QuestIDs);
		}
		else if (npcia is NPCIATalk npciatalk)
		{
			LoadDialog(npciatalk, title, camT, focusT);
		}
	}

	private static void closeInstance()
	{
		ApopMap.Cleared -= closeInstance;
		Instance.Close();
	}

	private static void LoadDialog(NPCIATalk npciatalk, string title, Transform camT, Transform focusT)
	{
		if (Instance == null)
		{
			CreateInstance(camT, focusT);
			Instance.rootTalk = npciatalk;
		}
		Instance.LoadTalk(npciatalk, title, camT, focusT);
	}

	public void toggleEditable()
	{
		if (Session.MyPlayerData.AccessLevel > 99)
		{
			editable = !editable;
			refreshEditable();
		}
	}

	private void LoadTalk(NPCIATalk npciatalk, string title, Transform camT, Transform focusT)
	{
		currentTalk = npciatalk;
		defaultTitle = title;
		this.camT = camT;
		this.focusT = focusT;
		List<NPCIA> list = npciatalk.children.Where((NPCIA p) => p.IsAvailable()).ToList();
		NPCIA nPCIA = list.FirstOrDefault((NPCIA p) => p.ApopState == ApopState.QuestTurnin);
		if (nPCIA != null)
		{
			Load(nPCIA, title, camT, focusT);
			return;
		}
		if (string.IsNullOrEmpty(npciatalk.Text) && list.Count == 1)
		{
			if (list[0] is NPCIAQuest)
			{
				Load(list[0], title, camT, focusT);
				return;
			}
			_ = (list[0] as NPCIAAction)?.Action;
		}
		refresh();
	}

	private static void CreateInstance(Transform camT, Transform focusT)
	{
		Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/NPCDialog"), UIManager.Instance.transform).GetComponent<UINPCDialog>();
		Instance.Init();
		NPCCamController.BeginNPCPush(camT, focusT, NPCCamController.MASK_BASE | (1 << Layers.NPCS));
	}

	protected override void Init()
	{
		base.Init();
		container = itemGOprefab.transform.parent;
		itemGOs = new List<GameObject>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		if (Session.MyPlayerData.AccessLevel < 100)
		{
			editableButton.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		Session.MyPlayerData.QuestStateUpdated += OnQuestStateUpdated;
	}

	private void OnDisable()
	{
		Session.MyPlayerData.QuestStateUpdated -= OnQuestStateUpdated;
	}

	private void OnQuestStateUpdated()
	{
		LoadTalk(currentTalk, defaultTitle, camT, focusT);
	}

	public override void OnBackClick(GameObject go)
	{
		Back();
	}

	public override void Back()
	{
		if (currentTalk != rootTalk && currentTalk.parent != null)
		{
			BackToTalk(currentTalk.parent as NPCIATalk, defaultTitle);
		}
		else
		{
			base.Back();
		}
	}

	protected override void Destroy()
	{
		AudioManager.PlayNpcFarewell(Entities.Instance.me.interactingNPC);
		Entities.Instance.me.EndNPCInteraction();
		NPCCamController.Close();
		base.Destroy();
	}

	private void BackToTalk(NPCIATalk npciatalk, string title)
	{
		if (npciatalk == null)
		{
			base.Back();
			return;
		}
		currentTalk = npciatalk;
		defaultTitle = title;
		List<NPCIA> list = npciatalk.children.Where((NPCIA p) => p.IsAvailable()).ToList();
		if (list.Count == 1)
		{
			if (list[0] is NPCIAQuest)
			{
				Back();
				return;
			}
			if (list[0] is NPCIAAction)
			{
				_ = ((NPCIAAction)list[0]).Action;
			}
		}
		refresh();
	}

	protected override void Resume()
	{
		base.Resume();
		BackToTalk(currentTalk, defaultTitle);
	}

	private void refresh()
	{
		apopImage.transform.parent.gameObject.SetActive(value: false);
		if (string.IsNullOrEmpty(currentTalk.Text))
		{
			lblDialog.gameObject.SetActive(value: false);
			dialogArrow.SetActive(value: false);
		}
		else
		{
			lblDialog.gameObject.SetActive(value: true);
			lblDialog.text = "[222222]" + Entities.Instance.me.ScrubText(currentTalk.Text) + "[-]";
			while (lblDialog.height < 60)
			{
				lblDialog.text += "\n";
			}
			dialogArrow.SetActive(value: true);
		}
		lblTitle.text = (string.IsNullOrEmpty(currentTalk.Title) ? defaultTitle : currentTalk.Title);
		lblSubtitle.text = currentTalk.Subtitle;
		foreach (GameObject itemGO in itemGOs)
		{
			itemGO.GetComponent<UINPCDialogOption>().Clicked -= OnLockedOptionClicked;
			itemGO.GetComponent<UINPCDialogOption>().Clicked -= OnOptionClicked;
			itemGO.GetComponent<UINPCDialogOption>().Clicked -= OnOptionWithImageClicked;
			itemGO.GetComponent<UINPCDialogOption>().Clicked -= OnBackClicked;
			itemGO.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO);
		}
		itemGOs.Clear();
		bool flag = true;
		foreach (NPCIA child in currentTalk.children)
		{
			if (!child.IsAvailable() && !child.DontHideWhenLocked)
			{
				continue;
			}
			GameObject gameObject = itemGOpool.Get();
			gameObject.transform.SetParent(container, worldPositionStays: false);
			gameObject.SetActive(value: true);
			itemGOs.Add(gameObject);
			UINPCDialogOption component = gameObject.GetComponent<UINPCDialogOption>();
			component.npcia = child;
			component.title = defaultTitle;
			if ((bool)component)
			{
				float num = Mathf.Min(Mathf.Round((float)Session.MyPlayerData.GetQSValue(child.sagaID) / (float)Session.MyPlayerData.QuestStrings[child.sagaID].MaxValue * 100f), 100f);
				if (child.sagaID <= 0)
				{
					component.buttonTitle.text = "[000000]" + child.CurrentLabel + "[-]";
				}
				else
				{
					string text = null;
					text = ((num <= 0f) ? new Color32(133, 133, 133, byte.MaxValue).ToBBCode() : ((!(num >= 100f)) ? new Color32(50, 50, 50, byte.MaxValue).ToBBCode() : new Color32(14, 107, 0, byte.MaxValue).ToBBCode()));
					string text2 = ((num > 0f) ? (num + "% Quest Complete") : "Quest Not Started");
					component.buttonTitle.text = "[000000]" + child.CurrentLabel + "[-] \n" + text + text2 + "[-]";
				}
			}
			component.icon.spriteName = child.Icon;
			ApopState apopState = child.ApopState;
			if (apopState == ApopState.Talk)
			{
				component.GuideIcon.gameObject.SetActive(value: false);
			}
			else if (child.Icon == "Icon-Quest" || child.Icon == "Icon-QuestTurnin")
			{
				component.GuideIcon.gameObject.SetActive(value: false);
			}
			else
			{
				component.GuideIcon.gameObject.SetActive(value: true);
			}
			switch (apopState)
			{
			case ApopState.QuestAvailable:
				component.GuideIcon.color = Color.white;
				component.GuideIcon.spriteName = "Icon-Quest";
				break;
			case ApopState.QuestTurnin:
				component.GuideIcon.color = Color.white;
				component.GuideIcon.spriteName = "Icon-QuestTurnin";
				break;
			case ApopState.QuestObjective:
				component.GuideIcon.color = Color.white;
				component.GuideIcon.spriteName = "Icon-QuestObjective";
				break;
			case ApopState.LockedQuestAvailable:
				component.GuideIcon.color = Color.gray;
				component.GuideIcon.spriteName = "Icon-Quest";
				break;
			case ApopState.LockedQuestTurnin:
				component.GuideIcon.color = Color.gray;
				component.GuideIcon.spriteName = "Icon-QuestTurnin";
				break;
			case ApopState.LockedQuestObjective:
				component.GuideIcon.color = Color.gray;
				component.GuideIcon.spriteName = "Icon-QuestObjective";
				break;
			}
			if (child.IsAvailable())
			{
				if (child.ImageUrl == "")
				{
					component.Clicked += OnOptionClicked;
				}
				else
				{
					component.npcia.ImageTitle = child.ImageTitle;
					component.npcia.ImageDesc = child.ImageDesc;
					component.Clicked += OnOptionWithImageClicked;
				}
				component.pulse.enabled = child.ApopState == ApopState.QuestTurnin;
				if (flag && child.ImageUrl != "")
				{
					flag = false;
					OnOptionWithImageClicked(component);
				}
				component.Lock.enabled = false;
				component.buttonTitle.color = Color.white;
			}
			else
			{
				component.Lock.enabled = true;
				component.buttonTitle.color = Color.gray;
				component.Clicked += OnLockedOptionClicked;
			}
		}
		if (currentTalk.parent != null)
		{
			GameObject gameObject2 = itemGOpool.Get();
			gameObject2.transform.SetParent(container, worldPositionStays: false);
			gameObject2.SetActive(value: true);
			itemGOs.Add(gameObject2);
			UINPCDialogOption component2 = gameObject2.GetComponent<UINPCDialogOption>();
			component2.npcia = currentTalk.parent;
			component2.title = defaultTitle;
			component2.buttonTitle.text = "Back";
			component2.icon.spriteName = "Icon-Chat";
			component2.GuideIcon.gameObject.SetActive(value: false);
			component2.Lock.enabled = false;
			component2.Clicked += OnBackClicked;
			component2.pulse.enabled = false;
			component2.buttonTitle.color = Color.black;
		}
		lblDialog.transform.GetComponentInChildren<UISprite>().ResetAndUpdateAnchors();
		container.GetComponent<UITable>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		currentOption = null;
	}

	private void OnBackClicked(UINPCDialogOption si)
	{
		Back();
	}

	private void OnLockedOptionClicked(UINPCDialogOption si)
	{
		string message = (string.IsNullOrEmpty(si.npcia.RequirementsText) ? "You do not meet the requirements!" : si.npcia.RequirementsText);
		MessageBox.Show("Locked", message);
	}

	private void OnOptionClicked(UINPCDialogOption si)
	{
		Load(si.npcia, defaultTitle, camT, focusT);
	}

	private void OnOptionClicked(NPCIA incoming)
	{
		Load(incoming, defaultTitle, camT, focusT);
	}

	private void OnOptionWithImageClicked(UINPCDialogOption si)
	{
		StartCoroutine(ShowApopImage(si));
	}

	private IEnumerator ShowApopImage(UINPCDialogOption si)
	{
		yield return null;
		if (currentOption == si)
		{
			yield break;
		}
		currentOption = si;
		apopImage.mainTexture = null;
		apopImage.enabled = false;
		apopImage.name = "Blank";
		if (si.npcia is NPCIATalk)
		{
			newsAction.gameObject.SetActive(value: true);
			newsAction.Label.text = si.npcia.children[0].Label;
			newsAction.Button.GetComponent<UISprite>().UpdateAnchors();
			newsAction.Button.onClick.Clear();
			newsAction.Button.onClick.Add(new EventDelegate(delegate
			{
				OnOptionClicked(si.npcia.children[0]);
			}));
		}
		btnCloseImage.onClick.Clear();
		btnCloseImage.onClick.Add(new EventDelegate(delegate
		{
			currentOption = null;
			apopImage.transform.parent.gameObject.SetActive(value: false);
		}));
		apopImage.transform.parent.gameObject.SetActive(value: true);
		progressBar.SetActive(value: false);
		if (!string.IsNullOrEmpty(si.npcia.ImageUrl))
		{
			if (si.npcia.sagaID > 0)
			{
				UISlider component = progressBar.GetComponent<UISlider>();
				float b = (float)Session.MyPlayerData.GetQSValue(si.npcia.sagaID) / (float)Session.MyPlayerData.QuestStrings[si.npcia.sagaID].MaxValue;
				b = (component.value = Mathf.Min(1f, b));
				lblProgress.text = Mathf.Round(b * 100f) + "% Completed";
			}
			if (!string.IsNullOrEmpty(currentOption.npcia.ImageTitle))
			{
				lblImageTitle.gameObject.SetActive(value: true);
				lblImageTitle.text = currentOption.npcia.ImageTitle;
			}
			else
			{
				lblImageTitle.gameObject.SetActive(value: false);
			}
			if (!string.IsNullOrEmpty(currentOption.npcia.ImageDesc))
			{
				lblImageDesc.gameObject.SetActive(value: true);
				lblImageDesc.text = currentOption.npcia.ImageDesc;
			}
			else
			{
				lblImageDesc.gameObject.SetActive(value: false);
			}
		}
		_ = Loader.IsVisible;
		loader.SetActive(value: true);
		if (si.npcia.ImageUrl != apopImage.name)
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + si.npcia.ImageUrl);
			string errorTitle = "Load Error";
			string friendlyMsg = "Failed to menu image: " + si.npcia.ImageUrl;
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
					if (ApopImage != null)
					{
						UnityEngine.Object.Destroy(ApopImage);
					}
					ApopImage = DownloadHandlerTexture.GetContent(www);
					apopImage.mainTexture = ApopImage;
					apopImage.name = si.npcia.ImageUrl;
					float aspectRatio = (float)ApopImage.width / (float)ApopImage.height;
					apopImage.aspectRatio = aspectRatio;
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message);
				}
			}
		}
		loader.SetActive(value: false);
		apopImage.enabled = true;
		progressBar.SetActive(si.npcia.sagaID > 0 && !string.IsNullOrEmpty(si.npcia.ImageUrl));
		imagePanelTable.Reposition();
	}

	private void OnDestroy()
	{
		if (ApopImage != null)
		{
			UnityEngine.Object.Destroy(ApopImage);
		}
	}

	private void refreshEditable()
	{
		editableInputField.SetActive(editable);
		saveButton.SetActive(editable);
		if (editable)
		{
			editableInputField.GetComponent<UIInput>().value = lblDialog.text;
		}
	}

	public void sendSaveRequest()
	{
		Game.Instance.SendApopUpdateRequest(currentTalk.ID, editableInputField.GetComponent<UIInput>().value);
		Close();
	}
}

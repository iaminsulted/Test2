using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class UIPreviewLoot : UIMenuWindow
{
	private static UIPreviewLoot instance;

	private int ShopId;

	private List<ChestButton> UIChestButtons = new List<ChestButton>();

	public GameObject UIChestButtonPrefab;

	public GameObject ChestListGO;

	public UIGrid ChestListTable;

	public GameObject NextItemButton;

	public UILabel LabelItemCount;

	public UILabel ShopTitle;

	private List<LootBoxRewardItem> lootItems;

	private int CurrentItemIndex;

	public UILabel ItemName;

	public UILabel ChestTitle;

	public UILabel ChestCost;

	public GameObject OpenChestUI;

	public UISprite ItemIcon;

	public UISprite ItemIconFg;

	public UISprite PreviewIcon;

	public UISprite PreviewIconFg;

	public UISprite QtyItemIcon;

	public UISprite QtyItemIconFg;

	public GameObject TopText;

	public GameObject PreviewParentGO;

	public GameObject PreviewIconGO;

	public GameObject PreviewWindowGO;

	public GameObject Menu;

	public GameObject ShopMenu;

	public GameObject Loader;

	public UIButton OpenButtonNGUI;

	public UIButton BuyButtonNGUI;

	private GameObject previewItem;

	public GameObject Duplicate;

	public UISprite ChestTokenSprite;

	public UILabel DuplicateQtyLabel;

	public UILabel LabelTokenQty;

	public GameObject DCPreview;

	public UILabel DCAmountLabel;

	public UILabel ItemQtyLabel;

	public GameObject GoDCPile;

	public GameObject DailyButtonGlow;

	public UILabel DailyButtonTimer;

	private GameObject zoomCamera;

	private Camera camMain;

	private GameObject playerGO;

	private ChestButton CurrentSelection;

	private AssetController assetController;

	private DateTime EndDate;

	[Header("Loot Details")]
	public UIItemDetails UIItemDetail;

	public GameObject itemGOprefab;

	public GameObject ChestDetailGO;

	public UITable lootDetailsUITable;

	public UILabel lblLootboxTitle;

	public UILabel LabelLootboxDesc;

	private List<Item> localLootItems;

	private Transform detailsContainer;

	private List<UIInventoryItem> itemGOs;

	private UIItem selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	public GameObject ChestPrefabJunk;

	public GameObject ChestPrefabCommon;

	public GameObject ChestPrefabUncommon;

	public GameObject ChestPrefabRare;

	public GameObject ChestPrefabEpic;

	public GameObject ChestPrefabLegendary;

	private Dictionary<RarityType, Chest> Chests = new Dictionary<RarityType, Chest>();

	private Chest CurrentChest;

	private Animator playerAnimator;

	private Dictionary<int, int> chestQty;

	private int DCAmount;

	private bool DCDisplayed;

	private GameObject rootGo3D;

	private ParticleSystem[] ParticleSystemsArray;

	private GameObject RewardGlowParticles;

	private Shop shop;

	private static Color32 White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 50);

	private static Color32 Green = new Color32(96, byte.MaxValue, 197, byte.MaxValue);

	private static Color32 Blue = new Color32(96, 189, byte.MaxValue, byte.MaxValue);

	private static Color32 Purple = new Color32(129, 88, 216, byte.MaxValue);

	private static Color32 Purple2 = new Color32(117, 42, 178, byte.MaxValue);

	private static Color32 Gold = new Color32(249, 242, 99, byte.MaxValue);

	private static Color32 Gold2 = new Color32(byte.MaxValue, 176, 20, byte.MaxValue);

	protected void OnEnable()
	{
		Game.Instance.ShopLoaded += ShopDataLoaded;
		Game.Instance.ReceivedLoot += OnReceivedLoot;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
		Session.MyPlayerData.ItemRemoved += OnItemUpdated;
		Session.MyPlayerData.ItemAdded += OnItemUpdated;
		Session.MyPlayerData.DailyRewardUpdated += OnDailyRewardUpdated;
	}

	protected void OnDisable()
	{
		if (Game.Instance != null)
		{
			Game.Instance.ShopLoaded -= ShopDataLoaded;
			Game.Instance.ReceivedLoot -= OnReceivedLoot;
		}
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
			Session.MyPlayerData.ItemRemoved -= OnItemUpdated;
			Session.MyPlayerData.ItemAdded -= OnItemUpdated;
			Session.MyPlayerData.DailyRewardUpdated -= OnDailyRewardUpdated;
		}
	}

	private void OnItemUpdated(InventoryItem obj)
	{
		UpdateChestQty();
	}

	private void OnDailyRewardUpdated(int day, DateTime date, int itemID)
	{
		EndDate = Session.MyPlayerData.RewardDate;
	}

	public static void LoadShop(int shopID = 114)
	{
		if (Entities.Instance.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot access this feature during combat");
			return;
		}
		if (Entities.Instance.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot access this feature while dead");
			return;
		}
		if (Entities.Instance.me.serverState == Entity.State.Interacting)
		{
			Notification.ShowText("Cannot access this feature while interacting");
			return;
		}
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/LootChestScreen"), UIManager.Instance.transform).GetComponent<UIPreviewLoot>();
			instance.Init();
		}
		instance.Load(shopID);
	}

	private void Load(int shopID)
	{
		ShopId = shopID;
		camMain = Camera.main;
		camMain.enabled = false;
		Game.Instance.DisableControls();
		UIGame.Instance.gameObject.SetActive(value: false);
		detailsContainer = itemGOprefab.transform.parent;
		itemGOs = new List<UIInventoryItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		AudioManager.Play2DSFX("sfx_engine_equip");
		RewardGlowParticles = UnityEngine.Object.Instantiate(SpellFXContainer.mInstance.LoadAsset("ChestOpen"), OpenChestUI.transform);
		RewardGlowParticles.transform.localPosition = new Vector3(0f, 0f, 2000f);
		RewardGlowParticles.SetLayerRecursively(LayerMask.NameToLayer("NGUI"));
		ParticleSystemsArray = RewardGlowParticles.GetComponentsInChildren<ParticleSystem>();
		RewardGlowParticles.SetActive(value: false);
		EndDate = Session.MyPlayerData.RewardDate;
		InvokeRepeating("CheckDailyTimer", 0f, 1f);
		lblGold.transform.parent.gameObject.SetActive(value: false);
		int num = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == 1742).Sum((InventoryItem x) => x.Qty);
		LabelTokenQty.text = num.ToString();
		LabelTokenQty.parent.GetComponent<UIItemTooltip>().SetItem(Items.Get(1742));
		OpenChestUI.SetActive(value: false);
		Menu.SetActive(value: false);
		ShopMenu.SetActive(value: false);
		UIChestButtonPrefab.SetActive(value: false);
		PreviewIconGO.SetActive(value: false);
		Duplicate.SetActive(value: false);
		Transform transform = Entities.Instance.me.wrapper.transform;
		rootGo3D = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LootBox"), transform.position, transform.rotation);
		zoomCamera = rootGo3D.transform.Find("ZoomCamera").gameObject;
		zoomCamera.GetComponent<Camera>().farClipPlane = Game.Instance.CurrentCell.CameraFarPlane;
		playerGO = new GameObject("PlayerLoot");
		playerGO.layer = Layers.CUTSCENE;
		playerGO.transform.SetParent(rootGo3D.transform, worldPositionStays: false);
		assetController = playerGO.AddComponent<PlayerAssetController>();
		EntityAsset entityAsset = new EntityAsset(Entities.Instance.me.baseAsset);
		entityAsset.WeaponRequired = EquipItemSlot.None;
		entityAsset.DualWield = false;
		entityAsset.equips.Remove(EquipItemSlot.Weapon);
		assetController.Init(entityAsset);
		zoomCamera.transform.SetPositionAndRotation(camMain.transform.position, camMain.transform.rotation);
		iTween.MoveTo(zoomCamera, rootGo3D.transform.Find("LookAtShop").position, 1f);
		iTween.RotateTo(zoomCamera, rootGo3D.transform.Find("LookAtShop").rotation.eulerAngles, 1f);
		StartCoroutine(LoadCharacter());
	}

	public static void OpenChest(int itemID)
	{
		if (instance != null)
		{
			instance.OpenChestByID(itemID);
		}
	}

	private void CheckDailyTimer()
	{
		if (Session.MyPlayerData.RewardDate < GameTime.ServerTime)
		{
			DailyButtonGlow.SetActive(value: true);
		}
		else
		{
			DailyButtonGlow.SetActive(value: false);
		}
		TimeSpan timeSpan = EndDate - GameTime.ServerTime;
		if (timeSpan.TotalSeconds <= 0.0)
		{
			DailyButtonTimer.text = "ready!";
			CancelInvoke("Tick");
			return;
		}
		DailyButtonTimer.text = "[FFC800]" + timeSpan.Hours + "h " + timeSpan.Minutes + "m " + timeSpan.Seconds + "s[-]";
	}

	public void refreshLootDetails()
	{
		foreach (UIInventoryItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnLootItemClicked;
		}
		itemGOs.Clear();
		detailsContainer.parent.GetComponent<UIScrollView>().ResetPosition();
		foreach (Item localLootItem in localLootItems)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(detailsContainer, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryItem component = obj.GetComponent<UIInventoryItem>();
			component.Init(localLootItem);
			component.Clicked += OnLootItemClicked;
			itemGOs.Add(component);
		}
		lootDetailsUITable.Reposition();
	}

	private void OnLootItemClicked(UIItem si)
	{
		if (selectedItem != null)
		{
			if (selectedItem == si)
			{
				if (!UIItemDetail.Visible)
				{
					UIItemDetail.LoadInventoryItem(selectedItem.Item);
				}
				else if (si.Item.HasPreview)
				{
					UIPreview.Show(selectedItem.Item);
				}
				selectedItem.Background.spriteName = "aq3dui-menuslot-selected";
				return;
			}
			selectedItem.Selected = false;
		}
		selectedItem = si;
		selectedItem.Selected = true;
		UIItemDetail.LoadInventoryItem(selectedItem.Item);
	}

	private IEnumerator LoadCharacter()
	{
		yield return assetController.LoadAsset();
		playerAnimator = playerGO.GetComponentInChildren<Animator>();
		Game.Instance.SendShopLoadRequest(ShopId);
	}

	private void ShopDataLoaded(Shop shop, string txt)
	{
		if (ShopId == shop.ID && shop.Type == ShopType.Shop)
		{
			LoadShop(shop);
		}
	}

	private void LoadShop(Shop shop)
	{
		ChestListGO.SetActive(value: true);
		ChestDetailGO.SetActive(value: false);
		this.shop = shop;
		ShopTitle.text = shop.Name;
		List<Item> shopChestTypes = shop.Items.Where((ShopItem p) => p.IsVisibleInStore()).Cast<Item>().ToList();
		shopChestTypes = shopChestTypes.Concat(Session.MyPlayerData.items.Where((InventoryItem p) => p.Type == ItemType.Chest && !shopChestTypes.Any((Item s) => s.ID == p.ID)).Cast<Item>()).ToList();
		for (int i = 0; i < shopChestTypes.Count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(UIChestButtonPrefab, ChestListTable.transform);
			obj.SetActive(value: true);
			ChestButton component = obj.GetComponent<ChestButton>();
			component.Init(shopChestTypes[i]);
			UIChestButtons.Add(component);
		}
		UpdateChestQty();
		ChestListTable.Reposition();
		ShopMenu.SetActive(value: true);
		Loader.SetActive(value: false);
	}

	private void UpdateChestQty()
	{
		int num = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == 1742).Sum((InventoryItem x) => x.Qty);
		LabelTokenQty.text = num.ToString();
		Dictionary<int, int> dictionary = Session.MyPlayerData.items.Where((InventoryItem p) => p.Type == ItemType.Chest).GroupBy((InventoryItem p) => p.ID, (int key, IEnumerable<InventoryItem> g) => new
		{
			ID = key,
			Qty = g.Sum((InventoryItem p) => p.Qty)
		}).ToDictionary(p => p.ID, p => p.Qty);
		foreach (ChestButton uIChestButton in UIChestButtons)
		{
			uIChestButton.SetQty(dictionary.ContainsKey(uIChestButton.ItemChest.ID) ? dictionary[uIChestButton.ItemChest.ID] : 0);
		}
		if (CurrentSelection != null)
		{
			ChestTitle.text = "[" + GetChestColor(CurrentSelection.ItemChest) + "]" + CurrentSelection.ItemChest.Name.ToUpper() + " x " + CurrentSelection.Qty + "[-]";
			OpenButtonNGUI.isEnabled = CurrentSelection.Qty > 0;
		}
	}

	public void Buy()
	{
		ConfirmationSlider.ConfirmBuy(CurrentSelection.ItemChest as ShopItem, ConfirmationSlider.ActionType.Buy, ConfirmationCallbackStackBuy);
	}

	private void ConfirmationCallbackStackBuy(bool confirm, int qty)
	{
		if (confirm)
		{
			Game.Instance.SendBuyRequest(ShopId, CurrentSelection.ItemChest.ID, qty);
		}
	}

	public void OpenDailyRewardsScreen()
	{
		UIDailyRewards.Load(this);
	}

	public void SelectChest(ChestButton curitem)
	{
		if (CurrentSelection == curitem)
		{
			return;
		}
		if (CurrentSelection != null)
		{
			CurrentSelection.Selected(isSelected: false);
		}
		if (curitem == null)
		{
			playerAnimator.Play("NoAction");
			CurrentChest.gameObject.SetActive(value: false);
			Menu.SetActive(value: false);
			return;
		}
		CurrentSelection = curitem;
		CurrentSelection.Selected(isSelected: true);
		Menu.SetActive(value: true);
		ChestTitle.text = "[" + GetChestColor(curitem.ItemChest) + "]" + curitem.ItemChest.Name.ToUpper() + " x " + curitem.Qty + "[-]";
		ChestCost.text = curitem.ItemChest.Cost.ToString();
		ChestCost.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
		OpenButtonNGUI.isEnabled = CurrentSelection.Qty > 0;
		ShopItem shopItem = shop.Items.Where((ShopItem p) => p.ID == curitem.ItemChest.ID).FirstOrDefault();
		BuyButtonNGUI.gameObject.SetActive(shopItem != null && !shopItem.IsNotBuyable && Session.Account.strCountryCode != "BE");
		BuyButtonNGUI.transform.parent.GetComponent<UITable>().Reposition();
		LoadCurrentChest();
	}

	private string GetChestColor(Item chest)
	{
		return chest.Rarity switch
		{
			RarityType.Common => "FFFFFF", 
			RarityType.Rare => "1F87C6", 
			RarityType.Epic => "8239D4", 
			_ => chest.RarityColor, 
		};
	}

	private void LoadCurrentChest()
	{
		if (CurrentSelection != null)
		{
			LoadChest(CurrentSelection.ItemChest);
			StartCoroutine(PlayChestEnter(CurrentSelection.ItemChest.Rarity));
		}
	}

	private void LoadChest(Item item)
	{
		if (CurrentChest != null)
		{
			CurrentChest.gameObject.SetActive(value: false);
			CurrentChest = null;
		}
		if (!Chests.ContainsKey(item.Rarity))
		{
			GameObject obj = UnityEngine.Object.Instantiate(item.Rarity switch
			{
				RarityType.Junk => ChestPrefabJunk, 
				RarityType.Common => ChestPrefabCommon, 
				RarityType.Uncommon => ChestPrefabUncommon, 
				RarityType.Rare => ChestPrefabRare, 
				RarityType.Epic => ChestPrefabEpic, 
				RarityType.Legendary => ChestPrefabLegendary, 
				_ => ChestPrefabCommon, 
			});
			obj.transform.SetParent(rootGo3D.transform, worldPositionStays: false);
			Chest component = obj.GetComponent<Chest>();
			component.Init();
			Chests[item.Rarity] = component;
		}
		CurrentChest = Chests[item.Rarity];
		CurrentChest.gameObject.SetActive(value: true);
	}

	public void OnPreviewClick(Item item)
	{
		UIPreview.Show(item);
	}

	public void OpenChest()
	{
		OpenChestByID(CurrentSelection.ItemChest.ID);
	}

	private void OpenChestByID(int itemID)
	{
		LoadChest(Items.Get(itemID));
		PlayChestOpenLoop();
		Game.Instance.aec.sendRequest(new RequestOpenLootBox(itemID));
		OpenChestUI.SetActive(value: true);
		NextItemButton.SetActive(value: false);
		TopText.SetActive(value: false);
		Menu.SetActive(value: false);
		ShopMenu.SetActive(value: false);
		zoomCamera.transform.SetPositionAndRotation(rootGo3D.transform.Find("LookAtOpen").position, rootGo3D.transform.Find("LookAtOpen").rotation);
	}

	public void ShowCurrentChestDetail()
	{
		if (!(CurrentSelection == null))
		{
			ShowChestDetail(CurrentSelection);
		}
	}

	public void ShowChestDetail(ChestButton curitem)
	{
		SelectChest(curitem);
		StartCoroutine(WaitForLootInfo(curitem.ItemChest.ID));
	}

	public IEnumerator WaitForLootInfo(int itemID)
	{
		WWWForm form = new WWWForm();
		form.AddField("IDs", itemID.ToString());
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/getlootboxdata", form);
		string errorTitle = "Failed to load Loot Info!";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "URL: " + www.url;
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
		try
		{
			ApiResponseLootBoxData apiResponseLootBoxData = JsonConvert.DeserializeObject<ApiResponseLootBoxData>(www.downloadHandler.text);
			if (!apiResponseLootBoxData.items.ContainsKey(itemID))
			{
				MessageBox.Show("Error", "LootBox data could not be loaded.");
			}
			else
			{
				LoadLoot(apiResponseLootBoxData.Name, apiResponseLootBoxData.Rarity, apiResponseLootBoxData.Info, apiResponseLootBoxData.items[itemID]);
			}
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
		}
	}

	public void LoadLoot(string name, int rarity, string info, List<Item> LootItems)
	{
		ShopTitle.text = "Chest Detail";
		lblLootboxTitle.text = "[" + Item.GetRarityColor((RarityType)rarity) + "]" + name + "[-]";
		LabelLootboxDesc.text = info;
		localLootItems = (from p in LootItems
			orderby p.Rarity descending, p.MaxStack > 1, p.Type == ItemType.ClassToken || p.Type == ItemType.Token
			select p).ToList();
		UIItemDetail.Visible = false;
		ChestListGO.SetActive(value: false);
		ChestDetailGO.SetActive(value: true);
		refreshLootDetails();
	}

	public void LootShowcaseError(Exception ex = null, string message = null)
	{
		if (message != null)
		{
			Debug.LogError(ex);
		}
		if (ex != null)
		{
			Debug.LogException(ex);
		}
		StopCoroutine("ShowNextItem");
		ExitChestUI();
		MessageBox.Show("Showcase Failed", "The chest showcase failed. All item rewards have been automatically added to your inventory.");
		ErrorReporting.Instance.ReportError("Lootbox_showcase_error", "Lootbox showcase error", ex.Message);
	}

	public void OnReceivedLoot(List<LootBoxRewardItem> newLootItems, int DC)
	{
		newLootItems.Reverse();
		lootItems = newLootItems;
		CurrentItemIndex = 0;
		DCDisplayed = false;
		DCAmount = DC;
		StartCoroutine("ShowNextItem");
	}

	public void OnNextItemClick()
	{
		StartCoroutine(OnNextItemCoroutine());
	}

	public IEnumerator OnNextItemCoroutine()
	{
		StopParticles();
		OpenChestUI.GetComponent<Animation>().Play("OpenChestUIExit");
		yield return new WaitForSeconds(0.5f);
		StopCoroutine("ShowNextItem");
		StartCoroutine("ShowNextItem");
	}

	public void ExitChestUI()
	{
		playerAnimator.Play("NoAction");
		CurrentChest.gameObject.SetActive(value: false);
		RewardGlowParticles.SetActive(value: false);
		CurrentItemIndex = 0;
		OpenChestUI.SetActive(value: false);
		Menu.SetActive(CurrentSelection != null);
		ShopMenu.SetActive(value: true);
		zoomCamera.transform.SetPositionAndRotation(rootGo3D.transform.Find("LookAtShop").position, rootGo3D.transform.Find("LookAtShop").rotation);
	}

	public IEnumerator ShowNextItem()
	{
		try
		{
			OpenChestUI.GetComponent<Animation>()["OpenChestUIExit"].normalizedTime = 0f;
			OpenChestUI.GetComponent<Animation>().Sample();
			OpenChestUI.GetComponent<Animation>().Stop();
			NextItemButton.SetActive(value: true);
			UnityEngine.Object.Destroy(previewItem);
			PreviewIconGO.SetActive(value: false);
			PreviewWindowGO.SetActive(value: false);
			Duplicate.SetActive(value: false);
			DCPreview.gameObject.SetActive(value: false);
		}
		catch (Exception ex)
		{
			LootShowcaseError(ex);
		}
		if (DCAmount > 0 && !DCDisplayed)
		{
			yield return ShowDCReward();
			DCDisplayed = true;
		}
		else if (lootItems != null && CurrentItemIndex < lootItems.Count)
		{
			yield return ShowBundleRewardItem(lootItems[CurrentItemIndex], CurrentItemIndex);
		}
		else
		{
			ExitChestUI();
		}
	}

	private IEnumerator ShowDCReward()
	{
		try
		{
			LabelItemCount.text = "1/" + (lootItems.Count + 1);
		}
		catch (Exception ex)
		{
			LootShowcaseError(ex);
		}
		PlayChestOpenLoop();
		yield return new WaitForSeconds(0.3f);
		PlayChestOpen(RarityType.Rare);
		yield return new WaitForSeconds(0.15f);
		try
		{
			ItemName.text = "[4D87DB]Dragon Crystals[-]";
			ItemIcon.spriteName = "DragonGem";
			ItemIconFg.spriteName = "";
			DCPreview.gameObject.SetActive(value: true);
			DCAmountLabel.text = "+" + DCAmount;
			DCAmountLabel.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
			iTween.ScaleFrom(GoDCPile, new Vector3(0f, 0f, 0f), 1f);
			PlayParticles(RarityType.Rare);
			TopText.SetActive(value: true);
		}
		catch (Exception ex2)
		{
			LootShowcaseError(ex2);
		}
		yield return new WaitForSeconds(0.5f);
	}

	private IEnumerator ShowBundleRewardItem(LootBoxRewardItem CurrentItem, int itemIndex)
	{
		CurrentItemIndex++;
		if (CurrentItem == null)
		{
			LootShowcaseError(null, "CurrentItem is null");
		}
		try
		{
			ItemName.text = "[" + CurrentItem.RarityColor + "]" + CurrentItem.Name;
			if (CurrentItem.Qty > 1)
			{
				UILabel itemName = ItemName;
				itemName.text = itemName.text + " x " + CurrentItem.Qty;
			}
			ItemName.text += "[-]";
			LabelItemCount.text = ((DCAmount > 0) ? (CurrentItemIndex + 1 + "/" + (lootItems.Count + 1)) : (CurrentItemIndex + "/" + lootItems.Count));
			ItemIcon.spriteName = CurrentItem.Icon;
			ItemIconFg.gameObject.SetActive(!string.IsNullOrEmpty(CurrentItem.IconFg));
			ItemIconFg.spriteName = CurrentItem.IconFg;
			TopText.SetActive(value: true);
		}
		catch (Exception ex)
		{
			LootShowcaseError(ex);
		}
		PlayChestOpenLoop();
		if (CurrentItem.HasPreview)
		{
			try
			{
				PreviewWindowGO.SetActive(value: true);
			}
			catch (Exception ex2)
			{
				LootShowcaseError(ex2);
			}
			EntityAsset entityAsset = null;
			if (CurrentItem.TravelParams != null)
			{
				if ((Entities.Instance?.me?.baseAsset?.gender ?? null) == null)
				{
					LootShowcaseError(null, "Entities.Instance.me error");
				}
				if (CurrentItem.TravelParams == null)
				{
					LootShowcaseError(null, "Travel Params are null");
				}
				int npcid = ((Entities.Instance.me.baseAsset.gender == "M") ? CurrentItem.TravelParams.mNpcID : CurrentItem.TravelParams.fNpcID);
				using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetAllNPCAssetData?idtosplit=" + npcid);
				string errorTitle = "Failed to Load Travel Form";
				string friendlyMsg = "Unable to communicate with the server.";
				string customContext = "npcid: " + npcid;
				yield return www.SendWebRequest();
				customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
				if (www.isHttpError)
				{
					ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
					LootShowcaseError();
					yield break;
				}
				if (www.isNetworkError)
				{
					ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
					LootShowcaseError();
					yield break;
				}
				if (www.error != null)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
					LootShowcaseError();
					yield break;
				}
				try
				{
					entityAsset = JsonConvert.DeserializeObject<Dictionary<int, EntityAsset>>(www.downloadHandler.text)[npcid];
				}
				catch (Exception ex3)
				{
					customContext = "Invalid NPC Data: " + customContext;
					friendlyMsg = "Unable to process response from the server.";
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex3.Message, null, customContext);
					LootShowcaseError(ex3, "Unable to process response from the server.");
					yield break;
				}
			}
			else if (CurrentItem.Type == ItemType.Pet)
			{
				try
				{
					entityAsset = new EntityAsset
					{
						prefab = CurrentItem.AssetName,
						bundle = CurrentItem.bundle,
						gender = "N"
					};
				}
				catch (Exception ex4)
				{
					LootShowcaseError(ex4);
				}
			}
			else
			{
				try
				{
					entityAsset = new EntityAsset(Entities.Instance.me.baseAsset);
					entityAsset.equips[CurrentItem.EquipSlot] = CurrentItem;
					if (CurrentItem.IsWeapon)
					{
						entityAsset.WeaponRequired = CurrentItem.EquipSlot;
						entityAsset.DualWield = false;
					}
				}
				catch (Exception ex5)
				{
					LootShowcaseError(ex5);
				}
			}
			AssetController previewAssetController = null;
			try
			{
				previewItem = new GameObject();
				previewItem.layer = LayerMask.NameToLayer("NGUI");
				previewItem.transform.SetParent(PreviewParentGO.transform, worldPositionStays: false);
				previewAssetController = ((entityAsset.gender == "N") ? ((AssetController)previewItem.AddComponent<NPCAssetController>()) : ((AssetController)previewItem.AddComponent<PlayerAssetController>()));
				previewAssetController.showorb = false;
				previewAssetController.Init(entityAsset);
			}
			catch (Exception ex6)
			{
				LootShowcaseError(ex6);
			}
			yield return previewAssetController.LoadAsset();
			Renderer[] renderers = null;
			try
			{
				if (!string.IsNullOrEmpty(previewAssetController.Error))
				{
					MessageBox.Show("Load Failed", "Item preview could not be loaded. All item rewards have been automatically added to your inventory.", delegate
					{
						OnNextItemClick();
					});
					yield break;
				}
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				renderers = previewItem.GetComponentsInChildren<Renderer>();
				Renderer[] array = renderers;
				foreach (Renderer obj in array)
				{
					obj.GetPropertyBlock(materialPropertyBlock);
					materialPropertyBlock.SetFloat("_Probe", 0f);
					obj.SetPropertyBlock(materialPropertyBlock);
				}
				previewItem.gameObject.SetActive(value: false);
			}
			catch (Exception ex7)
			{
				LootShowcaseError(ex7);
			}
			PlayChestOpen(CurrentItem.Rarity);
			yield return new WaitForSeconds(0.15f);
			try
			{
				previewItem.gameObject.SetActive(value: true);
				PreviewParentGO.transform.parent.localScale = Vector3.one * 180f;
				PreviewParentGO.transform.parent.localPosition = new Vector3(0f, -180f, -120f);
				PreviewParentGO.transform.localPosition = Vector3.zero;
				PreviewParentGO.transform.localRotation = Quaternion.identity;
			}
			catch (Exception ex8)
			{
				LootShowcaseError(ex8);
			}
			if (CurrentItem.TravelParams != null || CurrentItem.Type == ItemType.Pet)
			{
				try
				{
					SkinnedMeshRenderer[] componentsInChildren = previewItem.GetComponentsInChildren<SkinnedMeshRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].updateWhenOffscreen = true;
					}
				}
				catch (Exception ex9)
				{
					LootShowcaseError(ex9);
				}
				yield return new WaitForEndOfFrame();
				try
				{
					Bounds bounds = default(Bounds);
					Renderer[] array = renderers;
					foreach (Renderer renderer in array)
					{
						if (renderer != null)
						{
							bounds.Encapsulate(renderer.bounds);
						}
					}
					float num = 1f;
					Transform transform = PreviewParentGO.transform.FindChildRecursive("Root");
					if (transform != null)
					{
						num = 1f;
						transform.localScale = Vector3.one;
					}
					PreviewParentGO.transform.parent.localScale = Vector3.one * 180f / num;
					PreviewParentGO.transform.parent.localScale /= bounds.size.z / 5f;
					PreviewParentGO.transform.parent.localPosition = Vector3.zero;
					Vector3 size = bounds.size;
					size = Vector3.Normalize(size);
					if (size.z < 0.9f)
					{
						size = new Vector3(size.x, size.y, size.z * 2f);
					}
					PreviewParentGO.transform.localPosition = new Vector3(PreviewParentGO.transform.localPosition.x, 0f - size.z, PreviewParentGO.transform.localPosition.z);
					PreviewParentGO.transform.localPosition += Vector3.forward;
					PreviewParentGO.transform.GetChild(0).GetChild(0).localPosition = Vector3.zero;
				}
				catch (Exception ex10)
				{
					LootShowcaseError(ex10);
				}
			}
			else
			{
				try
				{
					switch (CurrentItem.EquipSlot)
					{
					case EquipItemSlot.Belt:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckBelt");
						break;
					case EquipItemSlot.Gloves:
						previewAssetController.GetComponentInChildren<Animator>().Play("Check2Hands");
						break;
					case EquipItemSlot.Boots:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckBoots");
						break;
					case EquipItemSlot.Shoulders:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckShoulders");
						break;
					case EquipItemSlot.Back:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckCape");
						break;
					case EquipItemSlot.Helm:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckHelm");
						break;
					case EquipItemSlot.Weapon:
					case EquipItemSlot.Pistol:
					case EquipItemSlot.Bow:
						previewAssetController.GetComponentInChildren<Animator>().Play("CheckSword");
						break;
					default:
						previewAssetController.GetComponentInChildren<Animator>().Play("Victory");
						break;
					}
				}
				catch (Exception ex11)
				{
					LootShowcaseError(ex11);
					yield break;
				}
			}
			try
			{
				iTween.MoveFrom(previewItem.gameObject, iTween.Hash("position", new Vector3(0f, 1f, 0f), "islocal", true, "time", 1));
				iTween.ScaleFrom(previewItem.gameObject, new Vector3(0f, 0f, 0f), 1f);
			}
			catch (Exception ex12)
			{
				LootShowcaseError(ex12);
			}
		}
		else
		{
			yield return new WaitForSeconds(0.3f);
			PlayChestOpen(CurrentItem.Rarity);
			yield return new WaitForSeconds(0.15f);
			UISprite qtyItemIcon = QtyItemIcon;
			string spriteName = (PreviewIcon.spriteName = CurrentItem.Icon);
			qtyItemIcon.spriteName = spriteName;
			QtyItemIconFg.gameObject.SetActive(!string.IsNullOrEmpty(CurrentItem.IconFg));
			PreviewIconFg.gameObject.SetActive(!string.IsNullOrEmpty(CurrentItem.IconFg));
			UISprite qtyItemIconFg = QtyItemIconFg;
			spriteName = (PreviewIconFg.spriteName = CurrentItem.IconFg);
			qtyItemIconFg.spriteName = spriteName;
			ItemQtyLabel.text = "+" + CurrentItem.Qty;
			ItemQtyLabel.parent.gameObject.SetActive(CurrentItem.MaxStack > 1);
			PreviewIconGO.SetActive(value: true);
			iTween.ScaleFrom(PreviewIcon.gameObject, Vector3.zero, 1f);
		}
		RewardGlowParticles.SetActive(value: true);
		PlayParticles(CurrentItem.Rarity);
		yield return new WaitForSeconds(0.5f);
		if (CurrentItem.IsDupe)
		{
			Duplicate.SetActive(value: true);
			ChestTokenSprite.spriteName = Items.Get(1742).Icon;
			DuplicateQtyLabel.text = "+" + Item.GetLootBoxItemTokenSellPrice(CurrentItem.Rarity);
		}
		else
		{
			Duplicate.SetActive(value: false);
		}
	}

	public void OpenShop()
	{
		UIMiniShop.LoadShop(ShopId, ShopType.LootBoxItemShop, 1742);
	}

	public void CloseWindow()
	{
		Close();
	}

	public override void OnBackClick(GameObject go)
	{
		if (ChestDetailGO.activeSelf)
		{
			ShopTitle.text = shop.Name;
			ChestListGO.SetActive(value: true);
			ChestDetailGO.SetActive(value: false);
			detailsContainer.parent.GetComponent<UIScrollView>().ResetPosition();
		}
		else
		{
			base.OnBackClick(go);
		}
	}

	protected override void Destroy()
	{
		UnityEngine.Object.Destroy(rootGo3D);
		camMain.enabled = true;
		Game.Instance.EnableControls();
		UIGame.Instance.gameObject.SetActive(value: true);
		base.Destroy();
	}

	private IEnumerator PlayChestEnter(RarityType rarity)
	{
		playerAnimator.CrossFade("ChestEnter", 0.2f, -1, 0f);
		CurrentChest.Animator.Play("ChestEnter", -1, 0f);
		yield return new WaitForSeconds(0.3f);
		switch (rarity)
		{
		case RarityType.Epic:
		case RarityType.Legendary:
			AudioManager.Play2DSFX("Chest_Catch_Heavy");
			break;
		case RarityType.Uncommon:
		case RarityType.Rare:
			AudioManager.Play2DSFX("Chest_Catch_Medium");
			break;
		default:
			AudioManager.Play2DSFX("Chest_Catch_Small");
			break;
		}
	}

	private void PlayChestOpen(RarityType rarity)
	{
		try
		{
			AudioManager.Stop("Chest_Jiggle_Loop");
			switch (rarity)
			{
			case RarityType.Epic:
			case RarityType.Legendary:
				AudioManager.Play2DSFX("Chest_Open_Rare");
				break;
			case RarityType.Uncommon:
			case RarityType.Rare:
				AudioManager.Play2DSFX("Chest_Open_Uncommon");
				break;
			default:
				AudioManager.Play2DSFX("Chest_Open_Common");
				break;
			}
			playerAnimator.Play("ChestOpen");
			CurrentChest.Animator.Play("ChestOpen");
		}
		catch (Exception ex)
		{
			LootShowcaseError(ex);
		}
	}

	private void PlayChestOpenLoop()
	{
		try
		{
			AudioManager.Play2DSFX("Chest_Jiggle_Loop");
			playerAnimator.Play("ChestOpenLoop");
			CurrentChest.Animator.Play("ChestOpenLoop");
		}
		catch (Exception ex)
		{
			LootShowcaseError(ex);
		}
	}

	public void StopParticles()
	{
		ParticleSystem[] particleSystemsArray = ParticleSystemsArray;
		for (int i = 0; i < particleSystemsArray.Length; i++)
		{
			particleSystemsArray[i].Stop();
		}
	}

	public void ChangeParticleColor(ParticleSystem ps, Color colorA, Color colorB)
	{
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
		colorOverLifetime = ps.colorOverLifetime;
		colorOverLifetime.enabled = true;
		Gradient gradient = new Gradient();
		gradient = new Gradient();
		gradient.SetKeys(new GradientColorKey[2]
		{
			new GradientColorKey(colorA, 0f),
			new GradientColorKey(colorB, 1f)
		}, new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(0f, 1f)
		});
		colorOverLifetime.color = gradient;
		ps.Play();
	}

	public void PlayParticles(RarityType rarity)
	{
		switch (rarity)
		{
		case RarityType.Junk:
		{
			ParticleSystem particleSystem = ParticleSystemsArray[4];
			ParticleSystem.MainModule main = particleSystem.main;
			main.startSize = 0.05f;
			ChangeParticleColor(particleSystem, White, Color.white);
			ChangeParticleColor(ParticleSystemsArray[5], Color.white, Color.white);
			break;
		}
		case RarityType.Common:
		{
			ParticleSystemsArray[1].Play();
			ParticleSystem particleSystem = ParticleSystemsArray[0];
			ParticleSystem.MainModule main = particleSystem.main;
			main.maxParticles = 33;
			main.startSize = 2f;
			ChangeParticleColor(particleSystem, White, Color.white);
			particleSystem = ParticleSystemsArray[4];
			main = particleSystem.main;
			main.startSize = 0.05f;
			ChangeParticleColor(particleSystem, Color.white, Color.white);
			ChangeParticleColor(ParticleSystemsArray[5], Color.white, Color.white);
			break;
		}
		case RarityType.Uncommon:
		{
			ParticleSystemsArray[1].Play();
			ParticleSystem particleSystem = ParticleSystemsArray[0];
			ChangeParticleColor(particleSystem, Color.white, Green);
			ParticleSystem.MainModule main = particleSystem.main;
			main.maxParticles = 33;
			main.startSize = 2f;
			particleSystem = ParticleSystemsArray[2];
			ChangeParticleColor(particleSystem, Color.white, Green);
			particleSystem = ParticleSystemsArray[4];
			ChangeParticleColor(particleSystem, Color.green, Green);
			main = particleSystem.main;
			main.startSize = 0.05f;
			ChangeParticleColor(ParticleSystemsArray[5], Green, Color.green);
			break;
		}
		case RarityType.Rare:
		{
			ParticleSystemsArray[1].Play();
			ParticleSystem particleSystem = ParticleSystemsArray[0];
			ChangeParticleColor(particleSystem, White, Color.blue);
			ParticleSystem.MainModule main = particleSystem.main;
			main.maxParticles = 44;
			main.startSize = 2.5f;
			particleSystem = ParticleSystemsArray[2];
			ChangeParticleColor(particleSystem, Color.blue, Blue);
			particleSystem = ParticleSystemsArray[4];
			ChangeParticleColor(particleSystem, Color.blue, Blue);
			main = particleSystem.main;
			main.startSize = 0.06f;
			ChangeParticleColor(ParticleSystemsArray[5], Blue, Color.blue);
			break;
		}
		case RarityType.Epic:
		{
			ParticleSystemsArray[1].Play();
			ParticleSystemsArray[3].Play();
			ParticleSystem particleSystem = ParticleSystemsArray[0];
			ChangeParticleColor(particleSystem, White, Purple);
			ParticleSystem.MainModule main = particleSystem.main;
			main.maxParticles = 55;
			main.startSize = 3f;
			particleSystem = ParticleSystemsArray[2];
			ChangeParticleColor(particleSystem, Purple2, Purple);
			particleSystem = ParticleSystemsArray[4];
			ChangeParticleColor(particleSystem, Purple2, Purple);
			main = particleSystem.main;
			main.startSize = 0.07f;
			ChangeParticleColor(ParticleSystemsArray[5], Purple, Purple2);
			break;
		}
		case RarityType.Legendary:
		{
			ParticleSystemsArray[1].Play();
			ParticleSystemsArray[3].Play();
			ParticleSystem particleSystem = ParticleSystemsArray[0];
			ChangeParticleColor(particleSystem, White, Gold);
			ParticleSystem.MainModule main = particleSystem.main;
			main.maxParticles = 60;
			main.startSize = 3f;
			particleSystem = ParticleSystemsArray[2];
			ChangeParticleColor(particleSystem, Gold2, Gold);
			particleSystem = ParticleSystemsArray[4];
			ChangeParticleColor(particleSystem, Gold2, Gold);
			main = particleSystem.main;
			main.startSize = 0.07f;
			ChangeParticleColor(ParticleSystemsArray[5], Gold, Gold2);
			break;
		}
		}
	}
}

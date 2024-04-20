using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using StatCurves;
using UnityEngine;

public class UIHouseChooser : UIMenuWindow
{
	public UIButton OpenPublicHouseListBtn;

	public UIButton AddHouseSlotBtn;

	public UIButton OpenInventory;

	public GameObject AddHouseSlotBtnGO;

	public UIGrid HouseSlotGrid;

	public UIHouseSlot HouseSlotTemplate;

	public UIHouseSlotDetail HouseSlotDetail;

	public UIButton TrackQuest;

	public UILabel QuestButtonText;

	public GameObject QuestLockPanel;

	private AssetLoader<Texture2D> imageDownloader;

	public static UIHouseChooser Instance { get; private set; }

	public Dictionary<int, InventoryItem> MyMaps { get; private set; }

	public static void Show()
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/HouseChooser"), UIManager.Instance.transform).GetComponent<UIHouseChooser>();
			Instance.Init();
		}
	}

	public void ShowDetail(HouseData hData, UIHouseSlot hSlot)
	{
		HouseSlotDetail.Show(hData, hSlot, this, imageDownloader);
		HouseSlotDetail.gameObject.SetActive(value: true);
	}

	private void OnDestroy()
	{
		Instance = null;
		imageDownloader.DisposeAll();
	}

	private void refreshQuestLockPanel()
	{
		if (Session.MyPlayerData.HasBadgeID(765))
		{
			QuestLockPanel.SetActive(value: false);
		}
		else if (Session.MyPlayerData.CurrentlyTrackedQuest != null && Session.MyPlayerData.CurrentlyTrackedQuest.QSIndex == 219)
		{
			QuestButtonText.text = "Track Quest";
			TrackQuest.isEnabled = false;
		}
		else if ((from qID in Session.MyPlayerData.CurQuests
			select Quests.Get(qID) into q
			where q.QSIndex == 219
			select q).FirstOrDefault() != null)
		{
			QuestButtonText.text = "Track Quest";
			TrackQuest.isEnabled = true;
		}
		else
		{
			QuestButtonText.text = "Start Quest!";
			TrackQuest.isEnabled = true;
		}
	}

	protected override void Init()
	{
		base.Init();
		refreshQuestLockPanel();
		imageDownloader = new AssetLoader<Texture2D>();
		Dictionary<int, InventoryItem> dictionary = new Dictionary<int, InventoryItem>();
		foreach (InventoryItem item in Session.MyPlayerData.items.Where((InventoryItem i) => i.Type == ItemType.Map))
		{
			if (!dictionary.ContainsKey(item.ID))
			{
				dictionary.Add(item.ID, item);
			}
		}
		MyMaps = dictionary;
		if (Session.MyPlayerData.PersonalHouseData == null)
		{
			AEC.getInstance().sendRequest(new RequestHouseData(0, null, HouseDataCategory.PersonalHouseList, 0));
		}
		else
		{
			OnHouseDataAdded(Session.MyPlayerData.PersonalHouseData);
		}
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(AddHouseSlotBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onAddHouseSlotClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(OpenPublicHouseListBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onOpenPublicHouseListClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(TrackQuest.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onTrackQuestClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(OpenInventory.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(onOpenInventoryClicked));
		Session.MyPlayerData.PersonalHouseDataAdded += OnHouseDataAdded;
		Session.MyPlayerData.QuestStateUpdated += refreshQuestLockPanel;
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated += refreshLock;
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(AddHouseSlotBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(onAddHouseSlotClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(OpenPublicHouseListBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onOpenPublicHouseListClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(TrackQuest.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onTrackQuestClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(OpenInventory.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(onOpenInventoryClicked));
		Session.MyPlayerData.PersonalHouseDataAdded -= OnHouseDataAdded;
		Session.MyPlayerData.QuestStateUpdated -= refreshQuestLockPanel;
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated -= refreshLock;
	}

	private void onOpenInventoryClicked(GameObject go)
	{
		UIHouseItemList.Show();
	}

	private void refreshLock(Quest quest)
	{
		refreshQuestLockPanel();
	}

	private void onTrackQuestClicked(GameObject go)
	{
		Quest quest = (from qID in Session.MyPlayerData.CurQuests
			select Quests.Get(qID) into q
			where q.QSIndex == 219
			select q).FirstOrDefault();
		if (quest != null)
		{
			Session.MyPlayerData.TrackQuest(quest.ID);
		}
		else
		{
			AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseQuest));
		}
	}

	private void OnHouseDataAdded(List<HouseData> hDataList)
	{
		foreach (HouseData hData in hDataList)
		{
			GameObject obj = UnityEngine.Object.Instantiate(HouseSlotTemplate.gameObject);
			UIHouseSlot component = obj.GetComponent<UIHouseSlot>();
			component.Init(hData, this, imageDownloader);
			component.gameObject.SetActive(value: true);
			obj.transform.SetParent(HouseSlotGrid.transform, worldPositionStays: false);
		}
		AddHouseSlotBtnGO.transform.SetAsLastSibling();
		AddHouseSlotBtn.gameObject.SetActive(value: false);
		AddHouseSlotBtn.gameObject.SetActive(value: true);
		HouseSlotGrid.Reposition();
	}

	private void onAddHouseSlotClicked(GameObject go)
	{
		if (Session.MyPlayerData.PersonalHouseData.Count >= Session.MyPlayerData.HouseSlotMax)
		{
			MessageBox.Show("House Slot Limit", $"Sorry, you cannot have more than {Session.MyPlayerData.HouseSlotMax} house slots!");
			return;
		}
		if (Session.MyPlayerData.MC < Session.MyPlayerData.HouseSlotCost)
		{
			ConfirmationSpend.Show("Insufficient Funds!", "Would you like to purchase more DCs?", "gems", Session.MyPlayerData.HouseSlotCost, "Buy DCs", delegate(bool confirm)
			{
				if (confirm)
				{
					UIIAPStore.Show(showDCPage: true);
				}
			});
			return;
		}
		ConfirmationSpend.Show("Buy House Slot", "Buy a new house slot?", "gems", Session.MyPlayerData.HouseSlotCost, "Buy", delegate(bool confirm)
		{
			if (confirm)
			{
				HouseData houseData = new HouseData
				{
					Visibility = HouseVisibility.Private,
					Name = "My New House",
					MaxPlayers = 10,
					MapID = 1,
					OwnerID = Entities.Instance.me.ID
				};
				AEC.getInstance().sendRequest(new RequestHouseAdd(houseData));
			}
		});
	}

	private void onOpenPublicHouseListClicked(GameObject go)
	{
		UIPublicHouseChooser.Show();
	}
}

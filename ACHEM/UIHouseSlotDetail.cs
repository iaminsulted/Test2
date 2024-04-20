using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIHouseSlotDetail : MonoBehaviour
{
	private struct HouseDataRecord
	{
		public string Name;

		public int MaxPlayers;

		public int MapID;

		public int MapItemID;

		public HouseVisibility Visibility;

		public HouseDataRecord(HouseData hData)
		{
			Name = hData.Name;
			MaxPlayers = hData.MaxPlayers;
			Visibility = hData.Visibility;
			MapID = hData.MapID;
			MapItemID = hData.MapItemID;
		}

		public void ApplyToHouseData(HouseData hData)
		{
			hData.Name = Name;
			hData.MaxPlayers = MaxPlayers;
			hData.Visibility = Visibility;
			hData.MapID = MapID;
			hData.MapItemID = MapItemID;
			hData.ServerID = AEC.getInstance().ServerID;
		}
	}

	public class HouseSetting
	{
		private string defaultValue;

		public UIInput inputField { get; private set; }

		private event Func<string, string> onSubmit;

		private event Action<string> onInputChanged;

		public HouseSetting(string value, UIInput inputField, Action<string> onInputChanged, Func<string, string> onSubmit)
		{
			defaultValue = value;
			this.inputField = inputField;
			this.onInputChanged = onInputChanged;
			this.onSubmit = onSubmit;
			this.inputField.submitOnUnselect = true;
		}

		public void EnableInput()
		{
			UIEventListener uIEventListener = UIEventListener.Get(inputField.gameObject);
			uIEventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onSelect, new UIEventListener.BoolDelegate(OnInputSelected));
			EventDelegate.Add(inputField.onChange, OnInputChanged);
			EventDelegate.Add(inputField.onSubmit, OnInputSubmitted);
		}

		public void DisableInput()
		{
			UIEventListener uIEventListener = UIEventListener.Get(inputField.gameObject);
			uIEventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onSelect, new UIEventListener.BoolDelegate(OnInputSelected));
			EventDelegate.Remove(inputField.onChange, OnInputChanged);
			EventDelegate.Remove(inputField.onSubmit, OnInputSubmitted);
		}

		private void OnInputSelected(GameObject go, bool isSelected)
		{
			if (!isSelected)
			{
				inputField.value = defaultValue;
			}
		}

		private void OnInputChanged()
		{
			this.onInputChanged(inputField.value);
		}

		private void OnInputSubmitted()
		{
			defaultValue = this.onSubmit(inputField.value);
			inputField.value = defaultValue;
		}
	}

	public UIButton CloseButton;

	public UIButton SaveButton;

	public UIButton SettingsTab;

	public UIButton MapsTab;

	public GameObject SettingsWindow;

	public GameObject MapsWindow;

	public GameObject MapItemUITemplate;

	public UIGrid MapItemGrid;

	public UILabel WindowTitle;

	public UIInput HouseTitleInput;

	public UIInput MaxPlayersInput;

	public UIPopupList VisibilityDropdown;

	private UIHouseSlot houseSlot;

	private HouseData houseData;

	private List<HouseSetting> settingsList = new List<HouseSetting>();

	private List<ComHouseItem> houseItems;

	private AssetLoader<Texture2D> imageDownloader;

	private HouseDataRecord dataRecord;

	private HouseDataRecord inputRecord;

	private UIMapItem selectedMap;

	public UIHouseChooser houseChooser { get; private set; }

	public void Show(HouseData hData, UIHouseSlot hSlot, UIHouseChooser hChooser, AssetLoader<Texture2D> imageDownloader)
	{
		if (houseData == hData)
		{
			return;
		}
		if (houseData != null && IsModified())
		{
			Confirmation.Show("Unsaved Changes", "Would you like to save your changes before switching?", delegate(bool isYes)
			{
				SaveOrRevert(isYes);
				houseData = null;
				Show(hData, hSlot, hChooser, imageDownloader);
				base.gameObject.SetActive(value: true);
			});
			return;
		}
		settingsList.Clear();
		houseSlot = hSlot;
		houseChooser = hChooser;
		houseData = hData;
		this.imageDownloader = imageDownloader;
		dataRecord = new HouseDataRecord(hData);
		inputRecord = new HouseDataRecord(hData);
		HouseSetting item = new HouseSetting(inputRecord.Name, HouseTitleInput, delegate(string s)
		{
			UILabel windowTitle = WindowTitle;
			string text2 = (houseSlot.HouseName.text = (inputRecord.Name = s));
			windowTitle.text = text2;
		}, (string s) => s);
		settingsList.Add(item);
		HouseSetting item2 = new HouseSetting(inputRecord.MaxPlayers.ToString(), MaxPlayersInput, delegate(string s)
		{
			int maxPlayers = (string.IsNullOrEmpty(s) ? 1 : int.Parse(s));
			inputRecord.MaxPlayers = maxPlayers;
		}, delegate(string s)
		{
			int num = ((!string.IsNullOrEmpty(s)) ? int.Parse(s) : 0);
			if (num < 1)
			{
				MessageBox.Show("Invalid Input", "Max Players must be greater than 0");
				num = 1;
			}
			if (num > Session.MyPlayerData.HouseMaxPlayers)
			{
				MessageBox.Show("Max Player Limit", $"Max Players cannot exceed {Session.MyPlayerData.HouseMaxPlayers}");
				num = Session.MyPlayerData.HouseMaxPlayers;
			}
			inputRecord.MaxPlayers = num;
			return num.ToString();
		});
		settingsList.Add(item2);
		SetupInputText(inputRecord);
		SetupMapGrid();
	}

	public void SelectMap(UIMapItem uiMapItem, InventoryItem iItem)
	{
		if (selectedMap != null)
		{
			selectedMap.DisableCheckmark();
		}
		selectedMap = uiMapItem;
		inputRecord.MapID = iItem.MapID;
		inputRecord.MapItemID = iItem.ID;
		houseSlot.MapName.text = iItem.Name;
		houseSlot.SetImage(iItem);
	}

	private void OnEnable()
	{
		SaveButton.isEnabled = false;
		foreach (HouseSetting settings in settingsList)
		{
			settings.EnableInput();
		}
		UIEventListener uIEventListener = UIEventListener.Get(CloseButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(SaveButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnSaveButtonClicked));
		EventDelegate.Add(VisibilityDropdown.onChange, OnVisibilityChanged);
		UIEventListener uIEventListener3 = UIEventListener.Get(SettingsTab.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSettingsTabClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(MapsTab.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnMapsTabClicked));
	}

	private void OnDisable()
	{
		foreach (HouseSetting settings in settingsList)
		{
			settings.DisableInput();
		}
		UIEventListener uIEventListener = UIEventListener.Get(CloseButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClicked));
		UIEventListener uIEventListener2 = UIEventListener.Get(SaveButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnSaveButtonClicked));
		EventDelegate.Remove(VisibilityDropdown.onChange, OnVisibilityChanged);
		UIEventListener uIEventListener3 = UIEventListener.Get(SettingsTab.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSettingsTabClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(MapsTab.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnMapsTabClicked));
	}

	private void OnVisibilityChanged()
	{
		if (VisibilityDropdown.value == "Public")
		{
			if (dataRecord.Visibility != HouseVisibility.Public)
			{
				MessageBox.Show("Your House Will Be Public!", "Your house and your character's name will appear in the public house list, which is visible to everyone.");
				houseData.ServerID = AEC.getInstance().ServerID;
			}
			inputRecord.Visibility = HouseVisibility.Public;
			ServerInfo serverInfo = ServerInfo.Servers.Where((ServerInfo x) => x.ID == houseData.ServerID).First();
			houseSlot.PublicPrivate.text = "Public - Server: " + serverInfo.Name;
		}
		else if (VisibilityDropdown.value == "Friends Only")
		{
			inputRecord.Visibility = HouseVisibility.FriendsOnly;
			houseSlot.PublicPrivate.text = "Friends Only";
		}
		else if (VisibilityDropdown.value == "Guild Only")
		{
			inputRecord.Visibility = HouseVisibility.GuildOnly;
			houseSlot.PublicPrivate.text = "Guild Only";
		}
		else
		{
			inputRecord.Visibility = HouseVisibility.Private;
			houseSlot.PublicPrivate.text = "Private";
		}
	}

	private void OnSettingsTabClicked(GameObject go)
	{
		SettingsWindow.SetActive(value: true);
		MapsWindow.SetActive(value: false);
	}

	private void OnMapsTabClicked(GameObject go)
	{
		SettingsWindow.SetActive(value: false);
		MapsWindow.SetActive(value: true);
		SetupMapGrid();
	}

	private void SetupMapGrid()
	{
		MapItemGrid.transform.DestroyChildren();
		foreach (KeyValuePair<int, InventoryItem> myMap in houseChooser.MyMaps)
		{
			GameObject obj = UnityEngine.Object.Instantiate(MapItemUITemplate);
			UIMapItem component = obj.GetComponent<UIMapItem>();
			component.Init(myMap.Value, this, imageDownloader);
			component.gameObject.SetActive(value: true);
			obj.transform.SetParent(MapItemGrid.transform, worldPositionStays: false);
			if (component.MapInventoryItem.ID == inputRecord.MapItemID)
			{
				component.EnableCheckmark();
				selectedMap = component;
			}
		}
		MapItemGrid.Reposition();
	}

	private void SetupInputText(HouseDataRecord hData)
	{
		WindowTitle.text = hData.Name;
		HouseTitleInput.value = hData.Name;
		MaxPlayersInput.value = hData.MaxPlayers.ToString();
		switch (hData.Visibility)
		{
		case HouseVisibility.Public:
			VisibilityDropdown.value = "Public";
			break;
		case HouseVisibility.Private:
			VisibilityDropdown.value = "Private";
			break;
		case HouseVisibility.FriendsOnly:
			VisibilityDropdown.value = "Friends Only";
			break;
		case HouseVisibility.GuildOnly:
			VisibilityDropdown.value = "Guild Only";
			break;
		}
	}

	private void OnSaveButtonClicked(GameObject go)
	{
		if (new ChatFilter().ProfanityCheck(inputRecord.Name, shouldCleanSymbols: false).code > 0)
		{
			MessageBox.Show("Invalid House Title", "House titles cannot contain profanity!");
		}
		else if (inputRecord.MapItemID != dataRecord.MapItemID)
		{
			Confirmation.Show("Clear House", "Switching your house's map will kick all of the players and clear out all of the furniture. Save anyway?", delegate(bool isYes)
			{
				if (isYes)
				{
					Save();
				}
			});
		}
		else if (inputRecord.Visibility != HouseVisibility.Public && dataRecord.Visibility == HouseVisibility.Public)
		{
			Confirmation.Show("Kick Other Players", "Switching your house's visibility will kick all other players except you. Save anyway?", delegate(bool isYes)
			{
				if (isYes)
				{
					Save();
				}
			});
		}
		else
		{
			Save();
		}
	}

	private void Save()
	{
		dataRecord = inputRecord;
		dataRecord.ApplyToHouseData(houseData);
		AEC.getInstance().sendRequest(new RequestHouseUpdate(houseData));
		Debug.LogWarning("Saved!");
	}

	private bool IsModified()
	{
		return !inputRecord.Equals(dataRecord);
	}

	private void SaveOrRevert(bool shouldSave)
	{
		if (shouldSave)
		{
			Save();
		}
		else
		{
			houseSlot.RefreshFromHouseData();
		}
		base.gameObject.SetActive(value: false);
	}

	private void OnCloseClicked(GameObject go)
	{
		if (UIInput.selection != null)
		{
			UIInput.selection.isSelected = false;
		}
		if (IsModified())
		{
			Confirmation.Show("Unsaved Changes", "Would you like to save your changes before closing?", SaveOrRevert);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (IsModified())
		{
			SaveButton.isEnabled = true;
		}
		else
		{
			SaveButton.isEnabled = false;
		}
	}
}

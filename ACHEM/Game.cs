using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppsFlyerSDK;
using AQ3DServer.GameServer.CommClasses;
using Assets.Scripts.Game;
using Assets.Scripts.NetworkClient.CommClasses;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.ImageEffects;

public class Game : State
{
	public class InteractableRaycastResult
	{
		public readonly IInteractable HitInteractable;

		public readonly Ray Ray;

		public readonly RaycastHit Hit;

		public readonly Vector2 ScreenPos;

		public InteractableRaycastResult(IInteractable ia, Ray ray, RaycastHit hit, Vector2 screenPos)
		{
			HitInteractable = ia;
			Ray = ray;
			Hit = hit;
			ScreenPos = screenPos;
		}
	}

	private enum QuestTrackPrority
	{
		SideQuests,
		SagaQuests,
		CurrentMapSideQuests,
		ObjectiveInCurrentMap,
		TurnInNpcInCurrentMap
	}

	private bool isReady;

	public bool isInputEnabled;

	public AEC aec;

	public TargetReticleScript TargetReticle;

	private Transform baseTF;

	private GameObject cellGO;

	private GameObject entitiesGO;

	private GameObject fxGO;

	public SpellFXContainer field;

	public Entities entities;

	public List<ComMachine> ComMachines;

	public List<ComMachineListener> ComMachineListeners;

	public List<int> UsedMachines = new List<int>();

	public List<ComLoot> ComLootBags;

	public List<LocatorTransfer> LocatorTransfers;

	public List<TransferPadControl> transferPads;

	public Dictionary<int, NPCSpawn> Spawns = new Dictionary<int, NPCSpawn>();

	public Camera cam;

	public CameraController camController;

	public Camera uiCamera;

	public CameraWaterRaycast cameraWaterRaycast;

	public Camera selectionCamera;

	public Camera UICamera3D;

	public UIGame uigame;

	private AreaData areaData;

	public CombatSolver combat;

	public HousingManager housing;

	public Action<Vector2> onDrag;

	public Action<bool> onPress;

	public Action onBackgroundClick;

	private List<Response> DelayedUIResponses = new List<Response>();

	public DataPrefab CurrentCell;

	public bool TesterMode;

	private static Game instance;

	private AssetBundleLoader sountrackloader;

	private AssetBundleLoader maploader;

	private int CurrentSoundTrackID;

	private GameObject CurrentSoundTackGO;

	private bool firstCell;

	private static bool alreadyAskedGuestToConvert;

	private Action<Quest> OnShowSagaQuest;

	public Ping ping;

	private GameObject lastClickedObject;

	private float lastClickedTime;

	private int lastTouchId = int.MinValue;

	public const float MAX_CLICK_DISTANCE_DEFAULT = 40f;

	public static float Max_Click_Distance;

	public const float Double_Click_Time = 0.35f;

	public IInteractable CurrentInteractable;

	private InteractableRaycastResult currentInteractablePress1;

	private InteractableRaycastResult currentInteractablePress2;

	private Vector2 screenPos;

	private bool isUIVisible = true;

	private InteractableRaycastResult HoverInteractableCast;

	public bool isHoveringOverUI;

	private AdamTools adamTools;

	public static bool IngoreResponses;

	public AreaData AreaData => areaData;

	public static int CurrentCellID => instance.areaData.currentCellID;

	public static int CurrentAreaID => instance.areaData.id;

	public static string CurrentAreaName => instance.areaData.displayName;

	public static Game Instance => instance;

	public bool IsUIVisible
	{
		get
		{
			return isUIVisible;
		}
		set
		{
			if (isUIVisible != value)
			{
				isUIVisible = value;
				uiCamera.enabled = isUIVisible;
				if (isUIVisible)
				{
					cam.cullingMask |= 1 << Layers.CLICKIES;
					uiCamera.GetComponent<UICamera>().eventReceiverMask = LayerMask.GetMask("NGUI", "NGUIFLY");
				}
				else
				{
					cam.cullingMask &= ~(1 << Layers.CLICKIES);
					uiCamera.GetComponent<UICamera>().eventReceiverMask = 0;
					UIWindow.ClearWindows();
				}
			}
		}
	}

	public Vector3 HoverPoint { get; private set; }

	private bool IsShiftPressed
	{
		get
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}
	}

	private bool IsCtrlPressed
	{
		get
		{
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				return Input.GetKey(KeyCode.RightControl);
			}
			return true;
		}
	}

	public bool IsIntroMap
	{
		get
		{
			if (areaData != null)
			{
				return areaData.id == 249;
			}
			return false;
		}
	}

	public CellData CellData => areaData.cellMap[areaData.currentCellID];

	public event Action<string, string> AreaFlagUpdated;

	public event Action QuestLoaded;

	public event Action<Shop, string> ShopLoaded;

	public event Action<MergeShop, string> MergeShopLoaded;

	public event Action<List<LootBoxRewardItem>, int> ReceivedLoot;

	public event Action<ResponseChat> ChatReceived;

	public event Action<bool> AdWatchRewardReceived;

	public event Action MatchStateUpdated;

	public override void Awake()
	{
		base.Awake();
		instance = this;
		Session.MyPlayerData = new MyPlayerData();
		NPCCamController.ActiveUpdated += OnNPCCamActiveUpdated;
		DialogueSlotManager.Initialized += DisableControls;
		DialogueSlotManager.Closed += EnableControls;
		UIFullscreenWindow.OnFullscreen += OnFullscreenUI;
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated += CurrentlyTrackedQuestUpdated;
		Session.MyPlayerData.MergeAdded += NotificationManager.Instance.ScheduleCraftReadyNotification;
		Session.MyPlayerData.MergeRemoved += NotificationManager.Instance.HideCraftReadyNotification;
		Session.MyPlayerData.DailyRewardUpdated += HideAndScheduleNextDailyRewardReadyNotification;
		NotificationManager.Instance.HideDailyRewardReadyNotification();
		NotificationManager.Instance.ScheduleDailyRewardReadyNotification();
		Application.logMessageReceived += OnApplicationLogMessageReceived;
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_AwakeOk);
	}

	private void HideAndScheduleNextDailyRewardReadyNotification(int day, DateTime date, int itemID)
	{
		NotificationManager.Instance.HideDailyRewardReadyNotification();
		NotificationManager.Instance.ScheduleDailyRewardReadyNotification();
	}

	private void OnApplicationLogMessageReceived(string condition, string stackTrace, LogType type)
	{
		if (Session.MyPlayerData.AccessLevel >= 50 && (type == LogType.Error || type == LogType.Exception) && (!PlayerPrefs.HasKey("CHATERROR") || PlayerPrefs.GetInt("CHATERROR") == 1))
		{
			Chat.Notify(condition + "\n" + stackTrace, "[FF0000]", Chat.FilterType.ServerMessage);
		}
	}

	public void OnDestroy()
	{
		entities.Clear();
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated -= CurrentlyTrackedQuestUpdated;
		Session.MyPlayerData.MergeAdded -= NotificationManager.Instance.ScheduleCraftReadyNotification;
		Session.MyPlayerData.MergeRemoved -= NotificationManager.Instance.HideCraftReadyNotification;
		Session.MyPlayerData.DailyRewardUpdated -= HideAndScheduleNextDailyRewardReadyNotification;
		Session.MyPlayerData = null;
		instance = null;
		Session.Clear();
		Application.logMessageReceived -= OnApplicationLogMessageReceived;
	}

	private void Start()
	{
		Debug.Log("> Game.Start()");
		Loader.show("Connecting to server...", 0f);
		ResourceCache.InitAssets();
		baseTF = base.transform;
		entities = Entities.Instance;
		cam.eventMask = 0;
		cam.farClipPlane = 2500f;
		cam.layerCullDistances = Layers.GetAdjustedCullDistances(Layers.MinCullDistances, Layers.MaxCullDistances);
		cam.depthTextureMode |= DepthTextureMode.Depth;
		camController = cam.GetComponentInParent<CameraController>();
		uiCamera = UIManager.Instance.uiCamera;
		float depth = selectionCamera.depth;
		selectionCamera.CopyFrom(cam);
		selectionCamera.depth = depth;
		selectionCamera.transform.SetParent(cam.transform, worldPositionStays: false);
		selectionCamera.enabled = false;
		selectionCamera.cullingMask = 1 << Layers.SELECTIONCAMERA;
		selectionCamera.clearFlags = CameraClearFlags.Depth;
		selectionCamera.transform.localPosition = Vector3.zero;
		float depth2 = UICamera3D.depth;
		UICamera3D.CopyFrom(cam);
		UICamera3D.depth = depth2;
		UICamera3D.transform.SetParent(cam.transform, worldPositionStays: false);
		UICamera3D.cullingMask = 1 << Layers.UI3D;
		UICamera3D.clearFlags = CameraClearFlags.Depth;
		UICamera3D.transform.localPosition = Vector3.zero;
		combat = base.gameObject.AddComponent<CombatSolver>();
		combat.Init();
		TargetReticle.InitMaterial();
		ApopDownloader.Init(base.transform);
		SettingsManager.DrawDistanceUpdated += OnDrawDistanceUpdated;
		SettingsManager.IgnoreSettingUpdated += SyncIgnore;
		InputManager.ActionEvent += OnActionInputEvent;
		UICamera.onClick = (UICamera.VoidDelegate)Delegate.Combine(UICamera.onClick, new UICamera.VoidDelegate(OnClick));
		UICamera.onPress = (UICamera.BoolDelegate)Delegate.Combine(UICamera.onPress, new UICamera.BoolDelegate(OnPress));
		UICamera.onHover = (UICamera.BoolDelegate)Delegate.Combine(UICamera.onHover, new UICamera.BoolDelegate(OnHover));
		UICamera.onDoubleClick = (UICamera.VoidDelegate)Delegate.Combine(UICamera.onDoubleClick, new UICamera.VoidDelegate(OnDoubleClick));
		UICamera.onDrag = (UICamera.VectorDelegate)Delegate.Combine(UICamera.onDrag, new UICamera.VectorDelegate(OnDrag));
		aec = AEC.getInstance();
		aec.OnDisconnect += onDisconnect;
		aec.ResponseReceived += OnReceiveResponse;
		Login();
		Debug.Log("Game: ADDED Audio Listener");
		base.gameObject.AddComponent<AudioListener>();
		firstCell = true;
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_StartOk);
		ping = base.gameObject.AddComponent<Ping>();
	}

	private void OnDrawDistanceUpdated(int quality)
	{
		int num;
		float[] array;
		if (CurrentCell != null)
		{
			num = (CurrentCell.UseCustomCulling ? 1 : 0);
			if (num != 0)
			{
				array = CurrentCell.CustomLayers.CullDistanceMin();
				goto IL_0036;
			}
		}
		else
		{
			num = 0;
		}
		array = Layers.MinCullDistances;
		goto IL_0036;
		IL_0036:
		float[] cullDistanceMin = array;
		float[] cullDistanceMax = ((num != 0) ? CurrentCell.CustomLayers.CullDistanceMax() : Layers.MaxCullDistances);
		cam.layerCullDistances = Layers.GetAdjustedCullDistances(cullDistanceMin, cullDistanceMax);
	}

	public void Logout()
	{
		BackToLogin();
	}

	public void Logout(string title, string message)
	{
		BackToLogin();
		MessageBox.Show(title, message);
	}

	public override void Close()
	{
		base.Close();
		ClearCurrentTrack();
		ClearCurrentCell();
		AudioManager.CleanUp();
		NPCCamController.ActiveUpdated -= OnNPCCamActiveUpdated;
		DialogueSlotManager.Initialized -= DisableControls;
		DialogueSlotManager.Closed -= EnableControls;
		DialogueSlotManager.Close();
		UIFullscreenWindow.OnFullscreen -= OnFullscreenUI;
		SettingsManager.DrawDistanceUpdated -= OnDrawDistanceUpdated;
		SettingsManager.IgnoreSettingUpdated -= SyncIgnore;
		InputManager.ActionEvent -= OnActionInputEvent;
		UICamera.onClick = (UICamera.VoidDelegate)Delegate.Remove(UICamera.onClick, new UICamera.VoidDelegate(OnClick));
		UICamera.onPress = (UICamera.BoolDelegate)Delegate.Remove(UICamera.onPress, new UICamera.BoolDelegate(OnPress));
		UICamera.onDoubleClick = (UICamera.VoidDelegate)Delegate.Remove(UICamera.onDoubleClick, new UICamera.VoidDelegate(OnDoubleClick));
		aec.OnDisconnect -= onDisconnect;
		aec.ResponseReceived -= OnReceiveResponse;
		aec.close();
		PartyManager.Clear();
		ChannelManager.Instance.Clear();
		ProjectionController.Close();
		ClearLocalCache();
		Loader.close();
	}

	public NPCSpawn GetNpcSpawn(int id)
	{
		if (Spawns.ContainsKey(id))
		{
			return Spawns[id];
		}
		return null;
	}

	public void DestroyHousingInstance()
	{
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<House>());
	}

	public House GetNewHouseInstance()
	{
		DestroyHousingInstance();
		return base.gameObject.AddComponent<House>();
	}

	public void GetWaypointByTargetMapIDs(List<int> mapIDs, out ITrackable closestWaypoint, out int mapIndex)
	{
		int num = int.MaxValue;
		closestWaypoint = null;
		mapIndex = 0;
		foreach (int mapID in mapIDs)
		{
			for (int i = 0; i < transferPads.Count; i++)
			{
				TransferPadControl transferPadControl = transferPads[i];
				if (transferPadControl == null)
				{
					transferPads.RemoveAt(i);
					i--;
				}
				else if (transferPadControl.isActiveAndEnabled && transferPadControl.Areas.Contains(mapID))
				{
					int num2 = transferPadControl.Areas.IndexOf(mapID);
					if (num2 < num)
					{
						num = num2;
						closestWaypoint = transferPadControl;
						mapIndex = mapIDs.IndexOf(mapID);
					}
				}
			}
			foreach (BaseMachine value in BaseMachine.Map.Values)
			{
				if (value != null && value.IsActive && value.isActiveAndEnabled && value.Areas.Contains(mapID))
				{
					int num3 = value.Areas.IndexOf(mapID);
					if (num3 < num)
					{
						num = num3;
						closestWaypoint = value;
						mapIndex = mapIDs.IndexOf(mapID);
					}
				}
			}
			foreach (ClientTrigger value2 in ClientTrigger.Map.Values)
			{
				if (value2.IsActive && value2.isActiveAndEnabled && value2.Areas.Contains(mapID))
				{
					int num4 = value2.Areas.IndexOf(mapID);
					if (num4 < num)
					{
						num = num4;
						closestWaypoint = value2;
						mapIndex = mapIDs.IndexOf(mapID);
					}
				}
			}
		}
	}

	private void OnDrag(GameObject go, Vector2 delta)
	{
		if (!(CurrentInteractable is IDraggable draggable) || !draggable.IsDraggable(null))
		{
			camController.OnDrag(delta);
		}
	}

	private void UpdateDraggable()
	{
		if (CurrentInteractable == null)
		{
			return;
		}
		UICamera.MouseOrTouch mouseOrTouch = ((UIGame.ControlScheme != ControlScheme.HANDHELD && !Platform.IsMobile) ? UICamera.GetTouch(-1, createIfMissing: false) : UICamera.GetTouch(0, createIfMissing: false));
		if (mouseOrTouch == null)
		{
			return;
		}
		InteractableRaycastResult interactableRaycastResult = currentInteractablePress1;
		currentInteractablePress1 = GetInteractableFromRaycast(mouseOrTouch.pos);
		IDraggable draggable = CurrentInteractable as IDraggable;
		if (interactableRaycastResult == null || draggable == null || !draggable.IsDraggable(currentInteractablePress1))
		{
			return;
		}
		draggable.OnDrag(currentInteractablePress1, interactableRaycastResult);
		UICamera.MouseOrTouch mouseOrTouch2 = UICamera.GetTouch(1, createIfMissing: false);
		if (mouseOrTouch2 != null)
		{
			InteractableRaycastResult interactableRaycastResult2 = currentInteractablePress2;
			currentInteractablePress2 = GetInteractableFromRaycast(mouseOrTouch2.pos);
			if (interactableRaycastResult2 != null && draggable.IsSecondTouchDraggable(currentInteractablePress2))
			{
				draggable.OnSecondTouchDrag(currentInteractablePress2, interactableRaycastResult2);
			}
		}
	}

	public void OnPress(GameObject go, bool isPressed)
	{
		OnPress(go, isPressed, null);
	}

	public void OnPress(GameObject go, bool isPressed, IInteractable interactableOverride = null)
	{
		if (interactableOverride == null && UICamera.isOverUI && go.name != "Joystick" && isPressed)
		{
			camController.OnPress(isPressed: true);
			return;
		}
		InteractableRaycastResult interactableRaycastResult = ((UIGame.ControlScheme != ControlScheme.HANDHELD && !Platform.IsMobile) ? HoverInteractableCast : ((UICamera.currentTouch == null) ? null : GetInteractableFromRaycast(UICamera.currentTouch.pos)));
		if (interactableOverride != null)
		{
			CurrentInteractable = interactableOverride;
			currentInteractablePress1 = null;
			CurrentInteractable.OnPress(null, isPressed);
		}
		else if (CurrentInteractable == null && isPressed && (UICamera.currentTouchID == 0 || UICamera.currentTouchID == -1 || UICamera.currentTouchID == -100) && interactableRaycastResult != null && interactableRaycastResult.HitInteractable != null)
		{
			CurrentInteractable = interactableRaycastResult.HitInteractable;
			currentInteractablePress1 = interactableRaycastResult;
			CurrentInteractable.OnPress(interactableRaycastResult, isPressed: true);
		}
		else if (CurrentInteractable != null && !isPressed && (UICamera.currentTouchID == 0 || UICamera.currentTouchID == -1))
		{
			CurrentInteractable.OnPress(interactableRaycastResult, isPressed: false);
			CurrentInteractable = null;
			currentInteractablePress1 = null;
		}
		else if (CurrentInteractable != null && UICamera.currentTouchID == 1)
		{
			if (isPressed)
			{
				currentInteractablePress2 = interactableRaycastResult;
				CurrentInteractable.OnSecondTouchPress(interactableRaycastResult, isPressed: true);
			}
			else
			{
				currentInteractablePress2 = null;
				CurrentInteractable.OnSecondTouchPress(interactableRaycastResult, isPressed: false);
			}
		}
		if (!(CurrentInteractable is IDraggable draggable) || Input.GetMouseButtonDown(1) || !draggable.IsDraggable(interactableRaycastResult))
		{
			camController.OnPress(isPressed);
		}
	}

	public void OnClick(GameObject go)
	{
		if (isInputEnabled && entities.me != null)
		{
			ProcessClick();
			lastTouchId = UICamera.currentTouchID;
		}
	}

	private void ProcessClick()
	{
		if ((UICamera.isOverUI && UICamera.selectedObject.name != "Joystick") || (entities.me.visualState != 0 && entities.me.visualState != Entity.State.InCombat && entities.me.visualState != Entity.State.Interacting))
		{
			return;
		}
		bool flag = true;
		if (Physics.Raycast(cam.ScreenPointToRay(UICamera.lastEventPosition), out var hitInfo, Max_Click_Distance + camController.cameraDistanceCurrent, Layers.LAYER_MASK_MOUSECLICK))
		{
			bool flag2 = lastClickedObject == hitInfo.collider.gameObject && RealTime.time - lastClickedTime <= 0.35f && lastTouchId == UICamera.currentTouchID;
			IClickable[] components = hitInfo.collider.GetComponents<IClickable>();
			IClickable[] array = components;
			foreach (IClickable clickable in array)
			{
				if (flag2)
				{
					clickable.OnDoubleClick();
				}
				else
				{
					clickable.OnClick(hitInfo.point);
				}
			}
			flag = components.Length == 0;
			if (flag)
			{
				onBackgroundClick?.Invoke();
			}
			lastClickedObject = hitInfo.collider.gameObject;
			lastClickedTime = RealTime.time;
		}
		if (flag && ((UICamera.currentTouchID == -1 && !Input.GetMouseButton(1)) || Platform.IsMobile))
		{
			entities.me.Target = null;
			entities.me.TargetNode = null;
		}
	}

	public void OnDoubleClick(GameObject go)
	{
		if (isInputEnabled && !UICamera.isOverUI)
		{
			bool flag = true;
			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hitInfo, Max_Click_Distance + camController.cameraDistanceCurrent, Layers.LAYER_MASK_MOUSECLICK) && hitInfo.collider.GetComponent<IClickable>() != null)
			{
				flag = false;
			}
			if (flag && UICamera.currentScheme == UICamera.ControlScheme.Touch)
			{
				entities.me.moveController.Jump();
			}
		}
	}

	public void OnActionInputEvent(InputAction action)
	{
		if (isInputEnabled)
		{
			ApplyAction(action);
		}
	}

	public void ApplyAction(InputAction action)
	{
		switch (action)
		{
		case InputAction.Cancel:
			ModalWindow.Clear();
			if (UIWindow.Count > 0)
			{
				UIWindow.ClearWindows();
			}
			else
			{
				UIMainMenu.Load();
			}
			break;
		case InputAction.UI_Toggle:
			if (!IsIntroMap)
			{
				IsUIVisible = !IsUIVisible;
			}
			break;
		case InputAction.UI_Toggle_Sheathe:
			Sheathing.Instance.Toggle();
			break;
		case InputAction.UI_Toggle_Inventory:
			UIInventory.Toggle();
			break;
		case InputAction.UI_Toggle_Character:
			UICharInfo.Toggle(Entities.Instance.me);
			break;
		case InputAction.UI_Toggle_Crafting:
			UIMerge.Toggle();
			break;
		case InputAction.UI_Toggle_Potions:
			UIMiniShop.TogglePotionShop();
			break;
		case InputAction.UI_Toggle_Quests:
			UIQuest.Toggle();
			break;
		case InputAction.UI_Toggle_Social:
			UIFriendsList.Toggle();
			break;
		case InputAction.UI_Toggle_Guild:
			UIGuild.Toggle();
			break;
		case InputAction.UI_Toggle_Classes:
			UICharClasses.Toggle();
			break;
		case InputAction.UI_Toggle_DailyTasks:
			UIDailyTasks.Toggle();
			break;
		case InputAction.UI_Toggle_Travel:
			UIRegions.Toggle();
			break;
		case InputAction.UI_Toggle_Pvp:
			UIPvPMainMenu.Toggle();
			break;
		case InputAction.ZoomIn:
			instance.camController.ZoomInStep();
			break;
		case InputAction.ZoomOut:
			instance.camController.ZoomOutStep();
			break;
		case InputAction.OpenLoot:
			UILoot.Toggle();
			break;
		case InputAction.LootAll:
			if (UILoot.Instance == null)
			{
				UILoot.LoadAllNear();
			}
			else
			{
				UILoot.Instance.OnLootAllClick(null);
			}
			break;
		case InputAction.Target_Next:
			if (IsCtrlPressed)
			{
				combat.TargetPreviousEnemy();
			}
			else
			{
				combat.TargetNextEnemy();
			}
			if (!camController.panToTarget)
			{
				camController.ResetTargetLock();
			}
			break;
		case InputAction.TargetClosest:
			combat.TargetClosestEnemy();
			if (!camController.panToTarget)
			{
				camController.ResetTargetLock();
			}
			break;
		case InputAction.Spell_1:
		case InputAction.Spell_2:
		case InputAction.Spell_3:
		case InputAction.Spell_4:
		case InputAction.Spell_5:
			if (entities.me.AccessLevel < 100 || !IsShiftPressed || !IsCtrlPressed)
			{
				combat.TryCastSpell(action);
			}
			break;
		case InputAction.Cross_Skill:
			if (Session.MyPlayerData.UnlockedCrossSkillIDs.Count == 0)
			{
				Notification.ShowWarning("Reach Rank " + 10 + " in any class to unlock");
			}
			else
			{
				combat.TryCastSpell(action);
			}
			break;
		case InputAction.CustomAction_1:
			CustomItemAction(action);
			break;
		case InputAction.CustomAction_2:
		case InputAction.CustomAction_3:
		case InputAction.CustomAction_4:
			if (entities.me.IsInPvp || AreaData.IsPvpLobby)
			{
				combat.TryCastSpell(action);
			}
			else
			{
				CustomItemAction(action);
			}
			break;
		case InputAction.TargetParty_1:
			if (PartyManager.IsInParty && uigame.partyHUD.portraits.Count > 0 && uigame.partyHUD.portraits[0].Entity != null)
			{
				Entities.Instance.me.Target = uigame.partyHUD.portraits[0].Entity;
			}
			break;
		case InputAction.TargetParty_2:
			if (PartyManager.IsInParty && uigame.partyHUD.portraits.Count > 1 && uigame.partyHUD.portraits[1].Entity != null)
			{
				Entities.Instance.me.Target = uigame.partyHUD.portraits[1].Entity;
			}
			break;
		case InputAction.TargetParty_3:
			if (PartyManager.IsInParty && uigame.partyHUD.portraits.Count > 2 && uigame.partyHUD.portraits[2].Entity != null)
			{
				Entities.Instance.me.Target = uigame.partyHUD.portraits[2].Entity;
			}
			break;
		case InputAction.TargetParty_4:
			if (PartyManager.IsInParty && uigame.partyHUD.portraits.Count > 3 && uigame.partyHUD.portraits[3].Entity != null)
			{
				Entities.Instance.me.Target = uigame.partyHUD.portraits[3].Entity;
			}
			break;
		case InputAction.UI_Toggle_UseInventory:
		case InputAction.UI_Toggle_Pick_Up:
		case InputAction.MouseLeft:
		case InputAction.MouseRight:
		case InputAction.MouseMiddle:
		case InputAction.Autorun:
			break;
		}
	}

	public int LevelReqForAction(InputAction action)
	{
		if (!areaData.HasPvp && !areaData.IsPvpLobby)
		{
			_ = entities.me.DuelOpponentID;
			_ = 0;
		}
		return 0;
	}

	private void CustomItemAction(InputAction action)
	{
		int num = LevelReqForAction(action);
		if (num > entities.me.Level)
		{
			Notification.ShowWarning("Item slot unlocked at Level " + num);
			return;
		}
		InventoryItem customActionItem = UIGame.Instance.ActionBar.GetCustomActionItem(action);
		if (customActionItem != null)
		{
			SendItemUseRequest(customActionItem);
		}
	}

	private void InitializeUI()
	{
		UIGame.Init(base.transform);
		uigame = UIGame.Instance;
		uigame.ActionBar.Init(combat, entities.me);
		uigame.PortraitPlayer.Target = entities.me;
		CloseAllPortraits();
		CombatPopup.Init();
		ProjectionController.Init();
		ping.SendPingRequest();
		entities.me.TargetUpdateEvent += OnPlayerTargetUpdate;
		entities.me.TargetSelected += OnPlayerTargetSelected;
		entities.me.TargetDeselected += OnPlayerTargetDeselected;
		entities.me.DeathEvent += OnDeathEvent;
		entities.me.LevelUpdated += OnLevelUp;
		entities.me.ServerStateChanged += OnPlayerServerStateUpdate;
		entities.me.TargetNodeUpdated += OnTargetNodeUpdate;
		if (entities.me.HealthPercent == 0f)
		{
			OnDeathEvent(null);
		}
		foreach (Response delayedUIResponse in DelayedUIResponses)
		{
			OnReceiveResponse(delayedUIResponse);
		}
		DelayedUIResponses.Clear();
	}

	public void AddDelayedResponse(Response response)
	{
		DelayedUIResponses.Add(response);
	}

	private void Update()
	{
		if (!isReady)
		{
			return;
		}
		InputManager.Update();
		foreach (NPC npc in entities.NpcList)
		{
			npc.Update();
		}
		foreach (Player player in entities.PlayerList)
		{
			player.Update();
		}
		if (!UICamera.inputHasFocus && entities.me != null)
		{
			ProcessShortcuts();
		}
		UpdateCursor();
		UpdateDraggable();
	}

	private void OnHover(GameObject go, bool isHovering)
	{
	}

	public InteractableRaycastResult GetInteractableFromRaycast(Vector2 screenPosition)
	{
		Ray ray = cam.ScreenPointToRay(screenPosition);
		foreach (RaycastHit item in from x in Physics.RaycastAll(ray, Max_Click_Distance + camController.cameraDistanceCurrent, Layers.LAYER_MASK_MOUSECLICK)
			where x.collider.GetComponent<IInteractable>() != null
			orderby x.collider.GetComponent<IInteractable>().GetPriority() descending, x.distance
			select x)
		{
			IInteractable[] components = item.collider.GetComponents<IInteractable>();
			foreach (IInteractable obj in components)
			{
				InteractableRaycastResult interactableRaycastResult = new InteractableRaycastResult(obj, ray, item, screenPosition);
				if (obj.IsInteractable(interactableRaycastResult))
				{
					return interactableRaycastResult;
				}
			}
		}
		return new InteractableRaycastResult(null, ray, default(RaycastHit), screenPosition);
	}

	private void UpdateCursor()
	{
		HoverInteractableCast = null;
		if (!Platform.IsDesktop || !cam.enabled)
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
			return;
		}
		if (UIGame.ControlScheme != 0)
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
			return;
		}
		if (UICamera.hoveredObject != null && UICamera.hoveredObject.name != "UICamera" && UICamera.hoveredObject.layer == Layers.NGUI)
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
			return;
		}
		InteractableRaycastResult interactableFromRaycast = GetInteractableFromRaycast(Input.mousePosition);
		if (interactableFromRaycast != null && interactableFromRaycast.HitInteractable != null)
		{
			HoverInteractableCast = interactableFromRaycast;
			interactableFromRaycast.HitInteractable.OnHover();
		}
		else
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
		}
	}

	private void ProcessShortcuts()
	{
		switch (entities.me.AccessLevel)
		{
		case 101:
			ProcessAdminShortcuts();
			break;
		case 100:
			ProcessAdminShortcuts();
			break;
		case 60:
			ProcessModeratorShortcuts();
			break;
		case 50:
			ProcessTesterShortcuts();
			break;
		case 55:
			ProcessTesterShortcuts();
			break;
		}
	}

	private void ProcessAdminShortcuts()
	{
		if (Input.GetKey(KeyCode.F11))
		{
			foreach (Transform item in entities.me.wrapper.transform)
			{
				if (item.name != "Main Camera")
				{
					item.gameObject.SetActive(value: false);
				}
			}
		}
		if (Input.GetKey(KeyCode.F12))
		{
			foreach (Transform item2 in entities.me.wrapper.transform)
			{
				if (item2.name != "Main Camera")
				{
					item2.gameObject.SetActive(value: true);
				}
			}
		}
		if (IsShiftPressed)
		{
			if (Input.GetKeyUp(KeyCode.P))
			{
				Chat.Notify(areaData.cellMap[areaData.currentCellID].EnvPrefab, InterfaceColors.Chat.Red.ToBBCode());
				Chat.Notify(areaData.cellMap[areaData.currentCellID].CellPrefab, InterfaceColors.Chat.Red.ToBBCode());
			}
			if (Input.GetKeyUp(KeyCode.T))
			{
				DeveloperModeToggle();
			}
		}
		if (IsShiftPressed && IsCtrlPressed)
		{
			if (Input.GetKeyDown(KeyCode.L) && Application.isEditor)
			{
				Logout();
			}
			int num = -1;
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				num = 1;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				num = 2;
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				num = 3;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				num = 4;
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				num = 5;
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				num = 6;
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				num = 7;
			}
			if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				num = 8;
			}
			if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				num = 9;
			}
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				num = 0;
			}
			if (num != -1)
			{
				Chat.Instance.OnChatSubmit(SettingsManager.GetMacroText(num));
			}
		}
	}

	public void DeveloperModeToggle()
	{
		TesterMode = !TesterMode;
		if (TesterMode)
		{
			Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Triggers");
			Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("SpawnEditor");
		}
		else
		{
			Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Triggers"));
			Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("SpawnEditor"));
		}
	}

	private void ProcessModeratorShortcuts()
	{
	}

	private void ProcessTesterShortcuts()
	{
		if (!IsShiftPressed)
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.P))
		{
			Chat.Notify(areaData.cellMap[areaData.currentCellID].EnvPrefab, InterfaceColors.Chat.Red.ToBBCode());
			Chat.Notify(areaData.cellMap[areaData.currentCellID].CellPrefab, InterfaceColors.Chat.Red.ToBBCode());
		}
		if (Input.GetKeyUp(KeyCode.T))
		{
			TesterMode = !TesterMode;
			if (TesterMode)
			{
				Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Triggers");
			}
			else
			{
				Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Triggers"));
			}
		}
	}

	public void OnPlayerTargetUpdate(Entity e)
	{
		UpdateTargetPortrait(e);
		TargetReticle.Target = e;
		combat.IsAA = false;
		if (e != null && e.react != 0)
		{
			camController.ResetTargetLock();
		}
	}

	public void OnTargetNodeUpdate(ResourceNode node)
	{
		UpdateTargetNodePortrait(node);
		TargetReticle.Target = null;
		camController.ResetTargetLock();
	}

	public void UpdateTargetPortrait(Entity entity)
	{
		CloseAllPortraits();
		if (entity == null)
		{
			return;
		}
		if (entity is Player)
		{
			UIGame.Instance.PortraitTarget.Target = entity;
		}
		else if (entities.me.CanAttack(entity))
		{
			if (entity.isBoss)
			{
				UIGame.Instance.PortraitBoss.Target = entity;
			}
			else
			{
				UIGame.Instance.PortraitMob.Target = entity;
			}
		}
		else
		{
			UIGame.Instance.PortraitNPC.Target = entity;
		}
	}

	public void UpdateTargetNodePortrait(ResourceNode node)
	{
		CloseAllPortraits();
		if (!(node == null))
		{
			UIGame.Instance.PortraitResourceNode.Init(node);
		}
	}

	private void CloseAllPortraits()
	{
		if (UIGame.Instance != null)
		{
			UIGame.Instance.PortraitTarget?.Close();
			UIGame.Instance.PortraitMob?.Close();
			UIGame.Instance.PortraitBoss?.Close();
			UIGame.Instance.PortraitNPC?.Close();
			UIGame.Instance.PortraitResourceNode?.Close();
		}
	}

	private void OnPlayerTargetSelected(Entity e)
	{
		e.SelectedByPlayer();
	}

	private void OnPlayerTargetDeselected(Entity e)
	{
		e.DeselectedByPlayer();
	}

	private void OnDeathEvent(Entity e)
	{
		UIRespawnTimer.Show();
		cam.GetComponent<Grayscale>().enabled = true;
		combat.ClearQueuedSpell();
	}

	private void OnLevelUp()
	{
		UpdateAreaQuest();
	}

	private void OnPlayerServerStateUpdate(Entity.State previousState, Entity.State newState)
	{
		if (newState != Entity.State.Dead)
		{
			cam.GetComponent<Grayscale>().enabled = false;
		}
		if (previousState == Entity.State.InCombat && newState == Entity.State.Normal)
		{
			CombatPopup.PlayMessagePopup(entities.me.wrapper.transform.position, "Exit Combat", isMe: true);
		}
	}

	private IEnumerator WaitForLoaderToShow(Action callback)
	{
		Loader.show("Clearing Map...", 0f);
		yield return new WaitForEndOfFrame();
		yield return null;
		callback?.Invoke();
	}

	private void CellJoin()
	{
		Debug.Log(" Game.CellJoin() ");
		isReady = (isInputEnabled = false);
		Loader.show("Clearing Map...", 0f);
		ClearCurrentCell();
		StartCoroutine(moveToNextCell());
	}

	private void ClearCurrentTrack()
	{
		if (CurrentSoundTrackID > 0)
		{
			CurrentSoundTrackID = 0;
			UnityEngine.Object.Destroy(CurrentSoundTackGO);
			sountrackloader.Dispose();
		}
	}

	private void ClearCurrentCell()
	{
		UIGame.Reset();
		UIManager.Reset();
		IsUIVisible = true;
		NPCCamController.Close();
		disableMainCamera();
		HousingManager.Clear();
		camController.Clear();
		if (entities.me != null)
		{
			entities.me.Target = null;
			entities.me.TargetNode = null;
			entities.me.DisableControl();
		}
		if (areaData != null)
		{
			entities.FilterByCell(areaData.currentCellID);
		}
		transferPads.Clear();
		UsedMachines.Clear();
		BaseMachine.Map.Clear();
		MachineListener.Map.Clear();
		ClientTrigger.Map.Clear();
		LootBags.Clear();
		UnityEngine.Object.Destroy(cellGO);
		UnityEngine.Object.Destroy(entitiesGO);
		cellGO = (entitiesGO = null);
		field.Reset();
		LightmapSettings.lightProbes = null;
		LightmapSettings.lightmaps = null;
		StopAllCoroutines();
		maploader?.Dispose();
		Resources.UnloadUnusedAssets();
		Loader.show("Loading Map...", 0f);
	}

	private IEnumerator moveToNextCell()
	{
		Max_Click_Distance = 40f;
		ClientMovementController clientMovementController = Entities.Instance.me.moveController as ClientMovementController;
		if (clientMovementController != null)
		{
			clientMovementController.IsAutoRunEnabled = false;
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_AttemptMoveToNextCell);
		Debug.Log("moveToNextCell()" + areaData.currentCellID);
		if (!areaData.cellMap.ContainsKey(areaData.currentCellID))
		{
			Debug.LogError("No cell with ID '" + areaData.currentCellID + "' found.");
		}
		Notification.Clear();
		FancyNotification.Clear();
		yield return StartCoroutine(SoundTracks.Instance.Play(areaData.SoundTrackID, showLoader: true));
		Loader.show("Loading Map...", 0f);
		yield return new WaitForEndOfFrame();
		float start = GameTime.realtimeSinceServerStartup;
		CellData cellData = areaData.cellMap[areaData.currentCellID];
		maploader = AssetBundleManager.LoadAssetBundle(cellData.BundleInfo);
		while (!maploader.isDone)
		{
			Loader.show("Loading Map...", maploader.GetProgress());
			yield return null;
		}
		if (!string.IsNullOrEmpty(maploader.Error))
		{
			Debug.LogError(maploader.Error);
			MessageBox.Show("Error", "Map asset bundle could not be loaded. Please clear your cache and try again. Teleport to Battleon?", "OK", delegate
			{
				SendEndTranferRequest();
				SendAreaJoinCommand("battleon");
			});
			yield break;
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_LoadMapBundleOk);
		cellGO = new GameObject("cellGO");
		cellGO.transform.SetParent(baseTF, worldPositionStays: false);
		cellGO.SetActive(value: false);
		entitiesGO = new GameObject("entitiesGO");
		entitiesGO.transform.SetParent(baseTF, worldPositionStays: false);
		Debug.Log("Game.buildCurrentCell() 1 >> " + (GameTime.realtimeSinceServerStartup - start));
		string[] cellAssets = new string[2] { cellData.EnvPrefab, cellData.CellPrefab };
		for (int index = 0; index < cellAssets.Length; index++)
		{
			string cellAssetName = cellAssets[index];
			AssetBundleRequest abr = maploader.Asset.LoadAssetAsync<GameObject>(cellAssetName);
			while (!abr.isDone)
			{
				Loader.show("Loading Level...", (float)index / (float)cellAssets.Length + abr.progress / (float)cellAssets.Length);
				yield return null;
			}
			if (abr.asset == null)
			{
				Debug.LogError("Error: '" + cellAssetName + "' not found in assetbundle.");
				Debug.LogException(new Exception("Game.cs - buildCurrentCell, Cell Asset not found: " + cellAssetName));
				MessageBox.Show("Error", "Map prefab could not be loaded from the bundle. Please clear your cache and try again. Teleport to Battleon?", "OK", delegate
				{
					SendEndTranferRequest();
					SendAreaJoinCommand("battleon");
				});
				yield break;
			}
			Debug.Log(cellAssetName + " AssetBundle.Load >> " + (GameTime.realtimeSinceServerStartup - start));
			((GameObject)UnityEngine.Object.Instantiate(abr.asset, cellGO.transform)).name = cellAssetName;
			Debug.Log(cellAssetName + " instantiated >> " + (GameTime.realtimeSinceServerStartup - start));
		}
		Debug.Log("Game.buildCurrentCell() 2 >> " + (GameTime.realtimeSinceServerStartup - start));
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_InstantiateMapOk);
		cellGO.transform.Find(cellData.EnvPrefab).gameObject.AddComponent<GrassToggle>();
		CurrentCell = cellGO.GetComponentInChildren<DataPrefab>();
		if (CurrentCell == null)
		{
			Debug.LogException(new Exception("Game.cs - buildCurrentCell, CurrentCell not found!"));
			yield break;
		}
		cellGO.SetActive(value: true);
		Debug.Log("Game.buildCurrentCell() 7 >> " + (GameTime.realtimeSinceServerStartup - start));
		UIMapEditor.go_dict = new Dictionary<int, GameObject>();
		if (Session.MyPlayerData.IsQuestAvailable(areaData.SagaQuest) && areaData.id == areaData.SagaQuest.MapID && (areaData.SagaQuest.QSValue == 0 || !Session.MyPlayerData.HasQuest(areaData.SagaQuest.ID)) && DialogueSlotManager.ReloadID == -1)
		{
			yield return DialogueSlotManager.Instance.PreLoad(areaData.SagaQuest.SagaDialogID);
		}
		try
		{
			if (Instance.AreaData.dbSpawns != null)
			{
				UnityEngine.Object.Destroy(Instance.AreaData.dbSpawns);
			}
			Instance.AreaData.dbSpawns = new GameObject("DB");
			Instance.AreaData.dbSpawns.transform.parent = cellGO.transform.Find(cellData.CellPrefab);
			foreach (ComMapEntity mapEntityPlayerSpawner in Instance.AreaData.mapEntityPlayerSpawners)
			{
				UIMapEditor.CreatePlayerSpawnerPlatformGO(Instance.AreaData.dbSpawns, mapEntityPlayerSpawner);
			}
			foreach (ComMapEntity mapEntityTransferPad in Instance.AreaData.mapEntityTransferPads)
			{
				StartCoroutine(UIMapEditor.LoadEntityAsset(Instance.AreaData.dbSpawns, mapEntityTransferPad));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
		try
		{
			TransferPadControl[] componentsInChildren = cellGO.GetComponentsInChildren<TransferPadControl>();
			foreach (TransferPadControl transferPadControl in componentsInChildren)
			{
				transferPads.Add(transferPadControl);
				foreach (LocatorTransfer locatorTransfer in LocatorTransfers)
				{
					if (!(transferPadControl.UniqueID == locatorTransfer.UniqueID))
					{
						continue;
					}
					foreach (int area in locatorTransfer.Areas)
					{
						if (!transferPadControl.Areas.Contains(area))
						{
							transferPadControl.Areas.Add(area);
						}
					}
					break;
				}
			}
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_TransferPadsOk);
			NPCSpawn.Init(cellGO.GetComponentsInChildren<NPCSpawn>());
			foreach (ComMachine comMachine in ComMachines)
			{
				if (BaseMachine.Map.ContainsKey(comMachine.ID))
				{
					BaseMachine baseMachine = BaseMachine.Map[comMachine.ID];
					if (comMachine.HasResource && baseMachine is ResourceMachine resourceMachine)
					{
						resourceMachine.SpawnNode(comMachine.TotalUsages, comMachine.Usages, comMachine.NodeID, comMachine.Rarity, comMachine.TradeSkillLevel, comMachine.JsonRequirements, comMachine.DropIDs);
					}
					if (baseMachine is HarpoonMachine harpoonMachine)
					{
						harpoonMachine.Sync(comMachine.VerticalRotation, comMachine.HorizontalRotation, snap: true);
					}
					_ = baseMachine is CapturePointMachine;
					baseMachine.Init(comMachine.State, comMachine.OwnerID, comMachine.Areas);
				}
				else
				{
					Debug.LogWarning("# " + comMachine.ID + " : Machine not found!");
				}
			}
			Dictionary<Transform, List<MachineListener>> dictionary = new Dictionary<Transform, List<MachineListener>>();
			foreach (ComMachineListener comMachineListener in ComMachineListeners)
			{
				if (!MachineListener.Map.ContainsKey(comMachineListener.ID))
				{
					continue;
				}
				MachineListener machineListener = MachineListener.Map[comMachineListener.ID];
				machineListener.UpdateServerTimeStamp(comMachineListener.timeStampBegin);
				if (machineListener is ITimedListener)
				{
					Transform targetTransform = machineListener.TargetTransform;
					if (!dictionary.ContainsKey(targetTransform))
					{
						dictionary.Add(targetTransform, new List<MachineListener>());
					}
					dictionary[targetTransform].Add(machineListener);
				}
			}
			foreach (List<MachineListener> value in dictionary.Values)
			{
				List<MachineListener> list = new List<MachineListener>();
				List<MachineListener> list2 = new List<MachineListener>();
				List<MachineListener> list3 = new List<MachineListener>();
				foreach (MachineListener item in value)
				{
					(item as ITimedListener).Init();
					if (item is MachineActiveMoveOverTime || item is MachineStateMoveOverTime || item is MachineActiveMove || item is MachineStateMove)
					{
						list.Add(item);
					}
					else if (item is MachineActiveRotateOverTime || item is MachineStateRotateOverTime || item is MachineActiveRotate || item is MachineStateRotate)
					{
						list2.Add(item);
					}
					else
					{
						list3.Add(item);
					}
				}
				List<MachineListener> list4 = list.OrderBy((MachineListener x) => (x as ITimedListener).LastServerTimestamp).ToList();
				List<MachineListener> list5 = list2.OrderBy((MachineListener x) => (x as ITimedListener).LastServerTimestamp).ToList();
				List<MachineListener> list6 = list3.OrderBy((MachineListener x) => (x as ITimedListener).LastServerTimestamp).ToList();
				foreach (MachineListener item2 in list4)
				{
					(item2 as ITimedListener).SyncToServer(item2 == list4.Last());
				}
				foreach (MachineListener item3 in list5)
				{
					(item3 as ITimedListener).SyncToServer(item3 == list5.Last());
				}
				foreach (MachineListener item4 in list6)
				{
					(item4 as ITimedListener).SyncToServer(item4 == list6.Last());
				}
			}
			(from p in cellGO.GetComponentsInChildren<TimeSynchronizationClientTrigger>()
				where p.IsWithinEventDuration
				orderby p.EventTime
				select p).ToList().ForEach(delegate(TimeSynchronizationClientTrigger p)
			{
				p.Trigger(checkRequirements: true);
			});
			Debug.Log("Game.buildCurrentCell() 9 >> " + (GameTime.realtimeSinceServerStartup - start));
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_MachinesOk);
			List<Transform> allChildren = cellGO.transform.GetAllChildren();
			INonStatic[] componentsInChildren2 = cellGO.GetComponentsInChildren<INonStatic>();
			INonStatic[] array = componentsInChildren2;
			foreach (INonStatic obj in array)
			{
				obj.parent = obj.transformParent;
			}
			array = componentsInChildren2;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transformParent = cellGO.transform;
			}
			Dictionary<Material, List<GameObject>> dictionary2 = new Dictionary<Material, List<GameObject>>();
			foreach (Transform item5 in allChildren)
			{
				Renderer[] componentsInChildren3 = item5.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren3)
				{
					if (!(renderer.sharedMaterial == null))
					{
						if (dictionary2.ContainsKey(renderer.sharedMaterial))
						{
							dictionary2[renderer.sharedMaterial].Add(renderer.gameObject);
							continue;
						}
						dictionary2.Add(renderer.sharedMaterial, new List<GameObject> { renderer.gameObject });
					}
				}
			}
			foreach (KeyValuePair<Material, List<GameObject>> item6 in dictionary2)
			{
				StaticBatchingUtility.Combine(item6.Value.ToArray(), cellGO);
			}
			array = componentsInChildren2;
			foreach (INonStatic obj2 in array)
			{
				obj2.transformParent = obj2.parent;
			}
			Spawns.Clear();
			NPCSpawn[] componentsInChildren4 = cellGO.GetComponentsInChildren<NPCSpawn>(includeInactive: true);
			foreach (NPCSpawn nPCSpawn in componentsInChildren4)
			{
				Spawns[nPCSpawn.ID] = nPCSpawn;
			}
			if (AreaData.spawnMetas.Count > 0)
			{
				foreach (ComSpawnMeta spawnMeta in AreaData.spawnMetas)
				{
					if (!spawnMeta.IsDB)
					{
						continue;
					}
					UINPCEditor.CreateSpawner(spawnMeta.SpawnID, new Vector3(spawnMeta.Path.First().Value.x, spawnMeta.Path.First().Value.y, spawnMeta.Path.First().Value.z), spawnMeta.RotationY.FirstOrDefault().Value, spawnMeta.AutoSpawn);
					Spawns[spawnMeta.SpawnID] = NPCSpawn.Map[spawnMeta.SpawnID];
					UINPCEditor.CreateRequirements(spawnMeta.SpawnID, spawnMeta.Requirements);
					foreach (KeyValuePair<int, ComVector3> item7 in spawnMeta.Path)
					{
						if (item7.Key != spawnMeta.Path.First().Key)
						{
							UINPCEditor.CreatePathNode(spawnMeta.SpawnID, item7.Key, new Vector3(item7.Value.x, item7.Value.y, item7.Value.z));
						}
					}
					UINPCEditor.LinkPaths(spawnMeta.SpawnID);
				}
			}
			new List<int>();
			List<int> list7 = entities.NpcList.FindAll((NPC npc) => npc.ApopIDs != null).SelectMany((NPC npc) => npc.ApopIDs).Distinct()
				.ToList();
			List<int> collection = cellGO.GetComponentsInChildren<CTANPCIA>(includeInactive: true).ToList().SelectMany((CTANPCIA ctan) => ctan.ApopIds)
				.ToList();
			list7.AddRange(collection);
			ApopDownloader.GetApops(list7, null);
			foreach (NPC npc in entities.NpcList)
			{
				npc.Init(entitiesGO.transform, Spawns.ContainsKey(npc.SpawnID) ? Spawns[npc.SpawnID] : null, cam);
				npc.Load();
			}
			foreach (ComLoot comLootBag in ComLootBags)
			{
				AddLoot(comLootBag);
			}
			Debug.Log("Game.buildCurrentCell() *>*> Found " + entities.NpcList.Count() + " NPCs");
			Debug.Log("Game.buildCurrentCell() 6 >> " + (GameTime.realtimeSinceServerStartup - start));
			Light[] array2 = UnityEngine.Object.FindObjectsOfType<Light>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].cullingMask &= ~(1 << Layers.UI3D);
			}
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_NpcsOk);
			Player player = Entities.Instance?.me;
			if (player != null)
			{
				initMe(player);
			}
			else
			{
				Debug.LogWarning("Player entity is null");
			}
			initPlayers();
			enableAllPlayers();
			UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_PlayersOk);
			enableMainCamera();
			Debug.Log("Game ready!");
			Game game = this;
			Game game2 = this;
			bool flag = true;
			game2.isInputEnabled = true;
			game.isReady = flag;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		if (areaData.HouseJoinData != null)
		{
			yield return HousingManager.HandleHouseJoin(areaData.HouseJoinData.hItems, areaData.HouseJoinData.hItemInfos, areaData.HouseJoinData.hItemCounts, areaData.HouseJoinData.hData);
		}
		else
		{
			UIGame.Instance.HouseControls.HideControls();
		}
		foreach (ComMapEntity mapEntityMachine in Instance.AreaData.mapEntityMachines)
		{
			if (mapEntityMachine == null || mapEntityMachine.Bundle == null)
			{
				Debug.LogError("Error loading ComMapEntity!");
			}
			else if (string.IsNullOrEmpty(mapEntityMachine.Bundle.FileName))
			{
				Debug.LogError("Machine bundle filename not set!");
			}
			else
			{
				yield return StartCoroutine(UIMapEditor.LoadEntityAsset(Instance.AreaData.dbSpawns, mapEntityMachine));
			}
		}
		SendEndTranferRequest();
		if (CurrentCell != null)
		{
			if (CurrentCell.maxClickDistance > 0f)
			{
				Max_Click_Distance = CurrentCell.maxClickDistance;
			}
			camController.SetupCellCamera(CurrentCell.MinCameraDistance, CurrentCell.MaxCameraDistance, CurrentCell.intensity, CurrentCell.focalLength, CurrentCell.focalSize, CurrentCell.aperture);
			camController.SetupCellParticles(CurrentCell.ParticleGroup, CurrentCell.PlayOnStart);
			if (CurrentCell.UseCustomCulling)
			{
				cam.layerCullDistances = Layers.GetAdjustedCullDistances(CurrentCell.CustomLayers.CullDistanceMin(), CurrentCell.CustomLayers.CullDistanceMax());
			}
			else
			{
				cam.layerCullDistances = Layers.GetAdjustedCullDistances(Layers.MinCullDistances, Layers.MaxCullDistances);
			}
			cam.farClipPlane = CurrentCell.CameraFarPlane;
			if (string.IsNullOrEmpty(areaData.UnlockBitFlagName) || areaData.UnlockBitFlagIndex == 0 || !Session.MyPlayerData.CheckBitFlag(areaData.UnlockBitFlagName, (byte)areaData.UnlockBitFlagIndex))
			{
				if (areaData.id == 1)
				{
					uigame.gameMenuBar.ShowStarterPackOffer();
				}
				SendBitFlagUpdateRequest(areaData.UnlockBitFlagName, (byte)areaData.UnlockBitFlagIndex, value: true);
				if (!string.IsNullOrEmpty(CurrentCell.FlythroughName))
				{
					if (int.TryParse(CurrentCell.FlythroughName, out var result))
					{
						DialogueSlotManager.Show(result, OnCellLoaded);
					}
					else
					{
						DialogueSlotManager.Show(CurrentCell.FlythroughName, OnCellLoaded);
					}
					yield break;
				}
			}
			if (DialogueSlotManager.ReloadID >= 0)
			{
				DialogueSlotManager.Show(DialogueSlotManager.ReloadID, skipTo: DialogueSlotManager.ReloadFrame, Callback: OnCellLoaded);
				DialogueSlotManager.ReloadID = -1;
				yield break;
			}
			if (DialogueSlotManager.HasLoadError())
			{
				DialogueSlotManager.ShowLoadErrorMsg();
				DialogueSlotManager.Close();
			}
			else if (DialogueSlotManager.HasPreloadedDialog())
			{
				yield return UIFade.Instance.FadeOut();
				DialogueSlotManager.LateShow(OnCellLoaded);
				UIFade.Instance.FadeIn();
				yield break;
			}
		}
		Loader.close();
		OnCellLoaded();
	}

	private void OnCellLoaded()
	{
		EnablePlayerController();
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_AttemptOnCellLoaded);
		UIPvpMatchScreen.Instance.Hide();
		uigame.WarMeter.Visible = Wars.IsActive;
		uigame.scoreBoard.Visible = Scoreboard.IsActive;
		uigame.speedrunTimer.root.SetActive(value: false);
		if (Entities.Instance.me.serverState != Entity.State.Dead)
		{
			UIRespawnTimer.Hide();
		}
		if (areaData.HasPvp)
		{
			Entities.Instance.me.CheckPvpState();
			uigame.pvpScore.GetComponent<UIPvPScore>().Show();
		}
		if (areaData.cellTimerDuration > 0f)
		{
			uigame.cellTimer.Init(AreaData.cellTimerDescription, AreaData.cellTimerDuration, AreaData.isCellTimerPaused);
		}
		else
		{
			uigame.cellTimer.Stop();
		}
		if (areaData.isDungeon)
		{
			string objective = areaData.cellMap[areaData.currentCellID].Objective;
			if (!string.IsNullOrEmpty(objective))
			{
				Notify("Objective: " + objective);
			}
		}
		if (Session.pendingGoto != null)
		{
			SendGotoRequest(Session.pendingGoto.CharID, Session.pendingGoto.Code);
			Session.pendingGoto = null;
		}
		OnShowSagaQuest = (Action<Quest>)Delegate.Combine(OnShowSagaQuest, new Action<Quest>(ShowQuestAfterSagaDialog));
		List<int> list = areaData.quests.Where((int p) => !Quests.HasKey(p)).ToList();
		if (list.Count > 0)
		{
			SendQuestLoadRequest(list);
		}
		else
		{
			UpdateAreaQuest();
		}
		if (firstCell)
		{
			firstCell = false;
			SendFriendListRequest();
			if (Session.IsGuest && !LoginManager.firstGuestSession)
			{
				ShowAskGuestToConvert();
			}
			else
			{
				UINews.LoadNewsApop();
				ApopMap.Cleared -= UINews.OnApopMapCleared;
				ApopMap.Cleared += UINews.OnApopMapCleared;
			}
			IAPStore.Init();
		}
		if (IsIntroMap && entities.me.AccessLevel == 0)
		{
			uigame.Chat.gameObject.SetActive(value: false);
		}
		else if (IsIntroMap)
		{
			Session.MyPlayerData.GetQSValue(51);
		}
		if (Session.MyPlayerData.GetGameParam("FeedbackTrigger") == "BattleonLand")
		{
			if (areaData != null && areaData.id == 1 && !Session.MyPlayerData.CheckBitFlag("ic2", 28))
			{
				Invoke("ShowFeedbackConfirmation", 2f);
			}
		}
		else if (Session.MyPlayerData.GetGameParam("FeedbackTrigger") == "BattleonComplete" && areaData != null && areaData.id == 353 && !Session.MyPlayerData.CheckBitFlag("ic2", 28))
		{
			Invoke("ShowFeedbackConfirmation", 2f);
		}
		UserTracking.Instance.RecordUserEvent(UserTracking.UserEvent.Game_OnCellLoadedOk);
		PlayerSpawn[] array = UnityEngine.Object.FindObjectsOfType(typeof(PlayerSpawn)) as PlayerSpawn[];
		foreach (PlayerSpawn playerSpawn in array)
		{
			MeshRenderer component = playerSpawn.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.enabled = false;
				UIMapEditor.AddPlayerSpawnerPlatformMesh(playerSpawn.gameObject);
			}
		}
		NPCSpawn[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(NPCSpawn)) as NPCSpawn[];
		foreach (NPCSpawn nPCSpawn in array2)
		{
			MeshRenderer component2 = nPCSpawn.GetComponent<MeshRenderer>();
			if (component2 != null)
			{
				component2.enabled = false;
				UINPCEditor.AddNpcSpawnerPlatformMesh(nPCSpawn.gameObject);
			}
		}
	}

	private void ShowFeedbackConfirmation()
	{
		Confirmation.Show("AQ3D Feedback", "Hello! Thank you for playing AQ3D! Please help us out by taking a very short survey to tell us about your experience so far.", delegate(bool b)
		{
			if (b)
			{
				ShowFeedBackForm();
			}
		});
		Instance.SendBitFlagUpdateRequest("ic2", 28, value: true);
	}

	private void ShowFeedBackForm()
	{
		Application.OpenURL(string.IsNullOrEmpty(Session.MyPlayerData.GetGameParam("FeedbackURL")) ? "https://forms.gle/xG1s7tWLU29qTeB89" : ("https://" + Session.MyPlayerData.GetGameParam("FeedbackURL")));
	}

	private void initMe(Player me)
	{
		if (me == null)
		{
			return;
		}
		if (me.wrapper == null)
		{
			GameObject gameObject = new GameObject("playerWrapper_" + me.ID);
			gameObject.transform.position = me.spawnPostion;
			gameObject.transform.rotation = me.spawnRotation;
			gameObject.transform.parent = baseTF;
			gameObject.layer = Layers.PLAYER_ME;
			me.SetWrapper(gameObject);
			me.SetCamera(cam);
			me.BuildController();
			me.BuildNamePlate();
			me.PlayEmote(me.stateEmote);
			camController.Init(gameObject.transform);
			camController.enabled = true;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<AudioListener>());
			GameObject gameObject2 = new GameObject("Audio Listener");
			gameObject2.transform.parent = baseTF;
			if (Camera.main?.transform != null)
			{
				gameObject2.AddComponent<CameraPlayerBlend>().mainCamera = Camera.main.transform;
			}
			gameObject2.transform.localPosition = Vector3.zero;
			((OmniMovementController)me.moveController).SetCamController(camController);
			combat.SetMoveController(me.moveController as OmniMovementController);
			InitializeUI();
		}
		else
		{
			me.wrapper.transform.position = me.spawnPostion;
			me.wrapper.transform.rotation = me.spawnRotation;
		}
	}

	private void initPlayers()
	{
		Debug.Log("Game.initPlayers() > Player count : " + entities.PlayerList.Count());
		foreach (Player item in entities.PlayerList.Where((Player x) => !x.isMe))
		{
			if (item.wrapper == null)
			{
				GameObject gameObject = new GameObject("playerWrapper_" + item.ID);
				gameObject.transform.position = item.spawnPostion;
				gameObject.transform.rotation = item.spawnRotation;
				gameObject.transform.parent = baseTF.Find("entitiesGO").transform;
				gameObject.layer = Layers.OTHER_PLAYERS;
				item.SetWrapper(gameObject);
				item.SetCamera(cam);
				item.BuildController();
				item.BuildNamePlate();
				item.PlayEmote(item.stateEmote);
			}
			else
			{
				item.wrapper.transform.position = item.spawnPostion;
				item.wrapper.transform.rotation = item.spawnRotation;
			}
		}
	}

	private void enableAllPlayers()
	{
		foreach (Player player in entities.PlayerList)
		{
			player.EnableControl();
		}
	}

	public void enableMainCamera()
	{
		cam.enabled = true;
		camController.enabled = true;
		camController.Reset();
		cameraWaterRaycast.SetFogProperties();
		cameraWaterRaycast.SetFogToNormal();
	}

	public void disableMainCamera()
	{
		if (camController != null)
		{
			camController.enabled = false;
		}
		CursorManager.Instance.SetCursor(CursorManager.Icon.Default);
	}

	public void SetObjectParentToBaseTF(GameObject GO)
	{
		GO.transform.SetParent(baseTF.transform, worldPositionStays: true);
	}

	private void AddLoot(ComLoot bag)
	{
		if (bag.Items != null && bag.Items.Count != 0)
		{
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("ChestWithParticles"));
			obj.name = "DropBag";
			obj.transform.parent = cellGO.transform;
			obj.transform.position = bag.Position;
			obj.GetComponent<LootChest>().Init(bag);
		}
	}

	public void Notify(string message, GameNotificationType type = GameNotificationType.Both)
	{
		if ((type & GameNotificationType.Chat) == GameNotificationType.Chat)
		{
			Chat.Notify(message);
		}
		if ((type & GameNotificationType.Standard) == GameNotificationType.Standard)
		{
			Notification.ShowText(message);
		}
	}

	private void CurrentlyTrackedQuestUpdated(Quest quest)
	{
		UpdateAreaQuest();
	}

	public void ShowQuestAfterSagaDialog(Quest quest)
	{
		if (quest != null && areaData != null && areaData.id == quest.MapID && areaData.SagaID == quest.QSIndex && quest.SagaDialogID > 0)
		{
			UIQuest.ShowLog(quest);
			OnShowSagaQuest = null;
		}
	}

	private void UpdateAreaQuest(bool autoAccept = true)
	{
		if (areaData == null)
		{
			return;
		}
		Quest quest = null;
		List<Quest> sagaQuests = Quests.GetSagaQuests(areaData.SagaID);
		foreach (Quest item in sagaQuests.Where((Quest p) => p.Auto))
		{
			if (Session.MyPlayerData.IsQuestAvailable(item) && !Session.MyPlayerData.HasQuest(item.ID))
			{
				if (Session.MyPlayerData.HasCurrentlyTrackedQuest || !Session.MyPlayerData.IsQuestAcceptable(item))
				{
					quest = item;
				}
				else if (autoAccept)
				{
					SendQuestAcceptRequest(item.ID);
					Session.MyPlayerData.ClearTrackedQuest();
				}
				break;
			}
			if (Session.MyPlayerData.IsQuestAvailable(item) && Session.MyPlayerData.HasQuest(item.ID))
			{
				if (Session.MyPlayerData.CurrentlyTrackedQuest == null)
				{
					Session.MyPlayerData.TrackQuest(item.ID);
					OnShowSagaQuest?.Invoke(quest);
					break;
				}
				if (Session.MyPlayerData.CurrentlyTrackedQuest.ID != item.ID)
				{
					quest = item;
					break;
				}
				OnShowSagaQuest?.Invoke(item);
			}
		}
		Session.MyPlayerData.AvailableQuest = quest;
		OnShowSagaQuest?.Invoke(quest);
		if (!Session.MyPlayerData.HasCurrentlyTrackedQuest)
		{
			Quest quest2 = sagaQuests.Where((Quest p) => Session.MyPlayerData.HasQuest(p.ID)).FirstOrDefault();
			if (quest2 != null)
			{
				Session.MyPlayerData.TrackQuest(quest2.ID);
			}
		}
		if (Session.MyPlayerData.HasCurrentlyTrackedQuest)
		{
			return;
		}
		Quest quest3 = null;
		QuestTrackPrority currentPriority = QuestTrackPrority.SideQuests;
		foreach (int curQuest in Session.MyPlayerData.CurQuests)
		{
			Quest quest4 = Quests.Get(curQuest);
			if (quest4 == null)
			{
				continue;
			}
			if (Session.MyPlayerData.IsQuestComplete(curQuest) && quest4.MapEndID == areaData.id)
			{
				TryUpdateCurrentQuest(quest3, ref currentPriority, quest4, QuestTrackPrority.TurnInNpcInCurrentMap);
				continue;
			}
			bool flag = false;
			foreach (QuestObjective objective in quest4.Objectives)
			{
				if (!Session.MyPlayerData.IsQuestObjectiveCompleted(curQuest, objective.ID) && objective.MapID == areaData.id)
				{
					flag = true;
					TryUpdateCurrentQuest(quest3, ref currentPriority, quest4, QuestTrackPrority.ObjectiveInCurrentMap);
					break;
				}
			}
			if (!flag)
			{
				if (quest4.MapID == areaData.id)
				{
					TryUpdateCurrentQuest(quest3, ref currentPriority, quest4, QuestTrackPrority.CurrentMapSideQuests);
				}
				else if (quest4.IsSagaQuest)
				{
					TryUpdateCurrentQuest(quest3, ref currentPriority, quest4, QuestTrackPrority.SagaQuests);
				}
				else
				{
					TryUpdateCurrentQuest(quest3, ref currentPriority, quest4, QuestTrackPrority.SideQuests);
				}
			}
		}
		if (quest3 != null)
		{
			Session.MyPlayerData.TrackQuest(quest3.ID);
		}
	}

	private void TryUpdateCurrentQuest(Quest current, ref QuestTrackPrority currentPriority, Quest candidate, QuestTrackPrority candidatePriority)
	{
		if (current == null || currentPriority < candidatePriority)
		{
			current = candidate;
			currentPriority = candidatePriority;
		}
	}

	public void HandleMachineAnimatorLayerWeight(ResponseMachineAnimatorLayerWeight response)
	{
		NPC npcBySpawnId = entities.GetNpcBySpawnId(response.npcID);
		if (npcBySpawnId == null || !(npcBySpawnId.assetController != null))
		{
			return;
		}
		Animator componentInChildren = npcBySpawnId.assetController.GetComponentInChildren<Animator>();
		if (componentInChildren != null && componentInChildren.GetLayerWeight(response.layerID) != response.weight)
		{
			if (response.crossfade != 0f)
			{
				StartCoroutine(CrossfadeAnimatorLayerWeight(componentInChildren, response));
			}
			else
			{
				componentInChildren.SetLayerWeight(response.layerID, response.weight);
			}
		}
	}

	public void HandleMachineAnimatorParameter(ResponseMachineAnimatorParameter response)
	{
		NPC npcBySpawnId = entities.GetNpcBySpawnId(response.npcID);
		if (npcBySpawnId == null)
		{
			return;
		}
		Animator componentInChildren = npcBySpawnId.assetController.GetComponentInChildren<Animator>();
		if (componentInChildren != null)
		{
			switch (response.animatorParameterType)
			{
			case AnimatorParameterType.Integer:
				componentInChildren.SetInteger(response.parameterName, response.valueInt);
				break;
			case AnimatorParameterType.Float:
				componentInChildren.SetFloat(response.parameterName, response.valueFloat);
				break;
			case AnimatorParameterType.Bool:
				componentInChildren.SetBool(response.parameterName, response.valueBool);
				break;
			case AnimatorParameterType.Trigger:
				componentInChildren.SetTrigger(response.parameterName);
				break;
			}
		}
	}

	public void HandleMachineCast(ResponseMachineCast response)
	{
		Player playerById = Entities.Instance.GetPlayerById(response.CasterId);
		if (playerById == null || playerById.isMe)
		{
			return;
		}
		if (response.MachineId == -1)
		{
			if (playerById.entitycontroller != null)
			{
				playerById.entitycontroller.CancelAction();
			}
		}
		else if (BaseMachine.Map.ContainsKey(response.MachineId))
		{
			ClickMachine clickMachine = (ClickMachine)BaseMachine.Map[response.MachineId];
			if (!(clickMachine == null))
			{
				playerById.PlayAnimation(EntityAnimations.Get(clickMachine.GetCastAnimation()), 0f, isCancellableByMovement: true);
			}
		}
	}

	public void HandleMachineAreaFlag(ResponseMachineAreaFlag response)
	{
		List<KeyValuePair<string, string>> areaFlags = areaData.areaFlags;
		for (int i = 0; i < areaFlags.Count; i++)
		{
			if (areaFlags[i].Key == response.key)
			{
				areaFlags[i] = new KeyValuePair<string, string>(response.key, response.value);
				if (this.AreaFlagUpdated != null)
				{
					this.AreaFlagUpdated(response.key, response.value);
				}
				return;
			}
		}
		areaFlags.Add(new KeyValuePair<string, string>(response.key, response.value));
		if (this.AreaFlagUpdated != null)
		{
			this.AreaFlagUpdated(response.key, response.value);
		}
	}

	public void HandleMachineHarpoonFire(ResponseMachineHarpoonFire response)
	{
		if (BaseMachine.Map.ContainsKey(response.machineID))
		{
			HarpoonMachine harpoonMachine = BaseMachine.Map[response.machineID] as HarpoonMachine;
			if (harpoonMachine != null)
			{
				harpoonMachine.Fire();
			}
		}
	}

	public void HandleMachineHarpoonSync(ResponseMachineHarpoonSync response)
	{
		if (BaseMachine.Map.ContainsKey(response.machineID))
		{
			HarpoonMachine harpoonMachine = BaseMachine.Map[response.machineID] as HarpoonMachine;
			if (harpoonMachine != null)
			{
				harpoonMachine.Sync(response.verticalRotation, response.horizontalRotation);
			}
		}
	}

	public void HandleMachineCollision(ResponseMachineCollision response)
	{
		if (!BaseMachine.Map.ContainsKey(response.machineID))
		{
			return;
		}
		CollideMachine collideMachine = BaseMachine.Map[response.machineID] as CollideMachine;
		if (!(collideMachine == null))
		{
			switch (response.collisionMode)
			{
			case CollisionMode.Enter:
				collideMachine.TriggerCTActionEnter();
				break;
			case CollisionMode.Exit:
				collideMachine.TriggerCTActionExit();
				break;
			case CollisionMode.Stay:
				collideMachine.TriggerCTActionStay();
				break;
			}
		}
	}

	public void HandleMachineListenerUpdate(ResponseMachineListenerUpdate response)
	{
		foreach (ComMachineListener listener in response.listeners)
		{
			if (MachineListener.Map.ContainsKey(listener.ID))
			{
				MachineListener.Map[listener.ID].UpdateServerTimeStamp(listener.timeStampBegin);
			}
		}
	}

	public void HandleMachinePlayAnimation(ResponseMachinePlayAnimation response)
	{
		switch ((MAPlayAnimation.Target)response.target)
		{
		case MAPlayAnimation.Target.Everyone:
		{
			Entity entity3 = entities.GetEntity((Entity.Type)response.entityType, response.id);
			if (entity3 != null && entity3.assetController != null && entity3.assetController.IsAssetLoadComplete)
			{
				entity3.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
			}
			break;
		}
		case MAPlayAnimation.Target.AllFriendlies:
		{
			Entity entity4 = entities.GetEntity((Entity.Type)response.entityType, response.id);
			if (entity4 != null && entity4.assetController != null && entity4.assetController.IsAssetLoadComplete && entity4.react == Entity.Reaction.Friendly)
			{
				entity4.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
			}
			break;
		}
		case MAPlayAnimation.Target.AllHostile:
		{
			Entity entity2 = entities.GetEntity((Entity.Type)response.entityType, response.id);
			if (entity2 != null && (entity2.react == Entity.Reaction.AgroAll || entity2.react == Entity.Reaction.AgroOtherKind || entity2.react == Entity.Reaction.Hostile) && entity2.assetController != null && entity2.assetController.IsAssetLoadComplete)
			{
				entity2.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
			}
			break;
		}
		case MAPlayAnimation.Target.AllPlayers:
		{
			Entity playerById = entities.GetPlayerById(response.id);
			if (playerById != null && playerById.assetController != null && playerById.assetController.IsAssetLoadComplete)
			{
				playerById.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
			}
			break;
		}
		case MAPlayAnimation.Target.TargetNPC:
		{
			NPC npcBySpawnId = entities.GetNpcBySpawnId(response.id);
			if (npcBySpawnId != null && npcBySpawnId.assetController != null)
			{
				if (npcBySpawnId.assetController.IsAssetLoadComplete)
				{
					npcBySpawnId.delayedAnimation = null;
					npcBySpawnId.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
					break;
				}
				EntityAnimation entityAnimation = EntityAnimations.Get(response.animationName);
				entityAnimation.crossfadeSpeed = response.crossfadeDuration;
				entityAnimation.layer = response.layer;
				entityAnimation.normalizedTime = response.normalizedTime;
				npcBySpawnId.delayedAnimation = entityAnimation;
			}
			break;
		}
		case MAPlayAnimation.Target.Self:
		{
			Entity entity = entities.GetEntity((Entity.Type)response.entityType, response.id);
			if (entity != null && entity.assetController != null && entity.assetController.IsAssetLoadComplete)
			{
				entity.PlayAnimation(response.animationName, response.crossfadeDuration, response.layer, response.normalizedTime);
			}
			break;
		}
		}
	}

	public void HandleMachineUpdate(ResponseMachineUpdate response)
	{
		if (BaseMachine.Map.ContainsKey(response.MachineID))
		{
			BaseMachine baseMachine = BaseMachine.Map[response.MachineID];
			byte state = response.State;
			if (state == 1 && baseMachine.IsSingleUse && UsedMachines.Contains(baseMachine.ID))
			{
				return;
			}
			Entity entity = entities.GetEntity(Entity.Type.Player, response.EntityId);
			if (entity != null)
			{
				if (!string.IsNullOrEmpty(baseMachine.Animation))
				{
					entity.PlayAnimation(EntityAnimations.Get(baseMachine.Animation), 0f, isCancellableByMovement: true);
				}
				if (baseMachine.TriggerAudioSource != null)
				{
					baseMachine.TriggerAudioSource.Play();
				}
				if (entity.isMe)
				{
					UsedMachines.Add(baseMachine.ID);
				}
			}
			baseMachine.SetState(state, response.EntityId);
		}
		if (ComMachines != null)
		{
			ComMachine comMachine = ComMachines.Where((ComMachine x) => x.ID == response.MachineID).FirstOrDefault();
			if (comMachine != null)
			{
				comMachine.State = response.State;
			}
		}
	}

	public void HandleMachineResourceChannel(ResponseMachineResourceChannel response)
	{
		if (BaseMachine.Map.ContainsKey(response.machineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.machineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.Channel(response.time);
			}
		}
	}

	public void HandleMachineResourceDespawn(ResponseMachineResourceDespawn response)
	{
		if (BaseMachine.Map.ContainsKey(response.MachineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.MachineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.DespawnNode();
			}
		}
	}

	public void HandleMachineResourceInteract(ResponseMachineResourceInteract response)
	{
		if (BaseMachine.Map.ContainsKey(response.MachineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.MachineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.Interact(response.PrimaryItemID, response.RareItemID, response.Rarity);
			}
		}
	}

	public void HandleMachineResourceInterrupt(ResponseMachineResourceInterrupt response)
	{
		if (BaseMachine.Map.ContainsKey(response.MachineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.MachineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.Interrupt();
			}
		}
	}

	public void HandleMachineResourceSpawn(ResponseMachineResourceSpawn response)
	{
		if (BaseMachine.Map.ContainsKey(response.machineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.machineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.SpawnNode(response.totalUsages, response.usages, response.nodeID, response.rarity, response.tradeSkillLevel, response.requirements, response.dropIDs);
			}
		}
	}

	public void HandleMachineResourceUsed(ResponseMachineResourceUsageUpdate response)
	{
		if (BaseMachine.Map.ContainsKey(response.MachineID))
		{
			ResourceMachine resourceMachine = BaseMachine.Map[response.MachineID] as ResourceMachine;
			if (resourceMachine != null)
			{
				resourceMachine.UpdateUsage(response.Usages);
			}
		}
	}

	public void HandleMachineSetCTState(ResponseMachineSetCTState response)
	{
		Dictionary<int, ClientTrigger> map = ClientTrigger.Map;
		if (!map.ContainsKey(response.triggerID))
		{
			return;
		}
		foreach (ClientTrigger value in map.Values)
		{
			if (value.ID == response.triggerID)
			{
				value.SetState(response.state, 0);
				break;
			}
		}
	}

	public static void ShowGuestFeature()
	{
		Confirmation.Show("Login Required!", "This feature cannot be accessed by a guest account. Would you like to create a login for your account now?", delegate(bool b)
		{
			if (b)
			{
				UIAccountCreate.Instance.ShowConvertGuest();
			}
		});
	}

	public static void ShowAskGuestToConvert()
	{
		if (!alreadyAskedGuestToConvert)
		{
			alreadyAskedGuestToConvert = true;
			UIAccountCreate.Instance.ShowConvertGuest("Welcome Back!", "You are currently playing as a Guest.\nCreate a login now to protect your progress!");
		}
	}

	private void setupDebug()
	{
		GameObject gameObject = null;
		gameObject = GameObject.Find("ctcnt");
		if (!(gameObject == null))
		{
			return;
		}
		gameObject = new GameObject("ctcnt");
		gameObject.transform.position = new Vector3(-3f, 3f, 10f);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (num3 = 0; num3 < 10; num3++)
		{
			for (num2 = 0; num2 < 10; num2++)
			{
				for (num = 0; num < 10; num++)
				{
					GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					gameObject2.transform.localScale = Vector3.one * 0.5f;
					gameObject2.name = num + ":" + num2 + ":" + num3;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localPosition = new Vector3(num, num2, num3);
				}
			}
		}
	}

	private void SendEndTranferRequest()
	{
		aec.sendRequest(new RequestEndTranfer());
	}

	private void sendDebugRequest()
	{
		RequestDebug requestDebug = new RequestDebug();
		requestDebug.floats.Add(UnityEngine.Random.value);
		requestDebug.floats.Add(UnityEngine.Random.value);
		requestDebug.floats.Add(UnityEngine.Random.value);
		aec.sendRequest(requestDebug);
	}

	private void handleDebugResponse(ResponseDebug r)
	{
		if (ResponseDebug.Count++ == 1 || ResponseDebug.Count == 999)
		{
			Debug.Log(GameTime.realtimeSinceServerStartup);
		}
		GameObject gameObject = GameObject.Find("ctcnt");
		if (gameObject != null)
		{
			Transform transform = gameObject.transform.Find(r.strings[0]);
			if (transform != null)
			{
				Color color = new Color(r.floats[0], r.floats[1], r.floats[2], 1f);
				transform.gameObject.GetComponent<Renderer>().material.color = color;
			}
		}
	}

	private void DisableCameraController()
	{
		if (camController != null)
		{
			camController.enabled = false;
		}
	}

	private void EnableCameraController()
	{
		if (camController != null)
		{
			camController.enabled = true;
		}
	}

	public void DisableMovementController()
	{
		entities.me.IsMovementDisabled = true;
	}

	public void EnablePlayerController()
	{
		if (entities != null && entities.me != null)
		{
			entities.me.IsMovementDisabled = false;
		}
	}

	private void StartInputDisable()
	{
	}

	private void EndInputDisable()
	{
	}

	public void OnFullscreenUI(bool fullscreen)
	{
		if (fullscreen)
		{
			DisableControls();
		}
		else
		{
			EnableControls();
		}
	}

	private void OnNPCCamActiveUpdated(bool active)
	{
		if (active)
		{
			IsUIVisible = true;
			DisableControls();
			DisableCameraController();
			if (entities.me != null && entities.me.Target != null)
			{
				entities.me.Target.Unhighlight();
			}
		}
		else
		{
			EnableControls();
			EnableCameraController();
			if (entities.me != null && entities.me.Target != null)
			{
				entities.me.Target.Highlight(entities.me.Target.GetReactionColor(Entities.Instance.me));
			}
		}
	}

	public void DisableControls()
	{
		IsUIVisible = true;
		isInputEnabled = false;
		InputManager.DisableInput();
		DisableCameraController();
		if (entities.me != null && entities.me.moveController != null)
		{
			entities.me.moveController.Stop();
		}
	}

	public void EnableControls()
	{
		isInputEnabled = true;
		InputManager.EnableInput();
		EnableCameraController();
	}

	public void Ignore(string targetName)
	{
		if (targetName != Entities.Instance.me.name.ToLower())
		{
			SettingsManager.Ignore(targetName);
			Chat.AddMessage(InterfaceColors.Chat.Light_Blue.ToBBCode() + "'" + targetName + "'[-]" + InterfaceColors.Chat.White.ToBBCode() + " has been added to the ignore list.[-]");
		}
	}

	public void UnIgnore(string targetName)
	{
		SettingsManager.UnIgnore(targetName);
	}

	private void ClearLocalCache()
	{
		Quests.Clear();
		Items.Clear();
		MergeShops.Clear();
		Shops.Clear();
		ApopMap.ClearApops(destroyNews: true);
		Dungeons.Clear();
		DailyRewards.Clear();
	}

	private void ReloadQuestData()
	{
		List<int> list = Session.MyPlayerData.CurQuests.Where((int p) => Quests.Get(p) != null).ToList();
		list.AddRange(areaData.quests.Where((int p) => Quests.Get(p) != null));
		SendQuestLoadRequest(list.Distinct().ToList());
		Quests.Clear();
		UsedMachines.Clear();
		UpdateAreaQuest();
		Session.MyPlayerData.UpdateTrackedQuest();
	}

	public void OnApplicationQuit()
	{
	}

	private IEnumerator CrossfadeAnimatorLayerWeight(Animator animator, ResponseMachineAnimatorLayerWeight response)
	{
		float elapsedTime = 0f;
		while (elapsedTime <= response.crossfade)
		{
			elapsedTime += Time.deltaTime;
			float num = animator.GetLayerWeight(response.layerID);
			if (num < response.weight)
			{
				num += Time.deltaTime / response.crossfade;
				num = ((num > response.weight) ? response.weight : num);
			}
			else if (num > response.weight)
			{
				num -= Time.deltaTime / response.crossfade;
				num = ((num < response.weight) ? response.weight : num);
			}
			animator.SetLayerWeight(response.layerID, num);
			yield return null;
		}
	}

	public void NamePlateButtonToggle()
	{
		if (!Session.MyPlayerData.devMode)
		{
			Session.MyPlayerData.devMode = true;
		}
		else
		{
			Session.MyPlayerData.devMode = false;
		}
	}

	public void ToggleCheat()
	{
		if (AdamTools.HasAccess())
		{
			if (adamTools == null)
			{
				adamTools = base.gameObject.AddComponent<AdamTools>();
			}
			adamTools.CheatsOn();
		}
	}

	public bool IsDragging()
	{
		if (CurrentInteractable is IDraggable draggable && draggable.IsDraggable(currentInteractablePress1))
		{
			return true;
		}
		InteractableRaycastResult interactableFromRaycast = GetInteractableFromRaycast(Input.mousePosition);
		if (interactableFromRaycast.HitInteractable is IDraggable draggable2 && draggable2.IsDraggable(interactableFromRaycast))
		{
			return true;
		}
		return false;
	}

	public bool IsInteracting()
	{
		InteractableRaycastResult interactableFromRaycast = GetInteractableFromRaycast(Input.mousePosition);
		if (interactableFromRaycast.HitInteractable != null && interactableFromRaycast.HitInteractable.IsInteractable(interactableFromRaycast))
		{
			return true;
		}
		return false;
	}

	public void Login()
	{
		Debug.Log("Game.Login()");
		RequestLogin requestLogin = new RequestLogin();
		requestLogin.id = Session.Account.chars[0].ID;
		requestLogin.token = Session.Account.strToken;
		aec.sendRequest(requestLogin);
	}

	private void onDisconnect(string message)
	{
		Debug.Log("Game.onDisconnect()");
		BackToLogin();
		if (Session.pendingGoto == null)
		{
			MessageBox.Show("Error", message);
		}
	}

	public void OnReceiveResponse(Response response)
	{
		if (IngoreResponses)
		{
			return;
		}
		Debug.Log("GameAEC - OnResponseReceived: " + response.ToString());
		Com.Type type = (Com.Type)response.type;
		Debug.Log("Response  " + type.ToString() + ":" + response.cmd);
		switch ((Com.Type)response.type)
		{
		case Com.Type.ServerTools:
			if (response.cmd == 11)
			{
				ResponseUpdatePosSpawn responseUpdatePosSpawn = response as ResponseUpdatePosSpawn;
				foreach (ComSpawnMeta spawnMeta in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta.SpawnID == responseUpdatePosSpawn.SpawnID)
					{
						spawnMeta.Path[responseUpdatePosSpawn.PathID] = new ComVector3(responseUpdatePosSpawn.Pos);
						spawnMeta.RotationY[responseUpdatePosSpawn.PathID] = responseUpdatePosSpawn.RotationY;
						break;
					}
				}
				UINPCEditor.CreateSpawner(responseUpdatePosSpawn.SpawnID, new Vector3(responseUpdatePosSpawn.Pos.x, responseUpdatePosSpawn.Pos.y, responseUpdatePosSpawn.Pos.z), responseUpdatePosSpawn.RotationY, AutoSpawn: true);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseUpdatePosSpawn.SpawnID);
				}
			}
			else if (response.cmd == 12)
			{
				ResponseHijackSpawn responseHijackSpawn = response as ResponseHijackSpawn;
				for (int i = 0; i < Instance.AreaData.spawnMetas.Count; i++)
				{
					if (Instance.AreaData.spawnMetas[i].SpawnID == responseHijackSpawn.SpawnID)
					{
						List<ComNPCMeta> collection = new List<ComNPCMeta>(Instance.AreaData.spawnMetas[i].Spawns);
						Instance.AreaData.spawnMetas[i] = new ComSpawnMeta(responseHijackSpawn.SpawnID, IsDB: true);
						Instance.AreaData.spawnMetas[i].Path.Add(responseHijackSpawn.PathID, responseHijackSpawn.Pos);
						Instance.AreaData.spawnMetas[i].RotationY.Add(responseHijackSpawn.PathID, responseHijackSpawn.RotationY);
						Instance.AreaData.spawnMetas[i].Spawns = new List<ComNPCMeta>(collection);
						break;
					}
				}
				UINPCEditor.CreateSpawner(responseHijackSpawn.SpawnID, new Vector3(responseHijackSpawn.Pos.x, responseHijackSpawn.Pos.y, responseHijackSpawn.Pos.z), responseHijackSpawn.RotationY, AutoSpawn: true);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseHijackSpawn.SpawnID);
				}
			}
			else if (response.cmd == 13)
			{
				ResponseAddSpawn responseAddSpawn = response as ResponseAddSpawn;
				Instance.AreaData.spawnMetas.Add(new ComSpawnMeta(responseAddSpawn.SpawnID, IsDB: true)
				{
					Path = new Dictionary<int, ComVector3> { { responseAddSpawn.PathID, responseAddSpawn.Pos } },
					RotationY = new Dictionary<int, int> { { responseAddSpawn.PathID, responseAddSpawn.RotationY } }
				});
				UINPCEditor.CreateSpawner(responseAddSpawn.SpawnID, new Vector3(responseAddSpawn.Pos.x, responseAddSpawn.Pos.y, responseAddSpawn.Pos.z), responseAddSpawn.RotationY, AutoSpawn: true);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseAddSpawn.SpawnID);
				}
			}
			else if (response.cmd == 14)
			{
				ResponseDeleteSpawn responseDeleteSpawn = response as ResponseDeleteSpawn;
				foreach (ComSpawnMeta spawnMeta2 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta2.SpawnID == responseDeleteSpawn.SpawnID)
					{
						if (NPCSpawn.GetInactiveSpawn(responseDeleteSpawn.SpawnID) != null)
						{
							spawnMeta2.IsDB = false;
							spawnMeta2.Path = new Dictionary<int, ComVector3>();
							spawnMeta2.RotationY = new Dictionary<int, int>();
							spawnMeta2.Path.Add(-1, new ComVector3(NPCSpawn.GetInactiveSpawn(responseDeleteSpawn.SpawnID).TargetTransform.position.x, NPCSpawn.GetInactiveSpawn(responseDeleteSpawn.SpawnID).TargetTransform.position.y, NPCSpawn.GetInactiveSpawn(responseDeleteSpawn.SpawnID).TargetTransform.position.z));
							spawnMeta2.RotationY.Add(-1, (int)NPCSpawn.GetInactiveSpawn(responseDeleteSpawn.SpawnID).TargetTransform.rotation.eulerAngles.y);
						}
						else
						{
							Instance.AreaData.spawnMetas.Remove(spawnMeta2);
						}
						break;
					}
				}
				UINPCEditor.DeleteSpawner(responseDeleteSpawn.SpawnID);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseDeleteSpawn.SpawnID);
				}
			}
			else if (response.cmd == 15)
			{
				ResponseAddNPC responseAddNPC = response as ResponseAddNPC;
				foreach (ComSpawnMeta spawnMeta3 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta3.SpawnID == responseAddNPC.SpawnID)
					{
						spawnMeta3.AutoSpawn = true;
						spawnMeta3.RespawnTime = 30f;
						spawnMeta3.DespawnTime = 6f;
						spawnMeta3.AggroRadius = 6f;
						spawnMeta3.LeashRadius = 40f;
						spawnMeta3.ChainRadius = 60f;
						spawnMeta3.AllowRegeneration = true;
						spawnMeta3.Spawns.Add(new ComNPCMeta
						{
							SpawnListNpcID = responseAddNPC.SpawnListNpcID,
							NPCID = responseAddNPC.NpcID,
							Level = responseAddNPC.Level,
							NameOverride = responseAddNPC.Name,
							Rate = 100f,
							ApopIDs = new List<int>(),
							ReactionOverride = responseAddNPC.Reaction,
							TeamID = -1
						});
						break;
					}
				}
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseAddNPC.SpawnID);
				}
			}
			else if (response.cmd == 16)
			{
				ResponseDeleteNPC responseDeleteNPC = response as ResponseDeleteNPC;
				bool flag3 = false;
				foreach (ComSpawnMeta spawnMeta4 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta4.SpawnID == responseDeleteNPC.SpawnID)
					{
						foreach (ComNPCMeta spawn in spawnMeta4.Spawns)
						{
							if (spawn.SpawnListNpcID == responseDeleteNPC.SpawnListNpcID)
							{
								spawnMeta4.Spawns.Remove(spawn);
								flag3 = true;
								break;
							}
						}
					}
					if (flag3)
					{
						break;
					}
				}
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseDeleteNPC.SpawnID);
				}
			}
			else if (response.cmd == 17)
			{
				ResponseEditNPC responseEditNPC = response as ResponseEditNPC;
				foreach (ComSpawnMeta spawnMeta5 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta5.SpawnID != responseEditNPC.relSpawnID)
					{
						continue;
					}
					spawnMeta5.AllowRegeneration = responseEditNPC.AllowRegen;
					spawnMeta5.RespawnTime = responseEditNPC.Respawn;
					spawnMeta5.DespawnTime = responseEditNPC.Despawn;
					spawnMeta5.AggroRadius = responseEditNPC.Aggro;
					spawnMeta5.LeashRadius = responseEditNPC.Leash;
					spawnMeta5.ChainRadius = responseEditNPC.Chain;
					spawnMeta5.AutoChain = responseEditNPC.AutoChain;
					spawnMeta5.MoveOverride = responseEditNPC.MoveOverride;
					spawnMeta5.Speed = responseEditNPC.Speed;
					spawnMeta5.UseRotation = responseEditNPC.UseRotation;
					spawnMeta5.Animations = responseEditNPC.Animations;
					spawnMeta5.SequentialAnimations = responseEditNPC.SequentialAnimations;
					spawnMeta5.MinTime = responseEditNPC.MinTime;
					spawnMeta5.MaxTime = responseEditNPC.MaxTime;
					foreach (ComNPCMeta spawn2 in spawnMeta5.Spawns)
					{
						if (spawn2.NPCID == responseEditNPC.relNpcID)
						{
							spawn2.Level = responseEditNPC.Level;
							spawn2.NameOverride = responseEditNPC.Name;
							spawn2.Rate = responseEditNPC.Rate;
							spawn2.ApopIDs = ((responseEditNPC.Apops != null) ? new List<int>(responseEditNPC.Apops) : new List<int>());
							spawn2.AnimationOverrides = ((responseEditNPC.AnimationOverrides != null) ? new Dictionary<string, string>(responseEditNPC.AnimationOverrides) : new Dictionary<string, string>());
							spawn2.TeamID = responseEditNPC.TeamID;
							spawn2.ReactionOverride = responseEditNPC.ReactionOverride;
							spawn2.NPCID = responseEditNPC.NpcID;
							break;
						}
					}
					spawnMeta5.SpawnID = responseEditNPC.SpawnID;
					break;
				}
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseEditNPC.SpawnID);
				}
			}
			else if (response.cmd == 18)
			{
				ResponseUpdatePathNPC responseUpdatePathNPC = response as ResponseUpdatePathNPC;
				foreach (ComSpawnMeta spawnMeta6 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta6.SpawnID == responseUpdatePathNPC.SpawnID)
					{
						UINPCEditor.DeletePathNode(responseUpdatePathNPC.PathID);
						UINPCEditor.CreatePathNode(responseUpdatePathNPC.SpawnID, responseUpdatePathNPC.PathID, new Vector3(responseUpdatePathNPC.Pos.x, responseUpdatePathNPC.Pos.y, responseUpdatePathNPC.Pos.z));
						spawnMeta6.Path[responseUpdatePathNPC.PathID] = responseUpdatePathNPC.Pos;
						spawnMeta6.RotationY[responseUpdatePathNPC.PathID] = responseUpdatePathNPC.RotationY;
						break;
					}
				}
				UINPCEditor.LinkPaths(responseUpdatePathNPC.SpawnID);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseUpdatePathNPC.SpawnID);
				}
			}
			else if (response.cmd == 19)
			{
				ResponseDeletePathNPC responseDeletePathNPC = response as ResponseDeletePathNPC;
				foreach (ComSpawnMeta spawnMeta7 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta7.SpawnID == responseDeletePathNPC.SpawnID)
					{
						UINPCEditor.DeletePathNode(responseDeletePathNPC.PathID);
						spawnMeta7.Path.Remove(responseDeletePathNPC.PathID);
						spawnMeta7.RotationY.Remove(responseDeletePathNPC.PathID);
						break;
					}
				}
				UINPCEditor.LinkPaths(responseDeletePathNPC.SpawnID);
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseDeletePathNPC.SpawnID);
				}
			}
			else if (response.cmd == 21)
			{
				Application.OpenURL((response as ResponseOpenInAdmin).URL);
			}
			else if (response.cmd == 24)
			{
				ResponseUpdateRequirements responseUpdateRequirements = response as ResponseUpdateRequirements;
				foreach (ComSpawnMeta spawnMeta8 in Instance.AreaData.spawnMetas)
				{
					if (spawnMeta8.SpawnID == responseUpdateRequirements.SpawnID)
					{
						spawnMeta8.Requirements = responseUpdateRequirements.Requirements;
						break;
					}
				}
				if (UINPCEditor.Instance != null)
				{
					UINPCEditor.Instance.UpdateAndRefresh(responseUpdateRequirements.SpawnID);
				}
			}
			else if (response.cmd == 26)
			{
				ResponseAddMapEntity responseAddMapEntity = response as ResponseAddMapEntity;
				bool machineAdded = false;
				switch (responseAddMapEntity.Entity.Type)
				{
				case MapEntityTypes.PlayerSpawner:
					Instance.AreaData.mapEntityPlayerSpawners.Add(responseAddMapEntity.Entity);
					UIMapEditor.CreatePlayerSpawnerPlatformGO(Instance.AreaData.dbSpawns, responseAddMapEntity.Entity);
					break;
				case MapEntityTypes.TransferPad:
					Instance.AreaData.mapEntityTransferPads.Add(responseAddMapEntity.Entity);
					StartCoroutine(UIMapEditor.LoadEntityAsset(Instance.AreaData.dbSpawns, responseAddMapEntity.Entity));
					break;
				case MapEntityTypes.Machine:
					Instance.AreaData.mapEntityMachines.Add(responseAddMapEntity.Entity);
					Instance.ComMachines.Add(responseAddMapEntity.comMachine);
					machineAdded = true;
					StartCoroutine(UIMapEditor.LoadEntityAsset(Instance.AreaData.dbSpawns, responseAddMapEntity.Entity));
					break;
				}
				if (UIMapEditor.Instance != null)
				{
					UIMapEditor.Instance.UpdateAndRefresh(responseAddMapEntity.Entity, machineAdded);
				}
			}
			else if (response.cmd == 30)
			{
				ResponseUpdateMapEntity resUME = response as ResponseUpdateMapEntity;
				GameObject gameObject2 = null;
				if (UIMapEditor.go_dict.ContainsKey(resUME.Entity.ID))
				{
					gameObject2 = UIMapEditor.go_dict[resUME.Entity.ID];
				}
				if (gameObject2 != null)
				{
					gameObject2.transform.position = resUME.Entity.Position;
					gameObject2.transform.rotation = Quaternion.Euler(0f, resUME.Entity.RotationY, 0f);
					dynamic val = JsonConvert.DeserializeObject<object>(resUME.Entity.Data);
					float x2 = (float)val["ScaleX"];
					float y = (float)val["ScaleY"];
					float z = (float)val["ScaleZ"];
					gameObject2.transform.localScale = new Vector3(x2, y, z);
					ComMapEntity comMapEntity = Instance.AreaData.mapEntityMachines.FirstOrDefault((ComMapEntity o) => o.ID == resUME.Entity.ID);
					if (comMapEntity == null)
					{
						comMapEntity = Instance.AreaData.mapEntityTransferPads.FirstOrDefault((ComMapEntity o) => o.ID == resUME.Entity.ID);
					}
					if (comMapEntity != null)
					{
						comMapEntity.Position = resUME.Entity.Position;
						comMapEntity.RotationY = resUME.Entity.RotationY;
						comMapEntity.Data = resUME.Entity.Data;
					}
				}
				for (int j = 0; j < Instance.AreaData.mapEntityMachines.Count; j++)
				{
					if (Instance.AreaData.mapEntityMachines[j].ID == resUME.Entity.ID)
					{
						Instance.AreaData.mapEntityMachines[j] = resUME.Entity;
						UIMapEditor.go_dict.Remove(resUME.Entity.ID);
						UnityEngine.Object.Destroy(gameObject2);
						StartCoroutine(UIMapEditor.LoadEntityAsset(Instance.AreaData.dbSpawns, resUME.Entity));
					}
				}
				if (UIMapEditor.Instance != null)
				{
					UIMapEditor.Instance.UpdateAndRefresh(resUME.Entity);
				}
			}
			else if (response.cmd == 28)
			{
				ResponseDeleteMapEntity responseDeleteMapEntity = response as ResponseDeleteMapEntity;
				foreach (ComMapEntity mapEntityPlayerSpawner in Instance.AreaData.mapEntityPlayerSpawners)
				{
					if (mapEntityPlayerSpawner.ID == responseDeleteMapEntity.ID)
					{
						Instance.AreaData.mapEntityPlayerSpawners.Remove(mapEntityPlayerSpawner);
						break;
					}
				}
				foreach (ComMapEntity mapEntityTransferPad in Instance.AreaData.mapEntityTransferPads)
				{
					if (mapEntityTransferPad.ID == responseDeleteMapEntity.ID)
					{
						Instance.AreaData.mapEntityTransferPads.Remove(mapEntityTransferPad);
						break;
					}
				}
				foreach (ComMapEntity mapEntityMachine in Instance.AreaData.mapEntityMachines)
				{
					if (mapEntityMachine.ID != responseDeleteMapEntity.ID)
					{
						continue;
					}
					try
					{
						dynamic val2 = JsonConvert.DeserializeObject<object>(mapEntityMachine.Data);
						BaseMachine baseMachine = BaseMachine.Map[int.Parse((string)val2["UniqueID"])];
						BaseMachine.Map.Remove(int.Parse((string)val2["UniqueID"]));
						if (baseMachine != null && baseMachine is InteractiveMachine)
						{
							foreach (MachineAction action in ((InteractiveMachine)baseMachine).Actions)
							{
								if (action is MAQuestObjective && (int)SettingsManager.TrackedQuestID == ((MAQuestObjective)action).QuestID)
								{
									Session.MyPlayerData.ClearTrackedQuest();
								}
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("error clearing tracked quest: " + ex.ToString());
					}
					Instance.AreaData.mapEntityMachines.Remove(mapEntityMachine);
					break;
				}
				UIMapEditor.DeleteGO(responseDeleteMapEntity.ID);
				if (UIMapEditor.Instance != null)
				{
					UIMapEditor.Instance.UpdateAndRefresh();
				}
			}
			else if (response.cmd == 34)
			{
				ResponseMapAssets responseMapAssets = response as ResponseMapAssets;
				if (UIMapEditor.Instance != null)
				{
					UIMapEditor.Instance.SetMapAssetChoices(responseMapAssets.mapAssets);
				}
			}
			else if (response.cmd == 36)
			{
				ResponseFavoriteMapAssets responseFavoriteMapAssets = response as ResponseFavoriteMapAssets;
				if (UIMapEditor.Instance != null)
				{
					UIMapEditor.Instance.SetMapAssetFavorites(responseFavoriteMapAssets.mapAssets);
				}
			}
			break;
		case Com.Type._DEBUG:
			handleDebugResponse((ResponseDebug)response);
			break;
		case Com.Type.Admin:
			if (response.cmd == 3)
			{
				Items.Clear();
				MergeShops.Clear();
				Shops.Clear();
				ApopMap.ClearApops(destroyNews: true);
				Dungeons.Clear();
				DailyRewards.Clear();
				ReloadQuestData();
			}
			else if (response.cmd == 4)
			{
				MergeShops.Clear();
				Shops.Clear();
			}
			else if (response.cmd == 5)
			{
				ReloadQuestData();
			}
			else if (response.cmd == 6)
			{
				LoginManager.RefreshMongoData();
			}
			else if (response.cmd == 1)
			{
				ResponseAdminQSSet responseAdminQSSet = (ResponseAdminQSSet)response;
				Session.MyPlayerData.SetQSValue(responseAdminQSSet.index, responseAdminQSSet.value);
				Session.Clear();
				UsedMachines.Clear();
				UpdateAreaQuest();
			}
			else if (response.cmd == 7)
			{
				Session.MyPlayerData.GetQSValue();
			}
			break;
		case Com.Type.AdWatch:
		{
			ResponseAdWatch responseAdWatch = (ResponseAdWatch)response;
			if (this.AdWatchRewardReceived != null)
			{
				this.AdWatchRewardReceived(responseAdWatch.Success);
			}
			break;
		}
		case Com.Type.Area:
			Debug.Log(response.cmd + " " + (byte)1);
			if (response.cmd == 1)
			{
				ResponseAreaJoin responseAreaJoin = response as ResponseAreaJoin;
				Debug.Log("Handling AreaJoin > " + responseAreaJoin.areadata.name);
				entities.ClearEntitiesExceptMe();
				field.Reset();
				ComAreaData areadata = responseAreaJoin.areadata;
				areaData = new AreaData(areadata);
				entities.me?.CheckPvpState();
				Wars.Clear();
				Wars.Set(responseAreaJoin.wars);
				if (uigame != null)
				{
					uigame.voteKick.TurnOff();
				}
				OnShowSagaQuest = null;
				if (DialogueSlotManager.Instance != null && DialogueSlotManager.Instance.Inprogress)
				{
					DialogueSlotManager.Close(reload: true);
				}
			}
			else if (response.cmd == 2)
			{
				ResponseAreaRemove responseAreaRemove = (ResponseAreaRemove)response;
				entities.RemovePlayerById(responseAreaRemove.entityID);
			}
			else if (response.cmd == 3)
			{
				Session.MyPlayerData.SetAreaList(((ResponseAreaList)response).areas, ((ResponseAreaList)response).regions);
			}
			else if (response.cmd == 5)
			{
				areaData.MatchState = ((ResponseAreaMatchState)response).MatchState;
				Instance.MatchStateUpdated?.Invoke();
			}
			else if (response.cmd == 4)
			{
				Wars.Set(((ResponseWarSync)response).wars);
			}
			else if (response.cmd == 7)
			{
				ResponseDynamicScale responseDynamicScale = (ResponseDynamicScale)response;
				if (responseDynamicScale.AreaID != CurrentAreaID || Entities.Instance.NpcList.Count == 0)
				{
					break;
				}
				if (!responseDynamicScale.OnAggro)
				{
					int dynamicPlayerCount = responseDynamicScale.DynamicPlayerCount;
					{
						foreach (NPC item in Entities.Instance.NpcList.Where((NPC npc) => npc.IsDynamicScaling))
						{
							if (item.serverState != Entity.State.InCombat || (item.serverState == Entity.State.InCombat && dynamicPlayerCount < item.DynamicPlayerCount))
							{
								item.HotBuildStats(GameCurves.GetDynamicScaleMultiplier(dynamicPlayerCount, areaData.maxusers, areaData.isLegacyDynamicScaling, areaData.dynamicScalingAmount, areaData.bossDynScalingAmount, item.isBoss));
								item.DynamicPlayerCount = dynamicPlayerCount;
							}
						}
						break;
					}
				}
				int entityID = responseDynamicScale.EntityID;
				int dynamicPlayerCount2 = responseDynamicScale.DynamicPlayerCount;
				NPC npcById4 = Entities.Instance.GetNpcById(entityID);
				if (npcById4 != null)
				{
					npcById4.HotBuildStats(GameCurves.GetDynamicScaleMultiplier(dynamicPlayerCount2, areaData.maxusers, areaData.isLegacyDynamicScaling, areaData.dynamicScalingAmount, areaData.bossDynScalingAmount, npcById4.isBoss));
					npcById4.DynamicPlayerCount = dynamicPlayerCount2;
				}
			}
			else if (response.cmd == 8)
			{
				uigame.speedrunTimer.root.SetActive(value: true);
				uigame.speedrunTimer.Init("Race to the Finish", 0f);
			}
			else if (response.cmd == 9)
			{
				uigame.speedrunTimer.Pause();
			}
			break;
		case Com.Type.Audio:
		{
			ResponsePlayAudioClip responsePlayAudioClip = (ResponsePlayAudioClip)response;
			float value = (float)(DateTime.UtcNow.Ticks - responsePlayAudioClip.TimeStamp.Ticks) / 10000000f;
			value = value.Clamp(0f, float.MaxValue);
			float value2 = responsePlayAudioClip.Delay - value;
			value2 = value2.Clamp(0f, float.MaxValue);
			if (response.cmd == 1)
			{
				AudioManager.Play2DSFX(responsePlayAudioClip.ClipName, responsePlayAudioClip.Delay, responsePlayAudioClip.Volume);
			}
			else if (response.cmd == 2 && Entities.Instance != null)
			{
				Entity entityByID = Entities.Instance.GetEntityByID(responsePlayAudioClip.EntityID);
				if (entityByID != null && entityByID.wrapperTransform != null)
				{
					AudioManager.Play3DSFX(responsePlayAudioClip.ClipName, SFXType.UI, entityByID.wrapperTransform, value2, toFollow: true, loop: false, 0, responsePlayAudioClip.Volume);
				}
			}
			break;
		}
		case Com.Type.Bank:
			if (response.cmd == 1)
			{
				ResponseBankItems responseBankItems = (ResponseBankItems)response;
				Session.MyPlayerData.SetBankItems(responseBankItems.BankID, responseBankItems.Items);
			}
			else if (response.cmd == 2)
			{
				ResponseItemTransfer responseItemTransfer = (ResponseItemTransfer)response;
				if (responseItemTransfer.Status == 0)
				{
					Session.MyPlayerData.TransferItem(responseItemTransfer.CharItemID, responseItemTransfer.FromBankID, responseItemTransfer.ToBankID, responseItemTransfer.TransferID);
				}
			}
			else if (response.cmd == 3)
			{
				ResponseBankPurchase responseBankPurchase = (ResponseBankPurchase)response;
				if (responseBankPurchase.ResponseType == 0)
				{
					Session.MyPlayerData.SetBankCount(responseBankPurchase.BankCount);
				}
			}
			else if (response.cmd == 4)
			{
				ResponseAllBankItems responseAllBankItems = (ResponseAllBankItems)response;
				Session.MyPlayerData.SetAllBankItems(responseAllBankItems.allItems);
			}
			break;
		case Com.Type.Cell:
			switch (response.cmd)
			{
			case 2:
			{
				ResponseCellAdd responseCellAdd = (ResponseCellAdd)response;
				if (responseCellAdd.cellID != areaData.currentCellID)
				{
					break;
				}
				Debug.Log("Cell Add >> Incoming ID: " + responseCellAdd.entity.ID);
				ComEntity entity2 = responseCellAdd.entity;
				Player playerById2 = entities.GetPlayerById(entity2.ID);
				if (playerById2 == null)
				{
					playerById2 = new Player(responseCellAdd.entity);
					entities.AddPlayer(playerById2);
					playerById2.setSpawnPosRot(new Vector3(entity2.posX, entity2.posY, entity2.posZ), entity2.rotY);
					if (isReady)
					{
						GameObject gameObject = new GameObject("playerWrapper" + entity2.ID);
						gameObject.transform.parent = baseTF.Find("entitiesGO").transform;
						gameObject.layer = Layers.OTHER_PLAYERS;
						gameObject.transform.SetPositionAndRotation(playerById2.spawnPostion, playerById2.spawnRotation);
						playerById2.SetWrapper(gameObject);
						playerById2.SetCamera(cam);
						playerById2.BuildController();
						playerById2.BuildNamePlate();
					}
				}
				break;
			}
			case 1:
			{
				ResponseCellJoin responseCellJoin = (ResponseCellJoin)response;
				AreaData.currentCellID = responseCellJoin.cellID;
				AreaData.NpcSpawnInfos = responseCellJoin.NpcSpawnInfos;
				AreaData.areaFlags = responseCellJoin.areaFlags;
				AreaData.cellTimerDescription = responseCellJoin.timerDescription;
				AreaData.cellTimerDuration = responseCellJoin.timerDuration - (GameTime.realtimeSinceServerStartup - responseCellJoin.timerTimeStamp);
				AreaData.isCellTimerPaused = responseCellJoin.isTimerPaused;
				AreaData.SoundTrackID = responseCellJoin.soundTrackID;
				AreaData.spawnMetas = ((responseCellJoin.spawnMetas == null) ? new List<ComSpawnMeta>() : responseCellJoin.spawnMetas);
				AreaData.npcPathNodes = new Dictionary<GameObject, int>();
				AreaData.mapEntityPlayerSpawners = responseCellJoin.mapEntities.Where((ComMapEntity x) => x.Type == MapEntityTypes.PlayerSpawner).ToList();
				AreaData.mapEntityTransferPads = responseCellJoin.mapEntities.Where((ComMapEntity x) => x.Type == MapEntityTypes.TransferPad).ToList();
				AreaData.mapEntityMachines = responseCellJoin.mapEntities.Where((ComMapEntity x) => x.Type == MapEntityTypes.Machine).ToList();
				AreaData.NpcSfxGreetings = responseCellJoin.NpcSfxGreetings;
				AreaData.NpcSfxFarewells = responseCellJoin.NpcSfxFarewells;
				ComMachines = responseCellJoin.machines;
				ComMachineListeners = responseCellJoin.listeners;
				ComLootBags = responseCellJoin.lootBags;
				LocatorTransfers = responseCellJoin.LocatorTransfers;
				Debug.Log("Handling ResponseCellJoin with cell ID " + responseCellJoin.cellID);
				foreach (ComEntity entity5 in responseCellJoin.entities)
				{
					if (entity5.type == Entity.Type.NPC)
					{
						NPC npc2 = new NPC(entity5);
						entities.AddNpc(npc2);
						continue;
					}
					Debug.Log("User in area with ID : " + entity5.ID);
					Player player = entities.GetPlayerById(entity5.ID);
					if (player == null)
					{
						player = new Player(entity5);
						bool isMe = entity5.ID == Session.MyPlayerData.ID;
						entities.AddPlayer(player, isMe);
					}
					else
					{
						player.InterruptKeyframeSpell();
						player.CastCancel();
						player.ComSync(entity5);
					}
					player.setSpawnPosRot(new Vector3(entity5.posX, entity5.posY, entity5.posZ), entity5.rotY);
				}
				Scoreboard.Start(responseCellJoin.scoreTarget, responseCellJoin.teamScores);
				CellJoin();
				break;
			}
			case 3:
			{
				ResponseCellRemove responseCellRemove = (ResponseCellRemove)response;
				entities.RemovePlayerById(responseCellRemove.entityID);
				break;
			}
			case 5:
				Scoreboard.UpdateTeamScore((response as ResponseScoreSync).teamScores);
				break;
			case 6:
			{
				ResponseSoundTrackUpdate responseSoundTrackUpdate = response as ResponseSoundTrackUpdate;
				StartCoroutine(SoundTracks.Instance.Play(responseSoundTrackUpdate.soundTrackID, showLoader: false));
				AreaData.SoundTrackID = responseSoundTrackUpdate.soundTrackID;
				break;
			}
			case 4:
			{
				ResponseCellTeleport responseCellTeleport = (ResponseCellTeleport)response;
				Entity entity = entities.GetEntity(responseCellTeleport.entityType, responseCellTeleport.entityId);
				if (entity != null)
				{
					entity.Teleport(responseCellTeleport.posX, responseCellTeleport.posY, responseCellTeleport.posZ, responseCellTeleport.rotY);
					if (entity.isMe)
					{
						SendEndTranferRequest();
					}
				}
				break;
			}
			case 7:
				uigame.cellTimer.Pause();
				break;
			case 8:
			{
				ResponseCellTimerStart responseCellTimerStart = response as ResponseCellTimerStart;
				uigame.cellTimer.Init(responseCellTimerStart.description, responseCellTimerStart.time);
				break;
			}
			case 9:
				uigame.cellTimer.Stop();
				break;
			case 10:
				uigame.cellTimer.Unpause();
				break;
			}
			break;
		case Com.Type.Channel:
			if (response.cmd == 2)
			{
				ResponseChannelAdd responseChannelAdd = (ResponseChannelAdd)response;
				ChannelManager.Instance.CreateChannel(responseChannelAdd.channelID, responseChannelAdd.channelType);
			}
			else if (response.cmd == 3)
			{
				ResponseChannelRemove responseChannelRemove = (ResponseChannelRemove)response;
				ChannelManager.Instance.DestroyChannel(responseChannelRemove.channelID);
			}
			break;
		case Com.Type.Chat:
		{
			ResponseChat obj2 = (ResponseChat)response;
			if (this.ChatReceived != null)
			{
				this.ChatReceived(obj2);
			}
			break;
		}
		case Com.Type.Combat:
			switch (response.cmd)
			{
			case 4:
				combat.HandleCombatSpell((ResponseCombatSpell)response);
				break;
			case 5:
				combat.HandleCombatEffectPulse((ResponseCombatEffectPulse)response);
				break;
			case 8:
				combat.HandleMachineSpell((ResponseMachineSpell)response);
				break;
			case 10:
				combat.HandleResetSpellCD((ResponseResetCD)response);
				break;
			case 11:
				combat.HandleAuraUpdate((ResponseAuraUpdate)response);
				break;
			case 6:
			case 7:
			case 9:
				break;
			}
			break;
		case Com.Type.CombatClasses:
			switch (response.cmd)
			{
			case 1:
			{
				ResponseClassAdd responseClassAdd = (ResponseClassAdd)response;
				Session.MyPlayerData.AddClass(responseClassAdd.charClass);
				break;
			}
			case 2:
			{
				ResponseClassEquip responseClassEquip = (ResponseClassEquip)response;
				Entity entity3 = entities.GetEntity(responseClassEquip.etyp, responseClassEquip.EntityID);
				if (entity3 != null)
				{
					entity3.SetClass(responseClassEquip.ClassID);
					entity3.SetClassXPAndTotalClassRank(responseClassEquip.ClassXP, entity3.TotalClassRank);
					if (entity3.isMe)
					{
						Session.MyPlayerData.EquipClass(responseClassEquip.ClassID);
					}
				}
				break;
			}
			case 3:
			{
				ResponseCrossSkillEquip responseCrossSkillEquip = (ResponseCrossSkillEquip)response;
				SettingsManager.SetActionSlotID(CombatSpellSlot.CrossSkill, responseCrossSkillEquip.SpellID);
				Session.MyPlayerData.CurrentCrossSkill = responseCrossSkillEquip.SpellID;
				break;
			}
			case 4:
			{
				ResponseSpellCooldowns responseSpellCooldowns = response as ResponseSpellCooldowns;
				if (responseSpellCooldowns.cooldowns == null)
				{
					break;
				}
				{
					foreach (SpellCooldown cooldown in responseSpellCooldowns.cooldowns)
					{
						combat.SetSpellTimestamp(cooldown.spellID, cooldown.timeStamp, cooldown.cooldown);
					}
					break;
				}
			}
			}
			break;
		case Com.Type.Command:
			if (response.cmd == 2)
			{
				ResponseCommandDialog dComm = (ResponseCommandDialog)response;
				DialogueSlotManager.Instance.IncomingMessage(dComm);
			}
			else if (response.cmd == 4)
			{
				ChatCommands.ShowHelp(((ResponseCommandHelp)response).chatCmd);
			}
			else if (response.cmd == 5)
			{
				ChatCommands.SetServerCommands(((ResponseGetChatCommands)response).commands);
			}
			break;
		case Com.Type.Emote:
		{
			ResponseEmote responseEmote = (ResponseEmote)response;
			entities.GetPlayerById(responseEmote.ID)?.PlayEmote(responseEmote.em);
			break;
		}
		case Com.Type.Entity:
			if (response.cmd == 1)
			{
				ResponseEntityAddGoldXP responseEntityAddGoldXP = (ResponseEntityAddGoldXP)response;
				if (entities.me.wrapper != null)
				{
					CombatPopup.PlayGoldXPPopups(entities.me.wrapper.transform.position, responseEntityAddGoldXP.dGold, responseEntityAddGoldXP.dXP, responseEntityAddGoldXP.dClassXP);
				}
				Session.MyPlayerData.UpdateGold(responseEntityAddGoldXP.fGold);
				Session.MyPlayerData.UpdateXP(responseEntityAddGoldXP.fXP);
				Session.MyPlayerData.UpdateClassXP(responseEntityAddGoldXP.classID, responseEntityAddGoldXP.fClassXP);
			}
			else if (response.cmd == 32)
			{
				ResponseGloryXpUpdate responseGloryXpUpdate = (ResponseGloryXpUpdate)response;
				Session.MyPlayerData.UpdateGloryXP(responseGloryXpUpdate.ClassID, responseGloryXpUpdate.GloryXP);
			}
			else if (response.cmd == 2)
			{
				ResponseEntityLevelUp responseEntityLevelUp = (ResponseEntityLevelUp)response;
				switch ((Entity.Type)responseEntityLevelUp.EntityType)
				{
				case Entity.Type.Player:
				{
					Entity playerById3 = entities.GetPlayerById(responseEntityLevelUp.EntityID);
					playerById3.SetLevel(responseEntityLevelUp.Level, responseEntityLevelUp.ScaledLevel, responseEntityLevelUp.ExpectedStat);
					playerById3.ShowLevelUpParticles();
					AudioManager.PlayCombatSFX("SFX_2SecondLevelUp", playerById3.isMe, playerById3.wrapper.transform);
					if (playerById3.isMe)
					{
						if (responseEntityLevelUp.Level == 8)
						{
							StartCoroutine(ReportAdvertizerInstallAttribution());
						}
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("af_level", responseEntityLevelUp.Level.ToString());
						AppsFlyer.sendEvent("af_level_achieved", dictionary);
						playerById3.ShowLevelUp();
						Session.MyPlayerData.SetXP(responseEntityLevelUp.XP, responseEntityLevelUp.XPToLevel);
						if (Session.IsGuest && (responseEntityLevelUp.Level == 3 || responseEntityLevelUp.Level == 7))
						{
							UIAccountCreate.Instance.ShowConvertGuest("Congrats!", "You just leveled up\nCreate a login now to protect your progress!", delayOpen: true);
						}
					}
					break;
				}
				case Entity.Type.NPC:
					entities.GetNpcById(responseEntityLevelUp.EntityID).SetLevel(responseEntityLevelUp.Level, responseEntityLevelUp.Level, responseEntityLevelUp.ExpectedStat);
					break;
				case Entity.Type.Undefined:
					break;
				}
			}
			else if (response.cmd == 21)
			{
				ResponseEntityRankUp responseEntityRankUp = (ResponseEntityRankUp)response;
				Entity playerById4 = entities.GetPlayerById(responseEntityRankUp.EntityID);
				playerById4.SetClassXPAndTotalClassRank(responseEntityRankUp.ClassXP, responseEntityRankUp.TotalClassRank);
				playerById4.ShowRankUpParticles();
				AudioManager.PlayCombatSFX("sfx_engine_powerhit", playerById4.isMe, playerById4.wrapper.transform);
				if (playerById4.isMe)
				{
					playerById4.ShowRankUp();
					Session.MyPlayerData.UpdateClassXP(responseEntityRankUp.ClassID, responseEntityRankUp.ClassXP);
					Session.MyPlayerData.OnClassRankUpdated(responseEntityRankUp.ClassID);
				}
			}
			else if (response.cmd == 3)
			{
				ComEntityUpdate entityUpdate = ((ResponseEntityUpdate)response).entityUpdate;
				entities.GetEntity(entityUpdate.entityType, entityUpdate.entityID)?.UpdateServerEffects(entityUpdate);
				combat.HandleEntityUpdate(entityUpdate, updateServerState: true, updateVisualState: true);
			}
			else if (response.cmd == 4)
			{
				ComEntity entity4 = ((ResponsePlayerRespawn)response).entity;
				Debug.Log("HandlePlayerRespawn > " + entity4.name);
				entities.GetPlayerById(entity4.ID)?.Respawn(entity4);
			}
			else if (response.cmd == 6)
			{
				ResponseEntityClass responseEntityClass = (ResponseEntityClass)response;
				combat.SetSkills(responseEntityClass.Spells);
			}
			else if (response.cmd == 7)
			{
				ResponseMCUpdate responseMCUpdate = (ResponseMCUpdate)response;
				Session.MyPlayerData.UpdateMC(responseMCUpdate.MC);
			}
			else if (response.cmd == 8)
			{
				ResponseGoldUpdate responseGoldUpdate = (ResponseGoldUpdate)response;
				Session.MyPlayerData.UpdateGold(responseGoldUpdate.Gold);
			}
			else if (response.cmd == 9)
			{
				ResponseBitFlagUpdate responseBitFlagUpdate = (ResponseBitFlagUpdate)response;
				Session.MyPlayerData.SetBitFlag(responseBitFlagUpdate.Badge.BitFlagName, (byte)responseBitFlagUpdate.Badge.BitFlagIndex, responseBitFlagUpdate.Value);
				if (responseBitFlagUpdate.ShowInChat)
				{
					Chat.Notify("\"" + responseBitFlagUpdate.Badge.Name + "\" unlocked!");
				}
				if (responseBitFlagUpdate.Notify)
				{
					FancyNotification.ShowText("Badge Unlocked!", responseBitFlagUpdate.Badge.Name);
				}
				if (responseBitFlagUpdate.Badge.Title != null)
				{
					Instance.SendEntityLoadBadgesRequest();
					Badge badge = Badges.Get(responseBitFlagUpdate.Badge.ID);
					if (badge != null)
					{
						badge.isNew = true;
					}
				}
			}
			else if (response.cmd == 10)
			{
				ResponseEntityAssetOverride responseEntityAssetOverride = (ResponseEntityAssetOverride)response;
				entities.GetEntity(responseEntityAssetOverride.etyp, responseEntityAssetOverride.EntityID)?.UpdateOverrideAsset(responseEntityAssetOverride.overrideAsset);
			}
			else if (response.cmd == 15)
			{
				ResponseEntityAssetUpdate responseEntityAssetUpdate = (ResponseEntityAssetUpdate)response;
				entities.GetEntity(responseEntityAssetUpdate.etyp, responseEntityAssetUpdate.EntityID)?.UpdateAsset(responseEntityAssetUpdate.asset);
			}
			else if (response.cmd == 11)
			{
				ResponseEntityCustomize responseEntityCustomize = (ResponseEntityCustomize)response;
				entities.GetEntity(responseEntityCustomize.etyp, responseEntityCustomize.EntityID)?.UpdateCustomization(responseEntityCustomize.haircolor, responseEntityCustomize.skincolor, responseEntityCustomize.eyecolor, responseEntityCustomize.lipcolor, responseEntityCustomize.hair, responseEntityCustomize.braid, responseEntityCustomize.stache, responseEntityCustomize.beard);
			}
			else if (response.cmd == 24)
			{
				ResponseEntityGender responseEntityGender = (ResponseEntityGender)response;
				entities.GetEntity(responseEntityGender.etyp, responseEntityGender.EntityID)?.UpdateGender(responseEntityGender.gender);
			}
			else if (response.cmd == 12)
			{
				ResponseDataSync responseDataSync = (ResponseDataSync)response;
				Session.MyPlayerData.LoadedBanks.Clear();
				Session.MyPlayerData.LoadedBanks.Add(0);
				Session.MyPlayerData.SetAllItems(responseDataSync.items);
				Session.MyPlayerData.merges = ((responseDataSync.merges != null) ? responseDataSync.merges : new List<Merge>());
				Session.MyPlayerData.charClasses = ((responseDataSync.charClasses != null) ? responseDataSync.charClasses : new List<CharClass>());
				Session.MyPlayerData.BitFields = responseDataSync.BitFields;
				Session.MyPlayerData.Gold = responseDataSync.Gold;
				Session.MyPlayerData.MC = responseDataSync.MC;
				Session.MyPlayerData.XPMultiplier = responseDataSync.XPMultiplier;
				Session.MyPlayerData.GoldMultiplier = responseDataSync.GoldMultiplier;
				Session.MyPlayerData.CXPMultiplier = responseDataSync.CXPMultiplier;
				Session.MyPlayerData.DailyQuestMultiplier = responseDataSync.DailyQuestMultiplier;
				Session.MyPlayerData.DailyTaskMultiplier = responseDataSync.DailyTaskMultiplier;
				Session.MyPlayerData.OnDataSync();
				Session.MyPlayerData.BankVaultCount = responseDataSync.BankVault;
			}
			else if (response.cmd == 16)
			{
				ResponseEntityPortraitUpdate responseEntityPortraitUpdate = (ResponseEntityPortraitUpdate)response;
				Entities.Instance.GetEntity(Entity.Type.Player, responseEntityPortraitUpdate.EntityID).UpdatePortrait(responseEntityPortraitUpdate.Portrait);
			}
			else if (response.cmd == 17)
			{
				ResponseEntityTitleUpdate responseEntityTitleUpdate = (ResponseEntityTitleUpdate)response;
				Entities.Instance.GetEntity(Entity.Type.Player, responseEntityTitleUpdate.EntityID).UpdateTitle(responseEntityTitleUpdate.Title, responseEntityTitleUpdate.TitleName);
			}
			else if (response.cmd == 18)
			{
				ResponseEntityLoadBadges responseEntityLoadBadges = (ResponseEntityLoadBadges)response;
				Badges.Init(responseEntityLoadBadges.Badges, responseEntityLoadBadges.Categories);
			}
			else if (response.cmd == 19)
			{
				ResponsePetEquip responsePetEquip = (ResponsePetEquip)response;
				Entities.Instance.GetEntity(Entity.Type.Player, responsePetEquip.EntityID).EquipPet(responsePetEquip.PetEquipItem);
			}
			else if (response.cmd == 25)
			{
				ResponsePetInteract responsePetInteract = (ResponsePetInteract)response;
				Player playerById5 = Entities.Instance.GetPlayerById(responsePetInteract.playerID);
				if (playerById5 == null)
				{
					break;
				}
				GameObject petGO = playerById5.petGO;
				if (petGO != null)
				{
					PetMovementController component = petGO.GetComponent<PetMovementController>();
					if (component != null)
					{
						component.ProcessPetInteractions(playerById5, responsePetInteract.animation);
					}
				}
			}
			else if (response.cmd == 20)
			{
				ResponsePetUnequip responsePetUnequip = (ResponsePetUnequip)response;
				Entities.Instance.GetEntity(Entity.Type.Player, responsePetUnequip.EntityID).UnequipPet();
			}
			else if (response.cmd == 22)
			{
				ResponseAFK responseAFK = (ResponseAFK)response;
				Entities.Instance.GetEntity(Entity.Type.Player, responseAFK.EntityID).SetNamePlateAFK(responseAFK.isAFK);
			}
			else if (response.cmd == 23)
			{
				_ = (ResponseServerDisconnect)response;
			}
			else if (response.cmd == 26)
			{
				ResponseEntityWeaponUpdate responseEntityWeaponUpdate = (ResponseEntityWeaponUpdate)response;
				Entities.Instance.GetEntity(responseEntityWeaponUpdate.entityType, responseEntityWeaponUpdate.entityID)?.UpdateWeaponAsset(responseEntityWeaponUpdate.equipItem);
			}
			else if (response.cmd == 27)
			{
				ResponseEntityToolUpdate responseEntityToolUpdate = (ResponseEntityToolUpdate)response;
				Entities.Instance.GetEntity(responseEntityToolUpdate.entityType, responseEntityToolUpdate.entityID)?.UpdateToolAsset(responseEntityToolUpdate.equipItem);
			}
			else if (response.cmd == 28)
			{
				ResponsePlayerTradeSkillAddXP responsePlayerTradeSkillAddXP = response as ResponsePlayerTradeSkillAddXP;
				if (entities.me.wrapper != null)
				{
					Vector3 position2 = entities.me.wrapper.transform.position;
					CombatPopup.PlayTradeSkillXPPopup(responsePlayerTradeSkillAddXP.tradeSkillType, position2, responsePlayerTradeSkillAddXP.xpGained);
				}
				if (responsePlayerTradeSkillAddXP.xpGained > 0)
				{
					TradeSkillXPBar.Instance.Show(responsePlayerTradeSkillAddXP.tradeSkillType, responsePlayerTradeSkillAddXP.xpGained, responsePlayerTradeSkillAddXP.xpFinal);
				}
				Session.MyPlayerData.UpdateTradeSkillXP(responsePlayerTradeSkillAddXP.tradeSkillType, responsePlayerTradeSkillAddXP.xpFinal);
			}
			else if (response.cmd == 29)
			{
				ResponsePlayerTradeSkillLevelUp responsePlayerTradeSkillLevelUp = response as ResponsePlayerTradeSkillLevelUp;
				Player playerById6 = Entities.Instance.GetPlayerById(responsePlayerTradeSkillLevelUp.playerID);
				if (playerById6 != null)
				{
					playerById6.SetTradeSkillLevel(responsePlayerTradeSkillLevelUp.tradeSkillType, responsePlayerTradeSkillLevelUp.level);
					playerById6.ShowTradeSkillUpParticles(responsePlayerTradeSkillLevelUp.tradeSkillType);
					if (playerById6.isMe)
					{
						playerById6.ShowTradeSkillUp(responsePlayerTradeSkillLevelUp.tradeSkillType);
						Session.MyPlayerData.SetTradeSkillXP(responsePlayerTradeSkillLevelUp.tradeSkillType, responsePlayerTradeSkillLevelUp.xp, responsePlayerTradeSkillLevelUp.xpToLevel);
					}
				}
			}
			else if (response.cmd == 30)
			{
				ResponseEntityCapstoneBarFill responseEntityCapstoneBarFill = (ResponseEntityCapstoneBarFill)response;
				Entity playerById7 = entities.GetPlayerById(responseEntityCapstoneBarFill.EntityID);
				if (playerById7.isMe && !playerById7.CombatClass.IsSkin)
				{
					playerById7.ShowCapstoneFill();
				}
			}
			else
			{
				if (response.cmd != 31)
				{
					break;
				}
				{
					foreach (KeyValuePair<int, int> pvpActionID in ((ResponsePvpActionEquip)response).PvpActionIDs)
					{
						Session.MyPlayerData.SetPvpAction((CombatSpellSlot)pvpActionID.Key, pvpActionID.Value);
					}
					break;
				}
			}
			break;
		case Com.Type.Friend:
			if (response.cmd == 6)
			{
				ResponseGoto rss = (ResponseGoto)response;
				string message = "Would you like to join " + rss.ToPlayerName + " in " + rss.AreaName + "?";
				Confirmation.Show("Summon", message, delegate(bool b)
				{
					if (b)
					{
						SendGotoRequest(rss.CharID, rss.Code);
					}
				});
			}
			else if (response.cmd == 4)
			{
				ResponseFriendList responseFriendList = (ResponseFriendList)response;
				if (responseFriendList.Friends != null)
				{
					Session.MyPlayerData.UpdateFriends(responseFriendList.Friends);
				}
				else
				{
					Session.MyPlayerData.UpdateFriends(new List<FriendData>());
				}
			}
			else if (response.cmd == 1)
			{
				ResponseFriend responseFriend = (ResponseFriend)response;
				if ((bool)SettingsManager.CanGetFriendRequests)
				{
					Chat.Notify(responseFriend.From + " would like to be your friend.");
					Session.MyPlayerData.AddFriendRequest(responseFriend);
				}
			}
			else if (response.cmd == 2)
			{
				Chat.Notify(((ResponseFriendAdd)response).friend.strName + " is now your friend.");
			}
			else if (response.cmd == 5)
			{
				ResponseSummon responseSummon = (ResponseSummon)response;
				Chat.Notify(responseSummon.From + " wants you to join them.", "[00A700]");
				Session.MyPlayerData.AddFriendRequest(responseSummon);
			}
			else if (response.cmd == 7)
			{
				Chat.Notify(((ResponseFriendAdded)response).GetMessage());
			}
			break;
		case Com.Type.Housing:
			HousingManager.HandleHousingResponse(response);
			break;
		case Com.Type.Item:
			if (response.cmd == 1)
			{
				ResponseItemEquip responseItemEquip = (ResponseItemEquip)response;
				Session.MyPlayerData.EquipItem(responseItemEquip.CharItemID, responseItemEquip.EquipID);
				break;
			}
			if (response.cmd == 2)
			{
				ResponseItemUnequip responseItemUnequip = (ResponseItemUnequip)response;
				Session.MyPlayerData.UnequipItem(responseItemUnequip.CharItemID);
				break;
			}
			if (response.cmd == 4)
			{
				ResponseItemRemove responseItemRemove = (ResponseItemRemove)response;
				Session.MyPlayerData.RemoveItem(responseItemRemove.CharItemID, responseItemRemove.BankID);
				break;
			}
			if (response.cmd == 5)
			{
				ResponseItemAdd responseItemAdd = (ResponseItemAdd)response;
				Session.MyPlayerData.AddItem(responseItemAdd.Item);
				responseItemAdd.Item.IsNew = true;
				break;
			}
			if (response.cmd == 3)
			{
				ResponseItemUpdate responseItemUpdate = (ResponseItemUpdate)response;
				Session.MyPlayerData.UpdateItem(responseItemUpdate.CharItemID, responseItemUpdate.Quantity, responseItemUpdate.BankID);
				break;
			}
			if (response.cmd == 9)
			{
				ResponseOpenLootBox responseOpenLootBox = (ResponseOpenLootBox)response;
				Session.MyPlayerData.UpdateMC(responseOpenLootBox.MCTotal);
				if (this.ReceivedLoot != null)
				{
					this.ReceivedLoot(responseOpenLootBox.Items, responseOpenLootBox.MC);
				}
				break;
			}
			if (response.cmd == 10)
			{
				ResponseItemDailyReward responseItemDailyReward = (ResponseItemDailyReward)response;
				Session.MyPlayerData.UpdateDailyReward(responseItemDailyReward.Day, responseItemDailyReward.Date, responseItemDailyReward.ItemID);
				break;
			}
			if (response.cmd == 7)
			{
				foreach (Item item2 in ((ResponseItemLoad)response).Items)
				{
					Items.Add(item2);
				}
				break;
			}
			if (response.cmd == 18)
			{
				ResponseItemInvReload responseItemInvReload = (ResponseItemInvReload)response;
				Session.MyPlayerData.SetAllItems(responseItemInvReload.InvItems);
				Session.MyPlayerData.ReloadInventory();
			}
			else if (response.cmd == 15)
			{
				if (response is ResponseItemLog responseItemLog)
				{
					Chat.AddMessage(InterfaceColors.Chat.Dark_White.ToBBCode() + responseItemLog.start + responseItemLog.count + "x[-] " + InterfaceColors.Chat.White.ToBBCode() + "[[-][" + Item.GetChatRarityColor(responseItemLog.rarity) + "]" + responseItemLog.name + "[-]" + InterfaceColors.Chat.White.ToBBCode() + "][-]" + InterfaceColors.Chat.Dark_White.ToBBCode() + responseItemLog.end + "[-]");
				}
			}
			else if (response.cmd == 16)
			{
				ResponseInfusion responseInfusion = (ResponseInfusion)response;
				Session.MyPlayerData.InfuseConfirmation(responseInfusion.cItem);
				Notification.ShowText("Infusion Successful!");
			}
			else if (response.cmd == 17)
			{
				ResponseExtract responseExtract = (ResponseExtract)response;
				Session.MyPlayerData.ExtractConfirmation(responseExtract.cItem);
				Notification.ShowText("Extraction Successful!");
			}
			else if (response.cmd == 20)
			{
				ResponseItemModifierReroll responseItemModifierReroll = (ResponseItemModifierReroll)response;
				Session.MyPlayerData.ItemRerollResponse(responseItemModifierReroll.modifier);
				Notification.ShowText("Reroll Successful");
			}
			else if (response.cmd == 22)
			{
				ResponseItemModifierRerollConfirm responseItemModifierRerollConfirm = (ResponseItemModifierRerollConfirm)response;
				Session.MyPlayerData.ItemModifierRerollConfirmation(responseItemModifierRerollConfirm.CItem);
			}
			break;
		case Com.Type.Login:
		{
			ResponseLogin responseLogin = (ResponseLogin)response;
			if (responseLogin.status == 0)
			{
				Debug.Log("Socketserver token authentication failure");
				MessageBox.Show("Error", responseLogin.message);
				BackToLogin();
				break;
			}
			Debug.Log("Logged in with ID " + Session.MyPlayerData.ID);
			GameTime.Init(responseLogin.Server, responseLogin.Login);
			Session.MyPlayerData.UserID = responseLogin.UserID;
			Session.MyPlayerData.ID = responseLogin.ID;
			Session.MyPlayerData.LoadedBanks.Clear();
			Session.MyPlayerData.LoadedBanks.Add(0);
			Session.MyPlayerData.SetAllItems(responseLogin.items);
			Session.MyPlayerData.merges = ((responseLogin.merges != null) ? responseLogin.merges : new List<Merge>());
			Session.MyPlayerData.charClasses = ((responseLogin.charClasses != null) ? responseLogin.charClasses : new List<CharClass>());
			Session.MyPlayerData.CurQuests = ((responseLogin.Quests != null) ? responseLogin.Quests : new List<int>());
			Session.MyPlayerData.CurQuestObjectives = ((responseLogin.QuestObjectives != null) ? responseLogin.QuestObjectives : new Dictionary<int, int>());
			Session.MyPlayerData.QuestStrings = ((responseLogin.QuestStrings != null) ? responseLogin.QuestStrings : new Dictionary<int, QuestString>());
			Session.MyPlayerData.QuestChains = ((responseLogin.QuestChains != null) ? responseLogin.QuestChains : new Dictionary<int, int>());
			Session.MyPlayerData.AccessLevel = responseLogin.AccessLevel;
			Session.MyPlayerData.BitFields = responseLogin.BitFields;
			Session.MyPlayerData.Gold = responseLogin.Gold;
			Session.MyPlayerData.MC = responseLogin.MC;
			Session.MyPlayerData.SetXP(responseLogin.XP, responseLogin.XPToLevel);
			Session.MyPlayerData.LevelCap = responseLogin.LevelCap;
			Session.MyPlayerData.EndGame = responseLogin.EndGame;
			Session.MyPlayerData.BankVaultCount = responseLogin.BankVault;
			Session.MyPlayerData.BankVaultCost = responseLogin.BankVaultCost;
			Session.MyPlayerData.RewardIndex = responseLogin.RewardIndex;
			Session.MyPlayerData.RewardDate = responseLogin.RewardDate;
			Session.MyPlayerData.ProductOffers = ((responseLogin.productOffers != null) ? responseLogin.productOffers : new Dictionary<ProductID, ProductOffer>());
			UINews.NewsID = responseLogin.NewsApopID;
			UINews.NewsState = (NewsState)responseLogin.NewsApopState;
			UINews.NewsVersion = responseLogin.NewsApopVersion;
			UINews.NewsCooldown = responseLogin.NewsApopCooldown;
			Session.MyPlayerData.MOTD = responseLogin.MOTD;
			Session.MyPlayerData.LatencyNotifyThreshold = responseLogin.LatencyNotifyThreshold;
			Session.MyPlayerData.LatencyDisconnectThreshold = responseLogin.LatencyDisconnectThreshold;
			Session.MyPlayerData.SetGameParams(responseLogin.GameParams);
			Products.ProductPackages = responseLogin.ProductPackages;
			Session.MyPlayerData.tradeSkillXP = responseLogin.tradeSkillXP;
			Session.MyPlayerData.tradeSkillXPToLevel = responseLogin.tradeSkillXPToLevel;
			Session.MyPlayerData.HouseSlotCost = responseLogin.HouseSlotCost;
			Session.MyPlayerData.HouseSlotMax = responseLogin.HouseSlotMax;
			Session.MyPlayerData.HouseItemMaxPerSlot = responseLogin.HouseItemMaxPerSlot;
			Session.MyPlayerData.HouseMaxPlayers = responseLogin.HouseMaxPlayers;
			Session.MyPlayerData.combatClassList = responseLogin.classes.OrderBy((CombatClass p) => p.SortOrder).ToList();
			foreach (CombatClass @class in responseLogin.classes)
			{
				Items.Add(@class.RewardItems);
				Badges.Add(@class.RewardBadges);
			}
			Items.Add(responseLogin.tokens);
			Items.Add(responseLogin.resources);
			SoundTracks.Instance.Set(responseLogin.soundTracks);
			SyncIgnore();
			int actionSlotID = SettingsManager.GetActionSlotID(CombatSpellSlot.CrossSkill);
			if (actionSlotID > 0)
			{
				SendCrossSkillEquipRequest(actionSlotID);
			}
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>
			{
				{
					12,
					SettingsManager.GetActionSlotID(CombatSpellSlot.PvpSpell1)
				},
				{
					13,
					SettingsManager.GetActionSlotID(CombatSpellSlot.PvpSpell2)
				}
			};
			if (dictionary2.Values.Any((int id) => id > 0))
			{
				SendPvpActionEquipRequest(dictionary2);
			}
			Sheathing.Instance.SendSheathingRequest(SettingsManager.AutoSheatheWeapons);
			Debug.Log("Logged in with ID " + Session.MyPlayerData.ID);
			break;
		}
		case Com.Type.Loot:
			if (response.cmd == 1)
			{
				ResponseLootAdd responseLootAdd = (ResponseLootAdd)response;
				AddLoot(responseLootAdd.Loot);
			}
			else if (response.cmd == 2)
			{
				ResponseLootRemoveItem responseLootRemoveItem = (ResponseLootRemoveItem)response;
				LootBags.RemoveItem(responseLootRemoveItem.LootBagID, responseLootRemoveItem.LootItemID, responseLootRemoveItem.ItemID);
			}
			break;
		case Com.Type.Machine:
			switch ((Com.CmdMachine)response.cmd)
			{
			case Com.CmdMachine.Update:
				HandleMachineUpdate((ResponseMachineUpdate)response);
				break;
			case Com.CmdMachine.Cast:
				HandleMachineCast((ResponseMachineCast)response);
				break;
			case Com.CmdMachine.Animation:
				HandleMachinePlayAnimation((ResponseMachinePlayAnimation)response);
				break;
			case Com.CmdMachine.AnimatorLayerWeight:
				HandleMachineAnimatorLayerWeight((ResponseMachineAnimatorLayerWeight)response);
				break;
			case Com.CmdMachine.AnimatorParameter:
				HandleMachineAnimatorParameter((ResponseMachineAnimatorParameter)response);
				break;
			case Com.CmdMachine.AreaFlag:
				HandleMachineAreaFlag((ResponseMachineAreaFlag)response);
				break;
			case Com.CmdMachine.CTState:
				HandleMachineSetCTState((ResponseMachineSetCTState)response);
				break;
			case Com.CmdMachine.Collision:
				HandleMachineCollision((ResponseMachineCollision)response);
				break;
			case Com.CmdMachine.HarpoonFire:
				HandleMachineHarpoonFire(response as ResponseMachineHarpoonFire);
				break;
			case Com.CmdMachine.HarpoonSync:
				HandleMachineHarpoonSync(response as ResponseMachineHarpoonSync);
				break;
			case Com.CmdMachine.ListenerUpdate:
				HandleMachineListenerUpdate((ResponseMachineListenerUpdate)response);
				break;
			case Com.CmdMachine.ResourceChannel:
				HandleMachineResourceChannel((ResponseMachineResourceChannel)response);
				break;
			case Com.CmdMachine.ResourceDespawn:
				HandleMachineResourceDespawn((ResponseMachineResourceDespawn)response);
				break;
			case Com.CmdMachine.ResourceInteract:
				HandleMachineResourceInteract((ResponseMachineResourceInteract)response);
				break;
			case Com.CmdMachine.ResourceInterrupt:
				HandleMachineResourceInterrupt((ResponseMachineResourceInterrupt)response);
				break;
			case Com.CmdMachine.ResourceSpawn:
				HandleMachineResourceSpawn((ResponseMachineResourceSpawn)response);
				break;
			case Com.CmdMachine.ResourceUsageUpdate:
				HandleMachineResourceUsed((ResponseMachineResourceUsageUpdate)response);
				break;
			case Com.CmdMachine.Click:
			case Com.CmdMachine.Enter:
			case Com.CmdMachine.Exit:
			case Com.CmdMachine.Trigger:
			case Com.CmdMachine.ResourceCollect:
			case Com.CmdMachine.ResourceDrop:
			case Com.CmdMachine.ResourceTrigger:
				break;
			}
			break;
		case Com.Type.Merge:
			if (response.cmd == 2)
			{
				ResponseMergeShopLoad responseMergeShopLoad = (ResponseMergeShopLoad)response;
				if (responseMergeShopLoad.mergeShop != null)
				{
					MergeShops.Add(responseMergeShopLoad.mergeShop);
				}
				if (this.MergeShopLoaded != null)
				{
					this.MergeShopLoaded(responseMergeShopLoad.mergeShop, responseMergeShopLoad.Message);
				}
			}
			else if (response.cmd == 6)
			{
				ResponseMergeAdd responseMergeAdd = (ResponseMergeAdd)response;
				Session.MyPlayerData.MergeAdd(responseMergeAdd.merge);
			}
			else if (response.cmd == 7)
			{
				ResponseMergeRemove responseMergeRemove = (ResponseMergeRemove)response;
				Session.MyPlayerData.MergeRemove(responseMergeRemove.MergeID);
			}
			break;
		case Com.Type.Message:
		{
			ResponseMessageBox responseMessageBox = (ResponseMessageBox)response;
			if (responseMessageBox.upg)
			{
				Confirmation.Show(responseMessageBox.title, responseMessageBox.msg, delegate(bool confirm)
				{
					if (confirm)
					{
						UIIAPStore.Show();
					}
				});
			}
			else
			{
				MessageBox.Show(responseMessageBox.title, responseMessageBox.msg);
			}
			break;
		}
		case Com.Type.Misc:
			if (response.cmd == 1)
			{
				ResponseNotify responseNotify = (ResponseNotify)response;
				Notify(responseNotify.Message, responseNotify.NotificationType);
			}
			if (response.cmd == 4)
			{
				ResponseEventDetails responseEventDetails = (ResponseEventDetails)response;
				Session.MyPlayerData.XPMultiplier = responseEventDetails.XPMultiplier;
				Session.MyPlayerData.GoldMultiplier = responseEventDetails.GoldMultiplier;
				Session.MyPlayerData.CXPMultiplier = responseEventDetails.CXPMultiplier;
				Session.MyPlayerData.DailyQuestMultiplier = responseEventDetails.DailyQuestMultiplier;
				Session.MyPlayerData.DailyTaskMultiplier = responseEventDetails.DailyTaskMultiplier;
			}
			break;
		case Com.Type.Move:
		{
			ResponseMovement responseMovement = (ResponseMovement)response;
			Player playerById9 = entities.GetPlayerById(responseMovement.PlayerID);
			if (playerById9 != null && playerById9 != entities.me && playerById9.moveController != null)
			{
				(playerById9.moveController as RemoteMovementController).UpdateRemote(responseMovement);
				playerById9.position = new Vector3(responseMovement.posX, responseMovement.posY, responseMovement.posZ);
				playerById9.rotation = Quaternion.Euler(0f, responseMovement.rotY, 0f);
			}
			break;
		}
		case Com.Type.NPC:
			switch (response.cmd)
			{
			case 7:
			{
				ResponseNPCPlayAnimation responseNPCPlayAnimation = response as ResponseNPCPlayAnimation;
				Entities.Instance.GetNpcBySpawnId(responseNPCPlayAnimation.spawnID)?.PlayAnimation(responseNPCPlayAnimation.animation, 1f);
				break;
			}
			case 8:
			{
				ResponseNPCBaited responseNPCBaited = response as ResponseNPCBaited;
				NPC npcBySpawnId4 = Entities.Instance.GetNpcBySpawnId(responseNPCBaited.spawnID);
				if (npcBySpawnId4 != null)
				{
					npcBySpawnId4.BaitState = responseNPCBaited.baitState;
				}
				break;
			}
			case 9:
			{
				ResponseNPCMovementBehavior responseNPCMovementBehavior = response as ResponseNPCMovementBehavior;
				NPC npcBySpawnId2 = Entities.Instance.GetNpcBySpawnId(responseNPCMovementBehavior.spawnID);
				if (npcBySpawnId2 != null)
				{
					npcBySpawnId2.moveBehavior = responseNPCMovementBehavior.behavior;
				}
				break;
			}
			case 1:
			{
				ResponseNPCDespawn responseNPCDespawn = (ResponseNPCDespawn)response;
				entities.RemoveNpcById(responseNPCDespawn.ID);
				break;
			}
			case 4:
				Entities.Instance.me.interactingNPC = null;
				break;
			case 10:
			{
				ResponseNPCNotify responseNPCNotify = response as ResponseNPCNotify;
				NPC npcBySpawnId3 = entities.GetNpcBySpawnId(responseNPCNotify.spawnID);
				if (npcBySpawnId3 != null)
				{
					npcBySpawnId3.NamePlateBubble(Entities.Instance.me.ScrubText(responseNPCNotify.message));
					string text = InterfaceColors.Chat.Yellow.ToBBCode() + npcBySpawnId3.name + ": " + InterfaceColors.Chat.Dark_White.ToBBCode();
					switch (responseNPCNotify.NotificationType)
					{
					default:
						UINpcNotification.Show(npcBySpawnId3.name, responseNPCNotify.message);
						Chat.Instance.AddText(text + responseNPCNotify.message + "[-]", Chat.FilterType.Notification);
						break;
					case GameNotificationType.Standard:
						UINpcNotification.Show(npcBySpawnId3.name, responseNPCNotify.message);
						break;
					case GameNotificationType.Chat:
						Chat.Instance.AddText(text + responseNPCNotify.message + "[-]", Chat.FilterType.Notification);
						break;
					}
				}
				break;
			}
			case 2:
			{
				ResponseNPCSpawn responseNPCSpawn = (ResponseNPCSpawn)response;
				NPC nPC = new NPC(responseNPCSpawn.entity);
				entities.AddNpc(nPC);
				if (isReady)
				{
					nPC.Init(entitiesGO.transform, Spawns.ContainsKey(nPC.SpawnID) ? Spawns[nPC.SpawnID] : null, cam);
					nPC.Load();
					if (!string.IsNullOrEmpty(responseNPCSpawn.spellFx))
					{
						SpellFXContainer.mInstance.CreateEntityFX(nPC, isCasterMe: false, responseNPCSpawn.spellFx);
					}
				}
				break;
			}
			case 6:
			{
				ResponseNPCTeamEvent responseNPCTeamEvent = response as ResponseNPCTeamEvent;
				Scoreboard.UpdateTeamNPC(responseNPCTeamEvent.spawnID, responseNPCTeamEvent.teamID);
				break;
			}
			case 5:
			{
				ResponseNPCTurnTo responseNPCTurnTo = (ResponseNPCTurnTo)response;
				NPC npcBySpawnId = entities.GetNpcBySpawnId(responseNPCTurnTo.spawnID);
				if (npcBySpawnId == null || npcBySpawnId.wrapper == null)
				{
					break;
				}
				npcBySpawnId.isInteracting = responseNPCTurnTo.isInteracting;
				bool flag = true;
				if (npcBySpawnId.Spawn != null)
				{
					foreach (InteractionRequirement requirement in npcBySpawnId.Spawn.Requirements)
					{
						if (!requirement.IsRequirementMet(Session.MyPlayerData))
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					break;
				}
				Player playerById8 = entities.GetPlayerById(responseNPCTurnTo.playerID);
				bool flag2 = npcBySpawnId.entitycontroller.ContainsNPCStaticAnimations();
				if (playerById8 != null && playerById8.wrapper != null)
				{
					float angle = 0f;
					if (npcBySpawnId.CanRotate && !flag2)
					{
						Vector3 targetDirection = Vector3.Normalize(playerById8.wrapperTransform.position - npcBySpawnId.wrapperTransform.position);
						angle = npcBySpawnId.TurnTo(targetDirection);
					}
					if (responseNPCTurnTo.isInteracting)
					{
						npcBySpawnId.InteractWithPlayer(angle);
					}
					Vector3 targetDirection2 = Vector3.Normalize(npcBySpawnId.wrapperTransform.position - playerById8.wrapperTransform.position);
					playerById8.TurnTo(targetDirection2);
				}
				else if (npcBySpawnId.CanRotate && !flag2)
				{
					Vector3 targetDirection3 = Vector3.Normalize(responseNPCTurnTo.position - npcBySpawnId.wrapperTransform.position);
					npcBySpawnId.TurnTo(targetDirection3);
				}
				break;
			}
			case 3:
				break;
			}
			break;
		case Com.Type.NpcMove:
			if (response.cmd == 1)
			{
				ResponseMovePath responseMovePath = (ResponseMovePath)response;
				entities.GetNpcById(responseMovePath.ID)?.Move(responseMovePath.Path, responseMovePath.PathingType, responseMovePath.Speed, responseMovePath.StartTS, responseMovePath.Animations, responseMovePath.UseRotation, responseMovePath.RotationY, responseMovePath.SequentialAnimations);
			}
			else if (response.cmd == 2)
			{
				ResponsePathSpeed responsePathSpeed = (ResponsePathSpeed)response;
				NPC npcById = entities.GetNpcById(responsePathSpeed.ID);
				if (npcById != null && npcById.moveController != null)
				{
					(npcById.moveController as NPCMovementController).SetMoveSpeed(responsePathSpeed.Speed);
				}
			}
			else if (response.cmd == 3)
			{
				ResponsePathStop responsePathStop = (ResponsePathStop)response;
				NPC npcById2 = entities.GetNpcById(responsePathStop.ID);
				if (npcById2 != null && npcById2.moveController != null)
				{
					(npcById2.moveController as NPCMovementController).Stop();
				}
			}
			else if (response.cmd == 4)
			{
				ResponseStopSync responseStopSync = (ResponseStopSync)response;
				NPC npcById3 = entities.GetNpcById(responseStopSync.entityId);
				if (npcById3 != null && npcById3.moveController != null)
				{
					NPCMovementController obj = (NPCMovementController)npcById3.moveController;
					obj.Stop();
					Vector3 position = new Vector3(responseStopSync.posX, responseStopSync.posY, responseStopSync.posZ);
					obj.SyncPosition(position, responseStopSync.rotY);
				}
			}
			break;
		case Com.Type.Party:
			if (response.cmd == 1)
			{
				ResponsePartyInvite responsePartyInvite = (ResponsePartyInvite)response;
				Chat.Notify(responsePartyInvite.GetMessage());
				PartyManager.AddPartyInvite(responsePartyInvite);
				AudioManager.Play2DSFX("Notif_Social");
				Confirmation.Show("Party Invitation", responsePartyInvite.GetMessage(), delegate(bool b)
				{
					int leaderID = PartyManager.PartyInvite.LeaderID;
					if (!b)
					{
						PartyManager.RemovePartyInvite();
					}
					SendPartyJoinRequest(leaderID, b);
				}, isClosable: false, enableCollider: false);
			}
			else if (response.cmd == 5)
			{
				ResponsePartyList responsePartyList = (ResponsePartyList)response;
				PartyManager.SetPartyData(responsePartyList.Data, responsePartyList.LeaderID, responsePartyList.IsPrivate);
			}
			else if (response.cmd == 11)
			{
				PartyManager.UpdatePrivacy(((ResponsePartyPrivacy)response).IsPrivate);
			}
			else if (response.cmd == 3)
			{
				PartyManager.Clear();
			}
			else if (response.cmd == 8)
			{
				ResponseVoteKickStart responseVoteKick = (ResponseVoteKickStart)response;
				if (PartyManager.IsInParty)
				{
					if (uigame?.partyHUD != null)
					{
						UIPartyPortrait uIPartyPortrait = uigame.partyHUD.portraits.FirstOrDefault((UIPartyPortrait member) => member.IsID(responseVoteKick.playerID));
						if (uIPartyPortrait != null)
						{
							uigame.voteKick.StartVote();
							uigame.voteKick.Init(uIPartyPortrait.GetMemberName(), uigame.voteKickButton);
						}
					}
				}
				else
				{
					Player playerById = Entities.Instance.GetPlayerById(responseVoteKick.playerID);
					if (playerById != null)
					{
						uigame.voteKick.StartVote();
						uigame.voteKick.Init(playerById.name, uigame.voteKickButton);
					}
				}
			}
			else if (response.cmd == 10)
			{
				uigame.voteKick.TurnOff();
			}
			else
			{
				if (response.cmd == 12)
				{
					ResponsePartyDisconnect responsePartyDisconnect = response as ResponsePartyDisconnect;
					if (!PartyManager.IsInParty)
					{
						break;
					}
					UIPartyHUD partyHUD = uigame.partyHUD;
					if (!(partyHUD != null))
					{
						break;
					}
					{
						foreach (UIPartyPortrait portrait in partyHUD.portraits)
						{
							if (portrait.IsID(responsePartyDisconnect.playerID))
							{
								portrait.ShowDisconnect();
								break;
							}
						}
						break;
					}
				}
				if (response.cmd != 13)
				{
					break;
				}
				ResponsePartyReconnect responsePartyReconnect = response as ResponsePartyReconnect;
				if (!PartyManager.IsInParty)
				{
					break;
				}
				UIPartyHUD partyHUD2 = uigame.partyHUD;
				if (!(partyHUD2 != null))
				{
					break;
				}
				{
					foreach (UIPartyPortrait portrait2 in partyHUD2.portraits)
					{
						if (portrait2.IsID(responsePartyReconnect.playerID))
						{
							portrait2.HideDisconnect();
							break;
						}
					}
					break;
				}
			}
			break;
		case Com.Type.Player:
			HandleResponsePlayer(response);
			break;
		case Com.Type.PvP:
			HandleResponsePvP(response);
			break;
		case Com.Type.Quest:
			HandleResponseQuest(response);
			break;
		case Com.Type.Resource:
			HandleResponseResource(response);
			break;
		case Com.Type.SystemPerformance:
			HandleResponseSystemPerformance(response);
			break;
		case Com.Type.Trade:
			HandleResponseTrade(response);
			break;
		case Com.Type.Guild:
			HandleResponseGuild(response);
			break;
		case Com.Type.DailyTask:
			HandleResponseDailyTask(response);
			break;
		case Com.Type.ServerDailyTask:
			HandleResponseServerDailyTask(response);
			break;
		case Com.Type.Leaderboard:
			HandleResponseLeaderBoard(response);
			break;
		case Com.Type.DailyQuestReset:
			HandleResponseDailyQuestReset(response);
			break;
		case Com.Type.Sheathing:
			HandleResponseSheathing(response);
			break;
		case Com.Type.Mail:
		{
			ResponseMail responseMail = (ResponseMail)response;
			Session.MyPlayerData.mailbox = responseMail.mailbox;
			Session.MyPlayerData.mailrewards = responseMail.rewards;
			Session.MyPlayerData.mailRewardsList = responseMail.rewardItems;
			break;
		}
		case Com.Type.Joinmychannels:
		case Com.Type.NPCTemplates:
		case Com.Type.SpellTemplates:
		case Com.Type.Report:
		case Com.Type.SyncIgnore:
		case Com.Type.Disconnect:
		case Com.Type.EndTransfer:
		case Com.Type.DirectCommand:
		case Com.Type.SuperClient:
		case Com.Type.ServerEvent:
			break;
		}
	}

	public void SendItemUseRequest(InventoryItem iItem)
	{
		if (iItem.SpellID > 0)
		{
			combat.CastItem(iItem);
		}
		else if (iItem.Action != null)
		{
			if (iItem.Action is ItemActionTransfer)
			{
				SendItemTransferRequest(iItem);
			}
			if (iItem.Action is ItemActionChatCommand)
			{
				ChatCommands.ProcessCommand(((ItemActionChatCommand)iItem.Action).Command.Split(new char[2] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());
				aec.sendRequest(new RequestItemUse(iItem.CharItemID));
			}
			else
			{
				aec.sendRequest(new RequestItemUse(iItem.CharItemID));
			}
		}
	}

	private void SendItemTransferRequest(InventoryItem iItem)
	{
		if (!(iItem.Action is ItemActionTransfer itemActionTransfer))
		{
			return;
		}
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot travel during combat");
		}
		else if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (itemActionTransfer.MapID == areaData.id && itemActionTransfer.CellID == areaData.currentCellID)
		{
			Notification.ShowText("You are already in " + areaData.displayName);
		}
		else if (areaData.isDungeon)
		{
			Confirmation.Show("Warning", "You will lose any unsaved progress in this dungeon. Are you sure you want to leave this area?", delegate(bool conf)
			{
				if (conf)
				{
					aec.sendRequest(new RequestItemUse(iItem.CharItemID));
				}
			});
		}
		else
		{
			aec.sendRequest(new RequestItemUse(iItem.CharItemID));
		}
	}

	public void SendInfuseItemRequest(InventoryItem item)
	{
		aec.sendRequest(new RequestItemInfuse(item.CharItemID));
	}

	public void SendExtractItemRequest(InventoryItem item)
	{
		aec.sendRequest(new RequestItemExtract(item.CharItemID));
	}

	public void SendItemModiferRerollRequest(InventoryItem item)
	{
		aec.sendRequest(new RequestItemModifierReroll(item.CharItemID));
	}

	public void SendItemModifierRerollConfirmation()
	{
		aec.sendRequest(new RequestItemModifierConfirm());
	}

	public void SendLeaderboardRefreshRequest(int boardType)
	{
		aec.sendRequest(new RequestLeaderboard(boardType));
	}

	public void SendEquipBestRequest()
	{
		aec.sendRequest(new Request(10, 12));
	}

	public void SendEquipRequest(int CharItemID, InventoryItem.Equip equipId)
	{
		RequestItemEquip requestItemEquip = new RequestItemEquip();
		requestItemEquip.CharItemID = CharItemID;
		requestItemEquip.EquipID = equipId;
		aec.sendRequest(requestItemEquip);
	}

	public void ReportErrorToDB_Socket(string errorTitle, string errorMessage, string stacktrace, string context, string form)
	{
		RequestReportError requestReportError = ErrorReporting.Instance.GetRequestReportError("SOCKET REPORT: " + errorTitle, errorMessage, stacktrace, context, form);
		if (aec != null)
		{
			aec.sendRequest(requestReportError);
		}
	}

	private IEnumerator ReportAdvertizerInstallAttribution()
	{
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/SaveUserEvent?CharID=" + Session.MyPlayerData.ID + "&UserID=" + Session.MyPlayerData.UserID + "&Session=" + UnityWebRequest.EscapeURL(Session.Account.strToken));
		_ = "URL: " + www.url;
		yield return www.SendWebRequest();
		if (www.error != null)
		{
			Debug.LogError("ReportAdvertizerInstallAttribution error: " + www.error);
			yield break;
		}
		Debug.Log("ReportAdvertizerInstallAttribution success");
	}

	public void SendUnequipRequest(int CharItemID)
	{
		RequestItemUnequip requestItemUnequip = new RequestItemUnequip();
		requestItemUnequip.CharItemID = CharItemID;
		if (aec != null)
		{
			aec.sendRequest(requestItemUnequip);
		}
	}

	public void SendItemRemoveRequest(int CharItemID)
	{
		RequestItemRemove requestItemRemove = new RequestItemRemove();
		requestItemRemove.CharItemID = CharItemID;
		aec.sendRequest(requestItemRemove);
	}

	public void SendItemDustRequest(int CharItemID)
	{
		aec.sendRequest(new RequestDustLootBoxItem(CharItemID));
	}

	public void SendShopLoadRequest(int ShopID, ShopType shopType = ShopType.Shop)
	{
		aec.sendRequest(new RequestShopLoad(ShopID, shopType));
	}

	public void SendBuyRequest(int ShopID, int ItemID, int Qty, ShopType shopType = ShopType.Shop)
	{
		RequestTradeBuy requestTradeBuy = new RequestTradeBuy();
		requestTradeBuy.ShopID = ShopID;
		requestTradeBuy.ShopType = shopType;
		requestTradeBuy.ItemID = ItemID;
		requestTradeBuy.Qty = Qty;
		aec.sendRequest(requestTradeBuy);
	}

	public void SendSellRequest(int CharItemID, int qty = 1)
	{
		RequestTradeSell requestTradeSell = new RequestTradeSell();
		requestTradeSell.CharItemID = CharItemID;
		requestTradeSell.Qty = qty;
		aec.sendRequest(requestTradeSell);
	}

	public void SendQuestLoadRequest(List<int> questIDs)
	{
		if (questIDs != null && questIDs.Count >= 0)
		{
			RequestQuestLoad requestQuestLoad = new RequestQuestLoad();
			requestQuestLoad.QuestIDs = questIDs;
			aec.sendRequest(requestQuestLoad);
		}
	}

	public void SendHouseQuestAcceptRequest()
	{
		RequestHouseCommand r = new RequestHouseCommand(Com.CmdHousing.HouseQuest);
		aec.sendRequest(r);
	}

	public void SendQuestAcceptRequest(int questID)
	{
		RequestQuestAccept requestQuestAccept = new RequestQuestAccept();
		requestQuestAccept.QuestID = questID;
		aec.sendRequest(requestQuestAccept);
	}

	public void SendQuestAbandonRequest(int questID)
	{
		RequestQuestAbandon requestQuestAbandon = new RequestQuestAbandon();
		requestQuestAbandon.QuestID = questID;
		aec.sendRequest(requestQuestAbandon);
	}

	public void SendQuestCompleteRequest(int questID)
	{
		RequestQuestComplete requestQuestComplete = new RequestQuestComplete();
		requestQuestComplete.QuestID = questID;
		aec.sendRequest(requestQuestComplete);
	}

	public void SendQuestCompleteRequest(int questID, Queue<int> itemIDs)
	{
		RequestQuestComplete requestQuestComplete = new RequestQuestComplete();
		requestQuestComplete.QuestID = questID;
		requestQuestComplete.ChosenRewardIDs = itemIDs;
		aec.sendRequest(requestQuestComplete);
	}

	public void SendLootItemRequest(int lootBagID, int lootItemID)
	{
		aec.sendRequest(new RequestLootItem(lootBagID, lootItemID));
	}

	public void SendLootBagRequest(int lootBagID)
	{
		aec.sendRequest(new RequestLootBag(lootBagID));
	}

	public void SendLootAllItemsRequest()
	{
		aec.sendRequest(new Request(16, 5));
	}

	public void SendClassEquipRequest(int id)
	{
		RequestClassEquip requestClassEquip = new RequestClassEquip();
		requestClassEquip.ClassID = id;
		aec.sendRequest(requestClassEquip);
	}

	public void SendClassAddRequest(int id)
	{
		RequestClassAdd requestClassAdd = new RequestClassAdd();
		requestClassAdd.ClassID = id;
		aec.sendRequest(requestClassAdd);
	}

	public void SendCrossSkillEquipRequest(int spellID)
	{
		aec.sendRequest(new RequestCrossSkillEquip(spellID));
	}

	public void SendPvpActionEquipRequest(Dictionary<int, int> pvpActionIDs)
	{
		if (pvpActionIDs != null)
		{
			aec.sendRequest(new RequestPvpActionEquip(pvpActionIDs));
		}
	}

	public void SendEmoteRequest(StateEmote em)
	{
		aec.sendRequest(new RequestEmote(em));
	}

	public void SendSheatheRequest()
	{
	}

	public void SendUnsheatheRequest()
	{
	}

	public void SendRespawnRequest()
	{
		aec.sendRequest(new RequestPlayerRespawn());
	}

	public void SendPvPMatchLeaveRequest()
	{
		aec.sendRequest(new RequestPvpMatchLeave());
	}

	public void SendBitFlagUpdateRequest(string name, byte index, bool value)
	{
		if (!string.IsNullOrEmpty(name) && index != 0 && Session.MyPlayerData.CheckBitFlag(name, index) != value)
		{
			aec.sendRequest(new RequestBitFlagUpdate(name, index, value));
		}
	}

	public void SendMapsListRequest()
	{
		RequestAreaList r = new RequestAreaList();
		aec.sendRequest(r);
	}

	public void SendPartyGotoRequest(int charID)
	{
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot travel during combat");
		}
		else if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (entities.me.serverState == Entity.State.Interacting)
		{
			Notification.ShowText("Cannot travel during interactions");
		}
		else
		{
			aec.sendRequest(new RequestPartyGoto(charID));
		}
	}

	public void SendGotoRequest(int charID, string code)
	{
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot travel during combat");
		}
		else if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (entities.me.serverState == Entity.State.Interacting)
		{
			Notification.ShowText("Cannot travel during interactions");
		}
		else
		{
			aec.sendRequest(new RequestGoto(charID, code));
		}
	}

	public void SendPvPDuelChallengeRequest(string challengeeName)
	{
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Chat.AddMessage("Cannot start a duel during combat");
			return;
		}
		if (entities.me.serverState == Entity.State.Dead)
		{
			Chat.AddMessage("Cannot start a duel while dead");
			return;
		}
		Chat.AddMessage(InterfaceColors.Chat.Yellow.ToBBCode() + $"You have challenged {challengeeName} to a duel.");
		aec.sendRequest(new RequestPvPDuelChallenge(challengeeName));
	}

	public void SendPvPDuelAcceptRequest(int challengerId, bool accept)
	{
		if (accept && entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot start a duel during combat");
		}
		else if (accept && entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot start a duel while dead");
		}
		else
		{
			aec.sendRequest(new RequestPvPDuelAccept(challengerId, accept));
		}
	}

	public void SendDuelForfeitRequest()
	{
		if (Entities.Instance.me.DuelOpponentID != -1)
		{
			aec.sendRequest(new RequestDuelForfeit());
		}
	}

	public void SendAreaJoinCommand(string areaname)
	{
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot travel during combat");
		}
		else if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (areaData.isDungeon)
		{
			Confirmation.Show("Warning", "You will lose any unsaved progress in this dungeon. Are you sure you want to leave this area?", delegate(bool conf)
			{
				if (conf)
				{
					RequestCmd r = new RequestCmd
					{
						cmd = 2,
						args = { areaname }
					};
					aec.sendRequest(r);
				}
			});
		}
		else
		{
			RequestCmd requestCmd = new RequestCmd();
			requestCmd.cmd = 2;
			requestCmd.args.Add(areaname);
			aec.sendRequest(requestCmd);
		}
	}

	public void SendVoteKickCommand(string player)
	{
		RequestCmd requestCmd = new RequestCmd();
		requestCmd.cmd = 3;
		requestCmd.args.Add(player);
		aec.sendRequest(requestCmd);
	}

	public void SendItemDailyRewardRequest()
	{
		aec.sendRequest(new RequestItemDailyReward());
	}

	public void SendAreaJoinRequest(int id, bool isPrivate = false, bool enterScaled = false)
	{
		if (entities.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot travel during combat");
		}
		else if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (areaData.isDungeon)
		{
			if (!isPrivate && PartyManager.IsInParty)
			{
				Confirmation.Show("Public Dungeon.", "Entering a public dungeon instance will remove you from the current party. Would you like to continue?", delegate(bool b)
				{
					if (b)
					{
						aec.sendRequest(new RequestAreaJoin(id, isPrivate, enterScaled));
					}
				});
				return;
			}
			Confirmation.Show("Warning", "You will lose any unsaved progress in this dungeon. Are you sure you want to leave this area?", delegate(bool conf)
			{
				if (conf)
				{
					aec.sendRequest(new RequestAreaJoin(id, isPrivate, enterScaled));
				}
			});
		}
		else
		{
			aec.sendRequest(new RequestAreaJoin(id, isPrivate, enterScaled));
		}
	}

	public void SendTransferMapRequest(int mapID, int cellID, int spawnID, bool showConfirmation)
	{
		if (entities.me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot travel while dead");
		}
		else if (areaData.isDungeon && mapID != instance.areaData.id)
		{
			Confirmation.Show("Warning", "You will lose any unsaved progress in this dungeon. Are you sure you want to leave this area?", delegate(bool conf)
			{
				if (conf)
				{
					aec.sendRequest(new RequestEntityTransferMap(mapID, cellID, spawnID));
				}
			});
		}
		else if (showConfirmation)
		{
			Confirmation.Show("Travel", "Are you sure you want to leave this area?", delegate(bool conf)
			{
				if (conf)
				{
					aec.sendRequest(new RequestEntityTransferMap(mapID, cellID, spawnID));
				}
			});
		}
		else
		{
			aec.sendRequest(new RequestEntityTransferMap(mapID, cellID, spawnID));
		}
	}

	public void SendExitInstanceRequest()
	{
		if (areaData.IsInstance)
		{
			aec.sendRequest(new Request(7, 6));
		}
	}

	public void SendIATransferPadRequest(string id, int areaID, int cellID, int spawnID)
	{
		RequestIATransferPad requestIATransferPad = new RequestIATransferPad();
		requestIATransferPad.UniqueID = id;
		requestIATransferPad.AreaID = areaID;
		requestIATransferPad.CellID = cellID;
		requestIATransferPad.SpawnID = spawnID;
		aec.sendRequest(requestIATransferPad);
	}

	public void SendRequestEffectRemove(int effectID)
	{
		aec.sendRequest(new RequestEffectRemove(effectID));
	}

	public void SendEntityKillRequest()
	{
		aec.sendRequest(new Request(17, 13));
	}

	public void SendEntityPortraitUpdateRequest(int portrait)
	{
		aec.sendRequest(new RequestEntityPortraitUpdate(portrait));
	}

	public void SendEntityTitleUpdateRequest(int title)
	{
		aec.sendRequest(new RequestEntityTitleUpdate(title));
	}

	public void SendEntityLoadBadgesRequest()
	{
		aec.sendRequest(new RequestEntityLoadBadges());
	}

	internal void SendMergeShopsRequest(int msid)
	{
		RequestMergeShopLoad requestMergeShopLoad = new RequestMergeShopLoad();
		requestMergeShopLoad.MergeShopID = msid;
		aec.sendRequest(requestMergeShopLoad);
	}

	internal void SendMergeRequest(int msid, int mid)
	{
		RequestMerge requestMerge = new RequestMerge();
		requestMerge.ShopID = msid;
		requestMerge.MergeID = mid;
		aec.sendRequest(requestMerge);
	}

	public void SendMergeClaimRequest(int mergeID)
	{
		RequestMergeClaim requestMergeClaim = new RequestMergeClaim();
		requestMergeClaim.MergeID = mergeID;
		aec.sendRequest(requestMergeClaim);
	}

	public void SendMergeBuyOutRequest(int shopID, int mergeID)
	{
		RequestMergeBuyOut requestMergeBuyOut = new RequestMergeBuyOut();
		requestMergeBuyOut.ShopID = shopID;
		requestMergeBuyOut.MergeID = mergeID;
		aec.sendRequest(requestMergeBuyOut);
	}

	public void SendMergeSpeedupRequest(int mergeID)
	{
		RequestMergeSpeedup requestMergeSpeedup = new RequestMergeSpeedup();
		requestMergeSpeedup.MergeID = mergeID;
		aec.sendRequest(requestMergeSpeedup);
	}

	public void SendFriendListRequest()
	{
		RequestFriendList requestFriendList = new RequestFriendList();
		requestFriendList.CharID = Session.MyPlayerData.ID;
		aec.sendRequest(requestFriendList);
	}

	public void SendFriendRequest(int id)
	{
		if (Session.MyPlayerData.CanAddMoreFriends)
		{
			RequestFriend requestFriend = new RequestFriend();
			requestFriend.CharID = Session.MyPlayerData.ID;
			requestFriend.FriendID = id;
			aec.sendRequest(requestFriend);
		}
	}

	public void SendFriendRequest(string name)
	{
		if (Session.MyPlayerData.CanAddMoreFriends)
		{
			RequestFriend requestFriend = new RequestFriend();
			requestFriend.CharID = Session.MyPlayerData.ID;
			requestFriend.FriendID = -1;
			requestFriend.Name = name;
			aec.sendRequest(requestFriend);
		}
	}

	public void SendPartyInviteRequest(string name)
	{
		RequestPartyInvite requestPartyInvite = new RequestPartyInvite();
		requestPartyInvite.FromID = Session.MyPlayerData.ID;
		requestPartyInvite.To = name;
		aec.sendRequest(requestPartyInvite);
	}

	public void SendGuildInviteRequest(string name)
	{
		RequestGuildInvite r = new RequestGuildInvite(Session.MyPlayerData.ID, name);
		aec.sendRequest(r);
	}

	public void SendPartyJoinRequest(int leaderid, bool accept = true)
	{
		RequestPartyJoin requestPartyJoin = new RequestPartyJoin();
		requestPartyJoin.LeaderID = leaderid;
		requestPartyJoin.Accept = accept;
		aec.sendRequest(requestPartyJoin);
	}

	public void SendPartyRemoveRequest(int charID)
	{
		RequestPartyRemove requestPartyRemove = new RequestPartyRemove();
		requestPartyRemove.CharID = charID;
		aec.sendRequest(requestPartyRemove);
	}

	public void SendPartyPromoteRequest(int charID)
	{
		RequestPartyPromote requestPartyPromote = new RequestPartyPromote();
		requestPartyPromote.CharID = charID;
		aec.sendRequest(requestPartyPromote);
	}

	public void SendNPCDialogueStartRequest(int spawnID, bool objective)
	{
		RequestNPCDialogueStart r = new RequestNPCDialogueStart(spawnID, objective);
		aec.sendRequest(r);
	}

	public void SendNPCDialogueEndRequest(int spawnID)
	{
		RequestNPCDialogueEnd r = new RequestNPCDialogueEnd(spawnID);
		aec.sendRequest(r);
	}

	private void SyncIgnore()
	{
		RequestFriendSyncIgnore requestFriendSyncIgnore = new RequestFriendSyncIgnore();
		requestFriendSyncIgnore.CharID = Session.MyPlayerData.ID;
		requestFriendSyncIgnore.ignoreList = SettingsManager.ignoreList;
		requestFriendSyncIgnore.isFriendRequestEnabled = SettingsManager.CanGetFriendRequests;
		requestFriendSyncIgnore.isWhisperEnabled = SettingsManager.CanGetWhispers;
		requestFriendSyncIgnore.isPartyInviteEnabled = SettingsManager.CanGetPartyInvites;
		requestFriendSyncIgnore.isGuildInviteEnabled = SettingsManager.CanGetGuildInvites;
		requestFriendSyncIgnore.isGotoEnabled = SettingsManager.IsGotoOn;
		aec.sendRequest(requestFriendSyncIgnore);
	}

	public void SendAdWatchRewardRequest()
	{
		aec.sendRequest(new RequestAdWatch(AdManager.Instance.adRequestTokenGUID));
	}

	public void SendCreateGuildRequest(string guildName, string tag)
	{
		if (Session.MyPlayerData.Guild == null)
		{
			aec.sendRequest(new RequestJoinGuild(guildName, tag));
		}
		else
		{
			Chat.AddMessage("You are already in a guild.");
		}
	}

	public void SendJoinGuildRequest(int charID, int guildID, byte role = 0, bool accepted = true)
	{
		if (accepted)
		{
			if (Session.MyPlayerData.Guild == null)
			{
				aec.sendRequest(new RequestJoinGuild(charID, guildID, role));
			}
			else
			{
				Chat.AddMessage("You are already in a guild.");
			}
		}
	}

	public void SendLeaveGuildRequest()
	{
		if (Session.MyPlayerData.Guild != null)
		{
			aec.sendRequest(new RequestLeaveGuild(Session.MyPlayerData.ID, Session.MyPlayerData.Guild.guildID));
		}
		else
		{
			Chat.AddMessage("You are not in a guild.");
		}
	}

	public void SendKickGuildMemberRequest(int memberID)
	{
		if (Session.MyPlayerData.Guild != null)
		{
			aec.sendRequest(new RequestLeaveGuild(memberID, Session.MyPlayerData.ID, Session.MyPlayerData.Guild.guildID));
		}
		else
		{
			Chat.AddMessage("You are not in a guild.");
		}
	}

	public void SendDailyTaskUpdateRequest(int taskID)
	{
		aec.sendRequest(new ResponseDailyTaskCompleteButton(taskID));
	}

	public void SendGetChatCommandsRequest()
	{
		aec.sendRequest(new RequestGetChatCommands());
	}

	private void HandleResponsePlayer(Response response)
	{
		switch ((Com.CmdPlayer)response.cmd)
		{
		case Com.CmdPlayer.ProductOfferSet:
		{
			ResponseProductOfferSet responseProductOfferSet = (ResponseProductOfferSet)response;
			Session.MyPlayerData.SetStarterPack(responseProductOfferSet.ProductID, responseProductOfferSet.ExpireUTC);
			break;
		}
		case Com.CmdPlayer.TimedChoice:
			if (!(response is ResponseTimedChoice responseTimedChoice))
			{
				break;
			}
			if (uigame != null)
			{
				if (responseTimedChoice.choiceType == ResponseTimedChoice.ChoiceType.QueueReadyCheck)
				{
					AudioManager.Play2DSFX("SFX_PVP_QueueReady");
				}
				uigame.timePanel.Init(responseTimedChoice.title, responseTimedChoice.description, responseTimedChoice.time);
			}
			else
			{
				Instance.DelayedUIResponses.Add(response);
			}
			break;
		case Com.CmdPlayer.TimedChoiceCancel:
			uigame.timePanel.TurnOff();
			break;
		case Com.CmdPlayer.QueueStatus:
			if (uigame != null)
			{
				ResponseQueueStatus responseQueueStatus = (ResponseQueueStatus)response;
				if (responseQueueStatus.hasLeft)
				{
					Chat.Notify("You have left the queue", InterfaceColors.Chat.Red.ToBBCode(), Chat.FilterType.ServerMessage);
				}
				uigame.queueNotification.UpdateQueueInfo(responseQueueStatus.showNotification, responseQueueStatus.currentPlayers, responseQueueStatus.maxPlayers, responseQueueStatus.delayPenalty);
				Session.MyPlayerData.queueCount = responseQueueStatus.currentPlayers;
				Session.MyPlayerData.maxQueueCount = responseQueueStatus.maxPlayers;
				Session.MyPlayerData.isPvp = responseQueueStatus.isPvp;
				Session.MyPlayerData.hasLeft = responseQueueStatus.hasLeft;
				Session.MyPlayerData.showNotification = responseQueueStatus.showNotification;
				Session.MyPlayerData.queueDelayPenalty = responseQueueStatus.delayPenalty;
				if (UIPvPMainMenu.instance != null)
				{
					UIPvPMainMenu.instance.UpdateQueueDisplay();
				}
			}
			else
			{
				Instance.AddDelayedResponse(response);
			}
			break;
		case Com.CmdPlayer.PvPRecords:
		{
			ResponseUpdatePvPRecords responseUpdatePvPRecords = (ResponseUpdatePvPRecords)response;
			Session.MyPlayerData.UpdatePvpRecords(responseUpdatePvPRecords.records);
			break;
		}
		case Com.CmdPlayer.JoinQueue:
		case Com.CmdPlayer.LeaveQueue:
		case Com.CmdPlayer.PvPStats:
			break;
		}
	}

	private void HandleResponsePvP(Response response)
	{
		switch ((Com.CmdPvP)response.cmd)
		{
		case Com.CmdPvP.DuelChallenge:
		{
			ResponsePvPDuelChallenge responsePvPDuelChallenge = response as ResponsePvPDuelChallenge;
			if (!SettingsManager.CanGetPvPDuelRequests)
			{
				SendPvPDuelAcceptRequest(responsePvPDuelChallenge.ChallengerId, accept: false);
				break;
			}
			Player challenger = Entities.Instance.GetPlayerById(responsePvPDuelChallenge.ChallengerId);
			if (challenger != null)
			{
				Confirmation.Show("Duel Request", challenger.name + " has challenged you to a duel! Do you accept their challenge?", delegate(bool accept)
				{
					SendPvPDuelAcceptRequest(challenger.ID, accept);
				});
			}
			break;
		}
		case Com.CmdPvP.DuelComplete:
		{
			ResponsePvPDuelComplete responsePvPDuelComplete = response as ResponsePvPDuelComplete;
			Session.MyPlayerData.duelResult = new DuelResult(responsePvPDuelComplete.IsWinner);
			if (responsePvPDuelComplete.endImmediately)
			{
				Session.MyPlayerData.duelResult?.Show();
			}
			break;
		}
		case Com.CmdPvP.DuelCountdown:
			DuelStart.Show();
			break;
		case Com.CmdPvP.DuelStart:
		{
			ResponsePvPDuelStart responsePvPDuelStart = response as ResponsePvPDuelStart;
			int num = ((Entities.Instance.me.ID == responsePvPDuelStart.ChallengerId) ? responsePvPDuelStart.ChallengeeId : responsePvPDuelStart.ChallengerId);
			Player me = Entities.Instance.me;
			me.DuelOpponentID = num;
			me.CheckPvpState();
			Player playerById = Entities.Instance.GetPlayerById(num);
			if (playerById != null)
			{
				playerById.DuelOpponentID = me.ID;
				playerById.RefreshReaction();
			}
			break;
		}
		case Com.CmdPvP.PvPToggle:
		{
			ResponsePvPToggle responsePvPToggle = (ResponsePvPToggle)response;
			Entities.Instance.GetPlayerById(responsePvPToggle.ID).isPvPFlagged = responsePvPToggle.isPvPFlagged;
			break;
		}
		case Com.CmdPvP.PvPScore:
		{
			ResponsePvPScoreUpdate responsePvPScoreUpdate = (ResponsePvPScoreUpdate)response;
			_ = Entities.Instance.me;
			if (UIPvPScore.instance != null)
			{
				UIPvPScore.instance.updateScore(responsePvPScoreUpdate.values);
			}
			break;
		}
		case Com.CmdPvP.CapturePoint:
		{
			ResponseUpdateCapturePointStatus responseUpdateCapturePointStatus = (ResponseUpdateCapturePointStatus)response;
			_ = Entities.Instance.me;
			if (UIPvPScore.instance != null)
			{
				UIPvPScore.instance.updateCaptureUI(responseUpdateCapturePointStatus.values);
			}
			break;
		}
		case Com.CmdPvP.CapturePointTick:
		{
			ResponsePvPCaptureBarUpdate responsePvPCaptureBarUpdate = (ResponsePvPCaptureBarUpdate)response;
			_ = Entities.Instance.me;
			if (UIPvPScore.instance != null)
			{
				UIPvPScore.instance.updateCaptureProgressBar(responsePvPCaptureBarUpdate.capturePointID, responsePvPCaptureBarUpdate.capturePointVals);
			}
			break;
		}
		case Com.CmdPvP.TimerStart:
		{
			ResponsePvPTimerStart responsePvPTimerStart = (ResponsePvPTimerStart)response;
			if (UIPvPScore.instance != null)
			{
				UIPvPScore.instance.startTimer(responsePvPTimerStart.timeRemaining);
			}
			break;
		}
		case Com.CmdPvP.MatchStart:
			AudioManager.Play2DSFX("SFX_PVP_MatchStart_Count_0");
			break;
		case Com.CmdPvP.MatchEnd:
		{
			ResponsePvpMatchEnd responsePvpMatchEnd = response as ResponsePvpMatchEnd;
			UIPvpMatchScreen.Instance.Init(responsePvpMatchEnd.matchInfo, responsePvpMatchEnd.rewardInfo);
			AudioManager.Play2DSFX("SFX_PVP_MatchEnd");
			break;
		}
		case Com.CmdPvP.SoundTrackCountdown:
		{
			ResponsePvpMatchSoundTrackCountdown responsePvpMatchSoundTrackCountdown = response as ResponsePvpMatchSoundTrackCountdown;
			StartCoroutine(SoundTracks.Instance.Play(responsePvpMatchSoundTrackCountdown.soundTrackID, showLoader: false));
			AreaData.SoundTrackID = responsePvpMatchSoundTrackCountdown.soundTrackID;
			AudioManager.Play2DSFX("SFX_PVP_Count_54321");
			break;
		}
		case Com.CmdPvP.DuelAccept:
		case Com.CmdPvP.DuelForfeit:
		case Com.CmdPvP.MatchLeave:
			break;
		}
	}

	private void LoadQuests(Dictionary<int, Quest> quests)
	{
		foreach (KeyValuePair<int, Quest> quest in quests)
		{
			Quests.Add(quest.Key, quest.Value);
			if (quest.Value == null)
			{
				continue;
			}
			foreach (QuestRewardItem reward in quest.Value.Rewards)
			{
				Items.Add(reward);
			}
		}
		if (this.QuestLoaded != null)
		{
			this.QuestLoaded();
		}
		Session.MyPlayerData.UpdateTrackedQuest();
		UpdateAreaQuest();
	}

	private void HandleResponseQuest(Response response)
	{
		switch ((Com.CmdQuest)response.cmd)
		{
		case Com.CmdQuest.Abandon:
		{
			ResponseQuestAbandon responseQuestAbandon = response as ResponseQuestAbandon;
			Session.MyPlayerData.RemoveQuest(responseQuestAbandon.QuestID);
			UpdateAreaQuest();
			break;
		}
		case Com.CmdQuest.Accept:
		{
			Quest quest2 = (response as ResponseQuestAccept).Quest;
			if (quest2 == null)
			{
				break;
			}
			Quests.Add(quest2.ID, quest2);
			foreach (QuestRewardItem reward in quest2.Rewards)
			{
				Items.Add(reward);
			}
			Session.MyPlayerData.AddQuest(quest2.ID);
			Session.MyPlayerData.TrackQuest(quest2.ID);
			UpdateAreaQuest();
			break;
		}
		case Com.CmdQuest.Complete:
		{
			ResponseQuestComplete responseQuestComplete = response as ResponseQuestComplete;
			Session.MyPlayerData.TurnInQuest(responseQuestComplete.QuestID, responseQuestComplete.QSInc);
			if (!responseQuestComplete.HasNextQuest)
			{
				UpdateAreaQuest();
			}
			break;
		}
		case Com.CmdQuest.Load:
		{
			ResponseQuestLoad responseQuestLoad = response as ResponseQuestLoad;
			LoadQuests(responseQuestLoad.Quests);
			break;
		}
		case Com.CmdQuest.Progress:
		{
			ResponseQuestProgress responseQuestProgress = response as ResponseQuestProgress;
			Session.MyPlayerData.UpdateQuestObjective(responseQuestProgress.QOID, responseQuestProgress.Qty);
			QuestObjective objective = Quests.GetObjective(responseQuestProgress.QOID);
			if (objective == null)
			{
				break;
			}
			Quest quest = Quests.Get(objective.QuestID);
			if (quest != null)
			{
				if (quest.TurnInType == QuestTurnInType.Auto && (objective.Type != QuestObjectiveType.Talk || quest.EndDialogID == 0) && Session.MyPlayerData.IsQuestComplete(objective.QuestID) && (quest.Rewards == null || quest.Rewards.Count == 0))
				{
					SendQuestCompleteRequest(quest.ID);
				}
				if (objective.Type == QuestObjectiveType.Talk)
				{
					Entities.Instance.GetNpcBySpawnId(objective.RefID)?.NPCInteract();
				}
			}
			break;
		}
		case Com.CmdQuest.Meta:
		{
			ResponseQuestMeta responseQuestMeta = response as ResponseQuestMeta;
			Session.MyPlayerData.QuestStrings = responseQuestMeta.QuestStrings;
			Session.MyPlayerData.QuestChains = responseQuestMeta.QuestChains;
			Session.MyPlayerData.CurQuestObjectives = responseQuestMeta.QuestObjectives;
			break;
		}
		}
	}

	private void HandleResponseResource(Response response)
	{
		switch ((Com.CmdResource)response.cmd)
		{
		case Com.CmdResource.BobberDespawn:
		{
			ResponseFishingBobberDespawn responseFishingBobberDespawn = response as ResponseFishingBobberDespawn;
			if (BaseMachine.Map != null && BaseMachine.Map.ContainsKey(responseFishingBobberDespawn.machineID))
			{
				ResourceMachine resourceMachine3 = BaseMachine.Map[responseFishingBobberDespawn.machineID] as ResourceMachine;
				if (resourceMachine3 != null)
				{
					resourceMachine3.DespawnBobber(responseFishingBobberDespawn.playerID, immediate: false);
				}
			}
			break;
		}
		case Com.CmdResource.BobberSpawn:
		{
			ResponseFishingBobberSpawn responseFishingBobberSpawn = response as ResponseFishingBobberSpawn;
			if (BaseMachine.Map != null && BaseMachine.Map.ContainsKey(responseFishingBobberSpawn.machineID))
			{
				ResourceMachine resourceMachine4 = BaseMachine.Map[responseFishingBobberSpawn.machineID] as ResourceMachine;
				if (resourceMachine4 != null)
				{
					resourceMachine4.SpawnBobber(responseFishingBobberSpawn.playerID);
				}
			}
			break;
		}
		case Com.CmdResource.FishCatch:
		{
			ResponseFishingFishCatch responseFishingFishCatch = response as ResponseFishingFishCatch;
			if (BaseMachine.Map != null && BaseMachine.Map.ContainsKey(responseFishingFishCatch.machineID))
			{
				ResourceMachine resourceMachine2 = BaseMachine.Map[responseFishingFishCatch.machineID] as ResourceMachine;
				if (resourceMachine2 != null)
				{
					resourceMachine2.CatchFish(responseFishingFishCatch.playerID);
				}
			}
			break;
		}
		case Com.CmdResource.FishHook:
		{
			ResponseFishingFishHook responseFishingFishHook = response as ResponseFishingFishHook;
			if (BaseMachine.Map != null && BaseMachine.Map.ContainsKey(responseFishingFishHook.machineID))
			{
				ResourceMachine resourceMachine5 = BaseMachine.Map[responseFishingFishHook.machineID] as ResourceMachine;
				if (resourceMachine5 != null)
				{
					resourceMachine5.HookFish(responseFishingFishHook.playerID, responseFishingFishHook.rarity);
				}
			}
			break;
		}
		case Com.CmdResource.FishRelease:
		{
			ResponseFishingFishRelease responseFishingFishRelease = response as ResponseFishingFishRelease;
			if (BaseMachine.Map != null && BaseMachine.Map.ContainsKey(responseFishingFishRelease.machineID))
			{
				ResourceMachine resourceMachine = BaseMachine.Map[responseFishingFishRelease.machineID] as ResourceMachine;
				if (resourceMachine != null)
				{
					resourceMachine.ReleaseFish(responseFishingFishRelease.playerID, immediate: false);
				}
			}
			break;
		}
		}
	}

	private void HandleResponseSystemPerformance(Response response)
	{
		Com.CmdSystemPerformance cmd = (Com.CmdSystemPerformance)response.cmd;
		if (cmd != Com.CmdSystemPerformance.Ping)
		{
			_ = 2;
			return;
		}
		ResponsePing responsePing = (ResponsePing)response;
		ping.OnPingResponse(responsePing.ID);
	}

	private void HandleResponseTrade(Response response)
	{
		switch ((Com.CmdTrade)response.cmd)
		{
		case Com.CmdTrade.Buy:
			Notification.ShowText("Purchase Successful!");
			break;
		case Com.CmdTrade.Sell:
		{
			ResponseTradeSell responseTradeSell = (ResponseTradeSell)response;
			Session.MyPlayerData.SellItem(responseTradeSell.CharItemID, responseTradeSell.Quantity);
			Session.MyPlayerData.AddGold(responseTradeSell.Amount);
			break;
		}
		case Com.CmdTrade.ShopLoad:
		{
			Shop shop = ((ResponseShopLoad)response).shop;
			string message = ((ResponseShopLoad)response).Message;
			if (shop != null)
			{
				if (shop.Items == null)
				{
					shop.Items = new List<ShopItem>();
				}
				foreach (ShopItem item in shop.Items)
				{
					Items.Add(item);
				}
				if (shop.Type == ShopType.Shop)
				{
					shop.Items = shop.Items.OrderBy((ShopItem p) => p.SortOrder).ToList();
				}
				else
				{
					shop.Items = shop.Items.OrderByDescending((ShopItem p) => p.Rarity).ToList();
				}
				Shops.Add(shop);
				if (shop.ID == 569)
				{
					UIGuild.instance?.LoadMemberShop();
				}
			}
			if (this.ShopLoaded != null)
			{
				this.ShopLoaded(shop, message);
			}
			break;
		}
		}
	}

	private void HandleResponseGuild(Response response)
	{
		if (response.cmd == 2)
		{
			ResponseGuildInfo responseGuildInfo = (ResponseGuildInfo)response;
			Session.MyPlayerData.UpdateGuildInfo(responseGuildInfo.Guild);
		}
		if (response.cmd == 16)
		{
			Guild.GuildLeaderboardEntries = ((ResponseGuildLeaderboardEntries)response).LeaderboardEntries;
			Session.MyPlayerData.UpdateGuildInfo(Session.MyPlayerData.Guild);
		}
		if (response.cmd == 15)
		{
			ResponseGuildGoldXpInfo responseGuildGoldXpInfo = (ResponseGuildGoldXpInfo)response;
			if (Session.MyPlayerData.Guild != null)
			{
				Session.MyPlayerData.Guild.TotalXP = responseGuildGoldXpInfo.TotalXP;
				Session.MyPlayerData.Guild.MonthlyXP = responseGuildGoldXpInfo.MonthlyXP;
				Session.MyPlayerData.Guild.Gold = responseGuildGoldXpInfo.Gold;
				Session.MyPlayerData.UpdateGuildInfo(Session.MyPlayerData.Guild);
			}
		}
		else if (response.cmd == 4)
		{
			ResponseGuildInvite rgi = (ResponseGuildInvite)response;
			Chat.Notify(rgi.GetMessage());
			Session.MyPlayerData.guildInvite = rgi;
			AudioManager.Play2DSFX("Notif_Social");
			Confirmation.Show("Guild Invitation", rgi.GetMessage(), delegate(bool b)
			{
				if (!b)
				{
					rgi = null;
				}
				else
				{
					SendJoinGuildRequest(Session.MyPlayerData.ID, rgi.guildID, 0, b);
				}
			}, isClosable: false, enableCollider: false);
		}
		else if (response.cmd == 5)
		{
			ResponseEntityGuildUpdate responseEntityGuildUpdate = response as ResponseEntityGuildUpdate;
			(Entities.Instance.GetEntity(Entity.Type.Player, responseEntityGuildUpdate.entityID) as Player)?.UpdateGuildIDNameTag(responseEntityGuildUpdate.guildID, responseEntityGuildUpdate.guildTag, responseEntityGuildUpdate.guildName);
		}
		else if (response.cmd == 6)
		{
			ResponseGuildChangeRole responseGuildChangeRole = response as ResponseGuildChangeRole;
			Session.MyPlayerData.Guild.UpdateMemberRole(responseGuildChangeRole.memberID, (GuildRole)responseGuildChangeRole.newRole);
			Session.MyPlayerData.UpdateGuildInfo();
		}
		else if (response.cmd == 3)
		{
			ResponseLeaveGuild responseLeaveGuild = response as ResponseLeaveGuild;
			Session.MyPlayerData.Guild.RemoveGuildMember(responseLeaveGuild.memberID);
			Session.MyPlayerData.UpdateGuildInfo(leaveGuild: true);
		}
		else if (response.cmd == 7)
		{
			ResponseGuildMemberStatus responseGuildMemberStatus = response as ResponseGuildMemberStatus;
			if (responseGuildMemberStatus.isNewPlayer)
			{
				Session.MyPlayerData.Guild.guildMembers.Add(responseGuildMemberStatus.ID, responseGuildMemberStatus.guildMember);
			}
			else if (Session.MyPlayerData.Guild.guildMembers.ContainsKey(responseGuildMemberStatus.ID))
			{
				Session.MyPlayerData.Guild.guildMembers[responseGuildMemberStatus.ID] = responseGuildMemberStatus.guildMember;
			}
			Session.MyPlayerData.UpdateGuildInfo();
		}
		else if (response.cmd == 8)
		{
			ResponseUpdateGuildName responseUpdateGuildName = response as ResponseUpdateGuildName;
			Session.MyPlayerData.ChangeGuildNameTag(responseUpdateGuildName.newName);
		}
		else if (response.cmd == 14)
		{
			ResponseUpdateGuildTax responseUpdateGuildTax = response as ResponseUpdateGuildTax;
			Session.MyPlayerData.ChangeGuildTax(responseUpdateGuildTax.newTax);
		}
		else if (response.cmd == 9)
		{
			ResponseUpdateGuildTag responseUpdateGuildTag = response as ResponseUpdateGuildTag;
			Session.MyPlayerData.Guild.tag = responseUpdateGuildTag.newTag;
			Session.MyPlayerData.ChangeGuildNameTag(null, responseUpdateGuildTag.newTag);
		}
		else if (response.cmd == 10)
		{
			ResponseMOTDUpdate responseMOTDUpdate = response as ResponseMOTDUpdate;
			Session.MyPlayerData.Guild.MOTD = responseMOTDUpdate.motd;
			Session.MyPlayerData.MOTDUpdated(responseMOTDUpdate.motd);
			if (Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader)
			{
				MessageBox.Show("MOTD Updated", "The MOTD has been updated!");
			}
		}
	}

	public void SendGuildUpdateRequest()
	{
		RequestUpdateGuild r = new RequestUpdateGuild(Session.MyPlayerData.Guild.guildID);
		aec.sendRequest(r);
	}

	public void HandleResponseDailyTask(Response response)
	{
		if (response.cmd == 1)
		{
			ResponseDailyTaskInfo responseDailyTaskInfo = (ResponseDailyTaskInfo)response;
			Session.MyPlayerData.UpdateDailyTaskInfo(responseDailyTaskInfo.charDaily);
		}
	}

	public void HandleResponseServerDailyTask(Response response)
	{
		if (response.cmd == 1)
		{
			ResponseServerDailyTaskInitialize responseServerDailyTaskInitialize = (ResponseServerDailyTaskInitialize)response;
			Session.MyPlayerData.UpdateServerDailyTaskInitialization(responseServerDailyTaskInitialize.dailyTasks, responseServerDailyTaskInitialize.charDailyTasks);
		}
	}

	public void HandleResponseLeaderBoard(Response response)
	{
		if (response.cmd == 1)
		{
			ResponseLeaderboard responseLeaderboard = (ResponseLeaderboard)response;
			if (Session.MyPlayerData.leaderboards.Count < 1)
			{
				Session.MyPlayerData.leaderboards.Add(responseLeaderboard.leaderboard);
			}
			else
			{
				Session.MyPlayerData.leaderboards[0] = responseLeaderboard.leaderboard;
			}
			UILeaderboard.Instance.updatePlayerScore(responseLeaderboard.myPos, responseLeaderboard.myScore);
			UILeaderboard.Instance.Refresh(Session.MyPlayerData.leaderboards[0]);
		}
		if (response.cmd == 2)
		{
			ResponsePlayerLeaderboardScore responsePlayerLeaderboardScore = (ResponsePlayerLeaderboardScore)response;
			UILeaderboard.Instance.updatePlayerScore(responsePlayerLeaderboardScore.leaderboardPos, responsePlayerLeaderboardScore.score);
		}
	}

	public void HandleResponseDailyQuestReset(Response response)
	{
		Session.MyPlayerData.BitFields["id0"] = 0L;
		Session.MyPlayerData.BitFields["id1"] = 0L;
		Session.MyPlayerData.BitFields["id2"] = 0L;
	}

	public void HandleResponseSheathing(Response response)
	{
		ResponseSheathing responseSheathing = (ResponseSheathing)response;
		Player playerById = Entities.Instance.GetPlayerById(responseSheathing.playerID);
		Sheathing.Instance.Sheathe(playerById, responseSheathing.isSheathed);
	}

	public void SendGuildChangeRoleRequest(int requesterID, int guildMemberID, int guildID, byte guildRole, bool swap = false)
	{
		RequestGuildChangeRole r = new RequestGuildChangeRole(requesterID, guildMemberID, guildID, (GuildRole)guildRole, swap);
		aec.sendRequest(r);
	}

	public void SendGuildChangeNameRequest(string newName, int guildID)
	{
		if (Session.MyPlayerData.Guild != null)
		{
			aec.sendRequest(new RequestUpdateGuildName(newName, guildID));
		}
	}

	public void SendGuildChangeTagRequest(string newTag, int guildID)
	{
		if (Session.MyPlayerData.Guild != null)
		{
			aec.sendRequest(new RequestUpdateGuildTag(newTag, guildID));
		}
	}

	public void SendGuildMOTDUpdate(string motd, int guildID)
	{
		if (Session.MyPlayerData.Guild != null)
		{
			aec.sendRequest(new RequestMOTDUpdate(motd, guildID));
		}
	}

	public void SendApopUpdateRequest(int ApopID, string UpdatedText)
	{
		aec.sendRequest(new RequestEditApopText(ApopID, UpdatedText));
	}

	public void SendDeleteMailRequest(int charid, int mailid)
	{
		RequestMailDelete requestMailDelete = new RequestMailDelete();
		requestMailDelete.charID = charid;
		requestMailDelete.mailID = mailid;
		aec.sendRequest(requestMailDelete);
	}

	public void SendSendMailRequest(MailMessage mail, string recipient)
	{
		RequestMailSend requestMailSend = new RequestMailSend();
		requestMailSend.mail = mail;
		requestMailSend.recipient = recipient;
		aec.sendRequest(requestMailSend);
	}

	public void SendMailItemClaimRequest(List<RewardItem> items)
	{
		RequestMailItemClaim requestMailItemClaim = new RequestMailItemClaim();
		requestMailItemClaim.charid = Entities.Instance.me.ID;
		requestMailItemClaim.rewards = items;
		aec.sendRequest(requestMailItemClaim);
	}

	public void SendSeenMailRequest(MailMessage mail)
	{
		RequestMailSeen requestMailSeen = new RequestMailSeen();
		requestMailSeen.mail = mail;
		aec.sendRequest(requestMailSeen);
	}
}

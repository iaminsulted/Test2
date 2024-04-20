using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class House : MonoBehaviour
{
	public class ComFrame
	{
		public static readonly float UpdateInterval = 0.2f;

		private ComHouseItemMove frameBegin;

		private ComHouseItemMove frameEnd;

		public bool IsExpired;

		public bool IsLocked;

		private static DateTime renderTime => DateTime.UtcNow.AddSeconds((0f - UpdateInterval) * 2f);

		public bool IsComplete
		{
			get
			{
				if (IsExpired)
				{
					return PositionInFrame >= 1f;
				}
				return false;
			}
		}

		public float PositionInFrame => (float)(renderTime.Ticks - frameBegin.Timestamp.Ticks) / (float)(frameEnd.Timestamp.Ticks - frameBegin.Timestamp.Ticks);

		public void AddNewCom(ComHouseItemMove comMove)
		{
			if (IsExpired)
			{
				frameEnd = comMove;
				return;
			}
			if (comMove.PlacerID <= 0)
			{
				IsExpired = true;
			}
			if (frameBegin != null)
			{
				if (frameEnd != null)
				{
					updatePosRot();
					frameEnd.Position = frameBegin.Position;
					frameEnd.Rotation = frameBegin.Rotation;
					frameBegin = frameEnd;
					frameBegin.Timestamp = renderTime;
				}
				frameEnd = comMove;
				frameEnd.Timestamp = DateTime.UtcNow;
			}
			else
			{
				frameBegin = comMove;
				frameBegin.Timestamp = renderTime;
			}
		}

		private void updatePosRot()
		{
			float positionInFrame = PositionInFrame;
			frameBegin.Position = Vector3.LerpUnclamped(frameBegin.Position, frameEnd.Position, positionInFrame);
			frameBegin.Scale = Vector3.LerpUnclamped(frameBegin.Scale, frameEnd.Scale, positionInFrame);
			frameBegin.Rotation = Quaternion.LerpUnclamped(Quaternion.Euler(frameBegin.Rotation), Quaternion.Euler(frameEnd.Rotation), positionInFrame).eulerAngles;
			frameBegin.Timestamp = renderTime;
		}

		public ComHouseItemMove GetLerpedCom()
		{
			if (IsComplete)
			{
				if (frameEnd != null)
				{
					return frameEnd;
				}
				return frameBegin;
			}
			if (frameEnd == null)
			{
				return frameBegin;
			}
			updatePosRot();
			return frameBegin;
		}
	}

	private class HouseItemLoader
	{
		public AssetBundleRequest abr;

		public HashSet<int> comHouseItemIDs = new HashSet<int>();

		public GameObject Asset;
	}

	private const float UpdateInterval = 0.2f;

	private const float MaxPlacementDistance = 10f;

	private const float PlacingForce = 450f;

	public HouseItem hItemCurrent;

	private Dictionary<string, AssetBundleLoader> bundleLoaders = new Dictionary<string, AssetBundleLoader>();

	private Dictionary<string, HouseItemLoader> houseItemLoaders = new Dictionary<string, HouseItemLoader>();

	private Dictionary<int, HouseItem> houseItems = new Dictionary<int, HouseItem>();

	private Dictionary<int, ComHouseItem> comHouseItems = new Dictionary<int, ComHouseItem>();

	private Dictionary<int, ComFrame> comFrames = new Dictionary<int, ComFrame>();

	private AssetLoader<GameObject> assetLoader = new AssetLoader<GameObject>();

	public Action<bool> onEditModeChanged;

	private DateTime lastClick = DateTime.MinValue;

	private int PLACEMENT_LAYERS;

	private bool dragCurrentObject;

	private GameObject houseItemRoot;

	private bool waitingToPlace;

	public bool IsEditing { get; private set; }

	public HouseData houseData { get; private set; }

	private void Start()
	{
	}

	public void Show(HouseData hData)
	{
		PLACEMENT_LAYERS = Layers.DEFAULT | Layers.CLOSE | Layers.MIDDLE | Layers.MIDDLEFAR | Layers.FAR;
		Game instance = Game.Instance;
		instance.onBackgroundClick = (Action)Delegate.Combine(instance.onBackgroundClick, new Action(OnBackgroundClick));
		IsEditing = false;
		houseData = hData;
		if (Session.MyPlayerData.ID == hData.OwnerID)
		{
			UIGame.Instance.HouseControls.ShowOwnerControls();
		}
		else
		{
			UIGame.Instance.HouseControls.ShowGuestControls();
		}
	}

	public void OnDestroy()
	{
		if (Game.Instance != null)
		{
			Game instance = Game.Instance;
			instance.onBackgroundClick = (Action)Delegate.Remove(instance.onBackgroundClick, new Action(OnBackgroundClick));
		}
		Clear();
	}

	private void OnEnable()
	{
		UICamera.onDrag = (UICamera.VectorDelegate)Delegate.Combine(UICamera.onDrag, new UICamera.VectorDelegate(OnUICameraDrag));
		UICamera.onDragEnd = (UICamera.VoidDelegate)Delegate.Combine(UICamera.onDragEnd, new UICamera.VoidDelegate(OnUICameraDragEnd));
	}

	private void OnDisable()
	{
		UICamera.onDrag = (UICamera.VectorDelegate)Delegate.Remove(UICamera.onDrag, new UICamera.VectorDelegate(OnUICameraDrag));
		UICamera.onDragEnd = (UICamera.VoidDelegate)Delegate.Remove(UICamera.onDragEnd, new UICamera.VoidDelegate(OnUICameraDragEnd));
	}

	public void Clear()
	{
		UnityEngine.Object.Destroy(houseItemRoot);
		StopAllCoroutines();
		hItemCurrent = null;
		comHouseItems.Clear();
		houseItems.Clear();
		comFrames.Clear();
		assetLoader.DisposeAll();
	}

	public void SetHouseData(HouseData hData)
	{
	}

	public void EnterEditMode()
	{
		IsEditing = true;
		UIGame.Instance.HouseControls.OnEditModeChanged(isEditing: true);
	}

	public void ExitEditMode()
	{
		IsEditing = false;
		StopPlacing();
		UIGame.Instance.HouseControls.OnEditModeChanged(isEditing: false);
	}

	public void DeleteSelectedItem()
	{
		HouseItem hItem = hItemCurrent;
		StopPlacing();
		RequestRemove(hItem);
	}

	public void UpdateCurrentItem(HouseItem hItem)
	{
		if (hItem == null)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Delete))
		{
			StopPlacing();
			RequestRemove(hItem);
			return;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Quaternion quaternion = Quaternion.Euler(0f, Input.mouseScrollDelta.y * Time.deltaTime * 300f, 0f);
			hItem.transform.rotation = hItem.transform.rotation * quaternion;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			float num = Input.mouseScrollDelta.y * Time.deltaTime * 4f;
			hItem.transform.localScale = hItem.transform.localScale + Vector3.one * num;
		}
	}

	public void EnableDrag()
	{
		dragCurrentObject = true;
	}

	private void OnUICameraDrag(GameObject go, Vector2 vec)
	{
	}

	private void OnUICameraDragEnd(GameObject go)
	{
		if (dragCurrentObject && hItemCurrent != null)
		{
			Game.Instance.OnPress(hItemCurrent.gameObject, isPressed: true, hItemCurrent);
		}
		dragCurrentObject = false;
	}

	private void ProcessComFrames()
	{
		HashSet<int> hashSet = new HashSet<int>();
		foreach (KeyValuePair<int, ComFrame> comFrame in comFrames)
		{
			int key = comFrame.Key;
			ComFrame value = comFrame.Value;
			ComHouseItemMove lerpedCom = value.GetLerpedCom();
			if (lerpedCom != null && houseItems.TryGetValue(lerpedCom.ID, out var value2))
			{
				value2.Move(lerpedCom);
				value2.gameObject.SetActive(value: true);
				if (value.IsComplete)
				{
					value2.Solidify();
					hashSet.Add(key);
				}
			}
		}
		foreach (int item in hashSet)
		{
			comFrames.Remove(item);
		}
	}

	public void Update()
	{
		UpdateCurrentItem(hItemCurrent);
		ProcessComFrames();
	}

	public void OnBackgroundClick()
	{
		StopPlacing();
	}

	public void OnHouseItemClick(HouseItem hItem)
	{
		if (!(lastClick.AddSeconds(0.5) > DateTime.UtcNow))
		{
			lastClick = DateTime.UtcNow;
			if (waitingToPlace)
			{
				Debug.LogError("Clicked on an item while waiting to place another");
			}
			else if (IsEditing && hItem.PlacerID <= 0)
			{
				Gizmo.Enable(hItem.transform);
				ComHouseItemMove comHouseItemMove = hItem.GenerateMove();
				comHouseItemMove.PlacerID = Entities.Instance.me.ID;
				RequestMove(comHouseItemMove);
			}
		}
	}

	public void FixedUpdate()
	{
	}

	public void RequestClearAll()
	{
		AEC.getInstance().sendRequest(new RequestHouseItemClearAll());
	}

	private void RequestRemove(HouseItem hItem)
	{
		if (!(hItem == null))
		{
			AEC.getInstance().sendRequest(new RequestHouseItemRemove(hItem.ID));
		}
	}

	public void RequestSave()
	{
		AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseSave));
	}

	public void RequestExit()
	{
		AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.HouseExit));
	}

	private void RequestMove(ComHouseItemMove comMove)
	{
		AEC.getInstance().sendRequest(new RequestHouseItemMove(comMove));
	}

	public void DuplicateSelected()
	{
		if (hItemCurrent != null && (!Session.MyPlayerData.HouseItemCounts.TryGetValue(hItemCurrent.ItemID, out var value) || value < Items.Get(hItemCurrent.ItemID).MaxStack))
		{
			RequestAdd(hItemCurrent.GenerateAdd());
		}
	}

	public void RequestAdd(ComHouseItem chItem)
	{
		AEC.getInstance().sendRequest(new RequestHouseItemAdd(chItem));
	}

	public void RequestAddStartPlacing(int itemID, bool enableDrag = false)
	{
		if (Session.MyPlayerData.HouseItemCounts.TryGetValue(itemID, out var value) && value >= Items.Get(itemID).MaxStack)
		{
			return;
		}
		if (houseItems.Count >= Session.MyPlayerData.HouseItemMaxPerSlot)
		{
			Chat.Notify("You cannot place more than " + Session.MyPlayerData.HouseItemMaxPerSlot.ToString("n0") + " items in a house!");
			return;
		}
		if (enableDrag)
		{
			EnableDrag();
		}
		ComHouseItem comHouseItem = new ComHouseItem();
		comHouseItem.ItemID = itemID;
		comHouseItem.PlacerID = Entities.Instance.me.ID;
		waitingToPlace = true;
		RequestAdd(comHouseItem);
	}

	public void SelectItem(int houseItemID)
	{
		if (houseItems.TryGetValue(houseItemID, out var value))
		{
			EnterEditMode();
			OnHouseItemClick(value);
		}
	}

	private void StartPlacing(HouseItem hItem)
	{
		StopPlacing();
		if (!IsEditing)
		{
			Debug.LogError("Tried to start placing item while not in edit mode");
			return;
		}
		if (hItem.Locked)
		{
			Debug.LogError("placing of locked house items is not allowd");
		}
		hItemCurrent = hItem;
		hItemCurrent.PlacerID = Entities.Instance.me.ID;
		hItemCurrent.Liquify();
		StartCoroutine("HouseItemUpdater");
	}

	public void StopPlacing()
	{
		StopCoroutine("HouseItemUpdater");
		if (hItemCurrent != null)
		{
			Gizmo.Disable(hItemCurrent.transform);
			hItemCurrent.LockUntilServerSync();
			hItemCurrent.Solidify();
			ComHouseItemMove comHouseItemMove = hItemCurrent.GenerateMove();
			comHouseItemMove.PlacerID = -1;
			RequestMove(comHouseItemMove);
			hItemCurrent = null;
		}
		if (UIHouseItemList.IsOpenAndDisabled)
		{
			UIHouseItemList.Enable();
		}
	}

	private IEnumerator HouseItemUpdater()
	{
		while (true)
		{
			RequestMove(hItemCurrent.GenerateMove());
			yield return new WaitForSeconds(ComFrame.UpdateInterval);
		}
	}

	private void CreateRootGameObject()
	{
		if (houseItemRoot == null)
		{
			houseItemRoot = new GameObject("HouseItemRoot");
		}
	}

	private void InitHouseItem(GameObject Asset, int comHouseItemID)
	{
		HouseItem houseItem = UnityEngine.Object.Instantiate(Asset).AddComponent<HouseItem>();
		CreateRootGameObject();
		houseItem.transform.SetParent(houseItemRoot.transform, worldPositionStays: true);
		houseItems[comHouseItemID] = houseItem;
		if (comFrames.ContainsKey(comHouseItemID))
		{
			return;
		}
		if (comHouseItems.TryGetValue(comHouseItemID, out var value))
		{
			if (value.PlacerID == Entities.Instance.me.ID)
			{
				if (!value.Duplicate)
				{
					value.Rotation = (Quaternion.Euler(0f, Game.Instance.camController.transform.eulerAngles.y + 180f, 0f) * houseItem.transform.rotation).eulerAngles;
					value.Position = Entities.Instance.me.position + Entities.Instance.me.wrapperTransform.forward * 2f;
					value.Scale = houseItem.transform.localScale;
				}
				houseItem.ComSync(value);
				waitingToPlace = false;
				houseItem.Init();
				houseItem.gameObject.SetActive(value: true);
				if (dragCurrentObject)
				{
					Gizmo.Enable(houseItem.transform, showControls: true);
					Game.Instance.OnPress(houseItem.gameObject, isPressed: true, houseItem);
				}
				else
				{
					Gizmo.Enable(houseItem.transform);
				}
				StartPlacing(houseItem);
			}
			else if (value.PlacerID <= 0)
			{
				houseItem.ComSync(value);
				houseItem.Init();
				houseItem.gameObject.SetActive(value: true);
			}
			else
			{
				Debug.LogWarning("Waiting on load before showing lerped comms");
			}
		}
		else
		{
			Debug.LogWarning($"comHouseItem with id {comHouseItemID} is missing, cannot sync!");
		}
	}

	private void OnAssetLoad(GameObject GO, HouseItemLoader houseItemLoader)
	{
		GO.SetActive(value: false);
		houseItemLoader.Asset = GO;
		foreach (int comHouseItemID in houseItemLoader.comHouseItemIDs)
		{
			InitHouseItem(GO, comHouseItemID);
		}
	}

	private IEnumerator LoadHouseItemAsset(HouseItemInfo hItemInfo, HouseItemLoader houseItemLoader)
	{
		if (!bundleLoaders[hItemInfo.bInfo.FileName].isDone)
		{
			yield return bundleLoaders[hItemInfo.bInfo.FileName];
		}
		AssetBundleRequest abr = bundleLoaders[hItemInfo.bInfo.FileName].Asset.LoadAssetAsync<GameObject>(hItemInfo.AssetName);
		yield return abr;
		if (abr.asset as GameObject == null)
		{
			Debug.LogError("Asset request failed - " + hItemInfo.AssetName + " not found in " + hItemInfo.bInfo.FileName);
		}
		else
		{
			OnAssetLoad(abr.asset as GameObject, houseItemLoader);
		}
	}

	private IEnumerator waitForHouseItemDownload()
	{
		while (!assetLoader.IsFinished)
		{
			yield return null;
		}
	}

	public Coroutine AddItems(List<ComHouseItem> hItems, Dictionary<int, HouseItemInfo> hItemInfos)
	{
		foreach (ComHouseItem hItem in hItems)
		{
			AddItem(hItem, hItemInfos[hItem.ItemID]);
		}
		return StartCoroutine("waitForHouseItemDownload");
	}

	public void AddItem(ComHouseItem comHouseItem, HouseItemInfo hItemInfo)
	{
		comHouseItems[comHouseItem.ID] = comHouseItem;
		assetLoader.Get(hItemInfo.AssetName, hItemInfo.bInfo, delegate(GameObject asset)
		{
			InitHouseItem(asset, comHouseItem.ID);
		});
	}

	public void MoveItem(ComHouseItemMove comMove)
	{
		if (comFrames.TryGetValue(comMove.ID, out var value))
		{
			comFrames[comMove.ID].AddNewCom(comMove);
		}
		else if (comMove.PlacerID > 0)
		{
			if (comMove.PlacerID == Entities.Instance.me.ID)
			{
				if (houseItems.TryGetValue(comMove.ID, out var value2))
				{
					StartPlacing(value2);
				}
				return;
			}
			comFrames[comMove.ID] = new ComFrame();
			comFrames[comMove.ID].AddNewCom(comMove);
			if (houseItems.TryGetValue(comMove.ID, out var value3))
			{
				value3.Liquify();
			}
		}
		else
		{
			if (value != null)
			{
				comFrames.Remove(comMove.ID);
			}
			if (houseItems.TryGetValue(comMove.ID, out var value4))
			{
				value4.Move(comMove);
				value4.Solidify();
			}
		}
	}

	public void RemoveItem(int HouseItemID, int houseID)
	{
		if (houseID == houseData.HouseID)
		{
			if (hItemCurrent != null && hItemCurrent.ID == HouseItemID)
			{
				StopPlacing();
			}
			if (houseItems.TryGetValue(HouseItemID, out var value))
			{
				UnityEngine.Object.Destroy(value.gameObject);
			}
			houseItems.Remove(HouseItemID);
			comHouseItems.Remove(HouseItemID);
			comFrames.Remove(HouseItemID);
		}
	}
}

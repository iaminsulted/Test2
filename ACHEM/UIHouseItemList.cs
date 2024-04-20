using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;
using WebSocketSharp;

public class UIHouseItemList : UIMenuWindow
{
	public class UIHouseItemWrapper : MonoBehaviour
	{
		public InventoryItem iItem;

		public UIHouseItem uiHouseItem;

		private UIHouseItem template;

		private AssetLoader<GameObject> assetLoader;

		private Transform parent;

		public bool isInitialized => uiHouseItem != null;

		public void Enable()
		{
			uiHouseItem?.gameObject?.SetActive(value: true);
		}

		public void Disable()
		{
			uiHouseItem?.gameObject?.SetActive(value: false);
		}

		public void Show(Vector3 position)
		{
			if (uiHouseItem == null)
			{
				uiHouseItem = UnityEngine.Object.Instantiate(template);
			}
			uiHouseItem.Init(iItem, assetLoader, this);
			uiHouseItem.transform.position = position;
			uiHouseItem.transform.SetParent(parent, worldPositionStays: true);
			uiHouseItem.transform.localScale = Vector3.one;
			uiHouseItem.gameObject.SetActive(value: true);
		}

		private void OnDestroy()
		{
			if (uiHouseItem != null)
			{
				UnityEngine.Object.Destroy(uiHouseItem.gameObject);
			}
			uiHouseItem = null;
			parent = null;
			assetLoader = null;
			iItem = null;
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void Destroy()
		{
		}

		public void Init(InventoryItem iItem, UIHouseItem template, AssetLoader<GameObject> assetLoader, Transform parent)
		{
			this.iItem = iItem;
			this.template = template;
			this.assetLoader = assetLoader;
			this.parent = parent;
			base.transform.SetParent(parent);
		}
	}

	public static UIHouseItemList instance;

	public UIViewport Viewport;

	public Transform ObjectCamera;

	public UICamera uiCamera;

	public Transform UIStage;

	public UIHouseItem HouseItemTemplate;

	public UIScrollView scrollView;

	public SpringPanel springPanel;

	public UIButton btnMain;

	public UIHouseItemDetail detail;

	public BoxCollider InstantiationBounds;

	public Transform scrollTopLeft;

	public Transform topLeft;

	public Transform center;

	public Transform bottomRight;

	public Transform scrollBottom;

	public Vector3 cameraOffset;

	public UIInventoryTabs tabs;

	public UIScrollBar scrollBar;

	public UIWidget stretchContainer;

	public UILabel titleLabel;

	public UIInput SearchInput;

	public UIButton SearchClear;

	private AssetLoader<GameObject> assetLoader;

	private Dictionary<UIInventory.FilterType, List<string>> CategoryTagMap;

	private Dictionary<UIInventory.FilterType, string> filterLabels;

	private List<UIHouseItemWrapper> wrappers = new List<UIHouseItemWrapper>();

	private float panelHeight;

	private const int numColumns = 2;

	private bool isDragging;

	private bool isScrolling;

	private bool allowDragPlace;

	private string search;

	private UIInventory.FilterType currentFilterType;

	private UIInventory.SortType currentSortType;

	public static bool IsOpenAndDisabled
	{
		get
		{
			if (instance != null)
			{
				return !instance.gameObject.activeSelf;
			}
			return false;
		}
	}

	private void enable()
	{
		base.gameObject.SetActive(value: true);
		StartCoroutine(CullObjects());
	}

	public static void Enable()
	{
		if (instance != null)
		{
			instance.enable();
		}
	}

	public static void Show()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Housing/HouseItemListUI"), UIManager.Instance.transform).GetComponent<UIHouseItemList>();
			instance.Init();
		}
	}

	public void LateUpdate()
	{
		Update3DObjectCameraPosition();
	}

	public void Update3DObjectCameraPosition()
	{
		Vector3 vector = -(scrollTopLeft.position - topLeft.position) + UIStage.position;
		ObjectCamera.position = vector + cameraOffset;
	}

	private Vector3 calculatedPosition(int wrapperIndex)
	{
		return UIStage.transform.position + wrapperIndex / 2 * Vector3.down * panelHeight + Vector3.right * panelHeight * (wrapperIndex % 2);
	}

	private void Update()
	{
	}

	private void OnClick(GameObject go)
	{
		Vector3 vector = UICamera.lastWorldPosition - scrollTopLeft.position;
		int num = -(int)(vector.y / panelHeight);
		int num2 = (int)(vector.x / panelHeight);
		int num3 = num * 2 + num2;
		if (num3 >= 0 && num3 < wrappers.Count)
		{
			float yOffset = 0f;
			detail.Show(wrappers[num3].uiHouseItem, yOffset, this);
		}
	}

	private void OnPress(GameObject go, bool isPressed)
	{
		scrollView.Press(isPressed);
		if (!isPressed)
		{
			isScrolling = false;
			isDragging = false;
		}
	}

	private void OnScroll(GameObject go, float scrollAmount)
	{
		scrollView.Scroll(scrollAmount);
	}

	private void OnDrag(GameObject go, Vector2 delta)
	{
		if (isDragging)
		{
			if (isScrolling)
			{
				scrollView.Drag();
			}
		}
		else
		{
			if (HousingManager.houseInstance == null)
			{
				return;
			}
			if (Vector3.Angle(Vector3.left, delta.normalized) < 45f)
			{
				isScrolling = false;
				Vector3 vector = UICamera.lastWorldPosition - scrollTopLeft.position;
				int num = -(int)(vector.y / panelHeight);
				int num2 = (int)(vector.x / panelHeight);
				int num3 = num * 2 + num2;
				if (num3 < 0 || num3 >= wrappers.Count)
				{
					return;
				}
				InventoryItem iItem = wrappers[num3].iItem;
				if (Session.MyPlayerData.HouseItemCounts.TryGetValue(iItem.ID, out var value) && value >= iItem.Qty)
				{
					isDragging = true;
					return;
				}
				if (!HousingManager.houseInstance.IsEditing)
				{
					HousingManager.houseInstance.EnterEditMode();
				}
				if (iItem != null)
				{
					HousingManager.houseInstance.StopPlacing();
					HousingManager.houseInstance.RequestAddStartPlacing(iItem.ID, enableDrag: true);
				}
			}
			else
			{
				isScrolling = true;
			}
			isDragging = true;
		}
	}

	private void OnDragStart(GameObject go)
	{
	}

	private void OnDragEnd(GameObject go)
	{
		isDragging = false;
		isScrolling = false;
	}

	private IEnumerator CullObjects()
	{
		while (true)
		{
			for (int i = 0; i < wrappers.Count; i++)
			{
				if (InstantiationBounds.bounds.Contains(calculatedPosition(i)))
				{
					if (!wrappers[i].isInitialized)
					{
						wrappers[i].Show(calculatedPosition(i));
					}
					wrappers[i].Enable();
				}
				else
				{
					wrappers[i].Disable();
				}
			}
			base.gameObject.SetActive(value: true);
			yield return null;
		}
	}

	public void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnMain.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener2.onDragStart = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onDragStart, new UIEventListener.VoidDelegate(OnDragStart));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener3.onDrag, new UIEventListener.VectorDelegate(OnDrag));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener4.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onDragEnd, new UIEventListener.VoidDelegate(OnDragEnd));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener5.onPress, new UIEventListener.BoolDelegate(OnPress));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener6.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uIEventListener6.onScroll, new UIEventListener.FloatDelegate(OnScroll));
		UIEventListener uIEventListener7 = UIEventListener.Get(SearchClear.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnSearchClearClicked));
		EventDelegate.Add(SearchInput.onChange, OnSearchChanged);
		UIInventoryTabs uIInventoryTabs = tabs;
		uIInventoryTabs.onFilterButtonClicked = (Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>)Delegate.Combine(uIInventoryTabs.onFilterButtonClicked, new Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>(OnFilterButtonClicked));
		UIInventoryTabs uIInventoryTabs2 = tabs;
		uIInventoryTabs2.onSortButtonClicked = (Action<UIInventory.SortType>)Delegate.Combine(uIInventoryTabs2.onSortButtonClicked, new Action<UIInventory.SortType>(OnSortButtonClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnMain.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener2.onDragStart = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onDragStart, new UIEventListener.VoidDelegate(OnDragStart));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener3.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onDragEnd, new UIEventListener.VoidDelegate(OnDragEnd));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener4.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener4.onPress, new UIEventListener.BoolDelegate(OnPress));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnMain.gameObject);
		uIEventListener5.onScroll = (UIEventListener.FloatDelegate)Delegate.Remove(uIEventListener5.onScroll, new UIEventListener.FloatDelegate(OnScroll));
		UIEventListener uIEventListener6 = UIEventListener.Get(SearchClear.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnSearchClearClicked));
		EventDelegate.Remove(SearchInput.onChange, OnSearchChanged);
		UIInventoryTabs uIInventoryTabs = tabs;
		uIInventoryTabs.onFilterButtonClicked = (Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>)Delegate.Remove(uIInventoryTabs.onFilterButtonClicked, new Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>(OnFilterButtonClicked));
		UIInventoryTabs uIInventoryTabs2 = tabs;
		uIInventoryTabs2.onSortButtonClicked = (Action<UIInventory.SortType>)Delegate.Remove(uIInventoryTabs2.onSortButtonClicked, new Action<UIInventory.SortType>(OnSortButtonClicked));
	}

	private void OnSearchClearClicked(GameObject go)
	{
		search = null;
		SearchInput.value = "";
		refresh();
	}

	private void OnSearchChanged()
	{
		search = SearchInput.value.ToLower();
		refresh();
	}

	private void addTagsToCategory(string displayName, UIInventory.FilterType category, IEnumerable<string> tags)
	{
		CategoryTagMap.Add(category, tags.ToList());
		filterLabels.Add(category, displayName);
	}

	private bool IsInSearch(InventoryItem item)
	{
		if (search == null)
		{
			return true;
		}
		search = search.Trim();
		if (search == string.Empty)
		{
			return true;
		}
		return item.Name.ToLower().Contains(search);
	}

	protected override void Init()
	{
		base.Init();
		filterLabels = new Dictionary<UIInventory.FilterType, string>();
		CategoryTagMap = new Dictionary<UIInventory.FilterType, List<string>>();
		filterLabels.Add(UIInventory.FilterType.All, "House Items");
		addTagsToCategory("Furniture", UIInventory.FilterType.Furniture, new List<string> { "furniture", "chair", "bed", "table" });
		addTagsToCategory("Structural", UIInventory.FilterType.Structure, new List<string> { "structure", "structural", "column", "wall" });
		addTagsToCategory("Wall Decor", UIInventory.FilterType.WallDecor, new List<string> { "walldecor", "poster", "painting" });
		addTagsToCategory("Lighting", UIInventory.FilterType.Lighting, new List<string> { "lighting", "candle", "fire" });
		addTagsToCategory("FLoor Decor", UIInventory.FilterType.FloorDecor, new List<string> { "floordecor", "rug", "carpet" });
		addTagsToCategory("Statues", UIInventory.FilterType.Statue, new List<string> { "statue" });
		addTagsToCategory("Plants", UIInventory.FilterType.Plants, new List<string> { "plants", "outdoordecour" });
		addTagsToCategory("Miscellaneous", UIInventory.FilterType.Miscellaneous, new List<string> { "misc", "miscellaneous" });
		cameraOffset = (bottomRight.position - topLeft.position) / 2f;
		Viewport.sourceCamera = UIManager.Instance.uiCamera;
		HouseItemTemplate.gameObject.SetActive(value: false);
		panelHeight = NGUIMath.CalculateAbsoluteWidgetBounds(UnityEngine.Object.Instantiate(HouseItemTemplate, UIStage.transform).transform).extents.y * 2f;
		assetLoader = new AssetLoader<GameObject>();
		List<InventoryItem> iItems = Session.MyPlayerData.items.Where((InventoryItem x) => x.Type == ItemType.HouseItem).ToList();
		tabs.Init(iItems);
		currentFilterType = tabs.CurrentCategory;
		currentSortType = tabs.SortType;
		refresh();
		enable();
	}

	private void OnFilterButtonClicked(UIInventory.FilterType filterType, UIInventory.SortType sortType, UIInventory.FilterType prevFilterType)
	{
		currentFilterType = filterType;
		refresh();
	}

	private void OnSortButtonClicked(UIInventory.SortType sortType)
	{
		currentSortType = sortType;
		refresh();
	}

	public void refresh()
	{
		titleLabel.text = filterLabels[currentFilterType];
		UIStage.DestroyChildren();
		wrappers.Clear();
		scrollView.InvalidateBounds();
		IEnumerable<InventoryItem> items = Session.MyPlayerData.items.Where((InventoryItem x) => x.Type == ItemType.HouseItem && IsInSearch(x));
		foreach (InventoryItem sortedItem in UIInventory.GetSortedItems(currentSortType, items))
		{
			if (currentFilterType == UIInventory.FilterType.All)
			{
				UIHouseItemWrapper uIHouseItemWrapper = new GameObject("Wrapper").AddComponent<UIHouseItemWrapper>();
				uIHouseItemWrapper.Init(sortedItem, HouseItemTemplate, assetLoader, UIStage);
				wrappers.Add(uIHouseItemWrapper);
			}
			else if (!sortedItem.Tags.IsNullOrEmpty())
			{
				string[] first = sortedItem.Tags.ToLower().Split(',');
				if (CategoryTagMap.TryGetValue(currentFilterType, out var value) && first.Intersect(value).Any())
				{
					UIHouseItemWrapper uIHouseItemWrapper2 = new GameObject("Wrapper").AddComponent<UIHouseItemWrapper>();
					uIHouseItemWrapper2.Init(sortedItem, HouseItemTemplate, assetLoader, UIStage);
					wrappers.Add(uIHouseItemWrapper2);
				}
			}
		}
		scrollBottom.position = calculatedPosition(wrappers.Count + 1) - UIStage.position + topLeft.position;
		stretchContainer.UpdateAnchors();
		scrollView.ResetPosition();
	}

	private void OnDestroy()
	{
		assetLoader.DisposeAll();
		foreach (UIHouseItemWrapper wrapper in wrappers)
		{
			UnityEngine.Object.Destroy(wrapper.gameObject);
		}
		wrappers.Clear();
		UIScrollView uIScrollView = scrollView;
		uIScrollView.onMomentumCalcEnd = (Action)Delegate.Remove(uIScrollView.onMomentumCalcEnd, new Action(Update3DObjectCameraPosition));
		detail.Clear();
		UnityEngine.Object.Destroy(UIStage.gameObject);
	}

	protected override void Destroy()
	{
		UnityEngine.Object.Destroy(this);
		base.Destroy();
	}
}

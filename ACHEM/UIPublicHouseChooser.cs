using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIPublicHouseChooser : UIWindow
{
	public const int DOWNLOAD_WINDOW = 30;

	public UIButton ButtonClose;

	public UIButton ButtonClearSearch;

	public UIPooledScrollview pooledScrollView;

	public List<UIHouseDataSortButton> SortButtons;

	public UIInput SearchInput;

	public UIPublicHouseDetail PublicHouseDetail;

	public GameObject BackgroundImage;

	public UITexture BackgroundTex;

	public AssetLoader<Texture2D> assetLoader;

	private ScrollviewPool<HouseData> houseDataPool;

	private HouseDataCategory currentCategory = HouseDataCategory.CurrentPlayers;

	private Coroutine downloaderRoutine;

	private bool isListReversed;

	public static UIPublicHouseChooser Instance { get; protected set; }

	protected virtual void Awake()
	{
		Instance = this;
	}

	protected virtual void OnDestroy()
	{
		assetLoader.DisposeAll();
		Instance = null;
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ButtonClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(ButtonClearSearch.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onClearSearchClick));
		EventDelegate.Add(SearchInput.onSubmit, onSearchSubmitted);
		Session.MyPlayerData.PublicHouseDataAdded += onHouseDataAdded;
		foreach (UIHouseDataSortButton sortButton in SortButtons)
		{
			sortButton.onResetAll = (Action)Delegate.Combine(sortButton.onResetAll, new Action(ResetSortButtons));
			sortButton.onClick = (Action<HouseDataCategory, bool>)Delegate.Combine(sortButton.onClick, new Action<HouseDataCategory, bool>(OnSortButtonClicked));
		}
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ButtonClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(onCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(ButtonClearSearch.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onClearSearchClick));
		EventDelegate.Remove(SearchInput.onSubmit, onSearchSubmitted);
		Session.MyPlayerData.PublicHouseDataAdded -= onHouseDataAdded;
		foreach (UIHouseDataSortButton sortButton in SortButtons)
		{
			sortButton.onResetAll = (Action)Delegate.Remove(sortButton.onResetAll, new Action(ResetSortButtons));
			sortButton.onClick = (Action<HouseDataCategory, bool>)Delegate.Remove(sortButton.onClick, new Action<HouseDataCategory, bool>(OnSortButtonClicked));
		}
	}

	protected void onCloseClick(GameObject go)
	{
		Close();
	}

	private void onClearSearchClick(GameObject go)
	{
		if (downloaderRoutine == null)
		{
			downloaderRoutine = StartCoroutine(DownloadHouseData());
		}
		SearchInput.value = "";
	}

	private void onSearchSubmitted()
	{
		if (downloaderRoutine != null)
		{
			StopCoroutine(downloaderRoutine);
		}
		downloaderRoutine = null;
		AEC.getInstance().sendRequest(new RequestHouseData(0, null, HouseDataCategory.Query, Session.MyPlayerData.ClientPublicHouseDataVersion, isReversed: false, SearchInput.value));
	}

	public override void Close()
	{
		base.Close();
	}

	public IEnumerator DownloadHouseData()
	{
		while (true)
		{
			int centerIndex = pooledScrollView.GetCenterIndex();
			int num = Math.Max(0, centerIndex - 15);
			int num2 = Math.Min(Session.MyPlayerData.PublicHouseDataCount - 1, centerIndex + 15);
			List<int> list = new List<int>();
			for (int i = num; i <= num2; i++)
			{
				if (!Session.MyPlayerData.PublicHouseData.ContainsKey(i))
				{
					list.Add(i);
				}
			}
			if (list.Any())
			{
				AEC.getInstance().sendRequest(new RequestHouseData(num, list, currentCategory, Session.MyPlayerData.ClientPublicHouseDataVersion, isListReversed));
				list.Clear();
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private void OnSortButtonClicked(HouseDataCategory dCat, bool isReversed)
	{
		isListReversed = isReversed;
		currentCategory = dCat;
		if (downloaderRoutine == null)
		{
			downloaderRoutine = StartCoroutine(DownloadHouseData());
		}
		SearchInput.value = "";
		int centerIndex = pooledScrollView.GetCenterIndex();
		int lowerBoundIndex = Math.Max(0, centerIndex - 15);
		AEC.getInstance().sendRequest(new RequestHouseData(lowerBoundIndex, null, currentCategory, Session.MyPlayerData.ClientPublicHouseDataVersion, isReversed));
	}

	private void ResetSortButtons()
	{
		foreach (UIHouseDataSortButton sortButton in SortButtons)
		{
			sortButton.SetButtonState(0);
		}
	}

	private void onHouseDataAdded(Dictionary<int, HouseData> newData, HouseDataCategory dataCategory, bool isReversed)
	{
		switch (dataCategory)
		{
		case HouseDataCategory.PersonalHouseList:
			return;
		case HouseDataCategory.Query:
			ResetSortButtons();
			break;
		}
		switch (dataCategory)
		{
		case HouseDataCategory.HouseTitle:
			if (!isReversed)
			{
				SortButtons[0].SetButtonState(1);
			}
			else
			{
				SortButtons[0].SetButtonState(2);
			}
			break;
		case HouseDataCategory.PlayerName:
			if (!isReversed)
			{
				SortButtons[1].SetButtonState(1);
			}
			else
			{
				SortButtons[1].SetButtonState(2);
			}
			break;
		case HouseDataCategory.Map:
			if (!isReversed)
			{
				SortButtons[2].SetButtonState(1);
			}
			else
			{
				SortButtons[2].SetButtonState(2);
			}
			break;
		case HouseDataCategory.CurrentPlayers:
			if (!isReversed)
			{
				SortButtons[3].SetButtonState(1);
			}
			else
			{
				SortButtons[3].SetButtonState(2);
			}
			break;
		}
		houseDataPool.SetDataCache(Session.MyPlayerData.PublicHouseData, Session.MyPlayerData.PublicHouseDataCount);
	}

	private void InitPublicHouseList()
	{
		AEC.getInstance().sendRequest(new RequestHouseData(0, null, currentCategory, Session.MyPlayerData.ClientPublicHouseDataVersion));
		Init();
		houseDataPool = new ScrollviewPool<HouseData>();
		houseDataPool.Init(pooledScrollView, delegate(HouseData hData, int index, GameObject GO)
		{
			if (hData != null)
			{
				GO.GetComponent<UIPublicHouseSlot>().SetHouseDataAndRefresh(hData);
				GO.SetActive(value: true);
			}
		}, delegate
		{
		}, 22);
		foreach (GameObject poolObject in houseDataPool.PoolObjects)
		{
			poolObject.GetComponent<UIDragScrollView>().scrollView = pooledScrollView.scrollView;
		}
		foreach (UIHouseDataSortButton sortButton in SortButtons)
		{
			sortButton.Init();
		}
		SortButtons.Last().SetButtonState(1);
		pooledScrollView.ResetScrollview();
		downloaderRoutine = StartCoroutine(DownloadHouseData());
		assetLoader = new AssetLoader<Texture2D>();
	}

	public void ShowDetail(UIPublicHouseSlot uiPublicHouseSlot)
	{
		PublicHouseDetail.Init(uiPublicHouseSlot.HouseData, this);
		PublicHouseDetail.gameObject.SetActive(value: true);
		BackgroundImage.gameObject.SetActive(value: true);
		assetLoader.Get(uiPublicHouseSlot.HouseData.AssetName, uiPublicHouseSlot.HouseData.Bundle, delegate(Texture2D tex)
		{
			BackgroundTex.mainTexture = tex;
		});
	}

	public void CloseDetail()
	{
		PublicHouseDetail.gameObject.SetActive(value: false);
		BackgroundImage.gameObject.SetActive(value: false);
	}

	public static void Show()
	{
		if (Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Housing/PublicHouseList"), UIManager.Instance.transform);
			obj.name = "PublicHouseList";
			Instance = obj.GetComponent<UIPublicHouseChooser>();
			Instance.InitPublicHouseList();
		}
	}
}

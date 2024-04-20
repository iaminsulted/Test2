using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQuest : UIMenuWindow
{
	private static UIQuest instance;

	public UILabel lblTitle;

	public GameObject itemGOprefab;

	public UIQuestDetail uiQuestDetail;

	public GameObject QuestList;

	public GameObject CategoryButtonsContainer;

	public UIButton[] CategoryButtons;

	public QuestCategory Category;

	public Color SelectedCategorySpriteColor;

	public Color UnselectedCategorySpriteColor;

	public Color SelectedCategoryLabelColor;

	public Color UnselectedCategoryLabelColor;

	private QuestMode mode;

	private List<int> allQuestIDs;

	private List<int> missingQuestIDs;

	private List<Quest> availableQuests = new List<Quest>();

	private Transform container;

	private List<UIQuestListItem> itemGOs;

	private UIQuestListItem selectedItem;

	private ObjectPool<GameObject> itemGOpool;

	private List<int> acceptQuestIDs = new List<int>();

	private List<int> turnInQuestIDs = new List<int>();

	public static void ShowQuests(List<int> acceptQuestIds, List<int> turnInQuestIds)
	{
		Load(QuestMode.Quest, acceptQuestIds, turnInQuestIds);
	}

	public static void ShowLog()
	{
		Session.MyPlayerData.LoadQuestAreas();
		List<int> turnInQuestIds = Session.MyPlayerData.CurQuests.Where((int p) => Quests.Get(p) != null).ToList();
		Load(QuestMode.Log, turnInQuestIds, turnInQuestIds);
	}

	public static void ShowLog(Quest quest)
	{
		Session.MyPlayerData.LoadQuestAreas();
		List<int> turnInQuestIds = Session.MyPlayerData.CurQuests.Where((int p) => Quests.Get(p) != null).ToList();
		Load(QuestMode.Tracker, turnInQuestIds, turnInQuestIds);
		if (quest != null)
		{
			instance.ShowDetail(quest);
		}
	}

	private static void Load(QuestMode mode, List<int> acceptQuestIDs, List<int> turnInQuestIds)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Quest"), UIManager.Instance.transform).GetComponent<UIQuest>();
			instance.Init();
		}
		instance.acceptQuestIDs = acceptQuestIDs;
		instance.turnInQuestIDs = turnInQuestIds;
		List<int> iDs = (from x in instance.GetNpcQuestIds()
			where x > 0
			select x).ToList();
		instance.LoadQuests(mode, iDs);
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			ShowLog();
		}
		else
		{
			instance.Close();
		}
	}

	public void OnQuestLoaded()
	{
		if (missingQuestIDs.Count <= 0)
		{
			return;
		}
		foreach (int missingQuestID in missingQuestIDs)
		{
			if (!Quests.HasKey(missingQuestID))
			{
				return;
			}
		}
		refresh();
	}

	protected override void Init()
	{
		base.Init();
		uiQuestDetail.Init();
		Game.Instance.QuestLoaded += OnQuestLoaded;
		Session.MyPlayerData.QuestRemoved += OnQuestRemoved;
		Session.MyPlayerData.QuestAdded += OnQuestAdded;
		container = itemGOprefab.transform.parent;
		itemGOs = new List<UIQuestListItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		for (int i = 0; i < CategoryButtons.Length; i++)
		{
			int category = i;
			UIEventListener uIEventListener = UIEventListener.Get(CategoryButtons[i].gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, (UIEventListener.VoidDelegate)delegate
			{
				SetCategory(category);
			});
		}
		UIEventListener uIEventListener2 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	private List<int> GetNpcQuestIds()
	{
		List<int> list = new List<int>();
		foreach (int acceptQuestID in acceptQuestIDs)
		{
			list.Add(acceptQuestID);
		}
		foreach (int turnInQuestID in turnInQuestIDs)
		{
			if (Session.MyPlayerData.IsQuestComplete(turnInQuestID))
			{
				list.Add(turnInQuestID);
			}
		}
		return list.Distinct().ToList();
	}

	private void OnQuestAdded(int questID)
	{
		acceptQuestIDs.Add(questID);
		turnInQuestIDs.Add(questID);
		refresh();
		Quest quest = Quests.Get(questID);
		if (quest.Auto)
		{
			ShowDetail(quest);
		}
	}

	private void OnQuestRemoved(int questID)
	{
		if (mode != 0)
		{
			acceptQuestIDs.Remove(questID);
			turnInQuestIDs.Remove(questID);
		}
		refresh();
		if (uiQuestDetail.Visible && uiQuestDetail.quest.ID == questID)
		{
			Back();
		}
	}

	private void LoadQuests(QuestMode mode, List<int> IDs)
	{
		this.mode = mode;
		allQuestIDs = IDs;
		lblTitle.text = ((mode == QuestMode.Quest) ? "Quests" : "Quest Log");
		CategoryButtonsContainer.SetActive(mode != QuestMode.Quest);
		missingQuestIDs = new List<int>();
		foreach (int allQuestID in allQuestIDs)
		{
			if (!Quests.HasKey(allQuestID))
			{
				missingQuestIDs.Add(allQuestID);
			}
		}
		if (missingQuestIDs.Count > 0)
		{
			Game.Instance.SendQuestLoadRequest(missingQuestIDs);
		}
		else
		{
			refresh();
		}
	}

	public void SetCategory(int category)
	{
		Category = (QuestCategory)category;
		refresh();
	}

	public void refresh()
	{
		for (int i = 0; i < CategoryButtons.Length; i++)
		{
			bool flag = i == (int)Category;
			CategoryButtons[i].defaultColor = (flag ? SelectedCategorySpriteColor : UnselectedCategorySpriteColor);
			CategoryButtons[i].GetComponentInChildren<UISprite>().transform.localScale = (flag ? (Vector3.one * 1.2f) : Vector3.one);
			CategoryButtons[i].GetComponentInChildren<UILabel>().fontSize = (flag ? 14 : 12);
		}
		availableQuests.Clear();
		allQuestIDs = GetNpcQuestIds();
		foreach (int allQuestID in allQuestIDs)
		{
			Quest quest = Quests.Get(allQuestID);
			if (quest != null && ((mode == QuestMode.Log && (Category == QuestCategory.All || quest.IsInCategory(Category))) || (mode == QuestMode.Quest && Session.MyPlayerData.IsQuestAvailable(quest)) || (mode == QuestMode.Tracker && (Session.MyPlayerData.HasQuest(quest.ID) || Session.MyPlayerData.AvailableQuest == quest))))
			{
				availableQuests.Add(quest);
			}
		}
		if (mode != QuestMode.Log && availableQuests.Count == 0)
		{
			Back();
		}
		else if (availableQuests.Count == 1)
		{
			ShowDetail(availableQuests[0]);
		}
		else if (mode == QuestMode.Quest)
		{
			Quest quest2 = availableQuests.Where((Quest p) => Session.MyPlayerData.IsQuestComplete(p.ID)).FirstOrDefault();
			if (quest2 != null)
			{
				ShowDetail(quest2);
			}
		}
		foreach (UIQuestListItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		availableQuests = (from x in availableQuests
			group x by x.TargetMapName into x
			orderby (!(x.Key == Game.Instance.AreaData.displayName)) ? 1 : 0, x.Key
			select x).SelectMany((IGrouping<string, Quest> x) => x.ToList()).ToList();
		if (Session.MyPlayerData.CurrentlyTrackedQuest != null)
		{
			Quest quest3 = availableQuests.Find((Quest p) => p.ID == Session.MyPlayerData.CurrentlyTrackedQuest.ID);
			if (quest3 != null)
			{
				availableQuests.Remove(quest3);
				availableQuests.Insert(0, quest3);
			}
		}
		foreach (Quest availableQuest in availableQuests)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIQuestListItem component = obj.GetComponent<UIQuestListItem>();
			component.Init(availableQuest);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void OnItemClicked(UIQuestListItem si)
	{
		if (selectedItem != null)
		{
			selectedItem.Selected = false;
		}
		selectedItem = si;
		selectedItem.Selected = true;
		ShowDetail(selectedItem.quest);
	}

	private void ShowDetail(Quest quest)
	{
		ShowQuestDetail();
		uiQuestDetail.LoadQuest(mode, quest, turnInQuestIDs);
	}

	protected override void Destroy()
	{
		Game.Instance.QuestLoaded -= OnQuestLoaded;
		Session.MyPlayerData.QuestRemoved -= OnQuestRemoved;
		Session.MyPlayerData.QuestAdded -= OnQuestAdded;
		uiQuestDetail.Destroy();
		for (int i = 0; i < CategoryButtons.Length; i++)
		{
			UIEventListener.Get(CategoryButtons[i].gameObject).Clear();
		}
		base.Destroy();
		instance = null;
	}

	public override void OnBackClick(GameObject go)
	{
		Back();
	}

	public override void Back()
	{
		if (uiQuestDetail.Visible && mode != 0 && availableQuests.Count > 1)
		{
			ShowQuestList();
			ShowLog();
		}
		else
		{
			base.Back();
		}
	}

	public void ShowQuestList()
	{
		for (int i = 0; i < CategoryButtons.Length; i++)
		{
			CategoryButtons[i].gameObject.SetActive(value: true);
		}
		QuestList.SetActive(value: true);
		uiQuestDetail.Visible = false;
	}

	public void ShowQuestDetail()
	{
		for (int i = 0; i < CategoryButtons.Length; i++)
		{
			CategoryButtons[i].gameObject.SetActive(value: false);
		}
		QuestList.SetActive(value: false);
		uiQuestDetail.Visible = true;
	}
}

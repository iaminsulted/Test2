using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICharClasses : UIMenuWindow
{
	public static UICharClasses instance;

	public UILabel ClassName;

	public UITable ClassTitleTable;

	public UISprite ClassTitleIcon;

	public GameObject inventoryClassTemplate;

	public GameObject TierButton;

	public UIClassSummaryTab ClassSummaryTab;

	public UIClassSpellsTab ClassSpellsTab;

	public UIClassRanksTab ClassRanksTab;

	public UILabel TotalRank;

	private List<UIInventoryClass> inventoryClasses;

	private Transform container;

	private UIInventoryClass selectedItem;

	private ObjectPool<GameObject> inventoryClassPool;

	private CharClass charClass;

	private GameObject TierButtonObject;

	private ClassDropDown GridDropDown;

	public GameObject[] levelStars;

	public Color[] totalRankColors;

	public static void Toggle()
	{
		if (instance == null)
		{
			Load(Session.MyPlayerData.charClasses.FirstOrDefault((CharClass x) => x.bEquip));
		}
		else
		{
			instance.Close();
		}
	}

	public static void LoadByID(int id)
	{
		Load(Session.MyPlayerData.combatClassList.First((CombatClass x) => x.ToCharClass().ClassID == id).ToCharClass());
	}

	public static void Load(CharClass target)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ClassUI"), UIManager.Instance.transform).GetComponent<UICharClasses>();
			instance.charClass = target;
			instance.Init();
		}
		else
		{
			instance.charClass = target;
			instance.Refresh();
		}
	}

	private void OnEnable()
	{
		Session.MyPlayerData.ClassEquipped += OnClassEquipped;
		Session.MyPlayerData.ClassAdded += OnClassAdded;
		Session.MyPlayerData.ClassXPUpdated += OnClassXPUpdated;
		Session.MyPlayerData.ClassRankUpdated += OnClassRankUpdated;
	}

	private void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassEquipped -= OnClassEquipped;
			Session.MyPlayerData.ClassAdded -= OnClassAdded;
			Session.MyPlayerData.ClassXPUpdated -= OnClassXPUpdated;
			Session.MyPlayerData.ClassRankUpdated -= OnClassRankUpdated;
		}
	}

	public void Refresh()
	{
		UpdateRankIcon();
		ClassName.text = charClass.ToCombatClass().Name;
		ClassTitleIcon.spriteName = charClass.ToCombatClass().Icon;
		ClassTitleTable.Reposition();
		ClassSummaryTab.Refresh(charClass);
		ClassSpellsTab.Refresh(charClass);
		ClassRanksTab.Refresh(charClass);
		foreach (UIInventoryClass inventoryClass in inventoryClasses)
		{
			inventoryClass.Refresh();
		}
	}

	public void OnClassEquipped(int classID)
	{
		if (instance != null)
		{
			instance.Close();
		}
		LoadByID(classID);
	}

	public void OnClassAdded(int classID)
	{
		LoadByID(classID);
	}

	private void OnClassRankUpdated(int classID, int rank)
	{
		LoadByID(classID);
	}

	private void OnClassXPUpdated(int classID, int classXP)
	{
		LoadByID(classID);
	}

	public bool IsVisibleInStore(CombatClass c)
	{
		if (!Session.MyPlayerData.IsClassAvailable(c))
		{
			return !c.HideUnavailable;
		}
		return true;
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		container = inventoryClassTemplate.transform.parent;
		inventoryClasses = new List<UIInventoryClass>();
		inventoryClassPool = new ObjectPool<GameObject>(inventoryClassTemplate);
		inventoryClassTemplate.SetActive(value: false);
		foreach (UIInventoryClass inventoryClass in inventoryClasses)
		{
			inventoryClassPool.Release(inventoryClass.gameObject);
			inventoryClass.Clicked -= OnItemClicked;
		}
		inventoryClasses.Clear();
		int num = Session.MyPlayerData.combatClassList.Max((CombatClass c) => c.ClassTier);
		int tier;
		for (tier = 0; tier <= num; tier++)
		{
			List<CombatClass> list = (from c in Session.MyPlayerData.combatClassList
				where c.ClassTier == tier
				orderby Session.MyPlayerData.OwnsClass(c.ID) descending, c.SortOrder
				select c).ToList();
			if (list.Count == 0)
			{
				continue;
			}
			MakeNewTierButton(tier);
			foreach (CombatClass item in list)
			{
				if (item.ID != 0 && IsVisibleInStore(item))
				{
					GameObject gameObject = inventoryClassPool.Get();
					gameObject.transform.SetParent(container, worldPositionStays: false);
					gameObject.SetActive(value: true);
					UIInventoryClass component = gameObject.GetComponent<UIInventoryClass>();
					component.Init(item);
					if (item.ToCharClass().ClassID == charClass.ClassID)
					{
						selectedItem = component;
					}
					component.Clicked += OnItemClicked;
					inventoryClasses.Add(component);
					GridDropDown.Contents.Add(gameObject);
				}
			}
		}
		Refresh();
	}

	private void MakeNewTierButton(int tier)
	{
		TierButtonObject = UnityEngine.Object.Instantiate(TierButton);
		TierButtonObject.transform.SetParent(container, worldPositionStays: false);
		GridDropDown = TierButtonObject.GetComponent<ClassDropDown>();
		GridDropDown.Label.text = GetTierLabel(tier);
		GridDropDown.Tier = tier;
		TierButtonObject.SetActive(value: true);
	}

	private string GetTierLabel(int tier)
	{
		return tier switch
		{
			0 => "Starter", 
			1 => "Tier I", 
			2 => "Tier II", 
			3 => "Tier III", 
			_ => "Incorrect Tier", 
		};
	}

	private void OnItemClicked(UIItem uiItem)
	{
		if (!(selectedItem == uiItem as UIInventoryClass))
		{
			selectedItem = uiItem as UIInventoryClass;
			Load(selectedItem.combatClass.ToCharClass());
		}
	}

	public void UpdateRankIcon()
	{
		if (Session.MyPlayerData.TotalClassRank % 100 == 0)
		{
			TotalRank.text = "100";
		}
		else
		{
			TotalRank.text = (Session.MyPlayerData.TotalClassRank % 100).ToString();
		}
		int num = Session.MyPlayerData.TotalClassRank / 500;
		if (Session.MyPlayerData.TotalClassRank % 500 < 101)
		{
			num--;
		}
		int num2 = (Session.MyPlayerData.TotalClassRank - 100) / 100 % 5 + 1;
		if ((Session.MyPlayerData.TotalClassRank - 100) / 100 % 5 == 0 && (Session.MyPlayerData.TotalClassRank - 100) % 500 == 0)
		{
			num2 = 5;
		}
		if (Session.MyPlayerData.TotalClassRank <= 100)
		{
			num2 = 0;
		}
		switch (num2)
		{
		case 1:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 2:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 3:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 4:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		case 5:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		default:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		}
		for (int i = 0; i < 5; i++)
		{
			if (levelStars[i].activeSelf)
			{
				levelStars[i].GetComponent<UISprite>().color = totalRankColors[num];
			}
		}
	}
}

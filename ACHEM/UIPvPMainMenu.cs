using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;

public class UIPvPMainMenu : UIMenuWindow
{
	private const int Min_PvP_Level = 5;

	private ObjectPool<GameObject> itemGOpool;

	public UIGrid grid;

	public UILabel Wins;

	public UILabel Losses;

	public UILabel Kills;

	public UILabel Deaths;

	public UILabel Assists;

	public UILabel ObjTaken;

	public UILabel ObjDef;

	public UILabel queueCount;

	public UILabel queueLabel;

	public UILabel GloryLevel;

	public List<UIClassGloryItem> classList;

	public static UIPvPMainMenu instance;

	private Transform container;

	private UIClassGloryItem selectedItem;

	private ObjectPool<GameObject> inventoryClassPool;

	private CharClass xcharClass;

	public GameObject inventoryGloryTemplate;

	public static void Toggle()
	{
		if (instance == null)
		{
			Show(Session.MyPlayerData.charClasses.FirstOrDefault((CharClass x) => x.bEquip));
		}
		else
		{
			instance.Close();
		}
	}

	public static void Show(CharClass target)
	{
		if (CheckAccess())
		{
			if (instance == null)
			{
				instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/UIPvPMenu"), UIManager.Instance.transform).GetComponent<UIPvPMainMenu>();
			}
			instance.queueCount.text = "In Queue...";
			instance.UpdateQueueDisplay();
			instance.xcharClass = target;
			instance.Init();
		}
	}

	private static bool CheckAccess()
	{
		if (Entities.Instance.me.Level < 5)
		{
			Notification.ShowWarning("Reach Level " + 5 + " to access PvP");
			return false;
		}
		return true;
	}

	protected override void Init()
	{
		base.Init();
		container = inventoryGloryTemplate.transform.parent;
		classList = new List<UIClassGloryItem>();
		inventoryClassPool = new ObjectPool<GameObject>(inventoryGloryTemplate);
		inventoryGloryTemplate.SetActive(value: false);
		foreach (UIClassGloryItem @class in classList)
		{
			inventoryClassPool.Release(@class.gameObject);
		}
		classList.Clear();
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
			foreach (CombatClass item in list)
			{
				if (item.ID != 0 && IsVisibleInStore(item))
				{
					GameObject obj = inventoryClassPool.Get();
					obj.transform.SetParent(container, worldPositionStays: false);
					obj.SetActive(value: true);
					UIClassGloryItem component = obj.GetComponent<UIClassGloryItem>();
					component.Init(item);
					if (item.ToCharClass().ClassID == xcharClass.ClassID)
					{
						selectedItem = component;
					}
					classList.Add(component);
				}
			}
		}
		updateClass();
	}

	private void OnEnable()
	{
		UpdatePvPRecords();
	}

	private void OnDisable()
	{
	}

	private void UpdatePvPRecords()
	{
		GloryLevel.text = Session.MyPlayerData.GloryLevel.ToString();
		Wins.text = Session.MyPlayerData.pvpPlayerRecords.WinsTotal.ToString();
		Losses.text = Session.MyPlayerData.pvpPlayerRecords.LossesTotal.ToString();
		Kills.text = Session.MyPlayerData.pvpPlayerRecords.KillsTotal.ToString();
		Deaths.text = Session.MyPlayerData.pvpPlayerRecords.DeathsTotal.ToString();
		Assists.text = Session.MyPlayerData.pvpPlayerRecords.AssistsTotal.ToString();
		ObjTaken.text = Session.MyPlayerData.pvpPlayerRecords.ObjectivesTakenTotal.ToString();
		ObjDef.text = Session.MyPlayerData.pvpPlayerRecords.ObjectivesDefendedTotal.ToString();
	}

	public void JoinQueue()
	{
		if (Session.MyPlayerData.showNotification && Session.MyPlayerData.isPvp)
		{
			AEC.getInstance().sendRequest(new RequestLeaveQueue());
		}
		else
		{
			AEC.getInstance().sendRequest(new RequestJoinQueue(1));
		}
	}

	public void UpdateQueueDisplay()
	{
		if (Session.MyPlayerData.showNotification && Session.MyPlayerData.isPvp)
		{
			queueLabel.text = "Leave Queue";
			queueCount.gameObject.SetActive(value: true);
		}
		else
		{
			queueLabel.text = "Join Queue";
			queueCount.gameObject.SetActive(value: false);
		}
	}

	public void GoToLobby()
	{
		Game.Instance.SendAreaJoinRequest(566);
		Close();
	}

	private void updateClass()
	{
		foreach (UIClassGloryItem @class in classList)
		{
			@class.Refresh();
		}
	}

	public bool IsVisibleInStore(CombatClass c)
	{
		if (!Session.MyPlayerData.IsClassAvailable(c))
		{
			return !c.HideUnavailable;
		}
		return true;
	}
}

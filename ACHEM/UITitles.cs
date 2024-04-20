using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITitles : MonoBehaviour
{
	private static Color32 Default = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private static Color32 Selected = new Color32(166, 166, 166, byte.MaxValue);

	public UIScrollView scrollView;

	public Transform Title;

	public GameObject HeadingCell;

	public GameObject TitleCell;

	public GameObject LockedCell;

	private UITitleClick currentSelection;

	public UILabel LabelDesc;

	public UILabel LabelName;

	private List<UITitleClick> titles;

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		Entities.Instance.me.TitleUpdated += OnTitleUpdated;
		scrollView.ResetPosition();
	}

	private void OnTitleUpdated(int badgeID, string title)
	{
		foreach (UITitleClick title2 in titles)
		{
			title2.CheckMark.SetActive(title2.badge.ID == badgeID);
		}
	}

	private void OnDisable()
	{
		Entities.Instance.me.TitleUpdated -= OnTitleUpdated;
	}

	private void Init()
	{
		TitleCell.SetActive(value: false);
		LockedCell.SetActive(value: false);
		if (!Badges.IsLoaded)
		{
			Badges.BadgesLoaded += BadgeLoaded;
			Game.Instance.SendEntityLoadBadgesRequest();
		}
		else
		{
			Refresh();
		}
	}

	private void BadgeLoaded()
	{
		Badges.BadgesLoaded -= BadgeLoaded;
		Refresh();
	}

	public void Refresh()
	{
		titles = new List<UITitleClick>();
		IOrderedEnumerable<Badge> orderedEnumerable = (from b in Badges.map.Values.Where(IsBadgeVisible)
			orderby b.Category.SortOrder
			select b).ThenByDescending(Session.MyPlayerData.HasBadge).ThenBy((Badge b) => b.Title);
		int num = 0;
		foreach (Badge item in orderedEnumerable)
		{
			if (num != item.TitleCategoryID)
			{
				num = item.TitleCategoryID;
				GameObject obj = Object.Instantiate(HeadingCell);
				obj.GetComponent<UITableHeading>().Init(item.Category.Name);
				obj.transform.SetParent(Title, worldPositionStays: false);
				obj.SetActive(value: true);
			}
			GameObject gameObject = (Session.MyPlayerData.HasBadge(item) ? Object.Instantiate(TitleCell) : Object.Instantiate(LockedCell));
			gameObject.transform.SetParent(Title, worldPositionStays: false);
			gameObject.SetActive(value: true);
			UITitleClick component = gameObject.GetComponent<UITitleClick>();
			component.badge = item;
			component.UITitles = this;
			string text = (Session.MyPlayerData.HasBadge(item) ? "[FFFFFF]" : "[B4B4B4]");
			component.Title.text = text + item.Title + "[-]";
			if (Entities.Instance.me.Title == item.ID)
			{
				component.CheckMark.SetActive(value: true);
				SetTitle(component);
			}
			if (!Session.MyPlayerData.HasBadge(item))
			{
				gameObject.GetComponent<UIButton>().enabled = false;
				component.Lock.SetActive(value: true);
			}
			component.ShowNotif();
			titles.Add(component);
		}
		Title.GetComponent<UITable>().Reposition();
	}

	private bool IsBadgeVisible(Badge badge)
	{
		if (badge.HideWhenLocked)
		{
			return Session.MyPlayerData.HasBadge(badge);
		}
		return true;
	}

	public void SetTitle(UITitleClick newSelection)
	{
		if (currentSelection != null)
		{
			currentSelection.BG.color = Default;
		}
		currentSelection = newSelection;
		currentSelection.BG.color = Selected;
		LabelName.text = newSelection.badge.Name;
		LabelDesc.text = newSelection.badge.Description;
	}

	public void Save()
	{
		if (Session.MyPlayerData.HasBadge(currentSelection.badge))
		{
			Game.Instance.SendEntityTitleUpdateRequest(currentSelection.badge.ID);
		}
		else
		{
			Notification.ShowText("Title has not been unlocked yet");
		}
	}
}

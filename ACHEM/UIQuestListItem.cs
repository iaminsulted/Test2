using System;
using UnityEngine;

public class UIQuestListItem : MonoBehaviour
{
	public Quest quest;

	public UISprite Background;

	public UISprite Lock;

	public UISprite SprComplete;

	public UISprite SprAvailable;

	public UILabel lblName;

	public UILabel lblZone;

	public GameObject TrackedIcon;

	public GameObject TrackedHighlight;

	private bool isSelected;

	private bool isEnabled;

	public bool Selected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (value != isSelected)
			{
				isSelected = value;
			}
		}
	}

	public event Action<UIQuestListItem> Clicked;

	public void Init(Quest quest)
	{
		this.quest = quest;
		Refresh();
	}

	private void Refresh()
	{
		if (quest != null)
		{
			lblName.text = quest.DisplayName;
			lblName.color = quest.DisplayColor;
			lblZone.text = quest.TargetMapName;
			lblZone.color = quest.DisplayColor.SetAlpha(0.8f);
			SprComplete.gameObject.SetActive(Session.MyPlayerData.IsQuestComplete(quest.ID));
			SprAvailable.gameObject.SetActive(Session.MyPlayerData.IsQuestAcceptable(quest) && !Session.MyPlayerData.HasQuest(quest.ID));
			Lock.gameObject.SetActive(!Session.MyPlayerData.IsQuestAcceptable(quest));
			bool active = Session.MyPlayerData.CurrentlyTrackedQuest == quest;
			TrackedIcon.SetActive(active);
			TrackedHighlight.SetActive(active);
		}
	}

	private void OnEnable()
	{
		Session.MyPlayerData.QuestStateUpdated += OnQuestStateUpdated;
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated += OnCurrentlyTrackedQuestUpdated;
		Refresh();
	}

	private void OnQuestStateUpdated()
	{
		Refresh();
	}

	private void OnCurrentlyTrackedQuestUpdated(Quest trackedQuest)
	{
		bool active = trackedQuest == quest;
		TrackedIcon.SetActive(active);
		TrackedHighlight.SetActive(active);
	}

	public void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestStateUpdated -= OnQuestStateUpdated;
			Session.MyPlayerData.CurrentlyTrackedQuestUpdated -= OnCurrentlyTrackedQuestUpdated;
		}
	}

	private void OnItemDestroyed()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnClick()
	{
		OnClicked();
	}

	protected void OnClicked()
	{
		this.Clicked?.Invoke(this);
	}
}

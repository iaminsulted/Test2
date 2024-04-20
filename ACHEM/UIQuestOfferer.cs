using System;
using System.Collections;
using UnityEngine;

public class UIQuestOfferer : MonoBehaviour
{
	public UILabel LabelQuestAvailable;

	public UILabel LabelQuestName;

	public UISprite SpriteDivider;

	public UISprite SpriteContainer;

	public UISprite SpriteOpenClose;

	public UISprite SpriteScrollIcon;

	public UILabel LabelQuestScroll;

	public UISprite SpriteLockedIcon;

	public UIButton OpenCloseButton;

	public UIButton SpriteContainerButton;

	private Quest available;

	private bool isMinimized;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
				OpenCloseButton.gameObject.SetActive(visible);
			}
		}
	}

	private void Start()
	{
	}

	public void Init()
	{
		Visible = false;
		UpdateOfferer();
	}

	public void OnEnable()
	{
		available = Session.MyPlayerData.AvailableQuest;
		Entities.Instance.me.LevelUpdated += OnLevelUpdated;
	}

	private void OnLevelUpdated()
	{
		UpdateOfferer();
	}

	public void OnDisable()
	{
		available = null;
		Entities.Instance.me.LevelUpdated -= OnLevelUpdated;
	}

	public void ShowQuest(Quest quest)
	{
		if (available != quest)
		{
			available = quest;
			if (available == null)
			{
				Visible = false;
				return;
			}
			Visible = true;
			UpdateOfferer();
		}
	}

	private void UpdateOfferer()
	{
		if (available != null)
		{
			LabelQuestName.text = available.Name;
			if (Session.MyPlayerData.HasQuest(available.ID))
			{
				LabelQuestAvailable.text = "Continue Quest...";
				SpriteScrollIcon.gameObject.SetActive(value: true);
				LabelQuestScroll.gameObject.SetActive(value: false);
				SpriteLockedIcon.gameObject.SetActive(value: false);
			}
			else if (Session.MyPlayerData.IsQuestAcceptable(available))
			{
				LabelQuestAvailable.text = "Quest Available";
				SpriteScrollIcon.gameObject.SetActive(value: true);
				LabelQuestScroll.gameObject.SetActive(value: true);
				SpriteLockedIcon.gameObject.SetActive(value: false);
			}
			else
			{
				LabelQuestAvailable.text = "Quest (Level " + available.RequiredLevel + ")";
				SpriteScrollIcon.gameObject.SetActive(value: false);
				SpriteLockedIcon.gameObject.SetActive(value: true);
			}
		}
	}

	public void OnClick()
	{
		UIQuest.ShowLog(available);
	}

	public void OnToggleTab()
	{
		StartCoroutine(ToggleTab());
	}

	private IEnumerator ToggleTab()
	{
		if (!isMinimized)
		{
			SpriteDivider.gameObject.SetActive(value: false);
			LabelQuestAvailable.gameObject.SetActive(value: false);
			LabelQuestName.gameObject.SetActive(value: false);
		}
		OpenCloseButton.isEnabled = false;
		SpriteContainerButton.isEnabled = false;
		SpriteScrollIcon.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
		SpriteLockedIcon.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
		int startWidth = (isMinimized ? 105 : 330);
		int endWidth = (isMinimized ? 330 : 105);
		float startRotation = (isMinimized ? 180 : 0);
		float endRotation = ((!isMinimized) ? 180 : 0);
		float time = 0.2f;
		float timeElapsed = 0f;
		Quaternion rotation;
		while (timeElapsed < time)
		{
			timeElapsed += Time.deltaTime;
			float num = timeElapsed / time;
			num = Mathf.Cos((num + 1f) * MathF.PI) / 2f + 0.5f;
			SpriteContainer.width = (int)Mathf.LerpUnclamped(startWidth, endWidth, num);
			float z = Mathf.LerpUnclamped(startRotation, endRotation, num);
			rotation = Quaternion.Euler(0f, 0f, z);
			OpenCloseButton.transform.rotation = rotation;
			yield return new WaitForEndOfFrame();
		}
		OpenCloseButton.isEnabled = true;
		SpriteContainerButton.isEnabled = true;
		SpriteContainer.width = endWidth;
		rotation = Quaternion.Euler(0f, 0f, endRotation);
		OpenCloseButton.transform.rotation = rotation;
		SpriteScrollIcon.updateAnchors = UIRect.AnchorUpdate.OnEnable;
		SpriteScrollIcon.UpdateAnchors();
		SpriteLockedIcon.updateAnchors = UIRect.AnchorUpdate.OnEnable;
		SpriteLockedIcon.UpdateAnchors();
		if (isMinimized)
		{
			SpriteDivider.gameObject.SetActive(value: true);
			LabelQuestAvailable.gameObject.SetActive(value: true);
			LabelQuestName.gameObject.SetActive(value: true);
		}
		isMinimized = !isMinimized;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHouseDataSortButton : MonoBehaviour
{
	[Serializable]
	public class SortButtonEntry
	{
		public HouseDataCategory SortType;

		public string ButtonLabel;

		public UISprite Sprite;
	}

	private int currentState;

	private int maxState;

	public UIButton button;

	public UILabel labelRef;

	public UISprite highlight;

	public Action onResetAll;

	public Action<HouseDataCategory, bool> onClick;

	public List<SortButtonEntry> entries = new List<SortButtonEntry>();

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(button.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSortClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(button.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSortClicked));
	}

	public void Init()
	{
		maxState = entries.Count;
		refresh(currentState);
	}

	public void SetButtonState(int state)
	{
		currentState = state;
		refresh(currentState);
	}

	public void ResetButtonState()
	{
		currentState = 0;
		refresh(currentState);
	}

	private void refresh(int currentState)
	{
		foreach (SortButtonEntry entry in entries)
		{
			if (entry.Sprite != null)
			{
				entry.Sprite.color = Color.gray;
			}
		}
		if (currentState == 0)
		{
			highlight.gameObject.SetActive(value: false);
			if (labelRef != null)
			{
				labelRef.text = entries[0].ButtonLabel;
			}
			return;
		}
		highlight.gameObject.SetActive(value: true);
		if (labelRef != null)
		{
			labelRef.text = entries[currentState - 1].ButtonLabel;
		}
		if (entries[currentState - 1].Sprite != null)
		{
			entries[currentState - 1].Sprite.color = Color.white;
		}
	}

	private void OnSortClicked(GameObject go)
	{
		currentState++;
		if (currentState > entries.Count)
		{
			currentState = 1;
		}
		int num = currentState;
		onResetAll?.Invoke();
		currentState = num;
		refresh(currentState);
		onClick?.Invoke(entries[currentState - 1].SortType, currentState == 2);
	}
}

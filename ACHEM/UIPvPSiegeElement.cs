using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPvPSiegeElement : MonoBehaviour
{
	public Color ActiveColor;

	public Color InactiveColor;

	public List<UIPvPSiegeElement> parents;

	public NPCSpawn spawn;

	public UISprite Sprite;

	public UISprite Highlight;

	public byte previousState;

	private Coroutine flashRoutine;

	private UIPvPSiege siegeUI;

	public bool IsUnLocked
	{
		get
		{
			if (parents.Count == 0)
			{
				return true;
			}
			foreach (UIPvPSiegeElement parent in parents)
			{
				if (parent.spawn.State == 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	public event Action StateUpdated;

	public void Init(UIPvPSiege mainUI)
	{
		siegeUI = mainUI;
	}

	public void OnEnable()
	{
		previousState = 0;
		spawn.StateUpdated += OnSpawnStateUpdated;
		foreach (UIPvPSiegeElement parent in parents)
		{
			parent.spawn.StateUpdated += OnSpawnStateUpdated;
		}
		RefershSpawnState();
	}

	public void OnDisable()
	{
		spawn.StateUpdated -= OnSpawnStateUpdated;
		foreach (UIPvPSiegeElement parent in parents)
		{
			parent.spawn.StateUpdated -= OnSpawnStateUpdated;
		}
	}

	public void Update()
	{
	}

	private void OnSpawnStateUpdated(byte state)
	{
		RefershSpawnState();
	}

	private void RefershSpawnState()
	{
		if (previousState != spawn.State)
		{
			Flash();
		}
		UISprite sprite = Sprite;
		int width = (Sprite.height = 13);
		sprite.width = width;
		if (spawn.State == 0)
		{
			Sprite.color = InactiveColor;
			Highlight.gameObject.SetActive(value: false);
		}
		else
		{
			Sprite.color = ActiveColor;
			if (IsUnLocked)
			{
				UISprite sprite2 = Sprite;
				width = (Sprite.height = 14);
				sprite2.width = width;
				Highlight.gameObject.SetActive(value: false);
			}
			else
			{
				Highlight.gameObject.SetActive(value: true);
			}
		}
		previousState = spawn.State;
		if (this.StateUpdated != null)
		{
			this.StateUpdated();
		}
	}

	private void Flash()
	{
		if (flashRoutine != null)
		{
			StopCoroutine(flashRoutine);
		}
		flashRoutine = StartCoroutine(FlashRoutine());
	}

	private IEnumerator FlashRoutine()
	{
		float duration = siegeUI.flashDuration;
		UISprite backgroundSprite = siegeUI.spriteBackground.GetComponent<UISprite>();
		float t = 0f;
		for (int i = 0; i < siegeUI.numberFlashes; i++)
		{
			while (t < duration)
			{
				t += Time.deltaTime;
				backgroundSprite.color = Color.Lerp(siegeUI.flashColor, siegeUI.baseColor, t / duration);
				yield return null;
			}
			t = 0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				backgroundSprite.color = Color.Lerp(siegeUI.baseColor, siegeUI.flashColor, t / duration);
				yield return null;
			}
		}
		flashRoutine = null;
	}
}

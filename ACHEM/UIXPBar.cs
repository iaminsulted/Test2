using System;
using System.Collections.Generic;
using UnityEngine;

public class UIXPBar : MonoBehaviour
{
	public UILabel Text;

	public UILabel BarName;

	public UIProgressBar Slider;

	public UISliderAnimation UISliderAnimation;

	public UISprite GreyInnerBar;

	public UISprite DividerSpritesPrefab;

	public ParticleAccentOnBar ParticleAccentOnBar;

	public UISprite xpBarColor;

	public UIWidget Container;

	private List<UISprite> DividerUISprites = new List<UISprite>();

	private Vector2 viewSize;

	private int numberOfIndexes = 10;

	private void Awake()
	{
		for (int i = 0; i < numberOfIndexes - 1; i++)
		{
			UISprite item = UnityEngine.Object.Instantiate(DividerSpritesPrefab, new Vector2(0f, 0f), Quaternion.identity, base.transform);
			DividerUISprites.Add(item);
		}
		UpdateBar(Session.MyPlayerData.XP, Session.MyPlayerData.XPToLevel, animate: false);
	}

	private void OnEnable()
	{
		Session.MyPlayerData.XPUpdated += OnXPUpdated;
		UIWidget container = Container;
		container.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(container.onChange, new UIWidget.OnDimensionsChanged(ResizeXPBar));
	}

	private void OnDisable()
	{
		Session.MyPlayerData.XPUpdated -= OnXPUpdated;
		UIWidget container = Container;
		container.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(container.onChange, new UIWidget.OnDimensionsChanged(ResizeXPBar));
	}

	private void OnXPUpdated(int xp, int xptolevel)
	{
		UpdateBar(xp, xptolevel);
	}

	private void ResizeXPBar()
	{
		viewSize = Container.localSize;
		int num = (int)viewSize.x;
		Slider.backgroundWidget.width = num;
		Slider.foregroundWidget.width = num - 2;
		GreyInnerBar.width = num;
		float num2 = viewSize.x / (float)numberOfIndexes;
		for (int i = 0; i < DividerUISprites.Count; i++)
		{
			DividerUISprites[i].transform.localPosition = new Vector2((float)(-(i + 1)) * num2, 7f);
		}
	}

	private void UpdateBar(int xp, int xptolevel, bool animate = true)
	{
		float num = 0f;
		if (xptolevel > 0)
		{
			num = (float)xp / (float)xptolevel;
		}
		if (Entities.Instance.me.Level == Session.MyPlayerData.LevelCap)
		{
			Text.text = (num * 100f).ToString("0.0") + "%";
			BarName.text = "Level " + Entities.Instance.me.Level + " (MAX)";
			xpBarColor.color = Color.Lerp(Color.yellow, Color.yellow, 0.4f);
			xpBarColor.gradientTop = Color.Lerp(Color.white, Color.white, 0.4f);
			xpBarColor.gradientBottom = Color.Lerp(Color.yellow, Color.black, 0.6f);
		}
		else
		{
			Text.text = (num * 100f).ToString("0.0") + "%";
			BarName.text = "Level " + Entities.Instance.me.Level;
		}
		if (animate)
		{
			UISliderAnimation.Animate(Slider, num, 0.5f, (int)viewSize.x, BarGoesLeft: true, ParticleAccentOnBar);
		}
		else
		{
			Slider.value = num;
		}
	}
}

using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class UIRankXPBar : MonoBehaviour
{
	public UILabel Text;

	public UILabel BarName;

	public UIProgressBar Slider;

	public UISliderAnimation UISliderAnimation;

	public UISprite GreyInnerBar;

	public UIWidget Container;

	public ParticleAccentOnBar ParticleAccentOnBar;

	private Vector2 viewSize;

	private float FillEndAmount;

	public UISprite DividerSpritesPrefab;

	private List<UISprite> DividerUISprites = new List<UISprite>();

	private int numberOfIndexes = 10;

	private void Awake()
	{
		for (int i = 0; i < numberOfIndexes - 1; i++)
		{
			UISprite item = UnityEngine.Object.Instantiate(DividerSpritesPrefab, new Vector2(0f, 0f), Quaternion.identity, base.transform);
			DividerUISprites.Add(item);
		}
		UpdateBar(Session.MyPlayerData.CurrentClass.ClassID, Session.MyPlayerData.EquippedClassXP, animate: false);
	}

	private void OnEnable()
	{
		Session.MyPlayerData.ClassXPUpdated += OnClassXPUpdated;
		Session.MyPlayerData.ClassEquipped += ClassEquipped;
		UIWidget container = Container;
		container.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(container.onChange, new UIWidget.OnDimensionsChanged(ResizeXPBar));
	}

	private void OnDisable()
	{
		Session.MyPlayerData.ClassXPUpdated -= OnClassXPUpdated;
		Session.MyPlayerData.ClassEquipped -= ClassEquipped;
		UIWidget container = Container;
		container.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(container.onChange, new UIWidget.OnDimensionsChanged(ResizeXPBar));
	}

	private void OnClassXPUpdated(int classID, int classXP)
	{
		if (classID == Session.MyPlayerData.EquippedClassID)
		{
			UpdateBar(classID, classXP);
		}
	}

	private void ClassEquipped(int id)
	{
		UpdateBar(id, Session.MyPlayerData.EquippedClassXP, animate: false);
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
			DividerUISprites[i].transform.localPosition = new Vector2((float)(i + 1) * num2, 7f);
		}
	}

	private void UpdateBar(int classID, int classXP, bool animate = true)
	{
		int equippedClassRank = Session.MyPlayerData.EquippedClassRank;
		_ = CombatClass.GetClassByID(classID).ClassTier;
		FillEndAmount = ClassRanks.GetRankXPPercent(classXP, equippedClassRank);
		if (equippedClassRank >= 100)
		{
			BarName.text = "";
			Slider.value = 1f;
			FillEndAmount = 1f;
			Text.text = "MAX";
		}
		else
		{
			BarName.text = Entities.Instance.me.CombatClass.Name + " Rank " + equippedClassRank;
			Text.text = (FillEndAmount * 100f).ToString("0.0") + "%";
		}
		if (animate)
		{
			UISliderAnimation.Animate(Slider, FillEndAmount, 0.5f, (int)viewSize.x, BarGoesLeft: false, ParticleAccentOnBar);
		}
		else
		{
			Slider.value = FillEndAmount;
		}
	}
}

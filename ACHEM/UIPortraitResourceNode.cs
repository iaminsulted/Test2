using System;
using StatCurves;
using UnityEngine;

public class UIPortraitResourceNode : MonoBehaviour
{
	public GameObject root;

	public UILabel labelPower;

	public UILabel labelUsages;

	public UISprite spriteIcon;

	public UISlider sliderDeduction;

	public UISlider sliderUsages;

	public Action<bool> VisibleUpdated;

	private ResourceNode resourceNode;

	private int usagesTotal;

	public void Awake()
	{
		root.SetActive(value: false);
	}

	public void Init(ResourceNode resourceNode)
	{
		root.SetActive(value: true);
		VisibleUpdated?.Invoke(obj: true);
		this.resourceNode = resourceNode;
		usagesTotal = resourceNode.usagesTotal;
		UpdateResourceNodeLevel();
		UpdateTradeSkillIcon();
		ResourceNode obj = this.resourceNode;
		obj.UsageUpdate = (Action)Delegate.Combine(obj.UsageUpdate, new Action(OnUsageUpdate));
		OnUsageUpdate();
	}

	public void Close()
	{
		root.SetActive(value: false);
		if (resourceNode != null)
		{
			ResourceNode obj = resourceNode;
			obj.UsageUpdate = (Action)Delegate.Remove(obj.UsageUpdate, new Action(OnUsageUpdate));
		}
		resourceNode = null;
		VisibleUpdated?.Invoke(obj: false);
	}

	private void OnUsageUpdate()
	{
		if (root.activeSelf)
		{
			string text = "";
			switch (resourceNode.tradeSkill)
			{
			case TradeSkillType.Fishing:
				text = "Fish";
				break;
			case TradeSkillType.Mining:
				text = "Ore";
				break;
			}
			labelUsages.text = resourceNode.usages + " / " + usagesTotal + " " + text;
			sliderUsages.value = (float)resourceNode.usages / (float)usagesTotal;
			sliderDeduction.value = (float)resourceNode.usages / (float)usagesTotal;
		}
	}

	private void UpdateResourceNodeLevel()
	{
		labelPower.text = resourceNode.power.ToString();
	}

	private void UpdateTradeSkillIcon()
	{
		spriteIcon.spriteName = "Icon-Class-" + Enum.GetName(typeof(TradeSkillType), resourceNode.tradeSkill);
	}
}

using System;
using StatCurves;
using UnityEngine;

public class TradeSkillUp : MonoBehaviour
{
	public Animator Anime;

	public UILabel labelDescription;

	public UILabel labelSkillLevel;

	public UILabel labelSkillUP;

	public UISprite spriteIcon;

	public static void Show(TradeSkillType type)
	{
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/TradeSkillUp"), UIManager.Instance.transform).GetComponentInChildren<TradeSkillUp>().Init(type);
	}

	private void Init(TradeSkillType type)
	{
		string text = Enum.GetName(typeof(TradeSkillType), type);
		labelSkillUP.text = text + " UP!";
		labelSkillLevel.text = text + " Level Up";
		switch (type)
		{
		case TradeSkillType.Fishing:
			spriteIcon.spriteName = "Icon-FishingRod";
			break;
		case TradeSkillType.Mining:
			spriteIcon.spriteName = "Icon-FishingRod";
			break;
		}
	}

	private void Remove()
	{
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
	}

	private void ActivateParticles()
	{
	}
}

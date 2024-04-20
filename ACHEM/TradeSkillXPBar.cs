using System;
using System.Collections;
using StatCurves;
using UnityEngine;

public class TradeSkillXPBar : MonoBehaviour
{
	public static TradeSkillXPBar Instance;

	public UILabel Label;

	public UIProgressBar UIProgressBar;

	public UISprite Fill;

	private bool isActive;

	private bool isHiding;

	private bool isLevelUp;

	private bool isShowingLevelUp;

	private float elapsed;

	private float current;

	private int end;

	private int start;

	private int levelEnd;

	private TradeSkillType tradeSkillType;

	private string tradeSkillName;

	private void Awake()
	{
		Instance = this;
		Instance.UIProgressBar.fillDirection = UIProgressBar.FillDirection.LeftToRight;
		Hide();
	}

	private void Update()
	{
		if (!isActive || isHiding || isShowingLevelUp)
		{
			return;
		}
		elapsed += Time.deltaTime;
		current = Mathf.Lerp(start, end, elapsed);
		UIProgressBar.Set(current / (float)levelEnd);
		Label.text = tradeSkillName + " XP: " + (int)current + " / " + levelEnd;
		if ((int)current == end)
		{
			if (isLevelUp)
			{
				StartCoroutine(ShowLevelUp());
				elapsed = 0f;
				start = 0;
				current = 0f;
				end = Session.MyPlayerData.tradeSkillXP[tradeSkillType];
				levelEnd = Session.MyPlayerData.tradeSkillXPToLevel[tradeSkillType];
				isLevelUp = false;
			}
			else
			{
				StartCoroutine(HideUI());
			}
		}
	}

	public void Show(TradeSkillType tradeSkillType, int xpGained, int xpFinal)
	{
		elapsed = 0f;
		this.tradeSkillType = tradeSkillType;
		start = Session.MyPlayerData.tradeSkillXP[tradeSkillType];
		current = start;
		levelEnd = Session.MyPlayerData.tradeSkillXPToLevel[tradeSkillType];
		tradeSkillName = Enum.GetName(typeof(TradeSkillType), tradeSkillType);
		if (xpGained > xpFinal)
		{
			end = levelEnd;
			isLevelUp = true;
		}
		else
		{
			end = start + xpGained;
			isLevelUp = false;
		}
		UIProgressBar.Set((float)start / (float)levelEnd);
		UIProgressBar.gameObject.SetActive(value: true);
		isActive = true;
	}

	private IEnumerator ShowLevelUp()
	{
		isShowingLevelUp = true;
		Label.text = "Level up!";
		yield return new WaitForSeconds(1f);
		isShowingLevelUp = false;
	}

	private IEnumerator HideUI()
	{
		isHiding = true;
		yield return new WaitForSeconds(1f);
		isHiding = false;
		Hide();
	}

	private void Hide()
	{
		isActive = false;
		elapsed = 0f;
		current = 0f;
		UIProgressBar.gameObject.SetActive(value: false);
		UIProgressBar.Set(0f);
		Label.text = "";
	}
}

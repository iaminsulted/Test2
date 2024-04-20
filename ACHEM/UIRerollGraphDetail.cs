using System;
using System.Collections;
using UnityEngine;

public class UIRerollGraphDetail : MonoBehaviour
{
	public UISprite HealthBar;

	public UISprite HealthNegBar;

	public UISprite AttackBar;

	public UISprite AttackNegBar;

	public UISprite ArmorBar;

	public UISprite ArmorNegBar;

	public UISprite EvadeBar;

	public UISprite EvadeNegBar;

	public UISprite CritBar;

	public UISprite CritNegBar;

	public UISprite HasteBar;

	public UISprite HasteNegBar;

	private const int FullBar = 79;

	private const float FullMod = 1.2f;

	private const float speed = 4f;

	public UILabel HealthNumber;

	public UILabel AttackNumber;

	public UILabel ArmorNumber;

	public UILabel EvadeNumber;

	public UILabel CritNumber;

	public UILabel HasteNumber;

	public void DisplayStatBar(float modDiffValue, int stat, float numericalStatValue)
	{
		string text = InterfaceColors.Stats.Red.ToBBCode() + "-";
		bool num = modDiffValue < 0f;
		int num2 = 1;
		if (num)
		{
			text = InterfaceColors.Stats.Green.ToBBCode() + "+";
			num2 = -1;
		}
		modDiffValue *= (float)num2;
		modDiffValue = modDiffValue / 1.2f * 79f;
		numericalStatValue = Mathf.Abs(numericalStatValue);
		switch (stat)
		{
		case 1:
			HealthNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, HealthBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, HealthNegBar));
			break;
		case 2:
			AttackNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, AttackBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, AttackNegBar));
			break;
		case 3:
			ArmorNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, ArmorBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, ArmorNegBar));
			break;
		case 4:
			EvadeNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, EvadeBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, EvadeNegBar));
			break;
		case 5:
			CritNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, CritBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, CritNegBar));
			break;
		case 6:
			HasteNumber.text = text + numericalStatValue;
			StartCoroutine(moveBar((float)(-num2) * modDiffValue, HasteBar));
			StartCoroutine(moveBar((float)num2 * modDiffValue, HasteNegBar));
			break;
		}
	}

	private IEnumerator moveBar(float value, UISprite bar)
	{
		_ = bar.width;
		if ((float)bar.width > value)
		{
			while ((float)bar.width > value)
			{
				bar.width = (int)Math.Floor(Mathf.Lerp(value, bar.width, Time.deltaTime * 4f));
				if (bar.width < 5)
				{
					bar.gameObject.SetActive(value: false);
					break;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}
		else
		{
			bar.gameObject.SetActive(value: true);
			while ((float)bar.width < value)
			{
				bar.width = (int)Math.Ceiling(Mathf.Lerp(bar.width, value, Time.deltaTime * 4f));
				yield return new WaitForSeconds(0.01f);
			}
		}
	}
}

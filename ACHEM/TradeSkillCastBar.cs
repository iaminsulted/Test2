using StatCurves;
using UnityEngine;

public class TradeSkillCastBar : MonoBehaviour
{
	public static TradeSkillCastBar Instance;

	public UILabel Label;

	public UIProgressBar UIProgressBar;

	public UISprite Fill;

	private float countdown;

	private float start;

	private bool isActive;

	private void Awake()
	{
		Instance = this;
		Instance.UIProgressBar.fillDirection = UIProgressBar.FillDirection.LeftToRight;
		Hide();
	}

	private void Update()
	{
		if (isActive)
		{
			countdown -= Time.deltaTime;
			UIProgressBar.Set(countdown / start);
			if (countdown <= 0f)
			{
				Hide();
			}
		}
	}

	public void Show(TradeSkillType tradeSkill, float time)
	{
		switch (tradeSkill)
		{
		case TradeSkillType.Fishing:
			Instance.Label.text = "Gone Fishin'!";
			break;
		case TradeSkillType.Mining:
			Instance.Label.text = "Mining Ore!";
			break;
		}
		start = time;
		countdown = time;
		UIProgressBar.gameObject.SetActive(value: true);
		UIProgressBar.Set(1f);
		isActive = true;
	}

	public void Hide()
	{
		isActive = false;
		UIProgressBar.Set(0f);
		UIProgressBar.gameObject.SetActive(value: false);
		countdown = 0f;
		start = 0f;
		Label.text = "";
	}
}

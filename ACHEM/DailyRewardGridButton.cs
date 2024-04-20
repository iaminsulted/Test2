using System;
using UnityEngine;

public class DailyRewardGridButton : MonoBehaviour
{
	public UILabel DayNumber;

	public UISprite Icon;

	public UIButton BtnClaim;

	public UISprite Glow;

	public UISprite XSprite;

	public UITweener TweenChest;

	private DateTime End;

	private void Awake()
	{
		TweenChest.enabled = false;
		DayNumber.gameObject.SetActive(value: true);
		BtnClaim.gameObject.SetActive(value: false);
	}

	public void SetDailyItem(string number, string spriteName)
	{
		DayNumber.text = "Day " + number;
		Icon.spriteName = spriteName;
		End = Session.MyPlayerData.RewardDate;
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnClaim.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClaimClick));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnClaim.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClaimClick));
	}

	public void OnClaimClick(GameObject go)
	{
		BtnClaim.isEnabled = false;
		Game.Instance.SendItemDailyRewardRequest();
	}

	public void StartTimer()
	{
		InvokeRepeating("UpdateSlider", 0f, 1f);
	}

	public void UpdateSlider()
	{
		TimeSpan timeSpan = End - GameTime.ServerTime;
		if (timeSpan.TotalSeconds <= 0.0)
		{
			TweenChest.enabled = true;
			DayNumber.gameObject.SetActive(value: false);
			BtnClaim.gameObject.SetActive(value: true);
			CancelInvoke("Tick");
			return;
		}
		DayNumber.text = "[FFC800]" + timeSpan.Hours + "h " + timeSpan.Minutes + "m " + timeSpan.Seconds + "s[-]";
	}
}

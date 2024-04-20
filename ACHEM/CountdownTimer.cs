using System;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
	public float TickDelay;

	public UILabel label;

	private DateTime endTime;

	private void Start()
	{
		InvokeRepeating("UpdateText", 0f, TickDelay);
	}

	private void UpdateText()
	{
		TimeSpan timeSpan = endTime - GameTime.ServerTime;
		label.text = "";
		if (timeSpan.Days > 0)
		{
			UILabel uILabel = label;
			uILabel.text = uILabel.text + timeSpan.Days + "d ";
		}
		UILabel uILabel2 = label;
		uILabel2.text = uILabel2.text + timeSpan.Hours + "h " + timeSpan.Minutes + "m " + timeSpan.Seconds + "s";
	}

	public void SetTime(float seconds)
	{
		endTime = DateTime.UtcNow.AddSeconds(seconds);
		UpdateText();
	}

	public void SetTime(DateTime endUTCTime)
	{
		endTime = endUTCTime;
	}
}

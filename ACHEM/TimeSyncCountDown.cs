using System;
using TMPro;
using UnityEngine;

public class TimeSyncCountDown : MonoBehaviour
{
	public float EventTime;

	public float CycleTime;

	public TextMeshProUGUI CountDownTimer;

	private float timeElapsed;

	private void OnEnable()
	{
		DateTime serverTime = GameTime.ServerTime;
		DateTime dateTime = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day);
		float num = (float)(serverTime - dateTime).TotalSeconds % CycleTime;
		_ = EventTime;
		timeElapsed = num;
		InvokeRepeating("CountDownFloat", 0f, 1f);
	}

	private void CountDownFloat()
	{
		if (timeElapsed < EventTime)
		{
			float num = EventTime - timeElapsed;
			CountDownTimer.text = (int)(num / 60f) + ":" + (((int)(num % 60f) >= 10) ? ((int)(num % 60f)).ToString() : ("0" + (int)(num % 60f)));
		}
		else if (timeElapsed >= EventTime && timeElapsed < CycleTime)
		{
			CountDownTimer.text = "--:--";
		}
		else
		{
			timeElapsed = 0f;
		}
		timeElapsed += 1f;
	}

	private void OnDisable()
	{
		CancelInvoke();
	}
}

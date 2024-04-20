using System;
using System.Collections;
using UnityEngine;

public class UIMergeTimer : MonoBehaviour
{
	public float TotalMinutes;

	public DateTime End;

	public UISlider Slider;

	public UILabel TimeLabel;

	public void Init(Merge ml)
	{
		TotalMinutes = ml.MergeMinutes;
		End = ml.TSComplete.Value;
		StartCoroutine(ProgressRoutine());
	}

	private IEnumerator ProgressRoutine()
	{
		TimeSpan ts = End - GameTime.ServerTime;
		while (ts.TotalSeconds > 0.0 && TotalMinutes != 0f)
		{
			ts = End - GameTime.ServerTime;
			Slider.value = (float)(((double)TotalMinutes - ts.TotalMinutes) / (double)TotalMinutes);
			TimeLabel.text = ts.Days + "d " + ts.Hours + "h " + ts.Minutes + "m " + ts.Seconds + "s";
			yield return null;
		}
		TimeLabel.text = "COMPLETE";
		Slider.value = 1f;
	}
}

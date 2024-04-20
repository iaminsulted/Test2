using System;
using System.Collections;
using UnityEngine;

public class UISpeedrunTimer : MonoBehaviour
{
	public GameObject root;

	public Transform target;

	public Transform origin;

	public UILabel description;

	public UILabel time;

	private float speedrunTime;

	private bool isPaused;

	private float lastTimeStamp;

	public void Awake()
	{
		Stop();
	}

	public void Update()
	{
		if (!isPaused)
		{
			speedrunTime += GameTime.realtimeSinceServerStartup - lastTimeStamp;
			lastTimeStamp = GameTime.realtimeSinceServerStartup;
			SetTimeText();
		}
	}

	public void Init(string description, float startTime, bool isPaused = false)
	{
		root.SetActive(value: true);
		lastTimeStamp = GameTime.realtimeSinceServerStartup;
		speedrunTime = 0f;
		this.description.text = description;
		this.isPaused = isPaused;
		SetTimeText();
		StartCoroutine(ShowSpeedrunTimer());
	}

	public void Pause()
	{
		isPaused = true;
	}

	public void Unpause()
	{
		isPaused = false;
		lastTimeStamp = GameTime.realtimeSinceServerStartup;
	}

	public void Stop()
	{
		root.SetActive(value: false);
		root.transform.parent = origin.parent;
		root.transform.localPosition = origin.localPosition;
		root.transform.localScale = Vector3.zero;
	}

	private void SetTimeText()
	{
		time.text = TimeSpan.FromSeconds(speedrunTime).ToString("hh\\:mm\\:ss");
	}

	private IEnumerator ShowSpeedrunTimer()
	{
		iTween.ScaleTo(root, origin.localScale, 1f);
		yield return new WaitForSeconds(2f);
		root.transform.parent = target.parent;
		iTween.MoveTo(root, iTween.Hash("position", target.localPosition, "islocal", true, "time", 1));
		iTween.ScaleTo(root, target.localScale, 1f);
	}
}

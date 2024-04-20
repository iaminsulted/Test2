using System;
using System.Collections;
using UnityEngine;

public class UICellTimer : MonoBehaviour
{
	public GameObject root;

	public Transform target;

	public Transform origin;

	public UILabel description;

	public UILabel time;

	private float timeRemaining;

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
			timeRemaining -= GameTime.realtimeSinceServerStartup - lastTimeStamp;
			lastTimeStamp = GameTime.realtimeSinceServerStartup;
			SetTimeText();
		}
	}

	public void Init(string description, float timeRemaining, bool isPaused = false)
	{
		root.SetActive(value: true);
		lastTimeStamp = GameTime.realtimeSinceServerStartup;
		this.timeRemaining = timeRemaining;
		this.description.text = description;
		this.isPaused = isPaused;
		SetTimeText();
		StartCoroutine(ShowCellTimer());
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
		time.text = TimeSpan.FromSeconds(timeRemaining).ToString("hh\\:mm\\:ss");
	}

	private IEnumerator ShowCellTimer()
	{
		iTween.ScaleTo(root, origin.localScale, 1f);
		yield return new WaitForSeconds(2f);
		root.transform.parent = target.parent;
		iTween.MoveTo(root, iTween.Hash("position", target.localPosition, "islocal", true, "time", 1));
		iTween.ScaleTo(root, target.localScale, 1f);
	}
}

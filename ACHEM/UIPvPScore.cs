using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPvPScore : MonoBehaviour
{
	public delegate void changedCaptureState(int captureID, int colorIndex);

	public delegate void captureProgressUI(int captureID, float progress, int team);

	public static UIPvPScore instance;

	public UILabel scoreFriendly;

	public UILabel scoreEnemy;

	public UISprite[] pointSprites;

	public UISprite[] pointBorderSprites;

	public UISlider[] captureProgressBars;

	public UISprite[] fillSprites;

	public UISprite[] siegeSprites;

	public Color[] captureColors;

	public Color[] captureColorsBorder;

	private Dictionary<int, List<float>> capturePointsCaptureProgress;

	public List<int> capStates;

	public changedCaptureState captureDel;

	public captureProgressUI captureProgressDel;

	public UILabel timeLabel;

	private float time;

	public UIPvPSiege siegeUI;

	public GameObject capturePointUI;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
			capStates = new List<int> { 0, 0, 0 };
			for (int i = 0; i < 100; i++)
			{
				capStates.Add(0);
			}
			UIPvPScore uIPvPScore = instance;
			uIPvPScore.captureProgressDel = (captureProgressUI)Delegate.Combine(uIPvPScore.captureProgressDel, new captureProgressUI(OnUpdateCaptureProgress));
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void Show()
	{
		if (Game.Instance.AreaData.PvpType == PvpType.Siege)
		{
			siegeUI.Init();
			siegeUI.gameObject.SetActive(value: true);
		}
		else
		{
			capturePointUI.SetActive(value: true);
		}
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		siegeUI.gameObject.SetActive(value: false);
		capturePointUI.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void updateCaptureUI(List<int> states)
	{
		capStates = states;
		if (states == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < states.Count - 1; i++)
		{
			num = 0;
			if (states[i] != 0)
			{
				num = ((states[i] == Entities.Instance.me.teamID) ? 1 : 2);
			}
			if (i < pointSprites.Length)
			{
				pointSprites[i].color = captureColors[num];
				pointBorderSprites[i].color = captureColorsBorder[num];
			}
			captureDel?.Invoke(i, num);
		}
	}

	public void updateCaptureProgressBar(int capID, List<float> vals)
	{
		if (capStates.Count <= 1)
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		int team = 0;
		for (int i = 0; i < vals.Count; i++)
		{
			if (vals[i] < 100f && vals[i] > num)
			{
				num = vals[i];
				num2 = i;
			}
		}
		if (capStates[capID - 1] == 0)
		{
			team = ((num2 == Entities.Instance.me.teamID) ? 1 : 2);
		}
		else if (capID < 4)
		{
			team = ((capStates[capID - 1] != Entities.Instance.me.teamID) ? 1 : 2);
		}
		captureProgressDel?.Invoke(capID, num / 100f, team);
	}

	public void OnUpdateCaptureProgress(int pointID, float progress, int teamID)
	{
		if (pointID <= 3)
		{
			if (progress > 0.01f && progress < 0.99f)
			{
				captureProgressBars[pointID - 1].gameObject.SetActive(value: true);
				captureProgressBars[pointID - 1].value = progress;
				fillSprites[pointID - 1].color = captureColors[teamID];
			}
			else
			{
				captureProgressBars[pointID - 1].gameObject.SetActive(value: false);
			}
		}
	}

	public void updateScore(List<int> vals)
	{
		scoreFriendly.text = "";
		scoreEnemy.text = "";
		for (int i = 0; i < vals.Count; i++)
		{
			if (i == 0)
			{
				if (Entities.Instance.me.teamID == 1)
				{
					UILabel uILabel = scoreFriendly;
					uILabel.text = uILabel.text + vals[0] + "\n";
				}
				else
				{
					UILabel uILabel2 = scoreFriendly;
					uILabel2.text = uILabel2.text + vals[1] + "\n";
				}
			}
			else if (Entities.Instance.me.teamID == 1)
			{
				UILabel uILabel3 = scoreEnemy;
				uILabel3.text = uILabel3.text + vals[1] + "\n";
			}
			else
			{
				UILabel uILabel4 = scoreEnemy;
				uILabel4.text = uILabel4.text + vals[0] + "\n";
			}
		}
	}

	public void startTimer(float startTime)
	{
		time = startTime;
		base.gameObject.SetActive(value: true);
		StopAllCoroutines();
		StartCoroutine(TickTimer());
	}

	public IEnumerator TickTimer()
	{
		while (time > 0f)
		{
			time -= 1f;
			updateTimer();
			yield return new WaitForSeconds(1f);
		}
	}

	public void updateTimer()
	{
		string text = Mathf.FloorToInt(time / 60f).ToString();
		int num = Mathf.FloorToInt(time % 60f);
		string text2 = "";
		if (time > 0f)
		{
			text2 = ((num >= 10) ? num.ToString() : ("0" + num));
		}
		else
		{
			text = "0";
			text2 = "00";
		}
		timeLabel.text = text + ":" + text2;
	}

	public void OnEnable()
	{
		UIWidget component = UIGame.Instance.PortraitTarget.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute - 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute - 45);
		component = UIGame.Instance.PortraitMob.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute - 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute - 45);
		component = UIGame.Instance.PortraitNPC.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute - 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute - 45);
		component = UIGame.Instance.PortraitBoss.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute - 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute - 45);
		component = UIGame.Instance.PortraitResourceNode.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute - 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute - 45);
		timeLabel.text = "10:00";
		for (int i = 0; i < pointSprites.Length; i++)
		{
			pointSprites[i].color = captureColors[0];
			pointBorderSprites[i].color = captureColorsBorder[0];
		}
		time = 600f;
		for (int j = 1; j < 4; j++)
		{
			OnUpdateCaptureProgress(j, 0f, 0);
		}
	}

	public void OnDisable()
	{
		UIWidget component = UIGame.Instance.PortraitTarget.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute + 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute + 45);
		component = UIGame.Instance.PortraitMob.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute + 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute + 45);
		component = UIGame.Instance.PortraitNPC.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute + 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute + 45);
		component = UIGame.Instance.PortraitBoss.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute + 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute + 45);
		component = UIGame.Instance.PortraitResourceNode.GetComponent<UIWidget>();
		component.bottomAnchor.Set(component.bottomAnchor.target, component.bottomAnchor.relative, component.bottomAnchor.absolute + 45);
		component.topAnchor.Set(component.topAnchor.target, component.topAnchor.relative, component.topAnchor.absolute + 45);
	}
}

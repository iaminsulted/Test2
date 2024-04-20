using System;
using UnityEngine;

internal class CapturePointUI : MonoBehaviour
{
	public Color[] colors;

	public Color[] colorsBorder;

	public Material[] mats;

	public Sprite[] sprites;

	public int capturePointID;

	public SpriteRenderer srIcon;

	public SpriteRenderer srCapColor;

	public SpriteRenderer progressBarForeground;

	public GameObject progressBarScaleHandle;

	public GameObject progressBarBackground;

	public GameObject zoneColor;

	public GameObject zoneColor2;

	public SpriteRenderer srIconBorder;

	private void Start()
	{
		UIPvPScore instance = UIPvPScore.instance;
		instance.captureDel = (UIPvPScore.changedCaptureState)Delegate.Combine(instance.captureDel, new UIPvPScore.changedCaptureState(OnUpdatedCapture));
		UIPvPScore instance2 = UIPvPScore.instance;
		instance2.captureProgressDel = (UIPvPScore.captureProgressUI)Delegate.Combine(instance2.captureProgressDel, new UIPvPScore.captureProgressUI(OnUpdatedCaptureProgress));
		OnUpdatedCaptureProgress(capturePointID, 0f, 1);
	}

	public void setCapturePointIcon(int pointID, bool passiveScoring)
	{
		capturePointID = pointID;
		if (!passiveScoring)
		{
			srIcon.gameObject.SetActive(value: true);
			srIcon.sprite = sprites[3];
		}
		else
		{
			srIcon.sprite = sprites[capturePointID - 1];
		}
	}

	private void OnUpdatedCapture(int pointID, int colorIndex)
	{
		if (pointID + 1 != capturePointID)
		{
			return;
		}
		if (srCapColor.color != colors[colorIndex])
		{
			switch (colorIndex)
			{
			case 1:
				AudioManager.Play2DSFX("SFX_PVP_Capture");
				break;
			case 2:
				AudioManager.Play2DSFX("SFX_PVP_ZoneLost");
				break;
			}
		}
		srCapColor.color = colors[colorIndex];
		srIconBorder.color = colorsBorder[colorIndex];
		zoneColor.GetComponent<Renderer>().material = mats[colorIndex];
		zoneColor2.GetComponent<Renderer>().material = mats[colorIndex];
	}

	private void OnUpdatedCaptureProgress(int pointID, float progress, int teamID)
	{
		if (capturePointID == pointID)
		{
			if (progress < 0.99f && progress > 0.01f)
			{
				progressBarBackground.SetActive(value: true);
				progressBarScaleHandle.SetActive(value: true);
				progressBarForeground.color = colors[teamID];
				progressBarScaleHandle.transform.localScale = new Vector3(progressBarBackground.transform.localScale.x * progress, progressBarBackground.transform.localScale.y, progressBarBackground.transform.localScale.z);
			}
			else
			{
				progressBarBackground.SetActive(value: false);
				progressBarScaleHandle.SetActive(value: false);
			}
		}
	}

	private void OnDestroy()
	{
		UIPvPScore instance = UIPvPScore.instance;
		instance.captureDel = (UIPvPScore.changedCaptureState)Delegate.Remove(instance.captureDel, new UIPvPScore.changedCaptureState(OnUpdatedCapture));
		UIPvPScore instance2 = UIPvPScore.instance;
		instance2.captureProgressDel = (UIPvPScore.captureProgressUI)Delegate.Remove(instance2.captureProgressDel, new UIPvPScore.captureProgressUI(OnUpdatedCaptureProgress));
	}
}

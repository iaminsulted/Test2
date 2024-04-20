using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector;

[CutsceneItem("Transitions", "Fade To Black", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CanvasFadeToBlack : CinemaGlobalAction
{
	private Color startColor = Color.clear;

	private Color endColor = Color.black;

	private GameObject canvasGO;

	private RectTransform canvas;

	private RectTransform image;

	private string prefabName = "Transition Canvas";

	private void Awake()
	{
		if (!base.transform.Find(prefabName + "(Clone)"))
		{
			canvasGO = Object.Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
		}
		else
		{
			canvasGO = base.transform.Find(prefabName + "(Clone)").gameObject;
		}
		canvasGO.transform.SetParent(base.transform);
		canvas = canvasGO.GetComponent<RectTransform>();
		image = canvasGO.transform.GetChild(0).GetComponent<RectTransform>();
		image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
		image.GetComponent<Image>().color = startColor;
		canvas.gameObject.SetActive(value: false);
	}

	public override void Trigger()
	{
		if ((bool)canvas && (bool)image)
		{
			canvas.gameObject.SetActive(value: true);
			image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
			image.GetComponent<Image>().color = startColor;
		}
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void End()
	{
		if ((bool)canvas)
		{
			canvas.gameObject.SetActive(value: false);
		}
	}

	public override void ReverseEnd()
	{
		if ((bool)canvas && (bool)image)
		{
			canvas.gameObject.SetActive(value: true);
			image.sizeDelta = new Vector2(canvas.rect.width, canvas.rect.height);
			image.GetComponent<Image>().color = endColor;
		}
	}

	public override void Stop()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		float transition = time / base.Duration;
		FadeToColor(startColor, endColor, transition);
	}

	public override void SetTime(float time, float deltaTime)
	{
		if ((bool)canvas)
		{
			if (time >= 0f && time <= base.Duration)
			{
				canvas.gameObject.SetActive(value: true);
				UpdateTime(time, deltaTime);
			}
			else if (image.gameObject.activeSelf)
			{
				canvas.gameObject.SetActive(value: false);
			}
		}
	}

	private void FadeToColor(Color start, Color end, float transition)
	{
		if ((bool)image)
		{
			image.GetComponent<Image>().color = Color.Lerp(start, end, transition);
		}
	}
}

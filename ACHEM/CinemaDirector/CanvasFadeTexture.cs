using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector;

[CutsceneItem("Transitions", "Fade Canvas Texture In-Out", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class CanvasFadeTexture : CinemaGlobalAction
{
	public Sprite sprite;

	public Color tint = Color.white;

	private GameObject canvasGO;

	private Image image;

	private string prefabName = "Transition Canvas";

	public void Setup()
	{
		if (!base.transform.Find(prefabName + "(Clone)"))
		{
			canvasGO = Object.Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
			canvasGO.transform.SetParent(base.transform);
			image = canvasGO.transform.GetChild(0).GetComponent<Image>();
			image.preserveAspect = true;
		}
		else
		{
			canvasGO = base.transform.Find(prefabName + "(Clone)").gameObject;
		}
		image = canvasGO.transform.GetChild(0).GetComponent<Image>();
		image.sprite = sprite;
		image.color = Color.clear;
		canvasGO.SetActive(value: false);
	}

	private void Awake()
	{
		Setup();
	}

	public override void Trigger()
	{
		Setup();
		if ((bool)canvasGO)
		{
			canvasGO.SetActive(value: true);
		}
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		if ((bool)canvasGO)
		{
			float num = time / base.Duration;
			if (num <= 0.25f)
			{
				FadeToColor(Color.clear, tint, num / 0.25f);
			}
			else if (num >= 0.75f)
			{
				FadeToColor(tint, Color.clear, (num - 0.75f) / 0.25f);
			}
		}
	}

	public override void SetTime(float time, float deltaTime)
	{
		if ((bool)canvasGO)
		{
			canvasGO.SetActive(value: true);
			if (time >= 0f && time <= base.Duration)
			{
				UpdateTime(time, deltaTime);
			}
			else if (canvasGO.activeSelf)
			{
				canvasGO.SetActive(value: false);
			}
		}
	}

	public override void End()
	{
		if ((bool)canvasGO)
		{
			canvasGO.SetActive(value: false);
		}
	}

	public override void ReverseEnd()
	{
		Trigger();
	}

	public override void Stop()
	{
		if ((bool)canvasGO)
		{
			canvasGO.SetActive(value: false);
		}
	}

	private void FadeToColor(Color from, Color to, float transition)
	{
		if ((bool)image)
		{
			image.GetComponent<Image>().color = Color.Lerp(from, to, transition);
		}
	}
}

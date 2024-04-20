using System.Collections;
using UnityEngine;

public class UIFade : MonoBehaviour
{
	private enum Type
	{
		FadeIn,
		FadeOut
	}

	private static UIFade instance;

	private UIPanel fadePanel;

	private Coroutine fadeCoroutine;

	public static UIFade Instance
	{
		get
		{
			if (instance == null)
			{
				Instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/FadePanel"), UIManager.Instance.transform).GetComponent<UIFade>();
				Instance.Init();
			}
			return instance;
		}
		private set
		{
			instance = value;
		}
	}

	private void Init()
	{
		fadePanel = GetComponent<UIPanel>();
	}

	private void InitFade()
	{
		if (fadeCoroutine != null)
		{
			StopCoroutine(fadeCoroutine);
		}
		base.gameObject.SetActive(value: true);
	}

	public Coroutine FadeOut()
	{
		InitFade();
		return fadeCoroutine = StartCoroutine(Fade(2.5f, Type.FadeOut));
	}

	public Coroutine FadeIn()
	{
		InitFade();
		return fadeCoroutine = StartCoroutine(Fade(0.8f, Type.FadeIn));
	}

	private IEnumerator Fade(float time, Type fadeType)
	{
		float curtime = time;
		while (curtime > 0f)
		{
			curtime -= Time.deltaTime;
			float num = curtime / time;
			switch (fadeType)
			{
			case Type.FadeIn:
				fadePanel.alpha = num;
				break;
			case Type.FadeOut:
				fadePanel.alpha = 1f - num;
				break;
			}
			yield return null;
		}
		if (fadeType == Type.FadeIn)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}

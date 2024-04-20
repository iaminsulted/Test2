using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchTitle : MonoBehaviour
{
	private const float TweenTimeGrowth = 0.35f;

	private const string TextVictory = "Victory";

	private const string TextDefeat = "Defeat";

	[SerializeField]
	private GameObject root;

	[SerializeField]
	private List<UISprite> glows;

	[SerializeField]
	private UILabel text;

	[SerializeField]
	private UILabel shadow;

	[SerializeField]
	private UISprite background;

	public Action Displayed;

	private Coroutine backgroundRoutine;

	public void Show(bool isWinner)
	{
		root.SetActive(value: true);
		if (isWinner)
		{
			ShowVictory();
		}
		else
		{
			ShowDefeat();
		}
		StopAllCoroutines();
		StartCoroutine(DisplayTitle());
		StartCoroutine(DisplayBackground());
	}

	public void Hide()
	{
		StopAllCoroutines();
		root.SetActive(value: false);
	}

	private void ShowDefeat()
	{
		text.text = "Defeat";
		shadow.text = "Defeat";
	}

	private void ShowVictory()
	{
		text.text = "Victory";
		shadow.text = "Victory";
	}

	private IEnumerator DisplayBackground()
	{
		background.alpha = 0f;
		while (true)
		{
			float num = Time.deltaTime / 2f;
			background.alpha += num;
			if (background.alpha >= 0.35f)
			{
				break;
			}
			yield return null;
		}
		background.alpha = 0.35f;
		backgroundRoutine = StartCoroutine(FadeOutBackground());
	}

	private IEnumerator DisplayTitle()
	{
		root.transform.localScale = Vector3.zero;
		glows.ForEach(delegate(UISprite x)
		{
			x.alpha = 0f;
		});
		text.alpha = 0f;
		shadow.alpha = 0f;
		while (true)
		{
			float delta = Time.deltaTime / 0.35f;
			root.transform.localScale += new Vector3(delta, delta, delta);
			glows.ForEach(delegate(UISprite x)
			{
				x.alpha += delta;
			});
			text.alpha += delta;
			shadow.alpha += delta;
			if (root.transform.localScale.x >= 1f)
			{
				break;
			}
			yield return null;
		}
		root.transform.localScale = new Vector3(1f, 1f, 1f);
		glows.ForEach(delegate(UISprite x)
		{
			x.alpha = 1f;
		});
		text.alpha = 1f;
		shadow.alpha = 1f;
		StartCoroutine(GrowTitle());
	}

	private IEnumerator FadeInBackground()
	{
		while (true)
		{
			float num = Time.deltaTime / 10f;
			background.alpha += num;
			if (background.alpha >= 0.35f)
			{
				break;
			}
			yield return null;
		}
		background.alpha = 0.35f;
		backgroundRoutine = StartCoroutine(FadeOutBackground());
	}

	private IEnumerator FadeOutBackground()
	{
		while (true)
		{
			float num = Time.deltaTime / 10f;
			background.alpha -= num;
			if (background.alpha <= 0.15f)
			{
				break;
			}
			yield return null;
		}
		background.alpha = 0.15f;
		backgroundRoutine = StartCoroutine(FadeInBackground());
	}

	private IEnumerator GrowTitle()
	{
		while (true)
		{
			float num = Time.deltaTime * Time.deltaTime * 1.35f;
			root.transform.localScale += new Vector3(num, num, num);
			if (root.transform.localScale.x >= 1.05f)
			{
				break;
			}
			yield return null;
		}
		root.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
		if (backgroundRoutine != null)
		{
			StopCoroutine(backgroundRoutine);
		}
		root.SetActive(value: false);
		Displayed?.Invoke();
	}
}

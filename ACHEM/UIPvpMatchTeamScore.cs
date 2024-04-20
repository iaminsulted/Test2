using System.Collections;
using UnityEngine;

public class UIPvpMatchTeamScore : MonoBehaviour
{
	private const float MaxScale = 1.025f;

	private const float MinScale = 0.975f;

	private const float ScaleSpeed = 0.075f;

	[SerializeField]
	private UISprite sprite;

	[SerializeField]
	private UILabel label;

	private bool isWinner;

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Init(int score, bool isWinner)
	{
		this.isWinner = isWinner;
		sprite.transform.localScale = Vector3.one;
		label.text = score.ToString();
	}

	public void Show()
	{
		if (isWinner)
		{
			sprite.alpha = 1f;
			StartCoroutine(ScaleUp());
		}
		else
		{
			sprite.alpha = 0f;
		}
	}

	public void Hide()
	{
		StopAllCoroutines();
	}

	private IEnumerator ScaleUp()
	{
		yield return new WaitForEndOfFrame();
		while (true)
		{
			float num = Time.deltaTime * 0.075f;
			sprite.transform.localScale += new Vector3(num, num, num);
			if (sprite.transform.localScale.x >= 1.025f)
			{
				break;
			}
			yield return null;
		}
		sprite.transform.localScale = new Vector3(1.025f, 1.025f, 1.025f);
		StartCoroutine(ScaleDown());
	}

	private IEnumerator ScaleDown()
	{
		yield return new WaitForEndOfFrame();
		while (true)
		{
			float num = Time.deltaTime * 0.075f;
			sprite.transform.localScale -= new Vector3(num, num, num);
			if (sprite.transform.localScale.x <= 0.975f)
			{
				break;
			}
			yield return null;
		}
		sprite.transform.localScale = new Vector3(0.975f, 0.975f, 0.975f);
		StartCoroutine(ScaleUp());
	}
}

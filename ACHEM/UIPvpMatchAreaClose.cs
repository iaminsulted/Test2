using System;
using System.Collections;
using UnityEngine;

public class UIPvpMatchAreaClose : MonoBehaviour
{
	private const int AreaCloseTime = 90;

	private const float AlphaTime = 2f;

	private const string TimePrefix = "Area Closing in\n";

	[SerializeField]
	private UILabel label;

	private float timeStampServer;

	public void Update()
	{
		float f = GameTime.realtimeSinceServerStartup - timeStampServer;
		int num = Mathf.Clamp(90 - Mathf.FloorToInt(f), 0, 90);
		string text = TimeSpan.FromSeconds(num).ToString("ss");
		if (text[0] == '0')
		{
			text = text.Remove(0, 1);
		}
		label.text = "Area Closing in\n";
		UILabel uILabel = label;
		uILabel.text = uILabel.text + text + " ";
		label.text += ((num != 1) ? "seconds" : "second");
		if (num <= 10 && label.alpha == 0f)
		{
			StartCoroutine(ShowTime());
		}
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Init(float timeStampServer)
	{
		label.alpha = 0f;
		this.timeStampServer = timeStampServer;
	}

	private IEnumerator ShowTime()
	{
		while (true)
		{
			label.alpha += Time.deltaTime * 2f;
			if (label.alpha >= 1f)
			{
				break;
			}
			yield return null;
		}
		label.alpha = 1f;
	}
}

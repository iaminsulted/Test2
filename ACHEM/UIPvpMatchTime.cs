using System;
using UnityEngine;

public class UIPvpMatchTime : MonoBehaviour
{
	[SerializeField]
	private UILabel label;

	public void Init(int matchSeconds)
	{
		label.text = TimeSpan.FromSeconds(matchSeconds).ToString("mm\\:ss");
		if (label.text[0] == '0')
		{
			label.text = label.text.Remove(0, 1);
		}
	}
}

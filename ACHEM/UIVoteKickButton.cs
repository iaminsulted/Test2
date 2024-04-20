using System;
using UnityEngine;

public class UIVoteKickButton : MonoBehaviour
{
	public UIVoteKick voteKick;

	public UISprite timeRadial;

	[NonSerialized]
	public string playerName;

	public void OnClick()
	{
		ShowPanel();
		HideButton();
	}

	public void OnHover(bool isOver)
	{
		GetComponent<UIWidget>().alpha = (isOver ? 1f : 0.6f);
	}

	public void ShowButton()
	{
		base.gameObject.SetActive(value: true);
	}

	public void HideButton()
	{
		base.gameObject.SetActive(value: false);
	}

	private void ShowPanel()
	{
		voteKick.Init(playerName, this);
	}
}

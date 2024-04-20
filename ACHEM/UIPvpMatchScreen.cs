using System;
using UnityEngine;

public class UIPvpMatchScreen : MonoBehaviour
{
	private static UIPvpMatchScreen instance;

	[SerializeField]
	private UIPvpMatchTitle title;

	[SerializeField]
	private UIPvpMatchScoreboard scoreboard;

	public static UIPvpMatchScreen Instance => instance;

	public void Awake()
	{
		instance = this;
		UIPvpMatchTitle uIPvpMatchTitle = title;
		uIPvpMatchTitle.Displayed = (Action)Delegate.Combine(uIPvpMatchTitle.Displayed, new Action(OnTitleDisplayed));
	}

	public void OnDestroy()
	{
		UIPvpMatchTitle uIPvpMatchTitle = title;
		uIPvpMatchTitle.Displayed = (Action)Delegate.Remove(uIPvpMatchTitle.Displayed, new Action(OnTitleDisplayed));
	}

	public void Init(PvpMatchInfo matchInfo, PvpRewardInfo rewardInfo)
	{
		Hide();
		title.Show(matchInfo.isWinner);
		scoreboard.Init(matchInfo, rewardInfo);
	}

	public void Hide()
	{
		title.Hide();
		scoreboard.Hide();
	}

	private void OnTitleDisplayed()
	{
		Game.Instance.DisableMovementController();
		scoreboard.Show();
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UIPvpMatchScoreboard : MonoBehaviour
{
	[SerializeField]
	private GameObject root;

	[SerializeField]
	private UIPvpMatchScoreboardDescription description;

	[SerializeField]
	private UIPvpMatchScoreboardTitle title;

	[SerializeField]
	private UIPvpMatchRewards rewards;

	[SerializeField]
	private UIPvpMatchTeam blueDragons;

	[SerializeField]
	private UIPvpMatchTeam redDragons;

	[SerializeField]
	private UIButton leaveMatch;

	[SerializeField]
	private UIPvpMatchAreaClose areaClose;

	[SerializeField]
	private UIPvpMatchTime time;

	[SerializeField]
	private UITexture background;

	private Texture defaultBG;

	public void Awake()
	{
		UIEventListener uIEventListener = UIEventListener.Get(leaveMatch.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnLeaveMatchClicked));
		defaultBG = background.mainTexture;
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
		UIEventListener uIEventListener = UIEventListener.Get(leaveMatch.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnLeaveMatchClicked));
	}

	public void Init(PvpMatchInfo matchInfo, PvpRewardInfo rewardInfo)
	{
		title.Init(matchInfo.isWinner);
		description.Init(matchInfo.areaName, Game.Instance.AreaData.PvpType.ToString());
		rewards.Init(rewardInfo.rewards);
		blueDragons.Init(matchInfo.yourTeam, matchInfo.yourScore, matchInfo.isWinner);
		redDragons.Init(matchInfo.enemyTeam, matchInfo.enemyScore, !matchInfo.isWinner);
		areaClose.Init(matchInfo.timeStampServer);
		time.Init(matchInfo.matchSeconds);
		background.mainTexture = defaultBG;
		StartCoroutine(LoadMapBackground(matchInfo.mapImage));
	}

	public void Show()
	{
		root.SetActive(value: true);
		blueDragons.Show();
		redDragons.Show();
		rewards.Show();
	}

	public void Hide()
	{
		rewards.Hide();
		blueDragons.Hide();
		redDragons.Hide();
		root.SetActive(value: false);
	}

	private void OnLeaveMatchClicked(GameObject go)
	{
		Game.Instance.SendPvPMatchLeaveRequest();
	}

	private IEnumerator LoadMapBackground(string mapImage)
	{
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/gamefiles/images/maps/" + mapImage);
		string errorTitle = "Loading Error";
		string friendlyMsg = "Failed to load map background.";
		string customContext = "URL: " + mapImage;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
		}
		else if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
		}
		else if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
		}
		else
		{
			background.mainTexture = DownloadHandlerTexture.GetContent(www);
		}
	}
}

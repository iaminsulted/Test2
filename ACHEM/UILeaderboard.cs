using System.Linq;
using UnityEngine;

public class UILeaderboard : UIMenuWindow
{
	public GameObject LeaderboardListRowTemplate;

	public UITable LeaderboardListTable;

	public UIScrollView LeaderboardListScroll;

	public UILabel windowTitle;

	public UIScrollView scrollView;

	public UILeaderboardDetails currentPlayer;

	public static UILeaderboard Instance { get; private set; }

	public static void Load(int leaderboardID)
	{
		if (Instance == null)
		{
			Instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/Leaderboard"), UIManager.Instance.transform).GetComponent<UILeaderboard>();
			Instance.Init();
			Game.Instance.SendLeaderboardRefreshRequest(leaderboardID);
		}
	}

	protected override void Init()
	{
		base.Init();
		Setup();
	}

	protected virtual void Setup()
	{
		LeaderboardListRowTemplate.SetActive(value: false);
		LeaderboardListTable.Reposition();
		AudioManager.Play2DSFX("UI_Bag_Open");
	}

	public override void Close()
	{
		base.Close();
		AudioManager.Play2DSFX("UI_Bag_Close");
	}

	public void Refresh(Leaderboard leaderboard, bool shouldReset = true)
	{
		if (leaderboard.type == Leaderboard.LeadboardType.Cr1tikalSpeedrun)
		{
			leaderboard.data = leaderboard.data.OrderBy((LeaderboardData x) => x.score).ToList();
		}
		else
		{
			leaderboard.data = leaderboard.data.OrderByDescending((LeaderboardData x) => x.score).ToList();
		}
		windowTitle.text = leaderboard.type.ToString();
		bool flag = false;
		for (int i = 0; i < leaderboard.data.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(LeaderboardListRowTemplate, LeaderboardListTable.transform);
			gameObject.SetActive(value: true);
			if (flag)
			{
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[0].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[2];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[1].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[3];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[2].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[3];
			}
			else
			{
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[0].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[0];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[1].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[1];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[2].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[1];
			}
			if (i < 3)
			{
				gameObject.GetComponent<UILeaderboardDetails>().sprites[i].SetActive(value: true);
				gameObject.GetComponent<UILeaderboardDetails>().rank.gameObject.SetActive(value: false);
			}
			else
			{
				gameObject.GetComponent<UILeaderboardDetails>().rank.text = i + 1 + ".";
			}
			gameObject.GetComponent<UILeaderboardDetails>().name.text = leaderboard.data[i].charName;
			gameObject.GetComponent<UILeaderboardDetails>().score.text = leaderboard.data[i].score.ToString() ?? "";
			if (leaderboard.data[i].charName == Entities.Instance.me.name)
			{
				gameObject.GetComponent<UILeaderboardDetails>().name.text = "[FFFFFF]" + gameObject.GetComponent<UILeaderboardDetails>().name.text;
				gameObject.GetComponent<UILeaderboardDetails>().rank.text = "[FFFFFF]" + gameObject.GetComponent<UILeaderboardDetails>().rank.text;
				gameObject.GetComponent<UILeaderboardDetails>().score.text = "[FFFFFF]" + gameObject.GetComponent<UILeaderboardDetails>().score.text;
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[0].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[4];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[1].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[5];
				gameObject.GetComponent<UILeaderboardDetails>().backgrounds[2].GetComponent<UISprite>().color = gameObject.GetComponent<UILeaderboardDetails>().colors[5];
			}
			else
			{
				gameObject.GetComponent<UILeaderboardDetails>().name.text = "[2A1610]" + gameObject.GetComponent<UILeaderboardDetails>().name.text;
				gameObject.GetComponent<UILeaderboardDetails>().rank.text = "[2A1610]" + gameObject.GetComponent<UILeaderboardDetails>().rank.text;
				gameObject.GetComponent<UILeaderboardDetails>().score.text = "[2A1610]" + gameObject.GetComponent<UILeaderboardDetails>().score.text;
			}
			flag = !flag;
		}
		currentPlayer.name.text = Entities.Instance.me.name;
		LeaderboardListTable.Reposition();
		scrollView.ResetPosition();
		if (shouldReset)
		{
			LeaderboardListScroll.ResetPosition();
		}
		else
		{
			LeaderboardListScroll.InvalidateBounds();
		}
	}

	public void updatePlayerScore(int playerPos, int score)
	{
		currentPlayer.score.text = score.ToString() ?? "";
		if (playerPos < 3 && playerPos > 0)
		{
			currentPlayer.sprites[playerPos - 1].SetActive(value: true);
			currentPlayer.rank.gameObject.SetActive(value: false);
		}
		else if (playerPos <= 0)
		{
			currentPlayer.rank.text = "N/A";
		}
		else
		{
			currentPlayer.rank.text = playerPos + ".";
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		Instance = null;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPvpMatchTeam : MonoBehaviour
{
	private readonly List<int> NoTeam = new List<int>();

	[SerializeField]
	private UIPvpMatchTeamScore score;

	[SerializeField]
	private List<UIPvpMatchPlayer> players;

	[SerializeField]
	private UIPvpMatchStat kills;

	[SerializeField]
	private UIPvpMatchStat deaths;

	[SerializeField]
	private UIPvpMatchStat assists;

	[SerializeField]
	private UIPvpMatchStat combatDamage;

	[SerializeField]
	private UIPvpMatchStat healingDone;

	[SerializeField]
	private UIPvpMatchStat pointsCaptured;

	public void Init(List<PvpPlayerStats> teamStats, int teamScore, bool isWinner)
	{
		score.Init(teamScore, isWinner);
		if (teamStats != null)
		{
			kills.Init(teamStats.Select((PvpPlayerStats x) => x.kills).ToList());
			deaths.Init(teamStats.Select((PvpPlayerStats x) => x.deaths).ToList());
			assists.Init(teamStats.Select((PvpPlayerStats x) => x.assists).ToList());
			combatDamage.Init(teamStats.Select((PvpPlayerStats x) => Entities.Instance.me.CalculateDisplayHpDelta(x.damageDealt)).ToList());
			healingDone.Init(teamStats.Select((PvpPlayerStats x) => Entities.Instance.me.CalculateDisplayHpDelta(x.healingDone)).ToList());
			pointsCaptured.Init(teamStats.Select((PvpPlayerStats x) => x.objectivesTaken).ToList());
			for (int i = 0; i < players.Count; i++)
			{
				if (teamStats.ElementAtOrDefault(i) != null)
				{
					Player playerById = Entities.Instance.GetPlayerById(teamStats[i].playerID);
					if (playerById != null)
					{
						players[i].Init(playerById.name, playerById.ClassIcon, playerById.isMe);
						continue;
					}
				}
				players[i].Init("", "", isMe: false);
			}
		}
		else
		{
			DisplayNoTeam();
		}
	}

	public void Show()
	{
		score.Show();
	}

	public void Hide()
	{
		score.Hide();
	}

	private void DisplayNoTeam()
	{
		kills.Init(NoTeam);
		deaths.Init(NoTeam);
		assists.Init(NoTeam);
		combatDamage.Init(NoTeam);
		healingDone.Init(NoTeam);
		pointsCaptured.Init(NoTeam);
		for (int i = 0; i < players.Count; i++)
		{
			players[i].Init("", "", isMe: false);
		}
	}
}

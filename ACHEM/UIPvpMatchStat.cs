using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchStat : MonoBehaviour
{
	private readonly Dictionary<PvpMatchStatType, string> ToolTipPrefabPaths = new Dictionary<PvpMatchStatType, string>
	{
		{
			PvpMatchStatType.Kills,
			"UIPvpMatchToolTip_Kills"
		},
		{
			PvpMatchStatType.Deaths,
			"UIPvpMatchToolTip_Deaths"
		},
		{
			PvpMatchStatType.Assists,
			"UIPvpMatchToolTip_Assists"
		},
		{
			PvpMatchStatType.CombatDamage,
			"UIPvpMatchToolTip_CombatDamage"
		},
		{
			PvpMatchStatType.HealingDone,
			"UIPvpMatchToolTip_HealingDone"
		},
		{
			PvpMatchStatType.PointsCaptured,
			"UIPvpMatchToolTip_PointsCaptured"
		},
		{
			PvpMatchStatType.Wins,
			"UIPvpMatchToolTip_Wins"
		},
		{
			PvpMatchStatType.Losses,
			"UIPvpMatchToolTip_Losses"
		},
		{
			PvpMatchStatType.GloryRank,
			"UIPvpMatchToolTip_GloryRank"
		}
	};

	[SerializeField]
	private PvpMatchStatType statType;

	[SerializeField]
	private UIPvpMatchStatGrid grid;

	private bool resourcesLoaded;

	private UIPvpMatchToolTip toolTip;

	private GameObject toolTipPrefab;

	public void Init(List<int> stats)
	{
		if (!resourcesLoaded)
		{
			LoadResources();
		}
		grid.AddStats(statType, stats);
	}

	public void OnTooltip(bool show)
	{
		if (toolTip == null)
		{
			LoadResources();
		}
		if (show)
		{
			toolTip.Show();
		}
		else
		{
			toolTip.Hide();
		}
	}

	private void LoadResources()
	{
		toolTipPrefab = Object.Instantiate(Resources.Load("UIElements/Pvp/ToolTips/" + ToolTipPrefabPaths[statType]), base.transform) as GameObject;
		toolTip = toolTipPrefab.GetComponent<UIPvpMatchToolTip>();
		resourcesLoaded = true;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchToolTip : MonoBehaviour
{
	public const string ToolTipsPath = "UIElements/Pvp/ToolTips/";

	private const int LabelOffsetXY = 6;

	private readonly Dictionary<PvpMatchToolTipType, string> ToolTips = new Dictionary<PvpMatchToolTipType, string>
	{
		{
			PvpMatchToolTipType.Gold,
			"Gold"
		},
		{
			PvpMatchToolTipType.MarkOfGlory,
			"Marks of Glory"
		},
		{
			PvpMatchToolTipType.XP,
			"Character XP"
		},
		{
			PvpMatchToolTipType.ClassXP,
			"Class XP"
		},
		{
			PvpMatchToolTipType.Glory,
			"Class Glory"
		},
		{
			PvpMatchToolTipType.Kills,
			"Kills"
		},
		{
			PvpMatchToolTipType.Deaths,
			"Deaths"
		},
		{
			PvpMatchToolTipType.Assists,
			"Assists"
		},
		{
			PvpMatchToolTipType.CombatDamage,
			"Damage Dealt"
		},
		{
			PvpMatchToolTipType.HealingDone,
			"Healing Done"
		},
		{
			PvpMatchToolTipType.PointsCaptured,
			"Zones Captured"
		},
		{
			PvpMatchToolTipType.Wins,
			"Wins"
		},
		{
			PvpMatchToolTipType.Losses,
			"Matches Played"
		},
		{
			PvpMatchToolTipType.GloryRank,
			"Total Glory Level"
		}
	};

	[SerializeField]
	private PvpMatchToolTipType toolTipType;

	[SerializeField]
	private GameObject root;

	[SerializeField]
	private UISprite sprite;

	[SerializeField]
	private UILabel label;

	public void Awake()
	{
		label.text = ToolTips[toolTipType];
		label.transform.localPosition = new Vector3(6f, 6f, 0f);
		sprite.SetDimensions((int)label.localSize.x + 12, (int)label.localSize.y + 12);
		root.SetActive(value: false);
	}

	public void Show()
	{
		root.SetActive(value: true);
	}

	public void Hide()
	{
		root.SetActive(value: false);
	}
}

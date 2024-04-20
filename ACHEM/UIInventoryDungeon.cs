using System;
using UnityEngine;

public class UIInventoryDungeon : MonoBehaviour
{
	public DungeonData dungeon;

	public UILabel lblName;

	public UISprite Background;

	public UISprite DungeonIcon;

	public Texture2D NormalIcon;

	public Texture2D ChallengeIcon;

	public GameObject icon_quest;

	public bool isSelected;

	public bool Selected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (value != isSelected)
			{
				isSelected = value;
				if (isSelected)
				{
					Background.spriteName = "Highlight";
				}
				else
				{
					Background.spriteName = "TransparentSprite";
				}
			}
		}
	}

	public event Action<UIInventoryDungeon> Clicked;

	public void Init(DungeonData d)
	{
		dungeon = d;
		lblName.text = d.map.DisplayName;
		DungeonIcon.spriteName = (d.map.IsChallenge ? ChallengeIcon.name : NormalIcon.name);
	}

	public void OnClick()
	{
		this.Clicked?.Invoke(this);
	}

	public void questIconOn(DungeonData d)
	{
		icon_quest.SetActive(Session.MyPlayerData.CurrentlyTrackedQuest != null && Session.MyPlayerData.CurrentlyTrackedQuest.TargetMapId == d.map.ID);
	}
}

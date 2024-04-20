using System;
using UnityEngine;

public class UIInventoryGuildMember : UIItem
{
	public UISprite Check;

	public UILabel Level;

	public UILabel lblLastOnline;

	public UISprite Online;

	public UISprite OnlineRing;

	public GuildMember guildMember;

	private readonly Color32 Online_Text = new Color32(0, 0, 0, byte.MaxValue);

	private readonly Color32 Offline_Text = new Color32(90, 90, 90, 155);

	public void Init(GuildMember guildMemberData)
	{
		guildMember = guildMemberData;
		GuildRole(guildMemberData.GuildRole);
		UpdateStatus();
		NameLabel.text = guildMember.Name;
		InfoLabel.text = guildMember.MapDisplayName;
		Level.text = "Level " + guildMember.Level;
	}

	private void GuildRole(GuildRole role)
	{
		Check.spriteName = "";
		Check.gameObject.SetActive(value: true);
		switch (role)
		{
		case global::GuildRole.Leader:
			Check.spriteName = "icon_guild_badge_01";
			break;
		case global::GuildRole.Officer:
			Check.spriteName = "icon_guild_badge_02";
			break;
		case global::GuildRole.Member:
			Check.spriteName = "icon_guild_badge_04";
			break;
		}
	}

	private void UpdateStatus()
	{
		Online.enabled = guildMember.ID != Session.MyPlayerData.ID;
		OnlineRing.enabled = guildMember.ID != Session.MyPlayerData.ID;
		if (guildMember.IsOnline)
		{
			Online.color = new Color32(10, 165, 62, byte.MaxValue);
			OnlineRing.color = new Color32(10, 165, 62, byte.MaxValue);
			NameLabel.color = Online_Text;
			Level.color = Online_Text;
			InfoLabel.color = Online_Text;
			lblLastOnline.gameObject.SetActive(value: false);
			return;
		}
		Online.color = new Color32(111, 111, 111, byte.MaxValue);
		OnlineRing.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		NameLabel.color = Offline_Text;
		Level.color = Offline_Text;
		InfoLabel.color = Offline_Text;
		lblLastOnline.text = "Last Online: ";
		TimeSpan timeSpan = DateTime.UtcNow - guildMember.LastOnlineDate;
		if (timeSpan.TotalDays >= 365.0)
		{
			lblLastOnline.text += "over a year ago";
		}
		else if (timeSpan.TotalDays >= 2.0)
		{
			UILabel uILabel = lblLastOnline;
			uILabel.text = uILabel.text + (int)Math.Floor(timeSpan.TotalDays) + " days ago";
		}
		else if (timeSpan.TotalHours >= 2.0)
		{
			UILabel uILabel2 = lblLastOnline;
			uILabel2.text = uILabel2.text + (int)Math.Floor(timeSpan.TotalHours) + " hours ago";
		}
		else
		{
			UILabel uILabel3 = lblLastOnline;
			uILabel3.text = uILabel3.text + (int)Math.Floor(timeSpan.TotalMinutes) + " minutes ago";
		}
	}
}

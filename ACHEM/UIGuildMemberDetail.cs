using System;
using UnityEngine;

public class UIGuildMemberDetail : MonoBehaviour
{
	public UILabel LabelName;

	public UILabel LabelStatus;

	public UIButton BtnGoto;

	public UIButton BtnSummon;

	public UIButton BtnKick;

	public UIButton BtnClose;

	public UIButton BtnWhisper;

	public UIButton BtnPromote;

	public UIButton BtnDemote;

	public UIButton BtnInvite;

	public GameObject Tint;

	public GuildMember member;

	public event Action Refresh;

	public void Init(GuildMember guildMember)
	{
		member = guildMember;
		LabelName.text = guildMember.Name;
		RefreshView();
	}

	public void RoleUpdated(bool leaveGuild)
	{
		if (!leaveGuild)
		{
			RefreshView();
		}
	}

	private void RefreshView()
	{
		string text = (member.OnSameServer ? "[FFFFFF]" : "[BBBBBB]");
		LabelStatus.text = (member.IsOnline ? ("Server - " + text + member.ServerName + "[-]") : (text + member.ServerName + "[-]"));
		if (member.ServerID > 0)
		{
			BtnWhisper.isEnabled = member.OnSameServer;
			BtnInvite.isEnabled = member.OnSameServer;
			BtnSummon.isEnabled = member.OnSameServer;
			BtnGoto.isEnabled = true;
		}
		else
		{
			BtnGoto.isEnabled = false;
			BtnSummon.isEnabled = false;
			BtnWhisper.isEnabled = false;
			BtnInvite.isEnabled = false;
			BtnKick.isEnabled = false;
			BtnPromote.isEnabled = false;
			BtnDemote.isEnabled = false;
		}
		BtnKick.isEnabled = Session.MyPlayerData.Guild.HasAuthority(member.ID);
		BtnPromote.isEnabled = Session.MyPlayerData.Guild.HasAuthority(member.ID, rankChange: true);
		BtnDemote.isEnabled = Session.MyPlayerData.Guild.HasAuthority(member.ID, rankChange: true) && member.GuildRole != GuildRole.Member;
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnGoto.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(GotoClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnSummon.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(SummonClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(CloseClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnWhisper.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(WhisperClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnInvite.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(InviteClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(BtnKick.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(KickMember));
		UIEventListener uIEventListener7 = UIEventListener.Get(BtnPromote.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(PromoteMember));
		UIEventListener uIEventListener8 = UIEventListener.Get(BtnDemote.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(DemoteMember));
		Session.MyPlayerData.GuildsUpdated += RoleUpdated;
		Tint.SetActive(value: true);
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnGoto.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(GotoClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnSummon.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(SummonClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(CloseClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnKick.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(KickMember));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnWhisper.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(WhisperClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(BtnInvite.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener6.onClick, new UIEventListener.VoidDelegate(InviteClick));
		UIEventListener uIEventListener7 = UIEventListener.Get(BtnPromote.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener7.onClick, new UIEventListener.VoidDelegate(PromoteMember));
		UIEventListener uIEventListener8 = UIEventListener.Get(BtnDemote.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener8.onClick, new UIEventListener.VoidDelegate(DemoteMember));
		Session.MyPlayerData.GuildsUpdated -= RoleUpdated;
	}

	private void GotoClick(GameObject go)
	{
		Tint.SetActive(value: true);
		if (member.OnSameServer)
		{
			Game.Instance.SendGotoRequest(member.ID, "");
		}
		else if (member.IsOnline)
		{
			Confirmation.Show("Switch Server", "This player is on a different server. Would you like to switch servers?", delegate(bool b)
			{
				if (b)
				{
					Session.pendingGoto = new GotoInfo(member.SummonLink);
					AEC.getInstance().sendRequest(new RequestDisconnect());
				}
			});
		}
		else
		{
			Chat.Notify(member.Name + " is not online right now. Please try again later.");
		}
	}

	private void SummonClick(GameObject go)
	{
		Tint.SetActive(value: true);
		if (!member.IsOnline)
		{
			Chat.Notify(member.Name + " is not online right now. Please try again later.");
			return;
		}
		if (!member.OnSameServer)
		{
			Chat.Notify(member.Name + " is not on this server. Please try again later.");
			return;
		}
		Confirmation.Show("Summon Member", "Would you like to invite " + member.Name + " to join you?", delegate(bool b)
		{
			if (b)
			{
				RequestSummon r = new RequestSummon
				{
					CharID = Session.MyPlayerData.ID,
					TargetCharID = member.ID
				};
				Game.Instance.aec.sendRequest(r);
				Chat.Notify("Invited " + member.Name);
			}
		});
	}

	private void WhisperClick(GameObject go)
	{
		Tint.SetActive(value: false);
		Chat.Instance.SetWhisper(member.Name);
		Chat.Instance.Focus();
	}

	private void InviteClick(GameObject _)
	{
		if (PartyManager.IsPartyFull())
		{
			Notification.ShowText("Party is full");
		}
		else if (PartyManager.IsMember(member.ID))
		{
			Chat.Notify("You are already in a party with " + member.Name);
		}
		else
		{
			Game.Instance.SendPartyInviteRequest(member.Name);
		}
	}

	private void CloseClick(GameObject go)
	{
		base.gameObject.SetActive(value: false);
	}

	private void KickMember(GameObject go)
	{
		Tint.SetActive(value: true);
		Confirmation.Show("Kick Guild Member", "Are you sure you want to kick " + member.Name + " from the Guild?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendKickGuildMemberRequest(member.ID);
				this.Refresh?.Invoke();
				base.gameObject.SetActive(value: false);
			}
		});
	}

	private void PromoteMember(GameObject go)
	{
		Tint.SetActive(value: true);
		if (Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader && member.GuildRole == GuildRole.Officer)
		{
			Tint.SetActive(value: true);
			Confirmation.Show("Promote Guild Member", $"Promote {member.Name} to {member.GuildRole + 1}? " + "NOTE: This will replace you as Leader! You cannot undo this. Are you sure?", delegate(bool b)
			{
				if (b)
				{
					Game.Instance.SendGuildChangeRoleRequest(Session.MyPlayerData.ID, member.ID, Session.MyPlayerData.Guild.guildID, (byte)(member.GuildRole + 1), swap: true);
					this.Refresh?.Invoke();
				}
			});
			return;
		}
		Confirmation.Show("Promote Guild Member", $"Promote {member.Name} to {member.GuildRole + 1}?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendGuildChangeRoleRequest(Session.MyPlayerData.ID, member.ID, Session.MyPlayerData.Guild.guildID, (byte)(member.GuildRole + 1));
				this.Refresh?.Invoke();
			}
		});
	}

	private void DemoteMember(GameObject go)
	{
		Tint.SetActive(value: true);
		Confirmation.Show("Demote Guild Member", $"Demote {member.Name} to {member.GuildRole + -1}?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendGuildChangeRoleRequest(Session.MyPlayerData.ID, member.ID, Session.MyPlayerData.Guild.guildID, (byte)(member.GuildRole - 1));
				this.Refresh?.Invoke();
			}
		});
	}
}

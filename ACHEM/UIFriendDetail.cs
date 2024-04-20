using System;
using UnityEngine;

public class UIFriendDetail : MonoBehaviour
{
	public UILabel LabelName;

	public UILabel LabelStatus;

	public UIButton BtnGoto;

	public UIButton BtnSummon;

	public UIButton BtnRemove;

	public UIButton BtnClose;

	public UIButton BtnWhisper;

	public UIButton btnParty;

	public UILabel BtnWhisperText;

	public GameObject Buttons;

	public GameObject Tint;

	public FriendData friend;

	public void Init(FriendData f)
	{
		friend = f;
		LabelName.text = f.strName;
		string text = (f.OnSameServer ? "[FFFFFF]" : "[BBBBBB]");
		LabelStatus.text = (f.IsOnline ? ("Server - " + text + f.ServerName + "[-]") : (text + f.ServerName + "[-]"));
		if (f.ServerID > 0)
		{
			BtnWhisper.isEnabled = friend.OnSameServer;
			btnParty.isEnabled = friend.OnSameServer;
			BtnSummon.isEnabled = friend.OnSameServer;
			BtnGoto.isEnabled = true;
		}
		else
		{
			BtnGoto.isEnabled = false;
			BtnSummon.isEnabled = false;
			BtnWhisper.isEnabled = false;
			btnParty.isEnabled = false;
		}
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnGoto.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(GotoClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnSummon.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(SummonClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnRemove.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(RemoveClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CloseClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnWhisper.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(WhisperClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnParty.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnPartyClick));
		Tint.SetActive(value: true);
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(BtnGoto.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(GotoClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnSummon.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(SummonClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnRemove.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(RemoveClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CloseClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(BtnWhisper.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(WhisperClick));
	}

	private void GotoClick(GameObject go)
	{
		Tint.SetActive(value: true);
		if (friend.OnSameServer)
		{
			Game.Instance.SendGotoRequest(friend.FriendID, "");
		}
		else if (friend.IsOnline)
		{
			Confirmation.Show("Switch Server", "Your friend is on a different server.  Would you like to switch servers?", delegate(bool b)
			{
				if (b)
				{
					Session.pendingGoto = new GotoInfo(friend.SummonLink);
					AEC.getInstance().sendRequest(new RequestDisconnect());
				}
			});
		}
		else
		{
			Chat.Notify(friend.strName + " is not online right now.  Please try again later.");
		}
	}

	private void SummonClick(GameObject go)
	{
		Tint.SetActive(value: true);
		if (!friend.IsOnline)
		{
			Chat.Notify(friend.strName + " is not online right now.  Please try again later.");
			return;
		}
		if (!friend.OnSameServer)
		{
			Chat.Notify(friend.strName + " is not on this server.  Please try again later.");
			return;
		}
		Confirmation.Show("Summon Friend", "Would you like to invite " + friend.strName + " to join you?", delegate(bool b)
		{
			if (b)
			{
				RequestSummon r = new RequestSummon
				{
					CharID = Session.MyPlayerData.ID,
					TargetCharID = friend.FriendID
				};
				Game.Instance.aec.sendRequest(r);
				Chat.Notify("Invited " + friend.strName);
			}
		});
	}

	private void WhisperClick(GameObject go)
	{
		Tint.SetActive(value: false);
		Chat.Instance.SetWhisper(friend.strName);
		Chat.Instance.Focus();
	}

	private void CloseClick(GameObject go)
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnPartyClick(GameObject g)
	{
		if (PartyManager.IsPartyFull())
		{
			Notification.ShowText("Party is full");
		}
		else if (PartyManager.IsMember(friend.FriendID))
		{
			Chat.Notify("You are already in a party with " + friend.strName);
		}
		else
		{
			Game.Instance.SendPartyInviteRequest(friend.strName);
		}
	}

	private void RemoveClick(GameObject go)
	{
		Tint.SetActive(value: true);
		Confirmation.Show("Remove Friend", "Are you sure you  don't want to be friends with " + friend.strName + "?", delegate(bool b)
		{
			if (b)
			{
				RequestFriendDelete r = new RequestFriendDelete
				{
					CharID = Session.MyPlayerData.ID,
					FriendID = friend.FriendID
				};
				Game.Instance.aec.sendRequest(r);
			}
		});
	}
}

using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIMailItem : MonoBehaviour
{
	public UILabel Title;

	public UILabel Sender;

	public MailMessage mail;

	public UIButton trashBtn;

	public UIMail uimail;

	public GameObject favBtn;

	public UISprite mailIcon;

	private ChatFilter chatFilter = new ChatFilter();

	public event Action<UIMailItem> Clicked;

	public void Init(MailMessage mail)
	{
		this.mail = mail;
		Title.text = mail.subject;
		Sender.text = mail.sender;
		if (mail.subject == null)
		{
			mail.subject = "";
		}
		if (mail.sender == null)
		{
			mail.sender = "";
		}
		if ((bool)SettingsManager.IsChatFiltered)
		{
			ChatFilteredMessage chatFilteredMessage = chatFilter.ProfanityCheck(mail.subject, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				Title.text = chatFilteredMessage.maskedMessage;
			}
			ChatFilteredMessage chatFilteredMessage2 = chatFilter.ProfanityCheck(mail.sender, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				Sender.text = chatFilteredMessage2.maskedMessage;
				if (chatFilteredMessage2.maskedMessage == null)
				{
					Sender.text = mail.sender;
				}
			}
		}
		if (mail.favorited)
		{
			favBtn.gameObject.GetComponent<UISprite>().color = Color.yellow;
		}
		else
		{
			favBtn.gameObject.GetComponent<UISprite>().color = Color.gray;
		}
		if (mail.icon == null)
		{
			mail.icon = "";
		}
		if (Regex.Replace(mail.icon, "[^a-zA-Z0-9]", "") != "envelope")
		{
			mailIcon.spriteName = "aq3d_icon_mail_letter_AE_Dev_64";
		}
		else if (mail.hasSeen)
		{
			mailIcon.spriteName = "aq3d_icon_mail_letter_Open_blue_64";
		}
		else
		{
			mailIcon.spriteName = "aq3d_icon_mail_letter_New_green_64";
		}
	}

	private void OnClick()
	{
		OnClicked();
	}

	protected void OnClicked()
	{
		this.Clicked?.Invoke(this);
		Game.Instance.SendSeenMailRequest(mail);
		mail.hasSeen = true;
		if (Regex.Replace(mail.icon, "[^a-zA-Z0-9]", "") != "envelope")
		{
			mailIcon.spriteName = "aq3d_icon_mail_letter_AE_Dev_64";
		}
		else
		{
			mailIcon.spriteName = "aq3d_icon_mail_letter_Open_blue_64";
		}
	}

	public void OnDeleteMailClicked()
	{
		if (mail.favorited)
		{
			MessageBox.Show("Cannot Delete", "You cannot delete mail that is favorited.", "Gotcha");
			return;
		}
		Confirmation.Show("Delete", "Deleting mail is permanent\n\nYou will not receive the rewards of deleted mail\n\nDo you still want to delete this mail?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendDeleteMailRequest(Session.MyPlayerData.ID, mail.id);
				foreach (MailMessage item in Session.MyPlayerData.mailbox)
				{
					if (item.id == mail.id)
					{
						item.hasDeleted = true;
					}
				}
				uimail.DisplayUpdate();
			}
		});
	}

	public void OnFavoriteClick()
	{
		if (!mail.favorited)
		{
			mail.favorited = true;
			favBtn.gameObject.GetComponent<UISprite>().color = Color.yellow;
		}
		else
		{
			mail.favorited = false;
			favBtn.gameObject.GetComponent<UISprite>().color = Color.gray;
		}
	}
}

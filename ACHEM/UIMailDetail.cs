using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMailDetail : MonoBehaviour
{
	public UILabel lblSubject;

	public UILabel lblSender;

	public UILabel lblMessage;

	public UILabel lblGoldReward;

	public UILabel lblDCReward;

	public UISprite Icon;

	public UIMail mainMenu;

	public UIMailItem UIMailItem;

	public UIScrollView DescriptionScroll;

	public UIScrollView RewardScroll;

	public GameObject DetailObject;

	public MailMessage mail;

	public UIButton itemClaim;

	public GameObject itemClaimUI;

	public GameObject NormalMail;

	public GameObject DevMail;

	public bool isRewardMail;

	public UIItem itemTemplate;

	private ObjectPool<GameObject> itemGOpool;

	private Transform container;

	private List<Item> uiItemRewards;

	private List<UIItem> itemGOs = new List<UIItem>();

	public UIGrid grid;

	public UILabel expiryText;

	private ChatFilter chatFilter = new ChatFilter();

	public void Load(MailMessage mail)
	{
		this.mail = mail;
		lblSubject.text = mail.subject;
		lblSender.text = mail.sender;
		lblMessage.text = mail.message;
		if (mail.subject == null)
		{
			mail.subject = "";
		}
		if (mail.sender == null)
		{
			mail.sender = "";
		}
		if (mail.message == null)
		{
			mail.message = "";
		}
		if ((bool)SettingsManager.IsChatFiltered)
		{
			ChatFilteredMessage chatFilteredMessage = chatFilter.ProfanityCheck(mail.subject, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				lblSubject.text = chatFilteredMessage.maskedMessage;
			}
			ChatFilteredMessage chatFilteredMessage2 = chatFilter.ProfanityCheck(mail.sender, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				lblSender.text = chatFilteredMessage2.maskedMessage;
			}
			if (chatFilteredMessage2.maskedMessage == null)
			{
				lblSender.text = mail.sender;
			}
			ChatFilteredMessage chatFilteredMessage3 = chatFilter.ProfanityCheck(mail.message, shouldCleanSymbols: false);
			if (chatFilteredMessage.code > 0)
			{
				lblMessage.text = chatFilteredMessage3.maskedMessage;
			}
		}
		DevMail.gameObject.SetActive(value: false);
		NormalMail.gameObject.SetActive(value: true);
		itemTemplate.gameObject.SetActive(value: false);
		itemGOpool = new ObjectPool<GameObject>(itemTemplate.gameObject);
		container = itemTemplate.transform;
		uiItemRewards = new List<Item>();
	}

	public void LoadRewardUI()
	{
		foreach (UIItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		isRewardMail = false;
		int num = 0;
		if (Session.MyPlayerData.mailrewards.Any())
		{
			foreach (RewardItem mailreward in Session.MyPlayerData.mailrewards)
			{
				if (mailreward.CharMailID == mail.id)
				{
					DevMail.gameObject.SetActive(value: true);
					NormalMail.gameObject.SetActive(value: false);
					isRewardMail = true;
					if (mail.hasRedeemed)
					{
						itemClaim.gameObject.SetActive(value: false);
					}
					else
					{
						GameObject obj = itemGOpool.Get();
						obj.transform.SetParent(container.parent, worldPositionStays: false);
						obj.SetActive(value: true);
						UIItem component = obj.GetComponent<UIItem>();
						component.Init(Session.MyPlayerData.mailRewardsList[num]);
						itemGOs.Add(component);
						itemClaim.gameObject.SetActive(value: true);
					}
				}
				num++;
			}
		}
		if (!isRewardMail)
		{
			DevMail.gameObject.SetActive(value: false);
			NormalMail.gameObject.SetActive(value: true);
		}
		else if (mail.endDate - DateTime.Today > TimeSpan.FromDays(1.0))
		{
			expiryText.text = "Expires in " + (mail.endDate - DateTime.Today).TotalDays + " Days";
		}
		else
		{
			expiryText.text = "Expires TODAY!";
		}
	}

	public void OnMailItemClaimClicked()
	{
		List<RewardItem> list = new List<RewardItem>();
		foreach (RewardItem mailreward in Session.MyPlayerData.mailrewards)
		{
			if (mailreward.CharMailID == mail.id)
			{
				list.Add(mailreward);
			}
		}
		Game.Instance.SendMailItemClaimRequest(list);
		foreach (MailMessage item in Session.MyPlayerData.mailbox)
		{
			if (item.id == mail.id)
			{
				item.hasRedeemed = true;
			}
		}
		itemClaim.gameObject.SetActive(value: false);
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
				mainMenu.DisplayUpdate();
			}
		});
	}

	public void onReportClicked()
	{
		UICharReport.Show(mail.sender);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMail : UIMenuWindow
{
	private static UIMail instance;

	public UIMailItem mailTemplate;

	private ObjectPool<GameObject> mailItemPool;

	private Transform container;

	private List<UIMailItem> uiMailItems;

	public UIMailDetail detail;

	public GameObject mainMenu;

	public UIButton btnDetailBack;

	public UIMailCompose composeDetail;

	public GameObject itemClaimUI;

	public UIGrid grid;

	public UIToggle toggleMailBlocked;

	public UIToggle toggleMailFriends;

	public UIToggle toggleMailAll;

	public GameObject mailSettings;

	public static UIMail Instance { get; private set; }

	public static void Load()
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/Mail"), UIManager.Instance.transform).GetComponent<UIMail>();
			Instance.Init();
		}
	}

	public static void Show()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UIMail"), UIManager.Instance.transform).GetComponent<UIMail>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnDetailBack.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnDetailBackClick));
		Setup();
	}

	protected virtual void Setup()
	{
		container = mailTemplate.transform.parent;
		uiMailItems = new List<UIMailItem>();
		mailItemPool = new ObjectPool<GameObject>(mailTemplate.gameObject);
		mailTemplate.gameObject.SetActive(value: false);
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
		AudioManager.Play2DSFX("UI_Mail_Open");
		DisplayMail();
	}

	public void DisplayMail()
	{
		uiMailItems.Clear();
		foreach (MailMessage mail in Session.MyPlayerData.mailbox)
		{
			if (SettingsManager.IsMailBlocked != "" && mail.startDate >= DateTime.Parse(SettingsManager.IsMailBlocked) && mail.icon != "" && !mail.hasDeleted)
			{
				Game.Instance.SendDeleteMailRequest(Session.MyPlayerData.ID, mail.id);
				mail.hasDeleted = true;
			}
			if (SettingsManager.IsMailFriends != "" && mail.startDate >= DateTime.Parse(SettingsManager.IsMailFriends) && mail.icon != "" && !mail.hasDeleted && !Session.MyPlayerData.friendsList.Any((FriendData friend) => friend.strName == mail.sender.TrimEnd(' ')) && (Session.MyPlayerData.Guild == null || !Session.MyPlayerData.Guild.guildMembers.Any((KeyValuePair<int, GuildMember> guildy) => guildy.Value.Name == mail.sender.TrimEnd(' '))))
			{
				Game.Instance.SendDeleteMailRequest(Session.MyPlayerData.ID, mail.id);
				mail.hasDeleted = true;
			}
			if (mail.endDate == DateTime.Today.AddDays(1.0))
			{
				Game.Instance.SendDeleteMailRequest(Session.MyPlayerData.ID, mail.id);
				mail.hasDeleted = true;
			}
			if (!mail.hasDeleted)
			{
				GameObject obj = mailItemPool.Get();
				obj.transform.SetParent(container, worldPositionStays: false);
				obj.SetActive(value: true);
				UIMailItem component = obj.GetComponent<UIMailItem>();
				component.Init(mail);
				component.Clicked += OnMailClicked;
				uiMailItems.Add(component);
			}
		}
	}

	public void DisplayUpdate()
	{
		foreach (UIMailItem uiMailItem in uiMailItems)
		{
			if (uiMailItem.mail.hasDeleted)
			{
				uiMailItem.gameObject.SetActive(value: false);
			}
		}
	}

	private void OnMailClicked(UIMailItem selectedItem)
	{
		detail.Load(selectedItem.mail);
		selectedItem.mail.hasSeen = true;
		foreach (MailMessage item in Session.MyPlayerData.mailbox)
		{
			if (item.id == selectedItem.mail.id)
			{
				item.hasSeen = true;
			}
		}
		mainMenu.gameObject.SetActive(value: false);
		detail.gameObject.SetActive(value: true);
		detail.LoadRewardUI();
	}

	public override void OnBackClick(GameObject go)
	{
		base.Back();
	}

	public virtual void OnDetailBackClick(GameObject go)
	{
		if (detail != null && detail.isActiveAndEnabled)
		{
			mainMenu.gameObject.SetActive(value: true);
			detail.gameObject.SetActive(value: false);
		}
	}

	public void OnComposeClicked()
	{
		if (composeDetail != null && composeDetail.isActiveAndEnabled)
		{
			composeDetail.gameObject.SetActive(value: false);
		}
		else
		{
			composeDetail.gameObject.SetActive(value: true);
		}
	}

	public void OnMailBlockAll()
	{
		if (SettingsManager.IsMailBlocked == "")
		{
			SettingsManager.IsMailBlocked.Set(DateTime.Now.ToString());
			SettingsManager.IsMailFriends.Set("");
			SettingsManager.IsMailAll.Set("");
		}
		toggleMailBlocked.value = SettingsManager.IsMailBlocked != "";
		toggleMailFriends.value = SettingsManager.IsMailFriends != "";
		toggleMailAll.value = SettingsManager.IsMailAll != "";
	}

	public void OnMailBlockFriends()
	{
		if (SettingsManager.IsMailFriends == "")
		{
			SettingsManager.IsMailFriends.Set(DateTime.Now.ToString());
			SettingsManager.IsMailBlocked.Set("");
			SettingsManager.IsMailAll.Set("");
		}
		toggleMailBlocked.value = SettingsManager.IsMailBlocked != "";
		toggleMailFriends.value = SettingsManager.IsMailFriends != "";
		toggleMailAll.value = SettingsManager.IsMailAll != "";
	}

	public void OnMailBlockNone()
	{
		if (SettingsManager.IsMailAll == "")
		{
			SettingsManager.IsMailAll.Set(DateTime.Now.ToString());
			SettingsManager.IsMailFriends.Set("");
			SettingsManager.IsMailBlocked.Set("");
		}
		toggleMailBlocked.value = SettingsManager.IsMailBlocked != "";
		toggleMailFriends.value = SettingsManager.IsMailFriends != "";
		toggleMailAll.value = SettingsManager.IsMailAll != "";
	}

	public void OnSettingsVisClicked()
	{
		mailSettings.SetActive(!mailSettings.activeSelf);
		toggleMailBlocked.value = SettingsManager.IsMailBlocked != "";
		toggleMailFriends.value = SettingsManager.IsMailFriends != "";
		toggleMailAll.value = SettingsManager.IsMailAll != "";
	}
}

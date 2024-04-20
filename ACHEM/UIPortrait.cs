using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class UIPortrait : MonoBehaviour, IiconsContext
{
	private int targetID;

	private string targetName;

	private Entity target;

	public UILabel lblName;

	public UILabel level;

	public UILabel arrow;

	public UILabel classRank;

	public UISprite LevelSprite;

	public UISprite RankSprite;

	public UISlider HpSlider;

	public UISlider HpBotSlider;

	public UISlider MpBotSlider;

	public UILabel hp;

	public UILabel FLareLabel;

	public UISlider MpSlider;

	public UILabel mp;

	public GameObject levelContainer;

	public UISprite HpBarColor;

	public GameObject SamuraiExecuteMarkerGO;

	public float Bot_Slider_Delay = 0.5f;

	public float Bot_Slider_Max_Delay = 2.5f;

	public float Top_Slider_Percent_Rate = 3.5f;

	public float Top_Slider_Increase_Rate = 1.75f;

	public float Bot_Slider_Percent_Rate = 1f;

	public GameObject effectPrefab;

	public Transform container;

	private List<UIEffectItem> uiEffects = new List<UIEffectItem>();

	private ObjectPool<GameObject> effectPool;

	private bool didTargetChangeThisFrame;

	private float newHp;

	public float newRp;

	private float hpBotSliderDelay;

	private float hpBotSliderTotalDelay;

	private float rpBotSliderDelay;

	private float rpBotSliderTotalDelay;

	public UICastBar CastBar;

	public UISprite PartyLeader;

	public UISprite Dragon;

	public UISprite Flare;

	public UISprite ClassIcon;

	public UISprite Kickstarter;

	public bool isMobPortrait;

	public Transform contextMenuLocation;

	public List<UIContextIconsMenu.IconImg> iconImages = new List<UIContextIconsMenu.IconImg>();

	private List<string> contextOptions = new List<string>();

	public GameObject[] levelStars;

	public Color[] totalRankColors;

	private bool visible = true;

	public Action<bool> VisibleUpdated;

	public List<string> ContextOptions
	{
		get
		{
			contextOptions.Clear();
			iconImages.Clear();
			if (target != null)
			{
				contextOptions.Add("Inspect");
				iconImages.Add(UIContextIconsMenu.IconImg.Inspect);
			}
			if (target != null && !(target is Player))
			{
				return contextOptions;
			}
			if (targetID == Entities.Instance.me.ID)
			{
				if (PartyManager.IsInParty && !Game.Instance.AreaData.HasPvp)
				{
					contextOptions.Add(InterfaceColors.Chat.Yellow.ToBBCode() + "Leave Party");
					iconImages.Add(UIContextIconsMenu.IconImg.LeaveParty);
				}
				return contextOptions;
			}
			contextOptions.Add("Whisper");
			iconImages.Add(UIContextIconsMenu.IconImg.Whisper);
			if (PartyManager.IsInParty)
			{
				if (PartyManager.IsMember(targetID))
				{
					contextOptions.Add(InterfaceColors.Chat.Yellow.ToBBCode() + "Go To");
					iconImages.Add(UIContextIconsMenu.IconImg.Goto);
					if (PartyManager.IsPrivateLeader)
					{
						contextOptions.Add(InterfaceColors.Chat.Yellow.ToBBCode() + "Promote to Leader");
						contextOptions.Add(InterfaceColors.Chat.Yellow.ToBBCode() + "Remove from Party");
						iconImages.Add(UIContextIconsMenu.IconImg.PromoteToLeader);
						iconImages.Add(UIContextIconsMenu.IconImg.RemoveFromParty);
					}
				}
				else if (PartyManager.IsPrivateLeader)
				{
					contextOptions.Add("Invite to Party");
					iconImages.Add(UIContextIconsMenu.IconImg.InviteParty);
				}
			}
			else
			{
				contextOptions.Add("Invite to Party");
				iconImages.Add(UIContextIconsMenu.IconImg.InviteParty);
			}
			if (Game.Instance.AreaData.isDungeon && !PartyManager.IsPrivate && target is Player && target.areaID == Entities.Instance.me.areaID)
			{
				contextOptions.Add("Vote Kick");
				iconImages.Add(UIContextIconsMenu.IconImg.VoteKick);
			}
			if (Session.MyPlayerData.IsInGuild && target != null && !((Player)target).IsInGuild)
			{
				contextOptions.Add("Send Guild Invite");
				iconImages.Add(UIContextIconsMenu.IconImg.InviteGuild);
			}
			if (!Session.MyPlayerData.IsFriendsWith(targetName))
			{
				contextOptions.Add("Add Friend");
				iconImages.Add(UIContextIconsMenu.IconImg.AddFriend);
			}
			if (Entities.Instance.me.DuelOpponentID == -1 && !Game.Instance.AreaData.isDungeon)
			{
				contextOptions.Add("Request Duel");
				iconImages.Add(UIContextIconsMenu.IconImg.Duel);
			}
			contextOptions.Add(SettingsManager.IsIgnoring(targetName) ? "Unignore" : "Ignore");
			iconImages.Add(UIContextIconsMenu.IconImg.Ignore);
			contextOptions.Add("Report");
			iconImages.Add(UIContextIconsMenu.IconImg.Report);
			return contextOptions;
		}
	}

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				NGUITools.SetActive(base.gameObject, visible);
				VisibleUpdated?.Invoke(visible);
			}
		}
	}

	public Entity Target
	{
		get
		{
			return target;
		}
		set
		{
			if (target != value)
			{
				CleanUp();
				target = value;
				if (target != null)
				{
					Init(target);
				}
				else
				{
					Close();
				}
				didTargetChangeThisFrame = true;
			}
		}
	}

	public void Awake()
	{
		effectPrefab.SetActive(value: false);
		effectPool = new ObjectPool<GameObject>(effectPrefab);
	}

	private void OnEnable()
	{
		PartyManager.PartyUpdated += OnPartyUpdated;
	}

	private void OnDisable()
	{
		PartyManager.PartyUpdated -= OnPartyUpdated;
	}

	public void ClassEquipped(int _)
	{
		UpdateClassRank(Session.MyPlayerData.EquippedClassRank);
	}

	private void OnDestroy()
	{
		ClearTarget();
		UIContextIconsMenu.Close();
	}

	private void OnPartyUpdated()
	{
		PartyLeader.enabled = PartyManager.IsPrivate && PartyManager.LeaderID == targetID;
	}

	public void LateUpdate()
	{
		if (didTargetChangeThisFrame)
		{
			HpSlider.value = newHp;
			MpSlider.value = newRp;
			HpBotSlider.value = HpSlider.value;
			MpBotSlider.value = MpSlider.value;
			didTargetChangeThisFrame = false;
		}
		else
		{
			UpdateSlider(HpSlider, HpBotSlider, newHp, ref hpBotSliderDelay, ref hpBotSliderTotalDelay);
			UpdateSlider(MpSlider, MpBotSlider, newRp, ref rpBotSliderDelay, ref rpBotSliderTotalDelay);
		}
	}

	private void UpdateSlider(UISlider topSlider, UISlider botSlider, float newValue, ref float botSliderDelay, ref float botSliderTotalDelay)
	{
		if (topSlider.value > newValue)
		{
			topSlider.value += GetSliderDelta(Top_Slider_Percent_Rate, newValue, topSlider.value);
			botSliderDelay = Bot_Slider_Delay;
		}
		else
		{
			topSlider.value += GetSliderDelta(Top_Slider_Increase_Rate, newValue, topSlider.value);
		}
		if (botSliderDelay > 0f)
		{
			botSliderDelay -= Time.deltaTime;
			botSliderTotalDelay += Time.deltaTime;
		}
		if ((botSliderDelay <= 0f || botSliderTotalDelay >= Bot_Slider_Max_Delay) && botSlider.value > topSlider.value)
		{
			botSlider.value += GetSliderDelta(Bot_Slider_Percent_Rate, topSlider.value, botSlider.value);
			if (botSlider.value <= topSlider.value)
			{
				botSliderTotalDelay = 0f;
			}
		}
		if (botSlider.value < topSlider.value)
		{
			botSlider.value = topSlider.value;
		}
	}

	private float GetSliderDelta(float rate, float newValue, float oldValue)
	{
		float value = Time.deltaTime * rate;
		value = Mathf.Clamp(value, 0f, Mathf.Abs(newValue - oldValue));
		return (newValue - oldValue > 0f) ? value : (0f - value);
	}

	public void Show(int targetId, string targetName)
	{
		Entity playerByName = Entities.Instance.GetPlayerByName(targetName);
		if (playerByName != null)
		{
			Target = playerByName;
			return;
		}
		ClearTarget();
		Init(targetId, targetName);
	}

	private void ClearTarget()
	{
		if (target != null)
		{
			target.StatUpdateEvent -= UpdateStats;
			target.EffectAdded -= EffectAdded;
			target.LevelUpdated -= UpdateLevel;
			target.ClassUpdated -= TargetClassUpdated;
			target.PortraitUpdated -= UpdateNamePlate;
			target.ClassRankUpdated -= UpdateClassRank;
			SettingsManager.HpRpBarUpdated -= UpdateStats;
			target = null;
		}
		CastBar.Hide();
	}

	public void Close()
	{
		CleanUp();
		Visible = false;
	}

	private void CleanUp()
	{
		ClearTarget();
		UIContextIconsMenu.Close();
	}

	private void UpdateLevel()
	{
		levelContainer.SetActive(value: true);
		level.text = target.Level.ToString();
		arrow.text = "";
		AreaData areaData = Game.Instance.AreaData;
		Player me = Entities.Instance.me;
		if (target != me && areaData.isScaling)
		{
			if (me.Level > areaData.MaxScalingLevel)
			{
				level.text = target.DisplayLevel.ToString();
				arrow.text = "";
			}
			else if (areaData.MinScalingLevel <= me.Level && me.Level <= areaData.MaxScalingLevel)
			{
				level.text = "[4BDD2A]" + target.DisplayLevel + "[-]";
				arrow.text = "[4BDD2A][b]" + ArtixString.UpArrow + "[-][-]";
			}
		}
		UpdateLevelSprite();
	}

	private void UpdateStats()
	{
		UpdateHP(target.CalculateDisplayStat(Stat.Health), target.CalculateDisplayStat(Stat.MaxHealth));
		UpdateRP(target.CalculateDisplayStat(Stat.Resource), target.CalculateDisplayStat(Stat.MaxResource));
	}

	private void EffectAdded(Effect effect)
	{
		AddEffect(effect);
		SortEffects();
	}

	private void OnUIEffectDestroyed(UIEffectItem uiEffect)
	{
		DestroyUIEffect(uiEffect);
		SortEffects();
	}

	private void AddEffect(Effect effect)
	{
		if (effect.template.type != EffectTemplate.EffectType.Passive && !effect.template.hideIcon && (!(effect.duration > 0f) || !(effect.timestampApplied + effect.duration < GameTime.realtimeSinceServerStartup)) && (!Entities.Instance.me.CanAttack(Target) || !effect.template.isHarmful || effect.casterID == Entities.Instance.me.ID || effect.template.status != Entity.StatusType.None || !effect.template.onlyAffectsHealth || !SettingsManager.CondenseEffects))
		{
			GameObject obj = effectPool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIEffectItem component = obj.GetComponent<UIEffectItem>();
			component.Destroyed += OnUIEffectDestroyed;
			component.SetItem(effect);
			uiEffects.Add(component);
		}
	}

	public void UpdateNamePlate(int plateNumber)
	{
		if (!isMobPortrait)
		{
			switch (plateNumber)
			{
			case 1:
			{
				Dragon.spriteName = "SilverWings";
				UISprite dragon4 = Dragon;
				Color color2 = (Flare.color = InterfaceColors.Portrait.White);
				dragon4.color = color2;
				break;
			}
			case 2:
			{
				Dragon.spriteName = "SilverDragon";
				UISprite dragon3 = Dragon;
				Color color2 = (Flare.color = InterfaceColors.Portrait.White);
				dragon3.color = color2;
				break;
			}
			case 3:
			{
				Dragon.spriteName = "SilverDragon";
				UISprite dragon2 = Dragon;
				Color color2 = (Flare.color = InterfaceColors.Portrait.Gold);
				dragon2.color = color2;
				break;
			}
			case 4:
			{
				Dragon.spriteName = "SilverKickstarter";
				UISprite dragon = Dragon;
				Color color2 = (Flare.color = InterfaceColors.Portrait.Gold);
				dragon.color = color2;
				break;
			}
			case 5:
				Dragon.spriteName = "GoldLegendaryKS";
				Flare.color = InterfaceColors.Portrait.Gold;
				Dragon.color = new Color32(191, 172, 132, byte.MaxValue);
				break;
			}
		}
	}

	private void Init(Entity targetEntity)
	{
		targetID = targetEntity.ID;
		targetName = targetEntity.name;
		targetEntity.EffectAdded += EffectAdded;
		targetEntity.LevelUpdated += UpdateLevel;
		targetEntity.ClassRankUpdated += UpdateClassRank;
		targetEntity.StatUpdateEvent += UpdateStats;
		targetEntity.ClassUpdated += TargetClassUpdated;
		targetEntity.PortraitUpdated += UpdateNamePlate;
		SettingsManager.HpRpBarUpdated += UpdateStats;
		bool flag = targetEntity.IsInPvp && targetEntity.CanAttack(Entities.Instance.me);
		lblName.text = (flag ? targetEntity.CombatClass.Name : targetName);
		PartyLeader.enabled = PartyManager.IsPrivate && PartyManager.LeaderID == targetID;
		ClassIcon.spriteName = target.ClassIcon;
		MpSlider.foregroundWidget.color = InterfaceColors.Resource.GetColor(target);
		hp.text = "";
		mp.text = "";
		newHp = (newRp = 1f);
		UpdateLevel();
		UpdateStats();
		CastBar.Init(targetEntity);
		ClearEffects();
		foreach (Effect effect in targetEntity.effects)
		{
			AddEffect(effect);
		}
		SortEffects();
		UpdateNamePlate(targetEntity.Portrait);
		if (target.AccessLevel >= 50)
		{
			Flare.gameObject.SetActive(value: true);
			FLareLabel.gameObject.SetActive(value: true);
			Kickstarter.gameObject.SetActive(value: false);
			if (target.AccessLevel >= 100)
			{
				Flare.color = InterfaceColors.Names.Staff;
				FLareLabel.text = "S";
			}
			else if (target.AccessLevel >= 60)
			{
				Flare.color = InterfaceColors.Names.Mod;
				FLareLabel.text = "M";
			}
			else if (target.AccessLevel >= 55)
			{
				Flare.color = InterfaceColors.Names.WhiteHat;
				FLareLabel.text = "W";
			}
			else if (target.AccessLevel >= 50)
			{
				Flare.color = InterfaceColors.Names.Tester;
				FLareLabel.text = "T";
			}
		}
		else
		{
			Flare.gameObject.SetActive(value: false);
		}
		Visible = true;
		if (target.type == Entity.Type.Player)
		{
			UpdateLevelSprite();
			UpdateClassRank(target.EquippedClassRank);
		}
		else
		{
			classRank.transform.parent.gameObject.SetActive(value: false);
		}
	}

	private void SortEffects()
	{
		uiEffects.Sort();
		int num = 0;
		foreach (UIEffectItem uiEffect in uiEffects)
		{
			uiEffect.gameObject.transform.SetSiblingIndex(num++);
		}
		container.GetComponent<UIGrid>().Reposition();
	}

	private void TargetClassUpdated()
	{
		ClassIcon.spriteName = target.ClassIcon;
		MpSlider.foregroundWidget.color = InterfaceColors.Resource.GetColor(target);
		if (target.resource == Entity.Resource.None)
		{
			mp.text = "";
			newRp = 1f;
			didTargetChangeThisFrame = true;
		}
	}

	private void Init(int entityId, string entityName)
	{
		targetID = entityId;
		targetName = entityName;
		lblName.text = targetName;
		PartyLeader.enabled = PartyManager.IsPrivate && PartyManager.LeaderID == targetID;
		levelContainer.SetActive(value: false);
		MpSlider.foregroundWidget.color = InterfaceColors.Resource.None;
		hp.text = "";
		mp.text = "";
		newHp = (newRp = 1f);
		CastBar.Hide();
		ClearEffects();
		UpdateNamePlate(1);
		Visible = true;
	}

	private void UpdateClassRank(int classRank)
	{
		if (target.TotalClassRank % 100 == 0)
		{
			this.classRank.text = "100";
		}
		else
		{
			this.classRank.text = (target.TotalClassRank % 100).ToString();
		}
		int num = target.TotalClassRank / 500;
		if (target.TotalClassRank % 500 < 101)
		{
			num--;
		}
		int num2 = (target.TotalClassRank - 100) / 100 % 5 + 1;
		if ((target.TotalClassRank - 100) / 100 % 5 == 0 && (target.TotalClassRank - 100) % 500 == 0)
		{
			num2 = 5;
		}
		if (target.TotalClassRank <= 100)
		{
			num2 = 0;
		}
		switch (num2)
		{
		case 1:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 2:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 3:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		case 4:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		case 5:
			levelStars[0].SetActive(value: true);
			levelStars[1].SetActive(value: true);
			levelStars[2].SetActive(value: true);
			levelStars[3].SetActive(value: true);
			levelStars[4].SetActive(value: true);
			break;
		default:
			levelStars[0].SetActive(value: false);
			levelStars[1].SetActive(value: false);
			levelStars[2].SetActive(value: false);
			levelStars[3].SetActive(value: false);
			levelStars[4].SetActive(value: false);
			break;
		}
		for (int i = 0; i < 5; i++)
		{
			if (levelStars[i].activeSelf)
			{
				levelStars[i].GetComponent<UISprite>().color = totalRankColors[num];
			}
		}
		UpdateRankSprite();
	}

	private void UpdateRankSprite()
	{
		RankSprite.spriteName = "Icon-TotalRank";
		classRank.color = Color.black;
	}

	private void UpdateLevelSprite()
	{
		LevelSprite.spriteName = ((target.Level < Session.MyPlayerData.LevelCap) ? "Icon-CircleGray" : "Icon-CircleGold");
	}

	private void ClearEffects()
	{
		for (int num = uiEffects.Count - 1; num >= 0; num--)
		{
			DestroyUIEffect(uiEffects[num]);
		}
		uiEffects.Clear();
	}

	private void DestroyUIEffect(UIEffectItem uiEffect)
	{
		uiEffect.Destroyed -= OnUIEffectDestroyed;
		effectPool.Release(uiEffect.gameObject);
		uiEffects.Remove(uiEffect);
	}

	private void UpdateHP(float hpCurrent, float hpMax)
	{
		if (!(hpMax > 0f) || !(hpCurrent >= 0f))
		{
			return;
		}
		float num = (newHp = hpCurrent / hpMax);
		hp.text = "";
		if ((bool)SettingsManager.ShowCurrentHp)
		{
			hp.text += hpCurrent;
			if ((bool)SettingsManager.ShowMaxHp)
			{
				hp.text += "/";
			}
		}
		if ((bool)SettingsManager.ShowMaxHp)
		{
			hp.text += hpMax;
		}
		if ((bool)SettingsManager.ShowPercentHp)
		{
			string text = (((double)num < 0.01) ? ($"{num * 100f:F1}" + "%") : (Mathf.FloorToInt(num * 100f) + "%"));
			if ((bool)SettingsManager.ShowCurrentHp || (bool)SettingsManager.ShowMaxHp)
			{
				UILabel uILabel = hp;
				uILabel.text = uILabel.text + "  (" + text + ")";
			}
			else
			{
				hp.text += text;
			}
		}
		if (!(SamuraiExecuteMarkerGO != null) || !(HpBarColor != null) || target == Entities.Instance.me)
		{
			return;
		}
		if (Session.MyPlayerData.EquippedClassID == 24 || Session.MyPlayerData.EquippedClassID == 25)
		{
			SamuraiExecuteMarkerGO.SetActive(value: true);
			if ((double)num <= 0.4)
			{
				HpBarColor.color = Color.yellow;
			}
			else
			{
				HpBarColor.color = Color.red;
			}
		}
		else
		{
			SamuraiExecuteMarkerGO.SetActive(value: false);
			HpBarColor.color = Color.red;
		}
	}

	private void UpdateRP(float rpCurrent, float rpMax)
	{
		if (rpMax <= 0f || rpCurrent < 0f)
		{
			return;
		}
		float num = (newRp = rpCurrent / rpMax);
		MpSlider.foregroundWidget.color = InterfaceColors.Resource.GetColor(target);
		mp.text = "";
		if ((bool)SettingsManager.ShowCurrentRp)
		{
			mp.text += rpCurrent;
			if ((bool)SettingsManager.ShowMaxRp)
			{
				mp.text += "/";
			}
		}
		if ((bool)SettingsManager.ShowMaxRp)
		{
			mp.text += rpMax;
		}
		if ((bool)SettingsManager.ShowPercentRp)
		{
			string text = Mathf.CeilToInt(num * 100f) + "%";
			if ((bool)SettingsManager.ShowCurrentRp || (bool)SettingsManager.ShowMaxRp)
			{
				UILabel uILabel = mp;
				uILabel.text = uILabel.text + "  (" + text + ")";
			}
			else
			{
				mp.text += text;
			}
		}
	}

	public void OnClick()
	{
		if (UIContextIconsMenu.instance != null)
		{
			UIContextIconsMenu.Close();
		}
		else
		{
			ShowContextMenu(this, ContextOptions, iconImages);
		}
	}

	private void OnFriendSelect()
	{
		if (Session.MyPlayerData.IsFriendsWith(targetName))
		{
			UIFriendsList.Show();
			return;
		}
		Confirmation.Show("Add Friend", "Would you like to send a friend request to " + targetName + "?", delegate(bool b)
		{
			if (b)
			{
				if (Session.MyPlayerData.CanAddMoreFriends)
				{
					Game.Instance.SendFriendRequest(targetID);
				}
				else
				{
					Chat.Notify("You can only have have up to " + 80 + " friends.", InterfaceColors.Chat.Red.ToBBCode());
				}
			}
		});
	}

	private void OnDuelSelect()
	{
		string challengeeName = Entities.Instance.GetPlayerById(targetID).name;
		Game.Instance.SendPvPDuelChallengeRequest(challengeeName);
	}

	private void OnPartyPromote()
	{
		if (!PartyManager.IsMember(targetID) || !PartyManager.IsPrivateLeader)
		{
			return;
		}
		Confirmation.Show("Promote Leader", "Are you sure you want to promote " + targetName + " to party leader?  You will no longer be able to manage the party.", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendPartyPromoteRequest(targetID);
			}
		});
	}

	private void OnVoteKick()
	{
		Game.Instance.SendVoteKickCommand(targetName.ToLower());
	}

	private void OnPartyInvite()
	{
		if (PartyManager.IsPartyFull())
		{
			Notification.ShowText("Party is full");
		}
		else if (!PartyManager.IsMember(targetID))
		{
			Notification.ShowText("Invite sent!");
			Game.Instance.SendPartyInviteRequest(targetName);
		}
		else
		{
			Chat.Notify("You are already in a party with " + targetName);
		}
	}

	private void OnGuildInvite()
	{
		if (Session.MyPlayerData.Guild.guildMembers[Session.MyPlayerData.ID].GuildRole == GuildRole.Member)
		{
			Chat.Notify("Must be an Officer or above to send invites.");
		}
		else if (Session.MyPlayerData.Guild.MemberLimitHasNotReached)
		{
			Game.Instance.SendGuildInviteRequest(targetName);
		}
		else
		{
			Chat.Notify("Guild member limit has been reached.");
		}
	}

	public void kickClick(GameObject go)
	{
		if (!PartyManager.IsPrivateLeader)
		{
			return;
		}
		string text = "Kick " + targetName + " out of the ";
		text += (Game.Instance.AreaData.isDungeon ? "dungeon?" : "party?");
		Confirmation.Show("Kick", text, delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendPartyRemoveRequest(targetID);
			}
		});
	}

	private void OnWhisper()
	{
		Chat.Notify("Send a private message to " + targetName);
		Chat.Instance.SetWhisper(targetName);
	}

	public void ContextSelect(int i)
	{
		try
		{
			string text = ContextOptions[i];
			if (text == InterfaceColors.Chat.Yellow.ToBBCode() + "Go To")
			{
				gotoClick(targetID);
				return;
			}
			switch (text)
			{
			case "Inspect":
				UICharInfo.Show(target);
				return;
			case "Unignore":
				Game.Instance.UnIgnore(targetName);
				return;
			case "Ignore":
				Game.Instance.Ignore(targetName);
				return;
			case "Report":
				UICharReport.Show(targetName);
				return;
			}
			if (text == InterfaceColors.Chat.Yellow.ToBBCode() + "Leave Party")
			{
				Game.Instance.SendPartyRemoveRequest(Entities.Instance.me.ID);
				return;
			}
			if (text == InterfaceColors.Chat.Yellow.ToBBCode() + "Remove from Party")
			{
				kickClick(base.gameObject);
				return;
			}
			if (text == InterfaceColors.Chat.Yellow.ToBBCode() + "Promote to Leader")
			{
				OnPartyPromote();
				return;
			}
			switch (text)
			{
			case "Invite to Party":
				OnPartyInvite();
				break;
			case "Send Guild Invite":
				OnGuildInvite();
				break;
			case "Whisper":
				OnWhisper();
				break;
			case "Add Friend":
				OnFriendSelect();
				break;
			case "Request Duel":
				OnDuelSelect();
				break;
			case "Vote Kick":
				OnVoteKick();
				break;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	public void ShowContextMenu(IiconsContext parent, List<string> args, List<UIContextIconsMenu.IconImg> iconImgs)
	{
		if (contextOptions.Count > 0)
		{
			if (contextOptions.Count == 1)
			{
				ContextSelect(0);
			}
			else
			{
				UIContextIconsMenu.Show(this, contextMenuLocation, contextOptions, iconImages);
			}
		}
	}

	private void gotoClick(int id)
	{
		Game.Instance.SendPartyGotoRequest(id);
	}
}

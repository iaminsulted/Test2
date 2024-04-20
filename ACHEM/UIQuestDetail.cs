using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIQuestDetail : MonoBehaviour
{
	private enum QuestStatus
	{
		Accept,
		Abandon,
		Turnin,
		Ok
	}

	private static Color32 DefaultColor = new Color32(141, 89, 49, byte.MaxValue);

	private static Color32 DisabledColor = new Color32(142, 124, 110, byte.MaxValue);

	private static Color32 Green = new Color32(56, 118, 29, byte.MaxValue);

	private static Color32 Red = new Color32(200, 0, 0, byte.MaxValue);

	private const int UnknownMapID = 344;

	public static List<string> StatusText = new List<string> { "Accept", "Abandon", "Turn In", "OK" };

	private QuestStatus status;

	public Quest quest;

	private QuestMode Mode;

	public UIQuest uiquest;

	public UIScrollView scrollview;

	public UITable table;

	public UIGrid grid;

	public UILabel description;

	public UILabel lvlRequirement;

	public UILabel objectiveLabel;

	public UILabel objectives;

	public UITable rewardTable;

	public UILabel classXPLabel;

	public UILabel rewardGold;

	public UILabel rewardXP;

	public UILabel rewardClassXP;

	public UILabel questAction;

	public UILabel shortDesc;

	public UILabel turninText;

	public UILabel itemReward;

	public UIButton btnAction;

	public GameObject TrackQuestContainer;

	public GameObject TurninTextContainer;

	public UIButton btnTeleport;

	public UIButton btnTrack;

	public GameObject itemGOprefab;

	public GameObject speechBubbleArrow;

	public UISprite white;

	private List<UIItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	public UIButtonGlowParticle UIButtonGlowParticle;

	private List<int> turninIDs;

	public UIItem uiSelectedItem;

	public Queue<int> ChosenRewardIDs = new Queue<int>();

	private bool visible;

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
				base.gameObject.SetActive(value);
			}
		}
	}

	public void Init()
	{
		Session.MyPlayerData.QuestAdded += OnQuestAdded;
		Session.MyPlayerData.QuestRemoved += OnQuestRemoved;
		Session.MyPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
		Session.MyPlayerData.CurrentlyTrackedQuestUpdated += OnCurrentlyTrackedQuestUpdated;
		UIEventListener uIEventListener = UIEventListener.Get(btnAction.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnTrack.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnTrackQuest));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnTeleport.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnTeleportClick));
		itemGOs = new List<UIItem>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void LoadQuest(QuestMode mode, Quest quest, List<int> turninIDs)
	{
		this.quest = quest;
		Mode = mode;
		this.turninIDs = turninIDs;
		ChosenRewardIDs.Clear();
		refresh();
	}

	private void refresh()
	{
		btnAction.enabled = true;
		status = QuestStatus.Ok;
		if (Session.MyPlayerData.CurQuests.Contains(quest.ID))
		{
			if ((quest.TurnInType == QuestTurnInType.Auto || quest.TurnInType == QuestTurnInType.Anywhere || (Mode == QuestMode.Quest && turninIDs.Contains(quest.ID))) && Session.MyPlayerData.IsQuestComplete(quest.ID))
			{
				status = QuestStatus.Turnin;
			}
			if (quest.TurnInType == QuestTurnInType.Auto && status == QuestStatus.Turnin)
			{
				return;
			}
			if (!quest.Auto && Mode != 0)
			{
				status = QuestStatus.Abandon;
			}
		}
		else
		{
			status = QuestStatus.Accept;
			if (!Session.MyPlayerData.IsQuestAcceptable(quest))
			{
				btnAction.GetComponent<UISprite>().color = Color.gray;
			}
			else
			{
				btnAction.GetComponent<UISprite>().color = Color.white;
			}
		}
		int num = quest.QuestRewardOptionQty - ChosenRewardIDs.Count;
		if (num == 1 && quest.QuestRewardType == QuestRewardType.Choice)
		{
			itemReward.text = "Select " + num + " rewards!";
		}
		else if (num >= 2 && quest.QuestRewardType == QuestRewardType.Choice)
		{
			itemReward.text = "Select " + num + " rewards!";
		}
		else if (quest.QuestRewardType == QuestRewardType.Random)
		{
			itemReward.text = "Receive " + quest.QuestRewardOptionQty + " random rewards!";
		}
		else
		{
			itemReward.text = "Item Rewards";
		}
		UIButtonGlowParticle.gameObject.SetActive(status == QuestStatus.Accept || status == QuestStatus.Turnin);
		foreach (UIItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
			itemGO.Clicked -= OnItemClicked;
		}
		itemGOs.Clear();
		shortDesc.text = quest.DisplayName;
		string text = ((status == QuestStatus.Turnin) ? quest.EndNPCName : quest.StartNPCName);
		string text2 = ((status == QuestStatus.Turnin) ? quest.EndText : quest.StartText);
		if (!string.IsNullOrEmpty(text2))
		{
			description.text = "";
			if (!string.IsNullOrEmpty(text))
			{
				description.text = "[d8000d][b]" + text + "[/b][-]\n";
			}
			UILabel uILabel = description;
			uILabel.text = uILabel.text + "[222222]" + Entities.Instance.me.ScrubText(text2) + "[-]";
			while (description.height < 60)
			{
				description.text += "\n";
			}
			description.gameObject.SetActive(value: true);
			speechBubbleArrow.SetActive(Mode == QuestMode.Quest);
		}
		else
		{
			description.gameObject.SetActive(value: false);
			speechBubbleArrow.SetActive(value: false);
		}
		white.ResetAndUpdateAnchors();
		if (quest.Objectives.Count > 1)
		{
			objectiveLabel.text = "    Objectives";
		}
		else
		{
			objectiveLabel.text = "    Objective";
		}
		string text3 = "";
		foreach (QuestObjective objective in quest.Objectives)
		{
			text3 = text3 + Session.MyPlayerData.GetQuestObjectiveProgressText(objective) + "\n";
		}
		if (text3.Length > 0)
		{
			text3 = text3.Substring(0, text3.Length - 1);
		}
		objectives.text = text3;
		if (Session.MyPlayerData.GoldMultiplier != 1f)
		{
			float num2 = (float)quest.GoldReward() / Session.MyPlayerData.GoldMultiplier;
			rewardGold.text = (int)num2 + " + " + (int)((float)quest.GoldReward() - num2);
		}
		else
		{
			rewardGold.text = quest.GoldReward().ToString();
		}
		if (Session.MyPlayerData.XPMultiplier != 1f)
		{
			float num3 = (float)quest.XPReward() / Session.MyPlayerData.XPMultiplier;
			rewardXP.text = (int)num3 + " + " + (int)((float)quest.XPReward() - num3);
		}
		else
		{
			rewardXP.text = quest.XPReward().ToString();
		}
		int num4 = quest.ClassXPReward(Entities.Instance.me.Level, Entities.Instance.me.EquippedClassRank);
		float num5 = (float)num4 / Session.MyPlayerData.CXPMultiplier;
		if (Session.MyPlayerData.CXPMultiplier != 1f)
		{
			rewardClassXP.text = (int)num5 + " + " + (int)((float)num4 - num5);
		}
		else
		{
			rewardClassXP.text = num4.ToString();
		}
		classXPLabel.gameObject.SetActive(num4 > 0);
		rewardClassXP.gameObject.SetActive(num4 > 0);
		rewardTable.Reposition();
		if (quest.Rewards.Count == 0)
		{
			grid.gameObject.SetActive(value: false);
		}
		else
		{
			foreach (QuestRewardItem reward in quest.Rewards)
			{
				GameObject obj = itemGOpool.Get();
				obj.transform.SetParent(grid.transform, worldPositionStays: false);
				obj.SetActive(value: true);
				UIItem component = obj.GetComponent<UIItem>();
				component.Init(reward);
				component.Clicked += OnItemClicked;
				component.CheckIcon.spriteName = "aq3d-ui-checkmark";
				itemGOs.Add(component);
			}
			grid.gameObject.SetActive(value: true);
			grid.Reposition();
		}
		TrackQuestContainer.SetActive(Session.MyPlayerData.HasQuest(quest.ID));
		UpdateTrackButton();
		questAction.text = StatusText[(int)status];
		if (status == QuestStatus.Accept && quest.IsSagaQuest && quest.QSValue == 0)
		{
			questAction.text = "Start Saga!";
		}
		if (quest.QuestRewardType > QuestRewardType.Default && ChosenRewardIDs.Count < quest.QuestRewardOptionQty)
		{
			num = quest.QuestRewardOptionQty - ChosenRewardIDs.Count;
			if (num == 1 && quest.QuestRewardType == QuestRewardType.Choice)
			{
				itemReward.text = "Select " + num + " rewards!";
			}
			else if (num >= 2 && quest.QuestRewardType == QuestRewardType.Choice)
			{
				itemReward.text = "Select " + num + " rewards!";
			}
			else if (quest.QuestRewardType == QuestRewardType.Random)
			{
				itemReward.text = "Receive " + quest.QuestRewardOptionQty + " random rewards!";
			}
			else
			{
				itemReward.text = "Item Rewards";
			}
		}
		TurninTextContainer.gameObject.SetActive(value: false);
		turninText.gameObject.SetActive(value: false);
		if (Session.MyPlayerData.IsQuestComplete(quest.ID))
		{
			if (quest.TurnInType == QuestTurnInType.QuestGiver && status != QuestStatus.Turnin)
			{
				TurninTextContainer.gameObject.SetActive(value: true);
				turninText.gameObject.SetActive(value: true);
				if (!string.IsNullOrEmpty(quest.TurninText))
				{
					turninText.text = Entities.Instance.me.ScrubText(quest.TurninText);
				}
				else
				{
					NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(quest.TurnInNpcSpawnId);
					if (npcBySpawnId != null)
					{
						if (quest.EndNPCName != "")
						{
							turninText.text = "Return to " + quest.EndNPCName + " in " + quest.EndMapName + ".";
						}
						if (quest.MapID == Game.CurrentAreaID && quest.TurnInNpcCellId == Game.CurrentCellID)
						{
							turninText.text = "Return to " + npcBySpawnId.name + " in " + quest.EndMapName + ".";
						}
						else
						{
							turninText.text = "Return to " + quest.EndMapName + ".";
						}
					}
					else
					{
						turninText.text = "Return to " + quest.EndMapName + ".";
					}
				}
			}
		}
		else if (Session.MyPlayerData.HasQuest(quest.ID))
		{
			foreach (QuestObjective objective2 in quest.Objectives)
			{
				if (Session.MyPlayerData.IsQuestObjectiveInProgress(quest.ID, objective2.ID))
				{
					if (!objective2.IsMapHidden && objective2.MapID != Game.CurrentAreaID)
					{
						turninText.gameObject.SetActive(value: true);
						turninText.text = "Travel to " + objective2.MapName + ".";
					}
					break;
				}
			}
		}
		if (Entities.Instance.me.Level < quest.RequiredLevel)
		{
			lvlRequirement.enabled = true;
			lvlRequirement.text = "Requires Level " + quest.RequiredLevel;
			lvlRequirement.color = Red;
		}
		else if (quest.IsScaling && Entities.Instance.me.ScaledLevel < Entities.Instance.me.Level)
		{
			lvlRequirement.enabled = true;
			lvlRequirement.text = "Rewards Scaled to Level " + Entities.Instance.me.GetRelativeLevel(Levels.GetScaledLevel(Entities.Instance.me.Level, quest.MapLevel, quest.MapMaxLevel), quest.Level);
			lvlRequirement.color = Green;
		}
		else
		{
			lvlRequirement.enabled = false;
			lvlRequirement.text = "";
		}
		table.Reposition();
		scrollview.ResetPosition();
	}

	private void Complete()
	{
		if (Session.MyPlayerData.CanSendUniqueRewardToBank(quest))
		{
			MessageBox.Show("Quest Reward", "One or more unique items given in this quest reward are in your bank.\n\nTransferring these reward items to your bank...", "Okay!", delegate
			{
				if (quest.QuestRewardType == QuestRewardType.Choice)
				{
					Game.Instance.SendQuestCompleteRequest(quest.ID, ChosenRewardIDs);
				}
				if (quest.QuestRewardType == QuestRewardType.Random)
				{
					List<QuestRewardItem> list3 = quest.Rewards.ToList();
					System.Random random = new System.Random(DateTime.Now.Millisecond);
					list3.OrderBy((QuestRewardItem x) => random.Next()).Take(quest.QuestRewardOptionQty);
					for (int j = 0; j < quest.QuestRewardOptionQty; j++)
					{
						ChosenRewardIDs.Enqueue(list3[j].ID);
					}
					Game.Instance.SendQuestCompleteRequest(quest.ID, ChosenRewardIDs);
					list3.Clear();
				}
				else
				{
					Game.Instance.SendQuestCompleteRequest(quest.ID);
				}
			});
			return;
		}
		if (quest.QuestRewardType == QuestRewardType.Choice)
		{
			Game.Instance.SendQuestCompleteRequest(quest.ID, ChosenRewardIDs);
		}
		if (quest.QuestRewardType == QuestRewardType.Random)
		{
			List<QuestRewardItem> list = quest.Rewards.ToList();
			System.Random random = new System.Random();
			random.Next();
			List<QuestRewardItem> list2 = list.OrderBy((QuestRewardItem a) => random.Next()).Take(quest.QuestRewardOptionQty).ToList();
			for (int i = 0; i < quest.QuestRewardOptionQty; i++)
			{
				ChosenRewardIDs.Enqueue(list2[i].ID);
			}
			Game.Instance.SendQuestCompleteRequest(quest.ID, ChosenRewardIDs);
			list.Clear();
		}
		else
		{
			Game.Instance.SendQuestCompleteRequest(quest.ID);
		}
	}

	private void OnBtnActionClick(GameObject go)
	{
		switch (status)
		{
		case QuestStatus.Turnin:
		{
			string text = Session.MyPlayerData.CanTurnIn(quest, ChosenRewardIDs);
			if (string.IsNullOrEmpty(text))
			{
				if (quest.QuestRewardType == QuestRewardType.Choice && ChosenRewardIDs.Count < quest.QuestRewardOptionQty)
				{
					Notification.ShowText("You must select the listed amount of rewards before turning in this quest");
					break;
				}
				Complete();
				btnAction.enabled = false;
			}
			else if (text.Equals("Unique item is at max stack", StringComparison.CurrentCulture))
			{
				Confirmation.Show("Quest Reward", "You already have the maximum amount of one or more of these unique item rewards.\n\nYou will not receive any more!\n\nDo you still want to turn in this quest?", delegate(bool b)
				{
					if (b)
					{
						Complete();
					}
					btnAction.enabled = false;
				});
			}
			else
			{
				Notification.ShowText(text);
			}
			break;
		}
		case QuestStatus.Abandon:
			Confirmation.Show("Abandon", "You will lose any progress on this quest. Are you sure you want to abandon the quest?", delegate(bool conf)
			{
				if (conf)
				{
					Game.Instance.SendQuestAbandonRequest(quest.ID);
				}
			});
			break;
		case QuestStatus.Accept:
		{
			if (Session.MyPlayerData.IsQuestAcceptable(quest))
			{
				Game.Instance.SendQuestAcceptRequest(quest.ID);
				break;
			}
			string questLockInfo = Session.MyPlayerData.GetQuestLockInfo(quest);
			if (!string.IsNullOrEmpty(questLockInfo))
			{
				Notification.ShowWarning(questLockInfo);
			}
			break;
		}
		default:
			uiquest.Back();
			break;
		}
	}

	private void OnQuestAdded(int questID)
	{
		if (quest.ID == questID)
		{
			refresh();
		}
	}

	private void OnQuestRemoved(int questID)
	{
	}

	private void OnQuestObjectiveUpdated(int questID, int qoID)
	{
		if (quest.ID == questID)
		{
			refresh();
		}
	}

	public void Destroy()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.QuestAdded -= OnQuestAdded;
			Session.MyPlayerData.QuestRemoved -= OnQuestRemoved;
			Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
			Session.MyPlayerData.CurrentlyTrackedQuestUpdated -= OnCurrentlyTrackedQuestUpdated;
			ChosenRewardIDs.Clear();
		}
	}

	private void OnTrackQuest(GameObject go)
	{
		if (Session.MyPlayerData.CurrentlyTrackedQuest == quest)
		{
			Notification.ShowText("You are already tracking this quest.");
		}
		else
		{
			Session.MyPlayerData.TrackQuest(quest.ID);
		}
	}

	private void OnTeleportClick(GameObject go)
	{
		if (quest != null)
		{
			if (quest.TargetMapId == 344)
			{
				Notification.ShowText("Target map unknown.");
			}
			else if (Game.Instance.AreaData.id == quest.TargetMapId)
			{
				Notification.ShowText("You are already in this map.");
			}
			else
			{
				Game.Instance.SendAreaJoinRequest(quest.TargetMapId);
			}
		}
	}

	private void OnCurrentlyTrackedQuestUpdated(Quest trackedQuest)
	{
		UpdateTrackButton();
	}

	private void UpdateTrackButton()
	{
		if (Session.MyPlayerData.CurrentlyTrackedQuest == quest)
		{
			btnTrack.defaultColor = DisabledColor;
			btnTrack.hover = DisabledColor;
			btnTrack.pressed = DisabledColor;
		}
		else
		{
			btnTrack.defaultColor = DefaultColor;
			btnTrack.hover = DefaultColor;
			btnTrack.pressed = DefaultColor;
		}
		btnTrack.UpdateColor(instant: true);
	}

	private void OnItemClicked(UIItem selectedItem)
	{
		if (quest.QuestRewardType != QuestRewardType.Choice || status != QuestStatus.Turnin || !(selectedItem != null) || ChosenRewardIDs.Contains(selectedItem.Item.ID))
		{
			return;
		}
		if (ChosenRewardIDs.Count >= quest.QuestRewardOptionQty)
		{
			ChosenRewardIDs.Dequeue();
		}
		ChosenRewardIDs.Enqueue(selectedItem.Item.ID);
		foreach (UIItem itemGO in itemGOs)
		{
			if (ChosenRewardIDs.Contains(itemGO.Item.ID) && status == QuestStatus.Turnin)
			{
				itemGO.CheckIcon.gameObject.SetActive(value: true);
			}
			else
			{
				itemGO.CheckIcon.gameObject.SetActive(value: false);
			}
		}
		int num = quest.QuestRewardOptionQty - ChosenRewardIDs.Count;
		if (num == 1 && quest.QuestRewardType == QuestRewardType.Choice && status == QuestStatus.Turnin)
		{
			itemReward.text = "Select " + num + " more item!";
		}
		else if (num >= 2 && quest.QuestRewardType == QuestRewardType.Choice && status == QuestStatus.Turnin)
		{
			itemReward.text = "Select " + num + " more items!";
		}
		else if (quest.QuestRewardType == QuestRewardType.Random && status == QuestStatus.Turnin)
		{
			itemReward.text = "Receive " + quest.QuestRewardOptionQty + " of these items:";
		}
		else
		{
			itemReward.text = "Item Rewards";
		}
	}
}

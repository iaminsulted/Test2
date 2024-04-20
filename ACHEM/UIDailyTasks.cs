using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIDailyTasks : UIMenuWindow
{
	private const int Level_Restriction = 3;

	private static UIDailyTasks instance;

	public GameObject TaskListItemTemplate;

	public GameObject MetaTask;

	public GameObject NonEndGameMetaDisplay;

	public GameObject TaskHeader;

	public GameObject EndGameMetaDisplay;

	public UILabel TaskXPRewardLabel;

	public UILabel TaskGoldRewardLabel;

	public UILabel NonEndGameMetaXpRewardLabel;

	public UILabel NonEndGameMetaGoldRewardLabel;

	public UISlider MetaTaskBar;

	public UILabel EndGameXPLabel;

	public UILabel EndGamegoldLabel;

	public UILabel EndGameRuneRewardLabel;

	public UISprite EndGameRuneIcon;

	public UIGrid RankTable;

	public Color[] completedColors;

	public CombinedDailyTask metaDaily;

	public int fakeTasks;

	private void Start()
	{
		Session.MyPlayerData.DailyTaskUpdated += UpdateTaskUI;
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			Show();
		}
		else
		{
			instance.Close();
		}
	}

	public static void Show()
	{
		if (CheckAccess() && instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/DailyTaskUI"), UIManager.Instance.transform).GetComponent<UIDailyTasks>();
			instance.Init();
			instance.UpdateTaskUI();
		}
	}

	private static bool CheckAccess()
	{
		if (Entities.Instance.me.Level < 3)
		{
			MessageBox.Show("Daily Tasks", "Daily Tasks are available starting at Level " + 3 + "!");
			return false;
		}
		return true;
	}

	protected override void Init()
	{
		fakeTasks = ArtixRandom.Range(1, 1000);
		if (fakeTasks > 999)
		{
			TaskHeader.GetComponent<UILabel>().text = "Fake Tasks";
		}
		base.Init();
	}

	private void UpdateTaskUI()
	{
		if (TaskListItemTemplate == null)
		{
			return;
		}
		TaskListItemTemplate.SetActive(value: true);
		int i = 1;
		for (int childCount = RankTable.transform.childCount; i < childCount; i++)
		{
			RankTable.transform.GetChild(i).gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(RankTable.transform.GetChild(i).gameObject);
		}
		if (Session.MyPlayerData.serverDailyTasks[0] == null)
		{
			return;
		}
		List<CharDailyTask> list = Session.MyPlayerData.charDailyTasks.OrderBy((CharDailyTask x) => x.taskID).ToList();
		List<DailyTask> list2 = Session.MyPlayerData.serverDailyTasks.OrderBy((DailyTask x) => x.taskID).ToList();
		List<CombinedDailyTask> list3 = new List<CombinedDailyTask>();
		for (int j = 0; j < list.Count; j++)
		{
			int xp = (int)Math.Ceiling((float)Levels.GetXPToLevel(Entities.Instance.me.Level, Session.MyPlayerData.LevelCap) * list2[j].xpRewardMultiplier * Session.MyPlayerData.DailyTaskMultiplier);
			int gold = (int)Math.Ceiling((float)Gold.GetBaseQuestGold(Entities.Instance.me.Level) * list2[j].goldRewardMultiplier);
			CombinedDailyTask item = new CombinedDailyTask(list[j].taskID, list[j].curQty, list2[j].targetQty, list2[j].desc, xp, gold, list[j].collected, list2[j].visible, list2[j].category, list2[j].metaTask);
			if (list2[j].metaTask)
			{
				metaDaily = item;
			}
			else if (list2[j].visible)
			{
				list3.Add(item);
			}
		}
		list3 = (from x in list3
			orderby x.completionPercent descending, x.collected
			select x).ToList();
		TaskListItemTemplate.transform.GetChild(0).GetComponent<UILabel>().text = "";
		TaskXPRewardLabel.text = list3[0].xpReward.ToString() ?? "";
		TaskGoldRewardLabel.text = list3[0].goldReward.ToString() ?? "";
		foreach (CombinedDailyTask item2 in list3)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(TaskListItemTemplate, RankTable.transform);
			gameObject.transform.GetChild(2).GetComponent<DailyTaskButtonData>().task = item2;
			gameObject.transform.GetChild(0).GetComponent<UILabel>().text = item2.desc;
			gameObject.transform.GetChild(1).GetChild(5).GetComponent<UILabel>()
				.text = item2.xpReward.ToString() ?? "";
			gameObject.transform.GetChild(1).GetChild(6).GetComponent<UILabel>()
				.text = item2.goldReward.ToString() ?? "";
			if (item2.completed && item2.collected)
			{
				gameObject.transform.GetChild(1).GetChild(1).GetComponent<UISprite>()
					.color = completedColors[0];
				gameObject.transform.GetChild(2).GetComponent<UIButton>().isEnabled = false;
				gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
					.text = "COMPLETE";
				gameObject.transform.GetChild(6).gameObject.SetActive(value: true);
				gameObject.transform.GetChild(6).gameObject.GetComponent<UISprite>().color = completedColors[2];
				gameObject.transform.GetChild(7).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(1).GetChild(5).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(1).GetChild(6).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(8).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(9).gameObject.SetActive(value: false);
			}
			else if (item2.completed)
			{
				gameObject.transform.GetChild(1).GetChild(1).GetComponent<UISprite>()
					.color = completedColors[0];
				gameObject.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
					.text = "(" + item2.curQty + "/" + item2.targetQty + ")";
				gameObject.transform.GetChild(2).GetComponent<UIButton>().isEnabled = true;
				gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: true);
				gameObject.transform.GetChild(6).gameObject.SetActive(value: true);
				gameObject.transform.GetChild(6).gameObject.GetComponent<UISprite>().color = completedColors[1];
				gameObject.transform.GetChild(7).gameObject.SetActive(value: true);
			}
			else
			{
				gameObject.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
					.text = "(" + item2.curQty + "/" + item2.targetQty + ")";
				gameObject.transform.GetChild(2).GetComponent<UIButton>().isEnabled = false;
				gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(5).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(6).gameObject.SetActive(value: false);
				gameObject.transform.GetChild(7).gameObject.SetActive(value: false);
			}
			if (!item2.collected)
			{
				gameObject.transform.GetChild(1).GetComponent<UISlider>().value = item2.completionPercent;
			}
			else
			{
				gameObject.transform.GetChild(1).GetComponent<UISlider>().value = 1f;
			}
		}
		if (!metaDaily.collected)
		{
			MetaTaskBar.value = metaDaily.completionPercent;
		}
		else
		{
			MetaTaskBar.value = 1f;
		}
		fillDetails(MetaTask, metaDaily);
		TaskListItemTemplate.SetActive(value: false);
		RankTable.Reposition();
	}

	private void fillDetails(GameObject taskTemplateUI, CombinedDailyTask combinedDaily)
	{
		taskTemplateUI.transform.GetChild(2).GetComponent<DailyTaskButtonData>().task = combinedDaily;
		taskTemplateUI.transform.GetChild(0).GetComponent<UILabel>().text = combinedDaily.desc;
		if (Entities.Instance.me.Level == Session.MyPlayerData.EndGame)
		{
			Item item = Items.Get(Infusion.GetInertRuneIDForLevel(Entities.Instance.me.Level));
			EndGameMetaDisplay.SetActive(value: true);
			NonEndGameMetaDisplay.SetActive(value: false);
			EndGameRuneIcon.spriteName = item.Icon;
			EndGameRuneRewardLabel.text = item.Name + " x" + Infusion.GetRunesPerDay();
			EndGameXPLabel.text = combinedDaily.xpReward.ToString() ?? "";
			EndGamegoldLabel.text = combinedDaily.goldReward.ToString() ?? "";
		}
		else
		{
			EndGameMetaDisplay.SetActive(value: false);
			NonEndGameMetaDisplay.SetActive(value: true);
			NonEndGameMetaXpRewardLabel.text = combinedDaily.xpReward.ToString() ?? "";
			NonEndGameMetaGoldRewardLabel.text = combinedDaily.goldReward.ToString() ?? "";
		}
		if (combinedDaily.completed && combinedDaily.collected)
		{
			taskTemplateUI.transform.GetChild(6).GetComponent<UISprite>().color = completedColors[0];
			taskTemplateUI.transform.GetChild(2).GetComponent<UIButton>().isEnabled = false;
			taskTemplateUI.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: false);
			taskTemplateUI.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
				.text = "COMPLETE!";
			taskTemplateUI.transform.GetChild(6).gameObject.SetActive(value: true);
			taskTemplateUI.transform.GetChild(6).gameObject.GetComponent<UISprite>().color = completedColors[2];
			taskTemplateUI.transform.GetChild(7).gameObject.SetActive(value: false);
		}
		else if (combinedDaily.completed)
		{
			taskTemplateUI.transform.GetChild(6).GetComponent<UISprite>().color = completedColors[0];
			taskTemplateUI.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
				.text = combinedDaily.curQty + "/" + combinedDaily.targetQty;
			taskTemplateUI.transform.GetChild(2).GetComponent<UIButton>().isEnabled = true;
			taskTemplateUI.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: true);
			taskTemplateUI.transform.GetChild(6).gameObject.SetActive(value: true);
			taskTemplateUI.transform.GetChild(6).gameObject.GetComponent<UISprite>().color = completedColors[1];
			taskTemplateUI.transform.GetChild(7).gameObject.SetActive(value: true);
		}
		else
		{
			taskTemplateUI.transform.GetChild(1).GetChild(3).GetComponent<UILabel>()
				.text = combinedDaily.curQty + "/" + combinedDaily.targetQty;
			taskTemplateUI.transform.GetChild(2).GetComponent<UIButton>().isEnabled = false;
			taskTemplateUI.transform.GetChild(2).GetChild(0).gameObject.SetActive(value: false);
			taskTemplateUI.transform.GetChild(6).gameObject.SetActive(value: false);
			taskTemplateUI.transform.GetChild(7).gameObject.SetActive(value: false);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class UIDailyRewards : UIWindow
{
	private static UIDailyRewards instance;

	private Transform container;

	public UILabel DailyProgressNumberLabel;

	public UIGrid DailyRewardsScreenGrid;

	public GameObject DailyRewardPrefab;

	public GameObject RewardWindow;

	private int DailyProgressNumber;

	public UISprite sprChest;

	public static void Load(UIPreviewLoot UIPreviewLoot)
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/DailyRewards"), UIManager.Instance.transform).GetComponent<UIDailyRewards>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		RewardWindow.SetActive(value: false);
		StartCoroutine(WaitForDailyLootInfo());
	}

	protected void OnEnable()
	{
		Session.MyPlayerData.DailyRewardUpdated += OnDailyRewardUpdated;
	}

	protected void OnDisable()
	{
		Session.MyPlayerData.DailyRewardUpdated -= OnDailyRewardUpdated;
	}

	private void OnDailyRewardUpdated(int day, DateTime date, int itemID)
	{
		Close();
		UIPreviewLoot.OpenChest(itemID);
	}

	private IEnumerator WaitForDailyLootInfo()
	{
		if (DailyRewards.IsEmpty)
		{
			using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Game/GetDailyRewards");
			string errorTitle = "Loading Error";
			string friendlyMsg = "Failed to load daily rewards.";
			string customContext = "URL: " + www.url;
			yield return www.SendWebRequest();
			customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
			if (www.isHttpError)
			{
				ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			}
			else if (www.isNetworkError)
			{
				ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else if (www.error != null)
			{
				ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			}
			else
			{
				try
				{
					foreach (KeyValuePair<int, Item> item in JsonConvert.DeserializeObject<Dictionary<int, Item>>(www.downloadHandler.text))
					{
						Item value = item.Value;
						Items.Add(value);
						DailyRewards.Add(item.Key, value);
					}
				}
				catch (Exception ex)
				{
					ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext);
				}
			}
		}
		Refresh();
	}

	public void Refresh()
	{
		DailyProgressNumber = Session.MyPlayerData.RewardIndex;
		DailyProgressNumberLabel.text = DailyProgressNumber + "/28";
		DailyRewardsScreenGrid.transform.DestroyChildren();
		for (int i = 1; i <= DailyRewards.map.Count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(DailyRewardPrefab, DailyRewardsScreenGrid.transform);
			obj.gameObject.SetActive(value: true);
			DailyRewardGridButton component = obj.GetComponent<DailyRewardGridButton>();
			component.SetDailyItem(i.ToString(), DailyRewards.GetChestIcon(i));
			if (i < DailyProgressNumber)
			{
				component.XSprite.gameObject.SetActive(value: true);
			}
			else if (i == DailyProgressNumber)
			{
				component.Glow.gameObject.SetActive(value: true);
				component.DayNumber.text = "";
				component.StartTimer();
			}
			if (DailyRewards.map[i].Rarity >= RarityType.Rare)
			{
				component.TweenChest.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			}
		}
		DailyRewardsScreenGrid.Reposition();
	}

	public void RemoveRewardWindow()
	{
		RewardWindow.SetActive(value: false);
	}

	public void OpenChest()
	{
	}
}

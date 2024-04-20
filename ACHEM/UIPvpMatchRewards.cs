using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchRewards : MonoBehaviour
{
	private const float MaxGlowAlpha = 1f;

	private const float MinGlowAlpha = 0.6f;

	private const float MaxGlowSpeed = 0.15f;

	private const float MinGlowSpeed = 0.05f;

	private const float RewardDelay = 0.25f;

	private const string RewardsPath = "UIElements/Pvp/Rewards/";

	private readonly Dictionary<PvpMatchRewardType, string> rewardPrefabPaths = new Dictionary<PvpMatchRewardType, string>
	{
		{
			PvpMatchRewardType.Gold,
			"UIPvpMatchReward_Gold"
		},
		{
			PvpMatchRewardType.MarkOfGlory,
			"UIPvpMatchReward_MarkOfGlory"
		},
		{
			PvpMatchRewardType.XP,
			"UIPvpMatchReward_XP"
		},
		{
			PvpMatchRewardType.ClassXP,
			"UIPvpMatchReward_ClassXP"
		},
		{
			PvpMatchRewardType.Glory,
			"UIPvpMatchReward_Glory"
		}
	};

	[SerializeField]
	private UISprite glow;

	[SerializeField]
	private UIGrid grid;

	private Dictionary<PvpMatchRewardType, GameObject> rewardPrefabs = new Dictionary<PvpMatchRewardType, GameObject>();

	private void OnEnable()
	{
		glow.alpha = 0.6f;
		StartCoroutine(IncreaseGlow());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Init(Dictionary<PvpMatchRewardType, int> rewards)
	{
		ClearRewards();
		AddRewards(rewards);
	}

	public void Show()
	{
		int num = 0;
		foreach (PvpMatchRewardType key in rewardPrefabs.Keys)
		{
			rewardPrefabs[key].GetComponent<UIPvpMatchReward>().Show(0.25f * (float)num);
			num++;
		}
	}

	public void Hide()
	{
		foreach (PvpMatchRewardType key in rewardPrefabs.Keys)
		{
			rewardPrefabs[key].GetComponent<UIPvpMatchReward>().Hide();
		}
	}

	private void ClearRewards()
	{
		foreach (PvpMatchRewardType key in rewardPrefabs.Keys)
		{
			rewardPrefabs[key].SetActive(value: false);
			Object.Destroy(rewardPrefabs[key]);
		}
		rewardPrefabs.Clear();
	}

	private void AddRewards(Dictionary<PvpMatchRewardType, int> rewards)
	{
		foreach (PvpMatchRewardType key in rewards.Keys)
		{
			GameObject value = Object.Instantiate(Resources.Load("UIElements/Pvp/Rewards/" + rewardPrefabPaths[key]) as GameObject, grid.transform);
			rewardPrefabs.Add(key, value);
		}
		grid.Reposition();
		foreach (PvpMatchRewardType key2 in rewardPrefabs.Keys)
		{
			rewardPrefabs[key2].GetComponent<UIPvpMatchReward>().Init(rewards[key2]);
		}
	}

	private float GetRandomGlowAlpha()
	{
		return Random.Range(0.6f, 1f);
	}

	private float GetRandomGlowSpeed()
	{
		return Random.Range(0.05f, 0.15f);
	}

	private IEnumerator IncreaseGlow()
	{
		float glowAlpha = GetRandomGlowAlpha();
		float glowSpeed = GetRandomGlowSpeed();
		while (true)
		{
			glow.alpha += Time.deltaTime * glowSpeed;
			if (glow.alpha >= glowAlpha)
			{
				break;
			}
			yield return null;
		}
		glow.alpha = glowAlpha;
		StartCoroutine(DecreaseGlow());
	}

	private IEnumerator DecreaseGlow()
	{
		float glowAlpha = GetRandomGlowAlpha();
		float glowSpeed = GetRandomGlowSpeed();
		while (true)
		{
			glow.alpha -= Time.deltaTime * glowSpeed;
			if (glow.alpha <= glowAlpha)
			{
				break;
			}
			yield return null;
		}
		glow.alpha = glowAlpha;
		StartCoroutine(IncreaseGlow());
	}
}

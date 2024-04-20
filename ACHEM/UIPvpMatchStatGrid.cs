using System.Collections.Generic;
using UnityEngine;

public class UIPvpMatchStatGrid : MonoBehaviour
{
	[SerializeField]
	private UIGrid grid;

	[SerializeField]
	private GameObject labelPrefab;

	private List<GameObject> labelPrefabs = new List<GameObject>();

	private List<UIPvpMatchStatLabel> highestStatLabels = new List<UIPvpMatchStatLabel>();

	private int currentHighestStat = 1;

	public void AddStats(PvpMatchStatType statData, List<int> stats)
	{
		Clear();
		bool flag = CanHighlight(statData);
		foreach (int stat in stats)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("UIElements/PvP/Stats/UIPvPMatchStatLabel") as GameObject, base.transform);
			labelPrefabs.Add(gameObject);
			UIPvpMatchStatLabel component = gameObject.GetComponent<UIPvpMatchStatLabel>();
			component.Stat = stat;
			if (!flag)
			{
				continue;
			}
			if (stat == currentHighestStat)
			{
				highestStatLabels.Add(component);
				component.MarkAsHighestStat();
			}
			else if (stat > currentHighestStat)
			{
				highestStatLabels.ForEach(delegate(UIPvpMatchStatLabel x)
				{
					x.MarkAsNormal();
				});
				highestStatLabels.Clear();
				highestStatLabels.Add(component);
				currentHighestStat = stat;
				component.MarkAsHighestStat();
			}
		}
		grid.Reposition();
	}

	private bool CanHighlight(PvpMatchStatType statData)
	{
		if (statData != PvpMatchStatType.CombatDamage && statData != PvpMatchStatType.HealingDone)
		{
			return statData == PvpMatchStatType.PointsCaptured;
		}
		return true;
	}

	private void Clear()
	{
		foreach (GameObject labelPrefab in labelPrefabs)
		{
			labelPrefab.SetActive(value: false);
			Object.Destroy(labelPrefab);
		}
		labelPrefabs.Clear();
		highestStatLabels.Clear();
	}
}

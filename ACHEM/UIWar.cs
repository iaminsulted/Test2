using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIWar : MonoBehaviour
{
	public WarBar WarBar;

	public UIGrid Grid;

	private Dictionary<int, WarBar> WarBars = new Dictionary<int, WarBar>();

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible == value)
			{
				return;
			}
			visible = value;
			if (visible)
			{
				foreach (War item in Wars.All)
				{
					WarBar warBar = Object.Instantiate(WarBar, Grid.transform);
					warBar.gameObject.SetActive(value: true);
					warBar.gameObject.name = "WarBar_" + item.Name;
					warBar.Init(item);
					WarBars.Add(item.WarID, warBar);
				}
				Grid.Reposition();
				return;
			}
			foreach (WarBar value2 in WarBars.Values)
			{
				Object.Destroy(value2.gameObject);
			}
			WarBars.Clear();
		}
	}

	protected virtual void Awake()
	{
		WarBar.gameObject.name = "WarBar_TEMPLATE";
		WarBar.gameObject.SetActive(value: false);
		Visible = false;
		Wars.WarsUpdated += OnWarsUpdated;
	}

	private void OnWarsUpdated()
	{
		foreach (War item in Wars.All)
		{
			if (WarBars.ContainsKey(item.WarID))
			{
				WarBars[item.WarID].Init(item);
			}
		}
		War war = Wars.All.Where((War p) => p.ProgressPercent >= 1f).FirstOrDefault();
		if (war != null)
		{
			FancyNotification.ShowText("War has Ended", war.Name + " won the war!");
		}
	}

	protected virtual void OnDestroy()
	{
		Wars.WarsUpdated -= OnWarsUpdated;
	}
}

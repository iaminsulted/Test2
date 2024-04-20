using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIEmotes : UIWindow
{
	public UIScrollView scrollview;

	public UITable table;

	public UILabel title;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIEmoteButton> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	private float lastEmoteTS = float.MinValue;

	private static UIEmotes instance;

	public static void Show()
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/Emotes"), UIManager.Instance.transform).GetComponent<UIEmotes>();
			instance.Init();
		}
		instance.Refresh();
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

	protected override void Init()
	{
		base.Init();
		itemGOs = new List<UIEmoteButton>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		container = itemGOprefab.transform.parent;
		Refresh();
	}

	private void Refresh()
	{
		foreach (UIEmoteButton itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (Emote item in (from p in Emotes.GetUnlockedEmotes()
			orderby p.name
			select p).ToList())
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIEmoteButton component = obj.GetComponent<UIEmoteButton>();
			component.Load(item);
			component.Clicked += OnItemClicked;
			itemGOs.Add(component);
		}
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		base.gameObject.SetActive(value: true);
	}

	private void OnItemClicked(Emote emote)
	{
		if (GameTime.realtimeSinceServerStartup - lastEmoteTS > 1f)
		{
			lastEmoteTS = GameTime.realtimeSinceServerStartup;
			Game.Instance.SendEmoteRequest(emote.em);
		}
	}
}

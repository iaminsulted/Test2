using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIChatCommands : UIWindow
{
	public GameObject listItemPrefab;

	public UIInput SearchInput;

	private static UIChatCommands instance;

	private Transform container;

	private List<UIChatCommandItem> listItems;

	private ObjectPool<GameObject> listItemPool;

	private string searchText;

	public static void Show()
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ChatCommands"), UIManager.Instance.transform).GetComponent<UIChatCommands>();
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
		listItems = new List<UIChatCommandItem>();
		listItemPool = new ObjectPool<GameObject>(listItemPrefab);
		listItemPrefab.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		container = listItemPrefab.transform.parent;
		Refresh();
	}

	public static void OnCommandsLoaded()
	{
		instance?.Refresh();
	}

	private void Refresh()
	{
		if (!ChatCommands.AreAllCommandsLoaded)
		{
			ChatCommands.LoadServerCommands();
			return;
		}
		EmptyList();
		AddCommands();
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		base.gameObject.SetActive(value: true);
	}

	private void EmptyList()
	{
		foreach (UIChatCommandItem listItem in listItems)
		{
			listItemPool.Release(listItem.gameObject);
		}
		listItems.Clear();
	}

	private void AddCommands()
	{
		List<ChatCommand> list = ChatCommands.GetAllCommands();
		if (!string.IsNullOrEmpty(searchText))
		{
			list = list.Where((ChatCommand command) => DoesCommandContainText(command, searchText)).ToList();
		}
		foreach (ChatCommand item in list)
		{
			GameObject obj = listItemPool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIChatCommandItem component = obj.GetComponent<UIChatCommandItem>();
			component.Load(item);
			listItems.Add(component);
		}
	}

	private bool DoesCommandContainText(ChatCommand command, string text)
	{
		bool num = command.command.ToLower().Contains(text);
		bool flag = command.description.ToLower().Contains(text);
		bool flag2 = command.aliases.Any((string alias) => alias.ToLower().Contains(text));
		ChatCommands.Role result;
		bool flag3 = Enum.TryParse<ChatCommands.Role>(text, ignoreCase: true, out result) && (command.permission & result) > ChatCommands.Role.None;
		return num || flag || flag2 || flag3;
	}

	public void UpdateSearch()
	{
		searchText = SearchInput.value.ToLower();
		Refresh();
	}

	public void ClearSearch()
	{
		searchText = "";
		SearchInput.value = "";
		Refresh();
	}
}

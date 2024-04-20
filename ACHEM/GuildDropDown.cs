using System.Collections.Generic;
using UnityEngine;

public class GuildDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public GameObject[] Hidden;

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public UILabel guildName;

	public UILabel guildTag;

	private Player entity;

	private void Start()
	{
		Refresh();
	}

	private void Refresh()
	{
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
		if (entity.IsInGuild)
		{
			guildName.text = entity.guildName;
			guildTag.text = entity.guildTag;
		}
	}

	public void OnButtonClick()
	{
		isOpen = !isOpen;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		Refresh();
		Grid.Reposition();
	}

	public void Load(Player entity)
	{
		this.entity = entity;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class StatsDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public GameObject[] Hidden;

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	private Entity.Type type;

	private void Start()
	{
		Refresh();
	}

	private void Refresh()
	{
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
	}

	public void OnButtonClick()
	{
		isOpen = !isOpen;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		if (type == Entity.Type.NPC)
		{
			GameObject[] hidden = Hidden;
			for (int i = 0; i < hidden.Length; i++)
			{
				hidden[i].SetActive(value: false);
			}
		}
		Refresh();
		Grid.Reposition();
	}

	public void Load(Entity entity)
	{
		type = entity.type;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		if (type == Entity.Type.NPC)
		{
			GameObject[] hidden = Hidden;
			for (int i = 0; i < hidden.Length; i++)
			{
				hidden[i].SetActive(value: false);
			}
		}
	}
}

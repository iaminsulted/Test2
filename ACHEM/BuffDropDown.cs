using System.Collections.Generic;
using UnityEngine;

public class BuffDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public void Add(GameObject content)
	{
		Contents.Add(content);
		content.transform.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
	}

	public void Clear()
	{
		foreach (GameObject content in Contents)
		{
			content.SetActive(value: false);
			Object.Destroy(content.gameObject);
		}
		Contents.Clear();
		Grid.Reposition();
	}

	public void Refresh()
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
		Refresh();
		Grid.Reposition();
	}
}

using System.Collections.Generic;
using UnityEngine;

public class ClassDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public int Tier;

	public UILabel Label;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

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
		Refresh();
		Grid.Reposition();
	}
}

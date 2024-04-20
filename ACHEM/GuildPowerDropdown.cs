using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildPowerDropdown : MonoBehaviour
{
	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public UITable table;

	public UILabel lblTitle;

	public UIButton folderButton;

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(folderButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnButtonClick));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(folderButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnButtonClick));
	}

	public void OnButtonClick(GameObject go)
	{
		isOpen = !isOpen;
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		UpdateIcons();
		foreach (GameObject content2 in Contents)
		{
			content2.SetActive(isOpen);
		}
		table.Reposition();
	}

	private void UpdateIcons()
	{
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
	}
}

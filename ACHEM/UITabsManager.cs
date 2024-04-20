using System.Collections.Generic;
using UnityEngine;

public class UITabsManager : MonoBehaviour
{
	[Header("Set to 0 for 1st tab")]
	public int DefaultTab;

	[Header("Need to set pages in same order as tabs")]
	public GameObject[] TabPages;

	private List<UIButton> tabButtons = new List<UIButton>();

	private void Start()
	{
		foreach (Transform item in base.transform)
		{
			tabButtons.Add(item.GetComponent<UIButton>());
		}
		GameObject[] tabPages = TabPages;
		for (int i = 0; i < tabPages.Length; i++)
		{
			tabPages[i].SetActive(value: false);
		}
		TabPages[DefaultTab].SetActive(value: true);
	}

	public void ChangeTab(int tab)
	{
		GameObject[] tabPages = TabPages;
		for (int i = 0; i < tabPages.Length; i++)
		{
			tabPages[i].SetActive(value: false);
		}
		TabPages[tab].SetActive(value: true);
		TabPages[tab].GetComponent<IRefreshable>()?.Refresh();
		foreach (UIButton tabButton in tabButtons)
		{
			_ = tabButton;
		}
	}
}

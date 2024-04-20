using System;
using System.Collections.Generic;
using UnityEngine;

public class BankTabGroup : MonoBehaviour
{
	public GameObject BankTabObject;

	public Transform GridTransform;

	public Action<int> SelectTabAction;

	private List<GameObject> banks = new List<GameObject>();

	public UIButton btnAddBankSlot;

	public UIScrollView scrollview;

	public void Init(int bankCount, int selectedBank, bool scrollToBottom = false)
	{
		if (banks.Count > 0)
		{
			foreach (GameObject bank in banks)
			{
				bank.SetActive(value: false);
				UnityEngine.Object.Destroy(bank);
			}
		}
		btnAddBankSlot.gameObject.SetActive(Session.MyPlayerData.BankVaultCount < 100);
		banks.Clear();
		if (Session.MyPlayerData.CurrentBankVault > 0)
		{
			for (int i = 1; i <= Session.MyPlayerData.CurrentBankVault; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(BankTabObject);
				gameObject.transform.SetParent(GridTransform, worldPositionStays: false);
				gameObject.SetActive(value: true);
				BankTab component = gameObject.GetComponent<BankTab>();
				bool flag = (selectedBank == 0 && i == 1) || (selectedBank != 0 && i == selectedBank);
				component.Init(this, i, flag);
				gameObject.GetComponent<UIToggle>().value = flag;
				banks.Add(gameObject);
			}
			if (Session.MyPlayerData.BankVaultCount < 100)
			{
				btnAddBankSlot.transform.SetAsLastSibling();
			}
			GridTransform.gameObject.GetComponent<UIGrid>().Reposition();
			scrollview.ResetPosition();
			if (scrollToBottom && scrollview.shouldMoveVertically)
			{
				scrollview.SetDragAmount(0f, 1f, updateScrollbars: false);
			}
		}
	}

	public void BroadcastClick(int BankID)
	{
		if (SelectTabAction != null)
		{
			SelectTabAction(BankID);
		}
	}
}

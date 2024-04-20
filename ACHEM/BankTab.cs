using System;
using UnityEngine;

public class BankTab : MonoBehaviour
{
	public int ID;

	public UILabel BankNumber;

	private BankTabGroup bg;

	private bool didInit;

	public void OnClick()
	{
		bg.BroadcastClick(ID);
	}

	private void OnDisable()
	{
		BankTabGroup bankTabGroup = bg;
		bankTabGroup.SelectTabAction = (Action<int>)Delegate.Remove(bankTabGroup.SelectTabAction, new Action<int>(SelectedBank));
	}

	public void Init(BankTabGroup group, int id, bool isSelected = false)
	{
		bg = group;
		ID = id;
		BankNumber.text = id.ToString();
		if (!didInit)
		{
			BankTabGroup bankTabGroup = bg;
			bankTabGroup.SelectTabAction = (Action<int>)Delegate.Combine(bankTabGroup.SelectTabAction, new Action<int>(SelectedBank));
		}
		didInit = true;
	}

	public void SelectedBank(int BankID)
	{
	}
}

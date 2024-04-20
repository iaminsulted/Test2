using System;
using StatCurves;
using UnityEngine;

public class ConfirmationSpendTokens : ModalWindow
{
	public UILabel QuantityLabel;

	public UILabel ClassXPLabel;

	public UILabel DescLabel;

	public UISlider QuantitySlider;

	public UILabel TokensLabel;

	public UIButton SpendButton;

	public UIButton CloseButton;

	public UISprite ClassIcon;

	private int maxValue;

	private int ClassID;

	private void Awake()
	{
		UIEventListener uIEventListener = UIEventListener.Get(SpendButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSpendClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(CloseButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	public void Refresh(CombatClass combatClass)
	{
		if (!combatClass.HasClassTokens)
		{
			Close();
			MessageBox.Show("Coming Soon", "Tokens not yet available for this class.");
			return;
		}
		ClassID = combatClass.ID;
		DescLabel.text = "Spend Class Tokens\nand get " + 4 + " Class XP for each.";
		int inventoryItemCount = Session.MyPlayerData.GetInventoryItemCount(combatClass.ClassTokenID);
		TokensLabel.text = inventoryItemCount.ToString();
		ClassIcon.spriteName = combatClass.Icon;
		int num = ClassRanks.GetMaxClassXP() - combatClass.ToCharClass().ClassXP;
		maxValue = Mathf.CeilToInt((float)num / 4f);
		maxValue = Mathf.Min(maxValue, inventoryItemCount, maxValue);
		if (maxValue <= 0)
		{
			Close();
			MessageBox.Show("Already Max Rank", "This class already has the maximum Class Rank. You cannot spend anymore tokens on Class XP for this class.");
		}
		else
		{
			QuantitySlider.numberOfSteps = maxValue;
			QuantitySlider.value = 0f;
			SetQty();
		}
	}

	public void SetQty()
	{
		int num = Mathf.RoundToInt((float)(maxValue - 1) * QuantitySlider.value) + 1;
		QuantityLabel.text = num + "/" + maxValue;
		ClassXPLabel.text = "Class EXP +" + num * 4;
	}

	private void OnSpendClick(GameObject go)
	{
		Mathf.RoundToInt((float)(maxValue - 1) * QuantitySlider.value);
		Close();
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	protected override void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}

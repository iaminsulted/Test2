using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ConfirmationGetTokens : ModalWindow
{
	public UILabel TitleLabel;

	[FormerlySerializedAs("Message")]
	public UILabel MessageLabel;

	public UIButton BuyButton;

	public UIButton CloseButton;

	public UIButton TravelButton;

	[FormerlySerializedAs("lblCost")]
	public UILabel CostLabel;

	[FormerlySerializedAs("lblMyGems")]
	public UILabel GemsLabel;

	public GameObject Balance;

	private CombatClass combatClass;

	private string travelLocation;

	private int travelToMapID;

	private void Awake()
	{
		UIEventListener uIEventListener = UIEventListener.Get(TravelButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnTravelClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BuyButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBuyClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(CloseButton.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	public void RefreshToGetClass(CombatClass combatClass)
	{
		this.combatClass = combatClass;
		TitleLabel.text = "Unlock Class";
		Balance.SetActive(value: true);
		BuyButton.gameObject.SetActive(value: true);
		Vector3 localPosition = TravelButton.transform.localPosition;
		localPosition.x = -86f;
		TravelButton.transform.localPosition = localPosition;
		int inventoryItemCount = Session.MyPlayerData.GetInventoryItemCount(combatClass.ClassTokenID);
		int num = combatClass.ClassTokenCost - inventoryItemCount;
		int classCostInDCs = Session.MyPlayerData.GetClassCostInDCs(combatClass);
		if (!Session.MyPlayerData.MeetsClassUnlockReqs(combatClass.ID))
		{
			TravelButton.gameObject.SetActive(value: false);
			localPosition = BuyButton.transform.localPosition;
			localPosition.x = 0f;
			BuyButton.transform.localPosition = localPosition;
			MessageLabel.text = "Complete all [FFCD5C]Token Requirements[-], then collect " + combatClass.Name + " Tokens to unlock this class. You may also use [486995]" + classCostInDCs + " Dragon Crystals[-] to unlock " + combatClass.Name + " now!";
		}
		else if (!combatClass.NeedsTokensToUnlock || !combatClass.HasClassTrainer)
		{
			TravelButton.gameObject.SetActive(value: false);
			localPosition = BuyButton.transform.localPosition;
			localPosition.x = 0f;
			BuyButton.transform.localPosition = localPosition;
			MessageLabel.text = "Class trainer not available yet. Class can be unlocked now for [486995]" + classCostInDCs + " Dragon Crystals[-]!";
		}
		else
		{
			TravelButton.gameObject.SetActive(value: true);
			localPosition = BuyButton.transform.localPosition;
			localPosition.x = 92f;
			BuyButton.transform.localPosition = localPosition;
			MessageLabel.text = "Unlock " + combatClass.Name + " by visiting [FFCD5C]" + combatClass.GetTokensNpcName + "[-] and completing daily quests for " + combatClass.Name + " Tokens, or use [486995]Dragon Crystals[-] to buy the remaining " + num + " Tokens and unlock the class now!";
			travelToMapID = combatClass.GetTokensMapID;
		}
		CostLabel.text = classCostInDCs.ToString();
		GemsLabel.text = Session.MyPlayerData.MC.ToString("n0");
	}

	public void RefreshToTravel(CombatClass combatClass, bool redirectedFromSpendTokens = false)
	{
		this.combatClass = combatClass;
		if (combatClass.GetTokensNpcName == null || combatClass.GetTokensMapName == null)
		{
			Close();
			MessageBox.Show("Coming Soon", "Tokens not yet available for this class.");
			return;
		}
		Balance.SetActive(value: false);
		BuyButton.gameObject.SetActive(value: false);
		Vector3 localPosition = TravelButton.transform.localPosition;
		localPosition.x = 0f;
		TravelButton.transform.localPosition = localPosition;
		TitleLabel.text = (redirectedFromSpendTokens ? "No Tokens" : "Get Tokens");
		MessageLabel.text = "Travel to [FFCD5C]" + combatClass.GetTokensMapName + "[-], then speak to [FFCD5C]" + combatClass.GetTokensNpcName + "[-] and complete daily quests to obtain more " + combatClass.Name + " Tokens!";
		if (redirectedFromSpendTokens)
		{
			MessageLabel.text = "You have no tokens to spend. " + MessageLabel.text;
		}
		travelToMapID = combatClass.GetTokensMapID;
	}

	protected void OnTravelClick(GameObject go)
	{
		Game.Instance.SendTransferMapRequest(travelToMapID, 1, 1, showConfirmation: false);
		Close();
	}

	protected void OnBuyClick(GameObject go)
	{
		int classCostInDCs = Session.MyPlayerData.GetClassCostInDCs(combatClass);
		if (Session.MyPlayerData.MC >= classCostInDCs)
		{
			Game.Instance.SendClassAddRequest(combatClass.ID);
		}
		else
		{
			Confirmation.Show("Insuffient funds", "You do not have enough [486995]Dragon Crystals[-] to unlock this class.", delegate(bool yes)
			{
				if (yes)
				{
					UIIAPStore.Show();
				}
			});
		}
		Close();
	}

	protected void OnCloseClick(GameObject go)
	{
		Close();
	}

	protected override void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class UIClassSummaryTab : MonoBehaviour, IRefreshable
{
	public UILabel ClassDescription;

	public UILabel UnlockInfo;

	public UISprite TokenIcon1;

	public UILabel TokenLabel1;

	public UILabel TokenQuantityLabel1;

	public UISprite TokenIcon2;

	public UILabel TokenLabel2;

	public UILabel TokenQuantityLabel2;

	public UISprite CosmeticIcon;

	public UILabel ClassRankLabel;

	public UILabel ClassRankXPLabel;

	public UILabel ClassTokensLabel;

	public UISlider ClassRankXPBar;

	public UIButton GetTokensButton;

	public UIButton SpendTokensButton;

	public UISprite ClassIcon;

	public ConfirmationGetTokens ConfirmationGetTokens;

	public ConfirmationSpendTokens ConfirmationSpendTokens;

	public GameObject Tokens;

	public UIButton EquipButton;

	public UILabel BuyLabel;

	public UITexture ClassImage;

	public GameObject LoadingIcon;

	public UICharClassesUnlockSlider UnlockSlider;

	public GameObject ChecklistItemPrefab;

	public GameObject ChecklistTransform;

	public GameObject UnlockRequirements;

	public GameObject[] Stars = new GameObject[3];

	public UITable CheckListTable;

	public GameObject ButtonGlow;

	private CharClass charClass;

	private CombatClass combatClass;

	private List<GameObject> ChecklistObjects = new List<GameObject>();

	private void OnEnable()
	{
		Session.MyPlayerData.ClassXPUpdated += OnClassXPUpdated;
		UIEventListener uIEventListener = UIEventListener.Get(EquipButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnEquipButtonClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(GetTokensButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnGetTokensButtonClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(SpendTokensButton.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSpendTokensButtonClicked));
		LoadClassImage();
	}

	private void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassXPUpdated -= OnClassXPUpdated;
		}
		UIEventListener uIEventListener = UIEventListener.Get(EquipButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnEquipButtonClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(GetTokensButton.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnGetTokensButtonClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(SpendTokensButton.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSpendTokensButtonClicked));
	}

	private void OnClassXPUpdated(int classID, int classXP)
	{
		RefreshClassXPBar();
	}

	public void OnEquipButtonClick(GameObject go)
	{
		if (combatClass == null)
		{
			return;
		}
		if (Session.MyPlayerData.OwnsClass(combatClass.ID))
		{
			switch (Entities.Instance.me.serverState)
			{
			case Entity.State.InCombat:
				Notification.ShowText("Cannot equip during combat");
				break;
			case Entity.State.Dead:
				Notification.ShowText("Cannot equip while dead");
				break;
			default:
				Game.Instance.SendClassEquipRequest(combatClass.ID);
				break;
			}
		}
		else if (Session.MyPlayerData.IsClassAvailable(combatClass))
		{
			int inventoryItemCount = Session.MyPlayerData.GetInventoryItemCount(combatClass.ClassTokenID);
			bool flag = combatClass.HasToken && inventoryItemCount < combatClass.ClassTokenCost;
			if (combatClass.IsSkin && !Session.MyPlayerData.MeetsClassUnlockReqs(combatClass.ID))
			{
				Notification.ShowText("You don't meet the requirements!");
			}
			else if (flag || !Session.MyPlayerData.MeetsClassUnlockReqs(combatClass.ID))
			{
				ConfirmationGetTokens.gameObject.SetActive(value: true);
				ConfirmationGetTokens.RefreshToGetClass(combatClass);
			}
			else
			{
				Game.Instance.SendClassAddRequest(combatClass.ID);
			}
		}
		else if (combatClass.BitFlagName == "iu0" && combatClass.BitFlagIndex == 6)
		{
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this class, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
		}
		else
		{
			MessageBox.Show("Locked!", combatClass.RequirementText);
		}
	}

	private void OnGetTokensButtonClicked(GameObject button)
	{
		if (Session.MyPlayerData.GetQSValue(192) >= 17 || Entities.Instance.me.Level > 10)
		{
			ConfirmationGetTokens.gameObject.SetActive(value: true);
			ConfirmationGetTokens.RefreshToTravel(combatClass);
		}
		else
		{
			MessageBox.Show("Cannot Travel Yet", "You must finish the tutorial quests in Battleon Foothills before you can travel to Class Trainers.");
		}
	}

	private void OnSpendTokensButtonClicked(GameObject button)
	{
		if (Session.MyPlayerData.GetInventoryItemCount(combatClass.ClassTokenID) > 0)
		{
			ConfirmationSpendTokens.gameObject.SetActive(value: true);
			ConfirmationSpendTokens.Refresh(combatClass);
		}
		else
		{
			ConfirmationGetTokens.gameObject.SetActive(value: true);
			ConfirmationGetTokens.RefreshToTravel(combatClass, redirectedFromSpendTokens: true);
		}
	}

	public void Refresh(CharClass charClass)
	{
		combatClass = charClass.ToCombatClass();
		ClassDescription.text = combatClass.Description;
		TokenIcon1.spriteName = combatClass.Icon;
		TokenIcon2.spriteName = TokenIcon1.spriteName;
		TokenLabel1.text = combatClass.Name.ToUpper() + " TOKENS";
		TokenLabel2.text = combatClass.Name.ToUpper() + " TOKENS";
		int classTokenID = combatClass.ClassTokenID;
		int inventoryItemCount = Session.MyPlayerData.GetInventoryItemCount(classTokenID);
		TokenQuantityLabel1.text = inventoryItemCount + "/" + combatClass.ClassTokenCost;
		TokenQuantityLabel2.text = inventoryItemCount.ToString();
		this.charClass = charClass;
		combatClass = charClass.ToCombatClass();
		ClassIcon.spriteName = combatClass.Icon;
		Tokens.SetActive(value: false);
		GetTokensButton.gameObject.SetActive(value: false);
		UnlockSlider.gameObject.SetActive(value: false);
		ClassRankXPBar.gameObject.SetActive(value: false);
		UnlockRequirements.SetActive(value: false);
		float y = -280f;
		UnlockInfo.text = "[875A00]Complete requirements to unlock " + combatClass.Name + " Tokens[-]";
		if (Session.MyPlayerData.OwnsClass(combatClass.ID))
		{
			if (combatClass.HasClassTokens)
			{
				Tokens.SetActive(value: true);
				GetTokensButton.gameObject.SetActive(value: true);
			}
			RefreshClassXPBar();
		}
		else if (!Session.MyPlayerData.MeetsClassUnlockReqs(combatClass.ID))
		{
			y = -280f;
		}
		else if (combatClass.HasClassTokens && !combatClass.IsClassFree)
		{
			RefreshUnlockSlider();
			y = -220f;
		}
		else
		{
			y = -165f;
		}
		Vector3 localPosition = EquipButton.transform.localPosition;
		localPosition.y = y;
		EquipButton.transform.localPosition = localPosition;
		ClassTokensLabel.text = "x" + Session.MyPlayerData.GetItemCount(combatClass.ClassTokenID);
		CheckIfEquipped(charClass);
		LoadClassImage();
		if (combatClass.IsSkin)
		{
			ClassRankLabel.text = "Skins don't count towards Total Rank";
		}
		else if (charClass.ClassRank >= 100)
		{
			ClassRankLabel.text = "MAX RANK";
		}
		else
		{
			ClassRankLabel.text = "RANK " + charClass.ClassRank;
		}
		CosmeticIcon.spriteName = ItemIcons.GetCosmeticIconBySlot(combatClass.WeaponRequired);
		for (int i = 0; i < Stars.Length; i++)
		{
			Stars[i].SetActive(i < combatClass.Difficulty);
		}
		if (combatClass.UnlockReqs.Count <= 0 || Session.MyPlayerData.MeetsClassUnlockReqs(combatClass.ID) || Session.MyPlayerData.OwnsClass(charClass.ClassID))
		{
			return;
		}
		UnlockRequirements.SetActive(value: true);
		foreach (GameObject checklistObject in ChecklistObjects)
		{
			UnityEngine.Object.Destroy(checklistObject);
		}
		foreach (ClassUnlockReq unlockReq in combatClass.UnlockReqs)
		{
			MakeNewChecklistItem(unlockReq.Description, unlockReq.PlayerMeetsReq());
		}
		CheckListTable.Reposition();
	}

	private void LoadClassImage()
	{
		ClassImage.gameObject.SetActive(value: false);
		LoadingIcon.SetActive(value: true);
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(LoadTexture(charClass.ToCombatClass().Name, Entities.Instance.me.baseAsset.gender));
		}
	}

	private void MakeNewChecklistItem(string reqText, bool isComplete)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ChecklistItemPrefab);
		ChecklistObjects.Add(gameObject);
		gameObject.transform.SetParent(ChecklistTransform.transform, worldPositionStays: false);
		UnlockReqUI component = gameObject.GetComponent<UnlockReqUI>();
		component.UpdateLabel(reqText);
		component.UILabel.color = (isComplete ? Color.grey : Color.black);
		component.ChekForCheckbox.gameObject.SetActive(isComplete);
		gameObject.SetActive(value: true);
	}

	public IEnumerator LoadTexture(string className, string gender)
	{
		string text = ((gender == "M") ? "_Male" : "_Female");
		string text2 = className + text + ".jpg";
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/gamefiles/images/classes/" + text2);
		string errorTitle = "Unable to LoadTexture: " + text2;
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "Image: " + text2;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
		}
		else if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
		}
		else if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
		}
		else
		{
			ClassImage.mainTexture = DownloadHandlerTexture.GetContent(www);
		}
		LoadingIcon.SetActive(value: false);
		ClassImage.gameObject.SetActive(value: true);
	}

	public void CheckIfEquipped(CharClass charClass)
	{
		if (Session.MyPlayerData.OwnsClass(charClass.ClassID))
		{
			if (charClass.ClassID == Session.MyPlayerData.CurrentClass.ClassID)
			{
				BuyLabel.text = "EQUIPPED";
				EquipButton.isEnabled = false;
				ButtonGlow.SetActive(value: false);
			}
			else
			{
				BuyLabel.text = "EQUIP CLASS";
				EquipButton.isEnabled = true;
				ButtonGlow.SetActive(value: true);
			}
			UnlockRequirements.SetActive(value: false);
		}
		else
		{
			EquipButton.isEnabled = true;
			CombatClass combatClass = charClass.ToCombatClass();
			bool flag = Session.MyPlayerData.IsClassAvailable(combatClass);
			bool flag2 = combatClass.ClassTokenID > 0 && combatClass.ClassTokenCost <= 0;
			BuyLabel.text = ((flag && flag2) ? "GET CLASS" : "UNLOCK CLASS");
			ButtonGlow.SetActive(value: true);
		}
	}

	private void RefreshUnlockSlider()
	{
		UnlockSlider.gameObject.SetActive(value: true);
		UnlockSlider.Refresh(charClass);
	}

	public void RefreshClassXPBar()
	{
		ClassRankXPBar.gameObject.SetActive(value: true);
		int classRank = charClass.ClassRank;
		ClassRankLabel.text = "Rank " + classRank;
		int num = charClass.ClassXP - ClassRanks.GetTotalXPToRankUp(classRank - 1);
		int num2 = ClassRanks.GetTotalXPToRankUp(classRank) - ClassRanks.GetTotalXPToRankUp(classRank - 1);
		float num3 = (float)num / (float)num2;
		ClassRankXPBar.value = ((classRank == 100) ? 1f : num3);
		ClassRankXPLabel.text = ArtixString.AddCommas(num) + " / " + ArtixString.AddCommas(num2) + " XP";
		ClassRankXPLabel.gameObject.SetActive(classRank < 100 && !combatClass.IsSkin);
	}

	public void Refresh()
	{
		Refresh(charClass);
	}
}

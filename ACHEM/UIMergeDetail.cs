using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMergeDetail : MonoBehaviour
{
	public UIScrollView scrollView;

	public UIGrid tblItems;

	public UITable tblView;

	public UILabel lblName;

	public UILabel lblLevel;

	public UILabel lblStats;

	public UILabel lblDesc;

	public UILabel lblBuyCost;

	public UILabel lblTime;

	public UILabel lblCraftCost;

	public UILabel lblSpeedUpCost;

	public UILabel lblRequirement;

	public UILabel lblPowerNumeric;

	public UILabel lblPowerText;

	public UILabel lblInfusionCap;

	public UIButton btnCraft;

	public UIButton btnBuy;

	public UIButton btnPreview;

	public UIButton btnClaim;

	public UIButton btnSpeedUp;

	public Merge merge;

	public UISprite SprIcon;

	public UISprite SprGuardian;

	private List<UIMergeListItem> mergeItems = new List<UIMergeListItem>();

	public GameObject MergeListItemTemplate;

	public GameObject PowerParent;

	public GameObject InfusionParent;

	public UIMergeTimer TimeBar;

	private int currentShopID;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
			}
		}
	}

	public int MergeID => merge.MergeID;

	private int Cost
	{
		get
		{
			if (merge.MergeMinutes == 0f)
			{
				return 0;
			}
			decimal num = (decimal)(merge.TSComplete.Value - GameTime.ServerTime).TotalSeconds / 60m / (decimal)merge.MergeMinutes;
			num = Math.Ceiling(num * 10m) / 10m;
			if (num <= 0.1m)
			{
				num = 0.1m;
			}
			else if (num >= 1m)
			{
				num = 1m;
			}
			return (int)Math.Ceiling(Math.Ceiling((decimal)merge.Cost * 0.5m) * num);
		}
	}

	public static event Action CraftingCompleted;

	public void LoadMerge(Merge m, int shopID)
	{
		Visible = true;
		currentShopID = shopID;
		merge = m;
		List<MergeItem> list = merge.MergeItems.OrderBy((MergeItem p) => p.SortOrder).ToList();
		MergeListItemTemplate.SetActive(value: false);
		foreach (UIMergeListItem mergeItem in mergeItems)
		{
			mergeItem.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(mergeItem.gameObject);
		}
		mergeItems.Clear();
		foreach (MergeItem item in list)
		{
			GameObject obj = UnityEngine.Object.Instantiate(MergeListItemTemplate);
			obj.transform.SetParent(tblItems.gameObject.transform, worldPositionStays: false);
			UIMergeListItem component = obj.GetComponent<UIMergeListItem>();
			component.Init(this, item);
			mergeItems.Add(component);
		}
		SprGuardian.gameObject.SetActive(m.IsGuardian);
		SprIcon.spriteName = m.Icon;
		lblName.text = "[" + m.RarityColor + "]" + m.Name + "[-]";
		if (m.MaxStack > 1)
		{
			UILabel uILabel = lblName;
			uILabel.text = uILabel.text + " [" + m.RarityColor + "]x" + m.Qty + "[-]";
		}
		lblRequirement.gameObject.SetActive(value: false);
		if (m.IsAvailable() && m.DisplayLevel != m.Level)
		{
			lblRequirement.text = "[005900]Craft scaled to Level " + m.DisplayLevel + "[-]";
			lblRequirement.gameObject.SetActive(value: true);
		}
		else if (!m.IsAvailable())
		{
			string lockInfo = m.GetLockInfo();
			if (!string.IsNullOrEmpty(lockInfo))
			{
				lblRequirement.text = "[ad0000]" + lockInfo + "[-]";
				lblRequirement.gameObject.SetActive(value: true);
			}
		}
		lblLevel.text = m.GetTagline(showPower: false);
		lblStats.gameObject.SetActive(m.HasStats);
		lblStats.text = Session.MyPlayerData.GetComparisonStatText(m);
		if (m.HasStats)
		{
			InfusionParent.SetActive(value: true);
			lblInfusionCap.text = m.PowerOffset + "/" + m.DisplayTimesInfusable;
		}
		else
		{
			InfusionParent.SetActive(value: false);
		}
		lblDesc.text = m.GetDescription();
		if (m.GetCombatPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = m.GetCombatPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else if (m.GetTradeSkillPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = m.GetTradeSkillPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else
		{
			PowerParent.SetActive(value: false);
		}
		lblBuyCost.text = merge.Cost.ToString();
		lblCraftCost.text = merge.MergeCost.ToString();
		btnPreview.gameObject.SetActive(merge.HasPreview);
		if (lblTime != null && merge != null)
		{
			int num = (int)(merge.MergeMinutes * 60f);
			lblTime.text = "Craft Time: " + ArtixString.FormatDuration(num);
			lblTime.text += "\nSpeed up: ";
			lblTime.text += ((merge.IsSpeedUpDisabled || merge.CurrencyCost <= 0) ? "Not Available" : (merge.CurrencyCost / 2 + " " + merge.CurrencyString + "[-]"));
		}
		CancelInvoke("Tick");
		if (!merge.TSComplete.HasValue)
		{
			TimeBar.gameObject.SetActive(value: false);
			btnCraft.gameObject.SetActive(value: true);
			if (merge.IsCraftOnly)
			{
				btnCraft.transform.localPosition = new Vector3(0f, -70f, 0f);
				btnBuy.gameObject.SetActive(value: false);
			}
			else
			{
				btnCraft.transform.localPosition = new Vector3(-90f, -70f, 0f);
				btnBuy.gameObject.SetActive(value: true);
			}
		}
		else
		{
			TimeBar.gameObject.SetActive(value: true);
			TimeBar.Init(merge);
			btnCraft.gameObject.SetActive(value: false);
			btnBuy.gameObject.SetActive(value: false);
			InvokeRepeating("Tick", 0f, 1f);
		}
		if (merge.IsSpeedUpDisabled)
		{
			btnSpeedUp.gameObject.SetActive(value: false);
		}
		tblItems.Reposition();
		tblView.Reposition();
		scrollView.ResetPosition();
	}

	public void Hide()
	{
		Visible = false;
	}

	public void OnPreviewClick()
	{
		UIPreview.Show(merge);
	}

	public void OnCraftClick(GameObject go)
	{
		if (!merge.IsMC)
		{
			MessageBox.Show("DATA ERROR", "Item is not eligible for crafting");
		}
		else if (merge.IsGuardian && !Session.MyPlayerData.IsGuardian())
		{
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this item, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
		}
		else if (!merge.IsAvailable())
		{
			string text = merge.GetLockInfo();
			if (!string.IsNullOrEmpty(text))
			{
				text = "You do not meet this item's crafting requirements!";
			}
			MessageBox.Show("Cannot Craft Yet", text);
		}
		else
		{
			string text2 = Session.MyPlayerData.CanCraft(merge);
			if (!string.IsNullOrEmpty(text2))
			{
				Notification.ShowText(text2);
			}
			else
			{
				Game.Instance.SendMergeRequest(currentShopID, MergeID);
			}
		}
	}

	public void OnBuyClick(GameObject go)
	{
		if (!merge.IsMC)
		{
			MessageBox.Show("Error", "Item is not eligible for crafting!");
			return;
		}
		if (merge.IsGuardian && !Session.MyPlayerData.IsGuardian())
		{
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this item, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
			return;
		}
		if (!merge.IsAvailable())
		{
			string text = merge.GetLockInfo();
			if (!string.IsNullOrEmpty(text))
			{
				text = "You do not meet this item's crafting requirements!";
			}
			MessageBox.Show("Cannot Craft Yet", text);
			return;
		}
		string text2 = Session.MyPlayerData.CanAddItem(merge);
		if (text2 != "")
		{
			Notification.ShowText(text2);
			return;
		}
		if (Session.MyPlayerData.MC < merge.Cost)
		{
			Confirmation.Show("Insufficient Fund", "You do not have sufficient Dragon Crystals to purchase the item. Would you like to buy Dragon Crystals?", ConfirmationCallbackBuyMC);
			return;
		}
		ConfirmationSpend.Show("Confirm", "Purchase '" + merge.Name + "' for " + merge.Cost + " Crystals?", merge.CurrencyString, merge.Cost, "Buy", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendMergeBuyOutRequest(currentShopID, merge.MergeID);
			}
		});
	}

	private void Tick()
	{
		bool flag = merge.MergeMinutes <= 0f || (merge.TSComplete.Value - GameTime.ServerTime).TotalSeconds <= 0.0;
		btnSpeedUp.gameObject.SetActive(!flag && !merge.IsSpeedUpDisabled);
		btnClaim.gameObject.SetActive(flag);
		lblSpeedUpCost.text = Cost.ToString();
		if (flag)
		{
			if (UIMergeDetail.CraftingCompleted != null)
			{
				UIMergeDetail.CraftingCompleted();
			}
			CancelInvoke("Tick");
		}
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnCraft.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCraftClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnBuy.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBuyClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnSpeedUp.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSpeedUpClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnClaim.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClaimClick));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnCraft.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCraftClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnBuy.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBuyClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnSpeedUp.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnSpeedUpClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnClaim.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClaimClick));
	}

	private void ConfirmationCallbackBuyMC(bool confirm)
	{
		if (confirm)
		{
			UIIAPStore.Show();
		}
	}

	private void OnClaimClick(GameObject go)
	{
		string text = Session.MyPlayerData.CanAddItem(merge);
		if (text != "")
		{
			Notification.ShowText(text);
		}
		else
		{
			Game.Instance.SendMergeClaimRequest(merge.MergeID);
		}
	}

	private void OnSpeedUpClick(GameObject go)
	{
		if (!merge.IsSpeedUpDisabled)
		{
			if (!merge.IsMC)
			{
				MessageBox.Show("DATA ERROR", "Item is not eligible for crafting");
			}
			else
			{
				if (merge.MergeMinutes <= 0f)
				{
					return;
				}
				string text = Session.MyPlayerData.CanAddItem(merge);
				if (text != "")
				{
					Notification.ShowText(text);
					return;
				}
				if (Session.MyPlayerData.MC < Cost)
				{
					Confirmation.Show("Insufficient Fund", "You do not have sufficient DragonGems to purchase the item. Would you like to buy DragonGems?", ConfirmationCallbackBuyMC);
					return;
				}
				ConfirmationSpend.Show("Confirm", "Speed up this item for " + Cost + " Gems?", merge.CurrencyString, Cost, "Speed up", delegate(bool b)
				{
					if (b)
					{
						Game.Instance.SendMergeSpeedupRequest(merge.MergeID);
					}
				});
			}
		}
		else
		{
			Notification.ShowText("No");
		}
	}
}

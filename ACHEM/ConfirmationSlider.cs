using System;
using System.Linq;
using UnityEngine;

public class ConfirmationSlider : ModalWindow
{
	public enum ActionType
	{
		Buy,
		Sell
	}

	private static ConfirmationSlider mInstance;

	private Action<bool, int> callback;

	public UILabel Title;

	public UILabel Message;

	public UILabel QtyLabel;

	public UILabel CostLabel;

	public UILabel BtnLabel;

	public UILabel lblMyGems;

	public UISprite CurrencySprite;

	public UISprite sprMyCoin;

	public UIButton BtnYes;

	public UIButton BtnNo;

	public UIButton BtnClose;

	public UISlider slider;

	public int Qty;

	public int MaxQty;

	public int Cost = 10;

	public int Balance;

	public int TotalCost;

	public ActionType Action;

	public static ConfirmationSlider Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConfirmationSlider"), UIManager.Instance.transform).GetComponent<ConfirmationSlider>();
				mInstance.name = "Confirmation";
				mInstance.Init();
			}
			return mInstance;
		}
	}

	private void Awake()
	{
		mInstance = this;
		UIEventListener uIEventListener = UIEventListener.Get(BtnYes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onYesClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnNo.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onNoClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(onCloseClick));
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void ShowConfirmation(string title, string message, string icon, int cost, int balance, int max, ActionType action, Action<bool, int> callback, ShopItem item)
	{
		Title.text = title;
		Message.text = message;
		this.callback = callback;
		Action = action;
		MaxQty = max;
		Cost = cost;
		slider.numberOfSteps = MaxQty;
		slider.value = 0f;
		Balance = balance;
		BtnLabel.text = action.ToString();
		UISprite currencySprite = CurrencySprite;
		string spriteName = (sprMyCoin.spriteName = icon);
		currencySprite.spriteName = spriteName;
		lblMyGems.text = balance.ToString("n0");
		if (item == null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("TokenShopClassIconOverlay");
		GameObject gameObject2 = GameObject.Find("TokenShopClassIconOverlay2");
		if (item.TokenID > 0)
		{
			gameObject.SetActive(value: true);
			gameObject2.SetActive(value: true);
			if (!string.IsNullOrEmpty(Items.Get(item.TokenID).IconFg))
			{
				gameObject.GetComponent<UISprite>().spriteName = Items.Get(item.TokenID).IconFg;
				gameObject2.GetComponent<UISprite>().spriteName = Items.Get(item.TokenID).IconFg;
			}
		}
		else
		{
			gameObject.SetActive(value: false);
			gameObject2.SetActive(value: false);
		}
	}

	private void onYesClick(GameObject go)
	{
		if (Action == ActionType.Buy && Balance < Cost * Qty)
		{
			Notification.ShowText("Insufficient funds");
			return;
		}
		callback(arg1: true, Qty);
		Close();
	}

	private void onNoClick(GameObject go)
	{
		callback(arg1: false, 0);
		Close();
	}

	private void onCloseClick(GameObject go)
	{
		Close();
	}

	protected override void Close()
	{
		callback = null;
		base.Close();
	}

	public void SetQty()
	{
		Qty = Mathf.RoundToInt((float)(MaxQty - 1) * slider.value) + 1;
		QtyLabel.text = Qty + "/" + MaxQty;
		TotalCost = Cost * Qty;
		CostLabel.text = TotalCost.ToString();
	}

	public static void Show(string title, string message, string currency, int cost, int max, ActionType action, Action<bool, int> callback)
	{
		string icon = ((currency == "Gold") ? "Coin" : "DragonGem");
		int balance = ((currency == "Gold") ? Session.MyPlayerData.Gold : Session.MyPlayerData.MC);
		Instance.ShowConfirmation(title, message, icon, cost, balance, max, action, callback, null);
	}

	public static void ConfirmBuy(ShopItem item, ActionType action, Action<bool, int> callback)
	{
		int num = 0;
		num = ((item.TokenID <= 0) ? (item.IsMC ? Session.MyPlayerData.MC : Session.MyPlayerData.Gold) : Session.MyPlayerData.GetItemCount(item.TokenID));
		int a = ((!item.TakesBagSpace) ? (item.MaxStack - Session.MyPlayerData.GetItemCount(item.ID)) : (Session.MyPlayerData.HasSlotsAvailableInInventory(1) ? item.MaxStack : Mathf.Min(item.MaxStack, Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == item.ID && x.Qty <= item.MaxStack).Sum((InventoryItem x) => x.MaxStack - x.Qty))));
		if (item.IsUnique)
		{
			a = item.MaxStack - Session.MyPlayerData.GetItemCount(item.ID);
		}
		a = Mathf.Min(a, 20);
		Instance.ShowConfirmation("Confirm", "How many [" + item.RarityColor + "]'" + item.Name + "'[-] would you like to buy?", item.CurrencyIcon, item.CurrencyCost, num, a, action, callback, item);
	}

	public static void ConfirmSell(InventoryItem item, ActionType action, Action<bool, int> callback)
	{
		Instance.ShowConfirmation("Sell Stack?", "How many [" + item.RarityColor + "]'" + item.Name + "'[-] would you like to sell?", "Coin", item.SellPrice, Session.MyPlayerData.Gold, item.Qty, action, callback, null);
	}
}

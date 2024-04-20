using System;
using UnityEngine;

public class ConfirmationSpend : ModalWindow
{
	protected static ConfirmationSpend mInstance;

	protected Action<bool> callback;

	public UILabel Title;

	public UILabel Message;

	public UIButton BtnAction;

	public UIButton BtnClose;

	public UISprite icon;

	public UISprite sprMyCoin;

	public UILabel lblCost;

	public UILabel lblMyGems;

	public UILabel lblButton;

	public UISprite ClassIconOverlayCost;

	public UISprite ClassIconOverlayCurrent;

	public GameObject DragonSprite;

	public GameObject CollectionSprite;

	public static ConfirmationSpend Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConfirmationSpend"), UIManager.Instance.transform).GetComponent<ConfirmationSpend>();
				mInstance.name = "ConfirmationSpend";
				mInstance.Init();
			}
			return mInstance;
		}
	}

	private void Awake()
	{
		mInstance = this;
		UIEventListener uIEventListener = UIEventListener.Get(BtnAction.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onYesClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(BtnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(onCloseClick));
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	protected void ShowConfirmationSpend(string title, string message, string icon, int cost, int balance, string action, Action<bool> callback, ShopItem item, bool isCollection)
	{
		Title.text = title;
		Message.text = "[000000]" + message + "[-]";
		lblCost.text = cost.ToString("n0");
		UISprite uISprite = this.icon;
		string spriteName = (sprMyCoin.spriteName = icon);
		uISprite.spriteName = spriteName;
		lblMyGems.text = balance.ToString("n0");
		lblButton.text = action;
		this.callback = callback;
		if (isCollection)
		{
			DragonSprite.SetActive(value: false);
			CollectionSprite.SetActive(value: true);
		}
		if (item != null && item.TokenID > 0)
		{
			string text2 = Items.Get(item.TokenID)?.IconFg;
			if (!string.IsNullOrEmpty(text2))
			{
				ClassIconOverlayCost.spriteName = text2;
				ClassIconOverlayCurrent.spriteName = text2;
				ClassIconOverlayCost.gameObject.SetActive(value: true);
				ClassIconOverlayCurrent.gameObject.SetActive(value: true);
			}
			else
			{
				ClassIconOverlayCost.gameObject.SetActive(value: false);
				ClassIconOverlayCurrent.gameObject.SetActive(value: false);
			}
		}
	}

	protected void onYesClick(GameObject go)
	{
		callback(obj: true);
		Close();
	}

	protected void onCloseClick(GameObject go)
	{
		Close();
	}

	protected override void Close()
	{
		callback = null;
		base.Close();
	}

	public static void Show(string title, string message, string currency, int cost, string action, Action<bool> callback, bool isCollection = false)
	{
		string text = ((currency == "Gold") ? "Coin" : "DragonGem");
		int balance = ((currency == "Gold") ? Session.MyPlayerData.Gold : Session.MyPlayerData.MC);
		Instance.ShowConfirmationSpend(title, message, text, cost, balance, action, callback, null, isCollection);
	}

	public static void ConfirmBuy(ShopItem item, Action<bool> callback, string extraMessage = "", bool isCollection = false)
	{
		int num = 0;
		num = ((item.TokenID <= 0) ? (item.IsMC ? Session.MyPlayerData.MC : Session.MyPlayerData.Gold) : Session.MyPlayerData.GetItemCount(item.TokenID));
		string text = "[" + Item.GetRarityColor(item.Rarity) + "]" + item.Name + "[-]";
		string text2 = item.CurrencyCost * item.Qty + " " + item.CurrencyString;
		string message = "Purchase " + text + " for " + text2 + "? " + extraMessage;
		Instance.ShowConfirmationSpend("Confirm", message, item.CurrencyIcon, item.CurrencyCost, num, "Buy", callback, item, isCollection);
	}
}

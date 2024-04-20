using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public class UIIAPStore : UIFullscreenWindow
{
	private const int AdWatchRewardDC = 4;

	public UIScrollView scrollViewDC;

	public UIScrollView scrollViewBundles;

	public UIGrid gridDC;

	public UIGrid gridBundles;

	public UIButton btnClose;

	public UILabel lblMC;

	public List<ProductButton> ProductButtons;

	public UIButton btnAdWatch;

	public UILabel lblAdWatch;

	public UILabel lblAdWatchReward;

	public UIToggle DCTab;

	public UIToggle BundlesTab;

	private static UIIAPStore instance;

	private bool isSyncing;

	private int adsAvailable;

	private DateTime dateAdAvailable;

	private TimeSpan timeUntilAdAvailable;

	private bool dataSyncComplete;

	public static void Show(bool showDCPage = false)
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/IAPStoreUI"), UIManager.Instance.transform).GetComponent<UIIAPStore>();
			BusyDialog.Show("Initializing Store");
			if (showDCPage)
			{
				instance.DCTab.startsActive = true;
				instance.BundlesTab.startsActive = false;
			}
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnAdWatch.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnAdWatchClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		foreach (ProductButton productButton in ProductButtons)
		{
			productButton.Clicked += OnProductClicked;
			productButton.Removed += OnProductRemoved;
		}
		Session.MyPlayerData.CurrencyUpdated += OnCurrencyUpdated;
		Session.MyPlayerData.DataSynced += OnDataSync;
		AdManager.Instance.OnAdAvailability += OnAdAvailability;
		AdManager.Instance.OnAdFinished += OnAdFinished;
		Game.Instance.AdWatchRewardReceived += OnAdWatchRewardReceived;
		IAPStore.OnInitComplete = (Action)Delegate.Combine(IAPStore.OnInitComplete, new Action(OnStoreInit));
		IAPStore.OnPurchaseSuccess = (Action<ITransaction>)Delegate.Combine(IAPStore.OnPurchaseSuccess, new Action<ITransaction>(OnPurchaseSuccess));
		IAPStore.OnValidateSuccess = (Action<string>)Delegate.Combine(IAPStore.OnValidateSuccess, new Action<string>(OnValidateSuccess));
		IAPStore.OnValidateFailed = (Action)Delegate.Combine(IAPStore.OnValidateFailed, new Action(OnValidateFailed));
		IAPStore.OnConsumeSuccess = (Action<ITransaction>)Delegate.Combine(IAPStore.OnConsumeSuccess, new Action<ITransaction>(OnConsumption));
		IAPStore.OnPurchaseFailed = (Action<string>)Delegate.Combine(IAPStore.OnPurchaseFailed, new Action<string>(OnPurchaseFailed));
		IAPStore.OnPurchaseCancelled = (Action<string>)Delegate.Combine(IAPStore.OnPurchaseCancelled, new Action<string>(OnPurchaseFailed));
		IAPStore.OnConsumeFailed = (Action<string>)Delegate.Combine(IAPStore.OnConsumeFailed, new Action<string>(OnPurchaseFailed));
		IAPStore.OnRestoreFailed = (Action<string>)Delegate.Combine(IAPStore.OnRestoreFailed, new Action<string>(OnRestoreFinished));
		IAPStore.OnRestoreFinished = (Action<string>)Delegate.Combine(IAPStore.OnRestoreFinished, new Action<string>(OnRestoreFinished));
		IAPStore.Init();
		OnCurrencyUpdated();
		scrollViewBundles.transform.parent.gameObject.SetActive(value: true);
		scrollViewDC.transform.parent.gameObject.SetActive(value: true);
		foreach (ProductButton productButton2 in ProductButtons)
		{
			productButton2.Init();
		}
		gridBundles.Reposition();
		scrollViewBundles.ResetPosition();
		if (Platform.IsMobile && Session.MyPlayerData.GetGameParam("UnityAds") != "0")
		{
			adsAvailable = -1;
			UpdateAdStatusClientSide();
			AdManager.Instance.GetAdAvailability();
		}
		else
		{
			btnAdWatch.gameObject.SetActive(value: false);
		}
		lblAdWatchReward.text = 4.ToString();
		gridDC.Reposition();
		scrollViewDC.ResetPosition();
		StartCoroutine(UpdateAdStatusRoutine());
	}

	private void OnProductRemoved(ProductButton productButton)
	{
		gridBundles.Reposition();
		scrollViewBundles.ResetPosition();
	}

	private void OnProductClicked(ProductButton productButton)
	{
		if (Main.IsPTR)
		{
			MessageBox.Show("PURCHASE FAILED", "Payment is not currently enabled on PTR.");
			return;
		}
		if (Session.IsGuest)
		{
			UIAccountCreate.Instance.ShowConvertGuest();
			return;
		}
		BusyDialog.Show("Purchasing " + productButton.Title);
		dataSyncComplete = false;
		IAPStore.PurchaseProduct(Products.ProductPackages[productButton.ProductID]);
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
	}

	public void OnAdWatchClick(GameObject go)
	{
		AdManager.Instance.RequestAdWatch();
	}

	private void OnCurrencyUpdated()
	{
		lblMC.text = Session.MyPlayerData.MC.ToString("n0");
	}

	public void OnStoreInit()
	{
		UpdateProductPrice();
		BusyDialog.Close();
	}

	public void UpdateProductPrice()
	{
		foreach (ProductButton productButton in ProductButtons)
		{
			string localizedPriceString = IAPStore.GetLocalizedPriceString(Products.ProductPackages[productButton.ProductID].StoreProductID);
			if (!string.IsNullOrEmpty(localizedPriceString))
			{
				productButton.SetPrice(localizedPriceString);
			}
		}
	}

	public void OnPurchaseSuccess(ITransaction t)
	{
		BusyDialog.Close();
		Debug.Log("OnPurchaseSuccess");
	}

	public void OnValidateSuccess(string productID)
	{
		BusyDialog.Close();
		Debug.Log("OnValidateSuccess");
	}

	public void OnValidateFailed()
	{
		BusyDialog.Close();
	}

	public void OnPurchaseFailed(string message)
	{
		BusyDialog.Close();
		MessageBox.Show("Purchase Failed", message);
	}

	public void OnConsumption(ITransaction t)
	{
		if (!dataSyncComplete)
		{
			StartCoroutine("ResyncTimeout");
		}
	}

	public void OnRestoreFinished(string message)
	{
		MessageBox.Show("Restore Purchases", "Finished Restoring Purchases");
	}

	public void OnDataSync()
	{
		dataSyncComplete = true;
		isSyncing = false;
		StopCoroutine("ResyncTimeout");
		BusyDialog.Close();
		MessageBox.Show("Purchase Successful", "Congratulations! Your order was successful and the package has been added to your account.");
		OnCurrencyUpdated();
		foreach (ProductButton productButton in ProductButtons)
		{
			productButton.Refresh();
		}
		gridBundles.Reposition();
		scrollViewBundles.ResetPosition();
	}

	protected override void Destroy()
	{
		foreach (ProductButton productButton in ProductButtons)
		{
			productButton.Clicked -= OnProductClicked;
			productButton.Removed -= OnProductRemoved;
		}
		AdManager.Instance.OnAdAvailability -= OnAdAvailability;
		AdManager.Instance.OnAdFinished -= OnAdFinished;
		Game.Instance.AdWatchRewardReceived -= OnAdWatchRewardReceived;
		IAPStore.OnInitComplete = (Action)Delegate.Remove(IAPStore.OnInitComplete, new Action(OnStoreInit));
		IAPStore.OnPurchaseSuccess = (Action<ITransaction>)Delegate.Remove(IAPStore.OnPurchaseSuccess, new Action<ITransaction>(OnPurchaseSuccess));
		IAPStore.OnValidateSuccess = (Action<string>)Delegate.Remove(IAPStore.OnValidateSuccess, new Action<string>(OnValidateSuccess));
		IAPStore.OnValidateFailed = (Action)Delegate.Remove(IAPStore.OnValidateFailed, new Action(OnValidateFailed));
		IAPStore.OnConsumeSuccess = (Action<ITransaction>)Delegate.Remove(IAPStore.OnConsumeSuccess, new Action<ITransaction>(OnConsumption));
		IAPStore.OnPurchaseFailed = (Action<string>)Delegate.Remove(IAPStore.OnPurchaseFailed, new Action<string>(OnPurchaseFailed));
		IAPStore.OnPurchaseCancelled = (Action<string>)Delegate.Remove(IAPStore.OnPurchaseCancelled, new Action<string>(OnPurchaseFailed));
		IAPStore.OnConsumeFailed = (Action<string>)Delegate.Remove(IAPStore.OnConsumeFailed, new Action<string>(OnPurchaseFailed));
		IAPStore.OnRestoreFailed = (Action<string>)Delegate.Remove(IAPStore.OnRestoreFailed, new Action<string>(OnRestoreFinished));
		IAPStore.OnRestoreFinished = (Action<string>)Delegate.Remove(IAPStore.OnRestoreFinished, new Action<string>(OnRestoreFinished));
		Session.MyPlayerData.CurrencyUpdated -= OnCurrencyUpdated;
		Session.MyPlayerData.DataSynced -= OnDataSync;
		base.Destroy();
		instance = null;
	}

	private IEnumerator ResyncTimeout()
	{
		BusyDialog.Show("Validating Purchase");
		isSyncing = true;
		yield return new WaitForSeconds(10f);
		if (isSyncing)
		{
			BusyDialog.Close();
			MessageBox.Show("Timeout", "Purchase was successful, but the server timed out.  Please re-login now to get your items.", "", delegate
			{
				Game.Instance.Logout();
			});
		}
	}

	private void OnAdFinished(bool success)
	{
		BusyDialog.Close();
		AdManager.Instance.GetAdAvailability();
		if (success)
		{
			Game.Instance.SendAdWatchRewardRequest();
			BusyDialog.Show("Collecting Reward...", closable: true);
		}
		else
		{
			MessageBox.Show("Ad Failed", "The advertisement failed to complete. Please try again later!");
		}
	}

	private void OnAdWatchRewardReceived(bool success)
	{
		BusyDialog.Close();
		if (success)
		{
			MessageBox.Show("Success", "Congratulations! You have received " + 4 + " Dragon Crystals.");
			Dictionary<string, string> eventValues = new Dictionary<string, string>();
			AppsFlyer.sendEvent("af_ad_view", eventValues);
		}
		else if (Main.Environment == Environment.Live)
		{
			MessageBox.Show("Ad Failed", "The server failed to verify the ad reward. Please try again later!");
		}
		else
		{
			MessageBox.Show("Ad Failed", "Advertisement awards are only delivered in the Live Environment!");
		}
	}

	private void OnAdAvailability(int adsLeft, int secondsLeft)
	{
		adsAvailable = adsLeft;
		dateAdAvailable = DateTime.Now.AddSeconds(secondsLeft);
		UpdateAdStatusClientSide();
	}

	private IEnumerator UpdateAdStatusRoutine()
	{
		while (true)
		{
			UpdateAdStatusClientSide();
			yield return new WaitForSecondsRealtime(1f);
		}
	}

	private void UpdateAdStatusClientSide()
	{
		if (!AdManager.Instance.IsInitialized)
		{
			lblAdWatch.text = "Advertisement not initialized. \nPlease try again later.";
			btnAdWatch.isEnabled = false;
			return;
		}
		if (adsAvailable == -1)
		{
			lblAdWatch.text = "Loading\n ";
			btnAdWatch.isEnabled = false;
			return;
		}
		if (adsAvailable > 0)
		{
			btnAdWatch.isEnabled = true;
			lblAdWatch.text = "Watch Ad\n" + adsAvailable + " Ads Left";
			return;
		}
		TimeSpan timeSpan = dateAdAvailable - DateTime.Now;
		if (timeSpan.TotalSeconds > 0.0)
		{
			lblAdWatch.text = "Next Ad Available\nIn " + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
			btnAdWatch.isEnabled = false;
		}
		else
		{
			lblAdWatch.text = "Loading\n";
			AdManager.Instance.GetAdAvailability();
		}
	}
}

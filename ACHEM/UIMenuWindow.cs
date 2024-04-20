using System;
using UnityEngine;

public abstract class UIMenuWindow : UIStackingWindow
{
	public UIButton btnClose;

	public UIButton btnBack;

	public UIButton btnMCAdd;

	public UILabel lblGold;

	public UILabel lblMC;

	protected override void Init()
	{
		base.Init();
		if (btnClose != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		}
		if (btnBack != null)
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(btnBack.gameObject);
			uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBackClick));
		}
		if (btnMCAdd != null)
		{
			UIEventListener uIEventListener3 = UIEventListener.Get(btnMCAdd.gameObject);
			uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnMCAddClick));
		}
		Session.MyPlayerData.CurrencyUpdated += OnCurrencyUpdated;
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
	}

	public virtual void OnBackClick(GameObject go)
	{
		Back();
	}

	public void OnMCAddClick(GameObject go)
	{
		UIIAPStore.Show();
	}

	private void OnCurrencyUpdated()
	{
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
	}

	protected override void Destroy()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.CurrencyUpdated -= OnCurrencyUpdated;
		}
		base.Destroy();
	}
}

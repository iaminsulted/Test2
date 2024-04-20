using System;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIPublicHouseSlot : MonoBehaviour
{
	public UILabel HouseName;

	public UILabel OwnerName;

	public UILabel PlayerCount;

	public UILabel MapName;

	public UIButton ShowDetailBtn;

	public UIButton EnterHouseBtn;

	public UITexture BackgroundTexture;

	public Action<UIPublicHouseSlot> ShowDetailClicked;

	private UIPublicHouseChooser publicHouseChooser;

	public HouseData HouseData { get; private set; }

	public void Init(HouseData hData, UIPublicHouseChooser publicHouseChooser)
	{
		HouseData = hData;
		this.publicHouseChooser = publicHouseChooser;
		HouseName.text = HouseData.Name;
	}

	public void SetHouseDataAndRefresh(HouseData hData)
	{
		HouseData = hData;
		HouseName.text = "  " + hData.Name;
		MapName.text = "  " + hData.MapName;
		PlayerCount.text = $"  {hData.CurrentPlayers} / {hData.MaxPlayers}";
		OwnerName.text = "  " + hData.OwnerName;
	}

	private void OnShowDetailClicked(GameObject go)
	{
		UIPublicHouseChooser.Instance.ShowDetail(this);
	}

	private void OnEnterHouseClicked(GameObject go)
	{
		AEC.getInstance().sendRequest(new RequestHouseJoin(HouseData));
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ShowDetailBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnShowDetailClicked));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ShowDetailBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnShowDetailClicked));
	}
}

using System;
using Assets.Scripts.Housing;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class UIPublicHouseDetail : MonoBehaviour
{
	public UILabel HouseTitle;

	public UILabel MapName;

	public UILabel OwnerName;

	public UILabel Players;

	public UIButton CloseBtn;

	public UIButton JoinBtn;

	public UIButton ReportBtn;

	public UILabel ModHouseID;

	public UILabel ModOwnerID;

	public UITexture backgroundTex;

	private HouseData data;

	private UIPublicHouseChooser publicHouseChooser;

	public void Init(HouseData data, UIPublicHouseChooser publicHouseChooser)
	{
		this.data = data;
		this.publicHouseChooser = publicHouseChooser;
		HouseTitle.text = data.Name;
		MapName.text = data.MapName;
		OwnerName.text = data.OwnerName;
		Players.text = $"{data.CurrentPlayers} / {data.MaxPlayers}";
		if (Session.MyPlayerData.HasBadgeID(133))
		{
			ModHouseID.transform.parent.gameObject.SetActive(value: true);
			ModOwnerID.transform.parent.gameObject.SetActive(value: true);
			ModHouseID.text = data.HouseID.ToString();
			ModOwnerID.text = data.OwnerID.ToString();
		}
	}

	private void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(ReportBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnReportBtnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(JoinBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnJoinBtnClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(CloseBtn.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnCloseBtnClick));
	}

	private void OnDisable()
	{
		UIEventListener.Get(ReportBtn.gameObject).onClick = OnReportBtnClick;
		UIEventListener uIEventListener = UIEventListener.Get(JoinBtn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnJoinBtnClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(CloseBtn.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCloseBtnClick));
	}

	private void OnJoinBtnClick(GameObject go)
	{
		AEC.getInstance().sendRequest(new RequestHouseJoin(data));
	}

	private void OnReportBtnClick(GameObject go)
	{
		UICharReport.Show(data.OwnerName, $"HousingReport: HouseID: {data.HouseID} OwnerID {data.OwnerID}");
	}

	private void OnCloseBtnClick(GameObject go)
	{
		publicHouseChooser.CloseDetail();
	}
}

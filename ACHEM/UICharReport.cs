using System;
using System.Collections.Generic;
using UnityEngine;

public class UICharReport : UIWindow
{
	public enum ReportReasons
	{
		Vulgarity,
		Hacking,
		Lewdness,
		Bullying
	}

	private string reason = "";

	private string comment = "";

	public List<UIReportSelection> items;

	public UILabel lblName;

	public UILabel lblLevel;

	public UIButton btnSend;

	public UIInput inputDetails;

	public string targetName;

	public string additionalContext;

	public int isDuplicateReport;

	private static UICharReport instance;

	public static void Show(string name, string additionalContext = null)
	{
		if (!(name.ToLower() == Entities.Instance.me.name.ToLower()))
		{
			if (instance == null)
			{
				UIWindow.ClearWindows();
				instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/CharReport"), UIManager.Instance.transform).GetComponent<UICharReport>();
			}
			instance.Init(name, additionalContext);
		}
	}

	public void Init(string name, string additionalContext = null)
	{
		this.additionalContext = additionalContext;
		base.name = name;
		lblName.text = name;
		lblLevel.text = "";
		UIEventListener uIEventListener = UIEventListener.Get(btnSend.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnReportClick));
	}

	public void OnSelectedItem(UIReportSelection selected)
	{
		DeSelectItems();
		reason = selected.label.text;
		selected.bitSelect = true;
		selected.SetColor(Color.yellow);
		Debug.Log("Reason set to : " + reason + " FOCUS? " + UICamera.inputHasFocus);
	}

	private void DeSelectItems()
	{
		foreach (UIReportSelection item in items)
		{
			item.bitSelect = false;
			item.SetColor(Color.white);
		}
	}

	private void OnReportClick(GameObject go)
	{
		comment = inputDetails.value;
		Debug.Log("Report Clicked. Send Report: User: " + base.name + " - Reason: " + reason + " - Comment: " + comment);
		RequestReport requestReport = new RequestReport();
		requestReport.UserName = base.name.ToLower();
		requestReport.Report = reason.ToLower();
		if (additionalContext != null)
		{
			requestReport.Comment = additionalContext + " Player Comment: " + comment.ToLower();
		}
		else
		{
			requestReport.Comment = comment.ToLower();
		}
		isDuplicateReport = Session.MyPlayerData.RecordChecker(base.name, DateTime.Now);
		if (isDuplicateReport == 0)
		{
			MessageBox.Show("Report", "You cannot submit another report for " + base.name + " yet.");
			return;
		}
		Game.Instance.aec.sendRequest(requestReport);
		MessageBox.Show("Report", "Your report has been successfully submitted.");
		Session.MyPlayerData.AddToReportRecord(base.name, DateTime.Now);
	}

	private void removeListeners()
	{
	}
}

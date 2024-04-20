using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class UISummonWord : UIMenuWindow
{
	public UILabel input;

	public UILabel inputURL;

	public UIInput SummonWordInput;

	public UIButton btnSummonJoin;

	public UIButton btnGetWord;

	public UIButton btnCopyWord;

	public UIButton btnCopyURL;

	private string word;

	private string summon_link;

	private static UISummonWord instance;

	public static void Show()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/SummonFriend"), UIManager.Instance.transform).GetComponent<UISummonWord>();
		}
		instance.Init();
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnGetWord.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnGetWordClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnCopyWord.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(CopyCode));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnCopyURL.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CopyURL));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnSummonJoin.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnJoinClicked));
		if (string.IsNullOrEmpty(Session.SummonWord))
		{
			GetWord();
		}
		else
		{
			instance.input.text = Session.SummonWord;
		}
	}

	public void OnJoinClicked(GameObject go)
	{
		if (instance.SummonWordInput.value.Length != 0)
		{
			validateSummonWord(instance.SummonWordInput.value.Trim(' '));
		}
	}

	private void GetWord()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("cid", Session.MyPlayerData.ID);
		wWWForm.AddField("t", Session.Account.strToken);
		StartCoroutine(instance.GrabWord(wWWForm));
	}

	private IEnumerator GrabWord(WWWForm form)
	{
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/GetSummonWord", form);
		string errorTitle = "Failed to grab Summon Word!";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "URL: " + www.url;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		try
		{
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			if (aPIResponse.Error == null)
			{
				word = aPIResponse.Message;
				input.text = word;
				Session.SummonWord = word;
			}
		}
		catch (Exception ex)
		{
			customContext = "Download Handler: " + www.downloadHandler.text + " " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, form, customContext, ex);
		}
	}

	private void OnGetWordClick(GameObject go)
	{
		GetWord();
	}

	public void CopyCode(GameObject go)
	{
		Util.Clipboard = input.text;
	}

	public void CopyURL(GameObject go)
	{
		Util.Clipboard = inputURL.text;
	}

	protected override void Destroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnGetWord.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnGetWordClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnCopyWord.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(CopyCode));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnCopyURL.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CopyURL));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnSummonJoin.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnJoinClicked));
		base.Destroy();
	}

	public void validateSummonWord(string code)
	{
		if (!string.IsNullOrEmpty(code))
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("q", code);
			StartCoroutine(instance.ValidateSummonWordRoutine(wWWForm, code));
		}
	}

	private IEnumerator ValidateSummonWordRoutine(WWWForm form, string code)
	{
		using UnityWebRequest www = UnityWebRequest.Post(Main.WebServiceURL + "/Game/FindSummon", form);
		string errorTitle = "Failed to validate Summon Word!";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "Summon Word: " + code;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, form, customContext);
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, form, customContext);
			yield break;
		}
		try
		{
			APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(www.downloadHandler.text);
			if (aPIResponse.Error != null)
			{
				yield break;
			}
			string[] array = aPIResponse.Message.Split(',');
			if (array.Length > 1)
			{
				int.TryParse(array[0], out var result);
				int.TryParse(array[1], out var result2);
				if (Game.Instance.aec.ID == result2 || (result2 == -1 && Game.Instance.aec.IP == "localhost"))
				{
					Game.Instance.SendGotoRequest(result, code);
					yield break;
				}
				Confirmation.Show("Switch Server", "Your friend is on a different server.  Would you like to switch servers?", delegate(bool b)
				{
					if (b)
					{
						Session.pendingGoto = new GotoInfo(code);
						AEC.getInstance().sendRequest(new RequestDisconnect());
					}
				});
				yield break;
			}
			throw new Exception("Error parsing request");
		}
		catch (Exception)
		{
			MessageBox.Show("Error", "Invalid Friend Code. Your friend may be offline. Please try again later.");
		}
	}
}

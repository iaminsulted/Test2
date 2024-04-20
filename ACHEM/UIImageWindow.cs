using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class UIImageWindow : UIFullscreenWindow
{
	public string URL;

	public UITexture TargetTexture;

	public UIButton btnClose;

	public UIGrid grid;

	public List<ClientTriggerAction> ButtonActions;

	public List<UIImageWindowButton> Buttons;

	public List<string> Labels;

	public static UIImageWindow cs;

	public static void LoadImageWindow(string url, List<ClientTriggerAction> actions = null, List<string> labels = null)
	{
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/ImageWindow"), UIManager.Instance.transform);
		obj.name = "ImageWindow";
		cs = obj.GetComponent<UIImageWindow>();
		cs.URL = url;
		cs.ButtonActions = ((actions != null) ? actions : new List<ClientTriggerAction>());
		cs.Labels = ((labels != null) ? labels : new List<string>());
		cs.Init();
		cs.Activate();
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
		Loader.close();
		cs = null;
	}

	public static void CloseWindow()
	{
		if (cs != null)
		{
			cs.Close();
		}
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void Activate()
	{
		for (int i = 0; i < Buttons.Count; i++)
		{
			Buttons[i].gameObject.SetActive(value: false);
		}
		Loader.show("Loading image window", 0f);
		StartCoroutine("LoadFromURL");
	}

	public IEnumerator LoadFromURL()
	{
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(Main.APPLICATION_PATH + "/" + URL);
		string errorTitle = "Failed to load image: " + URL;
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "URL: " + URL;
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
			Texture content = DownloadHandlerTexture.GetContent(www);
			if (content != null)
			{
				TargetTexture.width = Mathf.CeilToInt((float)TargetTexture.height * (float)content.width / (float)content.height);
				TargetTexture.mainTexture = content;
			}
		}
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		ActivateButtons();
		grid.Reposition();
		Loader.close();
	}

	public void ActivateButtons()
	{
		for (int i = 0; i < Buttons.Count; i++)
		{
			if (ButtonActions.ElementAtOrDefault(i) == null)
			{
				Buttons[i].gameObject.SetActive(value: false);
				continue;
			}
			Buttons[i].gameObject.SetActive(value: true);
			string label = ((Labels.ElementAtOrDefault(i) != null) ? Labels[i] : "");
			Buttons[i].Init(ButtonActions[i], label);
			Buttons[i].Clicked += OnButtonClicked;
		}
	}

	private void OnButtonClicked()
	{
		Close();
	}

	public override void Close()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		base.Close();
	}
}

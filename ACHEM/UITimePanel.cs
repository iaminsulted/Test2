using System;
using UnityEngine;

public class UITimePanel : MonoBehaviour
{
	public GameObject root;

	public UILabel title;

	public UILabel description;

	public UISprite timeMeter;

	public UIButton yes;

	public UIButton no;

	public UIButton close;

	public UITimeButton timeButton;

	private bool inProgress;

	private float elapsed;

	private float duration;

	public void Awake()
	{
		root.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(yes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickYes));
		UIEventListener uIEventListener2 = UIEventListener.Get(no.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickNo));
		UIEventListener uIEventListener3 = UIEventListener.Get(close.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickClose));
	}

	public void Update()
	{
		if (inProgress)
		{
			float num = 1f - elapsed / duration;
			timeMeter.transform.localScale = new Vector3(num, 1f, 1f);
			timeButton.timeRadial.fillAmount = num;
			elapsed += Time.deltaTime;
			if (elapsed >= duration)
			{
				timeMeter.transform.localScale = new Vector3(0f, 1f, 1f);
				timeButton.timeRadial.fillAmount = 0f;
				RespondNo();
			}
		}
	}

	public void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(yes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickYes));
		UIEventListener uIEventListener2 = UIEventListener.Get(no.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickNo));
		UIEventListener uIEventListener3 = UIEventListener.Get(close.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickClose));
	}

	public void Init(string title, string description, float duration)
	{
		inProgress = true;
		elapsed = 0f;
		timeMeter.fillAmount = 1f;
		root.SetActive(value: true);
		this.title.text = "[583d1b]" + title + "[-]";
		this.description.text = "[583d1b]" + description + "[-]";
		this.duration = duration;
	}

	private void OnClickYes(GameObject go)
	{
		RespondYes();
	}

	private void OnClickNo(GameObject go)
	{
		RespondNo();
	}

	private void OnClickClose(GameObject go)
	{
		root.SetActive(value: false);
		if (inProgress)
		{
			timeButton.ShowButton();
		}
	}

	private void RespondYes()
	{
		root.SetActive(value: false);
		timeButton.HideButton();
		if (inProgress)
		{
			AEC.getInstance().sendRequest(new RequestTimedChoice(choice: true));
			inProgress = false;
		}
	}

	private void RespondNo()
	{
		root.SetActive(value: false);
		timeButton.HideButton();
		if (inProgress)
		{
			AEC.getInstance().sendRequest(new RequestTimedChoice(choice: false));
			inProgress = false;
		}
	}

	public void TurnOff()
	{
		inProgress = false;
		root.SetActive(value: false);
		timeButton.HideButton();
	}

	public void TurnOn()
	{
		root.SetActive(value: true);
	}
}

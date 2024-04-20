using System;
using UnityEngine;

public class UIVoteKick : MonoBehaviour
{
	public UILabel player;

	public UISprite timeMeter;

	public UIButton voteYes;

	public UIButton voteNo;

	public UIButton close;

	public GameObject root;

	private bool voteInProgress;

	private float voteElapsed;

	private UIVoteKickButton voteKickButton;

	public void Awake()
	{
		root.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(voteYes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickYes));
		UIEventListener uIEventListener2 = UIEventListener.Get(voteNo.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickNo));
		UIEventListener uIEventListener3 = UIEventListener.Get(close.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickClose));
	}

	public void Update()
	{
		if (!voteInProgress)
		{
			return;
		}
		float num = 1f - voteElapsed / 20f;
		timeMeter.transform.localScale = new Vector3(num, 1f, 1f);
		if (voteKickButton != null)
		{
			voteKickButton.timeRadial.fillAmount = num;
		}
		voteElapsed += Time.deltaTime;
		if (voteElapsed >= 20f)
		{
			timeMeter.transform.localScale = new Vector3(0f, 1f, 1f);
			if (voteKickButton != null)
			{
				voteKickButton.timeRadial.fillAmount = 0f;
			}
			VoteNo();
		}
	}

	public void OnDestroy()
	{
		UIEventListener uIEventListener = UIEventListener.Get(voteYes.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickYes));
		UIEventListener uIEventListener2 = UIEventListener.Get(voteNo.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickNo));
		UIEventListener uIEventListener3 = UIEventListener.Get(close.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickClose));
	}

	public void Init(string name, UIVoteKickButton voteKickButton = null)
	{
		root.SetActive(value: true);
		player.text = name;
		this.voteKickButton = voteKickButton;
		if (voteKickButton != null)
		{
			voteKickButton.playerName = name;
		}
	}

	public void StartVote()
	{
		voteInProgress = true;
		voteElapsed = 0f;
		timeMeter.fillAmount = 1f;
	}

	private void OnClickYes(GameObject go)
	{
		VoteYes();
	}

	private void OnClickNo(GameObject go)
	{
		VoteNo();
	}

	private void OnClickClose(GameObject go)
	{
		root.SetActive(value: false);
		if (voteInProgress)
		{
			if (voteKickButton != null)
			{
				voteKickButton.ShowButton();
			}
			else
			{
				AEC.getInstance().sendRequest(new RequestVoteKickChoice(choice: false));
			}
		}
	}

	private void VoteYes()
	{
		root.SetActive(value: false);
		if (voteKickButton != null)
		{
			voteKickButton.HideButton();
		}
		if (voteInProgress)
		{
			AEC.getInstance().sendRequest(new RequestVoteKickChoice(choice: true));
			voteInProgress = false;
		}
	}

	private void VoteNo()
	{
		root.SetActive(value: false);
		if (voteKickButton != null)
		{
			voteKickButton.HideButton();
		}
		if (voteInProgress)
		{
			AEC.getInstance().sendRequest(new RequestVoteKickChoice(choice: false));
			voteInProgress = false;
		}
	}

	public void TurnOff()
	{
		voteInProgress = false;
		root.SetActive(value: false);
		if (voteKickButton != null)
		{
			voteKickButton.HideButton();
		}
	}
}

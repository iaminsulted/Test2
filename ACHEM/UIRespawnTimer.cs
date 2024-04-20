using System;
using UnityEngine;

public class UIRespawnTimer : MonoBehaviour
{
	public UIButton btnRespawn;

	public UILabel labelCountDown;

	private float startTime;

	private float remainingTime;

	public UITweener[] Tweens = new UITweener[3];

	private static UIRespawnTimer instance;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
				if (visible)
				{
					Entities.Instance.me.RespawnEvent += OnSelfRespawn;
				}
				else
				{
					Entities.Instance.me.RespawnEvent -= OnSelfRespawn;
				}
			}
		}
	}

	public static void Show()
	{
		instance.Visible = true;
		instance.Init();
	}

	public static void Hide()
	{
		if (instance != null)
		{
			instance.Visible = false;
		}
	}

	public void Awake()
	{
		instance = this;
		UIEventListener uIEventListener = UIEventListener.Get(btnRespawn.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnRespawnClick));
		Visible = false;
	}

	public void Init()
	{
		UITweener[] tweens = Tweens;
		foreach (UITweener obj in tweens)
		{
			obj.tweenFactor = 0f;
			obj.PlayForward();
		}
		startTime = GameTime.realtimeSinceServerStartup;
		btnRespawn.isEnabled = false;
		InvokeRepeating("OnTimerUpdate", 0f, 0.25f);
	}

	private void OnRespawnClick(GameObject go)
	{
		Game.Instance.SendRespawnRequest();
	}

	private void OnSelfRespawn()
	{
		Visible = false;
	}

	private void OnTimerUpdate()
	{
		remainingTime = Entities.Instance.me.respawnTime - (GameTime.realtimeSinceServerStartup - startTime);
		if (remainingTime <= 0f)
		{
			CancelInvoke();
			labelCountDown.text = "0";
			btnRespawn.isEnabled = true;
		}
		else
		{
			labelCountDown.text = Mathf.CeilToInt(remainingTime).ToString();
		}
	}

	private void OnDestroy()
	{
		CancelInvoke();
		instance = null;
	}
}

using System;
using System.Collections;
using UnityEngine;

public class AugmentGraph : ModalWindow
{
	protected Action callback;

	[Header("Normal Box")]
	public UILabel Title;

	public UIButton ButtonClose;

	public UISprite healthBar;

	public UISprite attackBar;

	public UISprite armorBar;

	public UISprite evasionBar;

	public UISprite criticalBar;

	public UISprite hasteBar;

	private const float speed = 8f;

	public static AugmentGraph Instance { get; protected set; }

	protected virtual void Awake()
	{
		Instance = this;
	}

	protected virtual void OnDestroy()
	{
		Instance = null;
	}

	protected virtual void Start()
	{
		if (ButtonClose != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(ButtonClose.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(onCloseClick));
		}
	}

	protected void onCloseClick(GameObject go)
	{
		if (callback != null)
		{
			callback();
		}
		Close();
	}

	protected override void Close()
	{
		callback = null;
		if (Instance != null)
		{
			Instance = null;
		}
		base.Close();
	}

	public static void Show(ItemModifier mod, Action callback = null)
	{
		if (Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("AugmentGraph"), UIManager.Instance.transform);
			obj.name = "AugmentGraph";
			Instance = obj.GetComponent<AugmentGraph>();
			Instance.Init();
		}
		Instance.callback = callback;
		Instance.Title.text = mod.name;
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.maxHealth), Instance.healthBar));
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.attack), Instance.attackBar));
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.armor), Instance.armorBar));
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.evasion), Instance.evasionBar));
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.crit), Instance.criticalBar));
		Instance.StartCoroutine(moveBar(Mathf.FloorToInt(161f * mod.haste), Instance.hasteBar));
		if (Instance.healthBar.width < 5)
		{
			Instance.healthBar.gameObject.SetActive(value: false);
		}
		if (Instance.attackBar.width < 5)
		{
			Instance.attackBar.gameObject.SetActive(value: false);
		}
		if (Instance.armorBar.width < 5)
		{
			Instance.armorBar.gameObject.SetActive(value: false);
		}
		if (Instance.evasionBar.width < 5)
		{
			Instance.evasionBar.gameObject.SetActive(value: false);
		}
		if (Instance.criticalBar.width < 5)
		{
			Instance.criticalBar.gameObject.SetActive(value: false);
		}
		if (Instance.hasteBar.width < 5)
		{
			Instance.hasteBar.gameObject.SetActive(value: false);
		}
	}

	private static IEnumerator moveBar(float value, UISprite bar)
	{
		_ = bar.width;
		bar.gameObject.SetActive(value: true);
		while ((float)bar.width < value)
		{
			bar.width = (int)Math.Ceiling(Mathf.Lerp(bar.width, value, Time.deltaTime * 8f));
			yield return new WaitForSeconds(0.01f);
		}
	}
}

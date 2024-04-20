using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UISettings : UIWindow
{
	public enum Tab
	{
		General,
		Graphics,
		Audio,
		KeyBindings,
		Portraits,
		Titles,
		Advanced,
		Developer
	}

	public UIGrid UIGrid;

	public UIToggle tabGeneral;

	public UIToggle tabVisual;

	public UIToggle tabKeyBinding;

	public UIToggle tabPortraits;

	[FormerlySerializedAs("titles")]
	public UIToggle tabTitles;

	public UIToggle tabAudio;

	public UIToggle tabAdvanced;

	public UIToggle tabDeveloper;

	private static UISettings mInstance;

	public List<GameObject> MobileOnly = new List<GameObject>();

	public List<GameObject> PCOnly = new List<GameObject>();

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public static void Show(Tab startingTab = Tab.General, float alpha = 0.8f)
	{
		UIWindow.ClearWindows();
		CreateInstance(alpha);
		mInstance.SetStartingTab(startingTab);
	}

	private void SetStartingTab(Tab startingTab)
	{
		switch (startingTab)
		{
		default:
			tabGeneral.startsActive = true;
			break;
		case Tab.Graphics:
			tabVisual.startsActive = true;
			break;
		case Tab.Audio:
			tabAudio.startsActive = true;
			break;
		case Tab.KeyBindings:
			tabKeyBinding.startsActive = true;
			break;
		case Tab.Portraits:
			tabPortraits.startsActive = true;
			break;
		case Tab.Titles:
			tabTitles.startsActive = true;
			break;
		case Tab.Advanced:
			tabTitles.startsActive = true;
			break;
		}
	}

	private static void CreateInstance(float alpha = 1f)
	{
		if (mInstance == null)
		{
			GameObject obj = Object.Instantiate(Resources.Load<GameObject>("UISettings"), UIManager.Instance.transform);
			obj.name = "UISettings";
			mInstance = obj.GetComponent<UISettings>();
			mInstance.Init();
			mInstance.SetAlpha(alpha);
		}
	}

	public void SetAvailableOptions()
	{
		if (Platform.IsMobile || UIGame.ControlScheme == ControlScheme.HANDHELD)
		{
			foreach (GameObject item in PCOnly)
			{
				item.SetActive(value: false);
			}
			return;
		}
		foreach (GameObject item2 in MobileOnly)
		{
			item2.SetActive(value: false);
		}
	}

	protected override void Init()
	{
		if (UIGame.ControlScheme == ControlScheme.HANDHELD)
		{
			tabKeyBinding.gameObject.SetActive(value: false);
		}
		if (Session.MyPlayerData != null && Session.MyPlayerData.AccessLevel >= 100)
		{
			tabDeveloper.gameObject.SetActive(value: true);
		}
		State currentState = StateManager.CurrentState;
		if (!(currentState is CharSelect) && !(currentState is Game))
		{
			tabAdvanced.gameObject.SetActive(value: false);
		}
		if (!(StateManager.CurrentState is Game))
		{
			tabPortraits.gameObject.SetActive(value: false);
			tabTitles.gameObject.SetActive(value: false);
			tabDeveloper.gameObject.SetActive(value: false);
		}
		base.Init();
		UIGrid.Reposition();
	}

	protected override void Destroy()
	{
		foreach (KeyValuePair<int, Badge> item in Badges.map)
		{
			item.Value.isNew = false;
		}
		base.Destroy();
		if (Game.Instance != null)
		{
			Game.Instance.EnableControls();
		}
	}

	private void SetAlpha(float alpha)
	{
		GetComponent<UIPanel>().alpha = alpha;
	}
}

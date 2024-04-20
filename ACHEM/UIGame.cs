using System;
using UnityEngine;

public class UIGame : MonoBehaviour
{
	public static ControlScheme ControlScheme;

	public static Action JSDoubleClicked;

	public Transform Container;

	public ActionBar ActionBar;

	public Chat Chat;

	public UIPortrait PortraitPlayer;

	public UIPortrait PortraitTarget;

	public UIPortrait PortraitMob;

	public UIPortrait PortraitBoss;

	public UIPortrait PortraitNPC;

	public UIPortraitResourceNode PortraitResourceNode;

	public UIXPBar XPBar;

	public UIQuestTrackerContainer QuestTrackerContainer;

	public UIQuestTracker QuestTracker;

	public QuestNotification QuestNotification;

	public UIWar WarMeter;

	public UIScoreboard scoreBoard;

	public Transform targetPositionForLevelUpDragon;

	public GameMenuBar gameMenuBar;

	public UIPartyHUD partyHUD;

	public UIVoteKick voteKick;

	public UIVoteKickButton voteKickButton;

	public UIQueueNotification queueNotification;

	public UITimePanel timePanel;

	public UIFishing fishing;

	public UICellTimer cellTimer;

	public UISpeedrunTimer speedrunTimer;

	public GameObject pvpScore;

	public UISprite vignette;

	public UIHouseControls HouseControls;

	public UIJoystick joystick;

	public Transform target;

	private static UIGame mInstance;

	private bool visible = true;

	public static UIGame Instance => mInstance;

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
			}
		}
	}

	public static Vector2 JSDelta
	{
		get
		{
			if (Instance != null && Instance.joystick != null && Instance.joystick.gameObject.activeInHierarchy)
			{
				return Instance.joystick.position;
			}
			return Vector2.zero;
		}
	}

	public Vector2 FixedToolTipPosition => UICamera.currentCamera.WorldToScreenPoint(target.position);

	public Vector2 MouseToolTipPosition => Input.mousePosition + new Vector3(-10f, 30f, 0f);

	public void Awake()
	{
		mInstance = this;
		SettingsManager.JoystickModeUpdated += JoystickModeUpdated;
		SettingsManager.VignetteUpdated += VignetteUpdated;
		if (joystick != null)
		{
			joystick.centerOnPress = (int)SettingsManager.JoystickMode != 0;
			joystick.allowDragReposition = (int)SettingsManager.JoystickMode == 2;
			joystick.DoubleClicked += OnJSDoubleClick;
		}
		if (vignette != null)
		{
			vignette.alpha = SettingsManager.VignetteOpacity;
		}
		pvpScore.GetComponent<UIPvPScore>().Hide();
	}

	private void JoystickModeUpdated(UIJoystick.Mode mode)
	{
		if (joystick != null)
		{
			joystick.centerOnPress = mode != UIJoystick.Mode.Fixed;
			joystick.allowDragReposition = mode == UIJoystick.Mode.Follow;
		}
	}

	private void VignetteUpdated(float opacity)
	{
		if (vignette != null)
		{
			vignette.alpha = opacity;
		}
	}

	public void OnDestroy()
	{
		mInstance = null;
		SettingsManager.JoystickModeUpdated -= JoystickModeUpdated;
		SettingsManager.VignetteUpdated -= VignetteUpdated;
		if (joystick != null)
		{
			joystick.DoubleClicked -= OnJSDoubleClick;
		}
	}

	private void OnJSDoubleClick()
	{
		if (JSDoubleClicked != null)
		{
			JSDoubleClicked();
		}
	}

	public static void Init(Transform parent)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
		{
			ControlScheme = ControlScheme.HANDHELD;
		}
		else
		{
			ControlScheme = ControlScheme.PC;
		}
		GameObject gameObject = SettingsManager.InterfaceMode switch
		{
			0L => (ControlScheme != ControlScheme.HANDHELD) ? UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UI Root (2D) - Game (PC)")) : UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UI Root (2D) - Game (Mobile)")), 
			1L => UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UI Root (2D) - Game (Mobile)")), 
			2L => UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UI Root (2D) - Game (PC)")), 
			_ => UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UI Root (2D) - Game (PC)")), 
		};
		gameObject.name = "UI Root (2D) - Game";
		gameObject.transform.SetParent(parent, worldPositionStays: false);
	}

	public static void Reset()
	{
		if (mInstance != null)
		{
			mInstance.Chat.gameObject.SetActive(value: true);
			mInstance.QuestTrackerContainer.gameObject.SetActive(value: true);
			mInstance.ActionBar.gameObject.SetActive(value: true);
			mInstance.XPBar.gameObject.SetActive(value: true);
			mInstance.pvpScore.GetComponent<UIPvPScore>().Hide();
		}
	}

	public void ShowNewStatsButton()
	{
		gameMenuBar.ShowDelayedStatsButton();
	}
}

using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UISettingGeneral : MonoBehaviour
{
	[FormerlySerializedAs("GraphicsBox")]
	public UISettings UISettings;

	public UITable Table;

	public UIWindow UIWindowParent;

	public UIToggle toggleWhisper;

	public UIToggle toggleFriends;

	public UIToggle togglePartyInvites;

	public UIToggle toggleGuildInvites;

	public UIToggle toggleGoto;

	public UIToggle togglePvPDuelRequests;

	public UIToggle toggleTargetHighlighting;

	public UIToggle toggleTargetArrow;

	public UIToggle toggleMyName;

	public UIToggle toggleMyTitle;

	public UIToggle toggleMyHealth;

	public UIToggle toggleMyGuildTag;

	public UIToggle togglePlayerNames;

	public UIToggle togglePlayerTitles;

	public UIToggle togglePlayerHealth;

	public UIToggle togglePlayerGuildTags;

	public UIToggle toggleNpcNames;

	public UIToggle toggleNpcTitles;

	public UIToggle toggleNpcHealth;

	public UIToggle toggleChatBubble;

	public UIToggle SChat;

	public UIToggle MChat;

	public UIToggle LChat;

	public UIToggle toggleCameraX;

	public UIToggle toggleCameraY;

	public UIToggle toggleFreeCamera;

	public UIToggle toggleCameraZoom;

	public UIToggle toggleTargetLock;

	public UIToggle toggleCurrentHp;

	public UIToggle toggleMaxHp;

	public UIToggle togglePercentHp;

	public UIToggle toggleCurrentRp;

	public UIToggle toggleMaxRp;

	public UIToggle togglePercentRp;

	public UIToggle toggleChatFilter;

	public UIToggle toggleOtherPets;

	public UIToggle toggleSpellErrors;

	public UIPopupList TargetButtonType;

	public UIToggle JoystickFixed;

	public UIToggle JoystickDrag;

	public UIToggle JoystickFollow;

	public UIToggle JoystickAutorun;

	public UIToggle SingleSpellAutorun;

	public UIToggle AreaSpellAutorun;

	public UIToggle AoeFaceTarget;

	public UIToggle AttackNoTarget;

	public UIToggle PrioritizePvpTargets;

	public UIToggle CondenseEffects;

	public UIToggle FPSMonitor;

	public UIToggle LatencyMonitor;

	public UIToggle DebugOutputLog;

	public UISlider WindowSize;

	public UISlider ChatTextSize;

	public UISlider ChatBackgroundOpacity;

	public UIToggle ChatMessageTimestamp;

	public UIToggle AutoSheatheWeapons;

	public UISlider TurnSpeed;

	public UISlider CameraSpeed;

	public UISlider ZoomSpeed;

	public UISlider FOV;

	public UISlider CameraShake;

	private Vector2 CameraSpeedRange = new Vector2(1f, 15f);

	private readonly Vector2 ZoomRange = new Vector2(1f, 3f);

	private readonly Vector2 FOVRange = new Vector2(24f, 60f);

	private readonly Vector2 CamShakeRange = new Vector2(0f, 1f);

	private readonly Vector2 WindowSizeRange = new Vector2(0.5f, 1f);

	private readonly Vector2 ChatTextSizeRange = new Vector2(10f, 30f);

	private readonly Vector2 ChatBackgroundOpacityRange = new Vector2(0.49f, 0.7f);

	private readonly Vector2 TurnSpeedRange = new Vector2(0.1f, 2f);

	public void OnEnable()
	{
		if (Platform.IsMobile)
		{
			CameraSpeedRange = new Vector2(5f, 20f);
		}
		UISlider cameraSpeed = CameraSpeed;
		cameraSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(cameraSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider zoomSpeed = ZoomSpeed;
		zoomSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(zoomSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider fOV = FOV;
		fOV.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(fOV.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider cameraShake = CameraShake;
		cameraShake.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(cameraShake.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider windowSize = WindowSize;
		windowSize.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(windowSize.onDragFinished, new UIProgressBar.OnDragFinished(SaveMenuSize));
		UISlider turnSpeed = TurnSpeed;
		turnSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(turnSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveTurnSpeed));
		UISlider chatTextSize = ChatTextSize;
		chatTextSize.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(chatTextSize.onDragFinished, new UIProgressBar.OnDragFinished(SaveChatTextSize));
		UISlider chatBackgroundOpacity = ChatBackgroundOpacity;
		chatBackgroundOpacity.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(chatBackgroundOpacity.onDragFinished, new UIProgressBar.OnDragFinished(SaveChatBackgroundOpacity));
		toggleWhisper.value = SettingsManager.CanGetWhispers;
		toggleFriends.value = SettingsManager.CanGetFriendRequests;
		togglePartyInvites.value = SettingsManager.CanGetPartyInvites;
		toggleGuildInvites.value = SettingsManager.CanGetGuildInvites;
		toggleGoto.value = SettingsManager.IsGotoOn;
		togglePvPDuelRequests.value = SettingsManager.CanGetPvPDuelRequests;
		toggleTargetHighlighting.value = SettingsManager.IsTargetHighlightOn;
		toggleTargetArrow.value = SettingsManager.IsTargetArrowOn;
		toggleMyName.value = SettingsManager.IsMyNameOn;
		toggleMyTitle.value = SettingsManager.IsMyTitleOn;
		toggleMyHealth.value = SettingsManager.IsMyHealthBarOn;
		toggleMyGuildTag.value = SettingsManager.IsMyGuildTagOn;
		togglePlayerNames.value = SettingsManager.IsPlayerNameOn;
		togglePlayerTitles.value = SettingsManager.IsPlayerTitleOn;
		togglePlayerHealth.value = SettingsManager.IsPlayerHealthBarOn;
		togglePlayerGuildTags.value = SettingsManager.IsPlayerGuildTagOn;
		toggleNpcNames.value = SettingsManager.IsNPCNameOn;
		toggleNpcTitles.value = SettingsManager.IsNPCTitleOn;
		toggleNpcHealth.value = SettingsManager.IsNpcHealthBarOn;
		toggleChatBubble.value = SettingsManager.IsChatBubbleOn;
		SetChatWindowSizeToggle(SettingsManager.ChatWindowSize);
		TargetButtonType.SetToIndex(SettingsManager.TargetButtonType);
		JoystickAutorun.value = SettingsManager.IsAutoRunOn;
		SetJoystickMode(SettingsManager.JoystickMode);
		SingleSpellAutorun.value = SettingsManager.SingleSpellCastAutorun;
		AreaSpellAutorun.value = SettingsManager.AreaSpellCastAutorun;
		AoeFaceTarget.value = SettingsManager.AoeFaceTarget;
		AttackNoTarget.value = SettingsManager.CanAttackWithoutTarget;
		PrioritizePvpTargets.value = SettingsManager.PrioritizePvpTargets;
		AutoSheatheWeapons.value = SettingsManager.AutoSheatheWeapons;
		CondenseEffects.value = SettingsManager.CondenseEffects;
		FPSMonitor.value = SettingsManager.FPSMonitor;
		LatencyMonitor.value = SettingsManager.LatencyMonitor;
		DebugOutputLog.value = SettingsManager.DebugOutputLog;
		toggleCameraZoom.value = SettingsManager.IsCameraZoomEnabled;
		toggleCameraX.value = SettingsManager.InvertX;
		toggleCameraY.value = SettingsManager.InvertY;
		toggleFreeCamera.value = SettingsManager.FreeCamera;
		toggleTargetLock.value = SettingsManager.TargetCameraLock;
		ChatMessageTimestamp.value = SettingsManager.IsTimestampEnabled;
		toggleCurrentHp.value = SettingsManager.ShowCurrentHp;
		toggleMaxHp.value = SettingsManager.ShowMaxHp;
		togglePercentHp.value = SettingsManager.ShowPercentHp;
		toggleCurrentRp.value = SettingsManager.ShowCurrentRp;
		toggleMaxRp.value = SettingsManager.ShowMaxRp;
		togglePercentRp.value = SettingsManager.ShowPercentRp;
		toggleChatFilter.value = SettingsManager.IsChatFiltered;
		toggleOtherPets.value = SettingsManager.OtherPetsVisible;
		toggleSpellErrors.value = SettingsManager.ShowSpellErrors;
		CameraSpeed.SetValue(SettingsManager.CameraSpeed, CameraSpeedRange);
		ZoomSpeed.SetValue(SettingsManager.ZoomSpeed, ZoomRange);
		FOV.SetValue(SettingsManager.FieldOfView, FOVRange);
		CameraShake.SetValue(SettingsManager.CameraShake, CamShakeRange);
		WindowSize.SetValue(SettingsManager.MenuSize, WindowSizeRange);
		ChatTextSize.SetValue((int)SettingsManager.ChatTextSize, ChatTextSizeRange);
		ChatBackgroundOpacity.SetValue(SettingsManager.ChatBackgroundOpacity, ChatBackgroundOpacityRange);
		TurnSpeed.SetValue(SettingsManager.TurnSpeed, TurnSpeedRange);
		UIWindowParent.ChangeSize();
		UISettings.SetAvailableOptions();
		Table.Reposition();
	}

	private void OnDisable()
	{
		UISlider cameraSpeed = CameraSpeed;
		cameraSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(cameraSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider zoomSpeed = ZoomSpeed;
		zoomSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(zoomSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider fOV = FOV;
		fOV.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(fOV.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider cameraShake = CameraShake;
		cameraShake.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(cameraShake.onDragFinished, new UIProgressBar.OnDragFinished(SaveCameraSettings));
		UISlider windowSize = WindowSize;
		windowSize.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(windowSize.onDragFinished, new UIProgressBar.OnDragFinished(SaveMenuSize));
		UISlider turnSpeed = TurnSpeed;
		turnSpeed.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(turnSpeed.onDragFinished, new UIProgressBar.OnDragFinished(SaveTurnSpeed));
		UISlider chatTextSize = ChatTextSize;
		chatTextSize.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(chatTextSize.onDragFinished, new UIProgressBar.OnDragFinished(SaveChatTextSize));
		UISlider chatBackgroundOpacity = ChatBackgroundOpacity;
		chatBackgroundOpacity.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Remove(chatBackgroundOpacity.onDragFinished, new UIProgressBar.OnDragFinished(SaveChatBackgroundOpacity));
	}

	public void WindowSizeChanged()
	{
		SettingsManager.MenuSize.Set(WindowSize.GetValue(WindowSizeRange), updatePref: false, savePrefsToDisk: false);
		UIWindowParent.ChangeSize();
	}

	private void SaveMenuSize()
	{
		SettingsManager.MenuSize.Set(WindowSize.GetValue(WindowSizeRange), updatePref: true, savePrefsToDisk: true, broadcastUpdate: false);
	}

	public void ChatTextSizeChanged()
	{
		SettingsManager.ChatTextSize.Set((int)ChatTextSize.GetValue(ChatTextSizeRange), updatePref: false, savePrefsToDisk: false);
	}

	public void SaveChatTextSize()
	{
		SettingsManager.ChatTextSize.Set((int)ChatTextSize.GetValue(ChatTextSizeRange), updatePref: true, savePrefsToDisk: true, broadcastUpdate: false);
	}

	public void ChatBackgroundOpacityChanged()
	{
		SettingsManager.ChatBackgroundOpacity.Set(ChatBackgroundOpacity.GetValue(ChatBackgroundOpacityRange), updatePref: false, savePrefsToDisk: false);
	}

	public void SaveChatBackgroundOpacity()
	{
		SettingsManager.ChatBackgroundOpacity.Set(ChatBackgroundOpacity.GetValue(ChatBackgroundOpacityRange), updatePref: true, savePrefsToDisk: true, broadcastUpdate: false);
	}

	public void OnToggleTimeStamp()
	{
		SettingsManager.IsTimestampEnabled.Set(ChatMessageTimestamp.value);
	}

	private void SaveTurnSpeed()
	{
		SettingsManager.TurnSpeed.Set(TurnSpeed.GetValue(TurnSpeedRange), updatePref: true, savePrefsToDisk: true, broadcastUpdate: false);
	}

	public void ResetToDefault()
	{
		toggleWhisper.value = SettingsManager.CanGetWhispers.Default;
		toggleFriends.value = SettingsManager.CanGetFriendRequests.Default;
		togglePartyInvites.value = SettingsManager.CanGetPartyInvites.Default;
		toggleGuildInvites.value = SettingsManager.CanGetGuildInvites.Default;
		toggleGoto.value = SettingsManager.IsGotoOn.Default;
		togglePvPDuelRequests.value = SettingsManager.CanGetPvPDuelRequests.Default;
		toggleTargetHighlighting.value = SettingsManager.IsTargetHighlightOn.Default;
		toggleTargetArrow.value = SettingsManager.IsTargetArrowOn.Default;
		toggleMyName.value = SettingsManager.IsMyNameOn.Default;
		toggleMyTitle.value = SettingsManager.IsMyTitleOn.Default;
		toggleMyHealth.value = SettingsManager.IsMyHealthBarOn;
		toggleMyGuildTag.value = SettingsManager.IsMyGuildTagOn.Default;
		togglePlayerNames.value = SettingsManager.IsPlayerNameOn.Default;
		togglePlayerTitles.value = SettingsManager.IsPlayerTitleOn.Default;
		togglePlayerHealth.value = SettingsManager.IsPlayerHealthBarOn;
		togglePlayerGuildTags.value = SettingsManager.IsPlayerGuildTagOn.Default;
		toggleNpcNames.value = SettingsManager.IsNPCNameOn.Default;
		toggleNpcTitles.value = SettingsManager.IsNPCTitleOn.Default;
		toggleNpcHealth.value = SettingsManager.IsNpcHealthBarOn;
		toggleChatBubble.value = SettingsManager.IsChatBubbleOn.Default;
		SetChatWindowSizeToggle(SettingsManager.ChatWindowSize.Default);
		toggleCameraX.value = SettingsManager.InvertX.Default;
		toggleCameraY.value = SettingsManager.InvertY.Default;
		toggleFreeCamera.value = SettingsManager.FreeCamera.Default;
		toggleCameraZoom.value = SettingsManager.IsCameraZoomEnabled.Default;
		toggleTargetLock.value = SettingsManager.TargetCameraLock.Default;
		TargetButtonType.SetToIndex(SettingsManager.TargetButtonType.Default);
		SetJoystickMode(SettingsManager.JoystickMode.Default);
		JoystickAutorun.value = SettingsManager.IsAutoRunOn.Default;
		SingleSpellAutorun.value = SettingsManager.SingleSpellCastAutorun.Default;
		AreaSpellAutorun.value = SettingsManager.AreaSpellCastAutorun.Default;
		AoeFaceTarget.value = SettingsManager.AoeFaceTarget.Default;
		AttackNoTarget.value = SettingsManager.CanAttackWithoutTarget.Default;
		PrioritizePvpTargets.value = SettingsManager.PrioritizePvpTargets.Default;
		AutoSheatheWeapons.value = SettingsManager.AutoSheatheWeapons.Default;
		CondenseEffects.value = SettingsManager.CondenseEffects.Default;
		FPSMonitor.value = SettingsManager.FPSMonitor.Default;
		LatencyMonitor.value = SettingsManager.LatencyMonitor.Default;
		DebugOutputLog.value = SettingsManager.DebugOutputLog.Default;
		toggleCurrentHp.value = SettingsManager.ShowCurrentHp.Default;
		toggleMaxHp.value = SettingsManager.ShowMaxHp.Default;
		togglePercentHp.value = SettingsManager.ShowPercentHp.Default;
		toggleCurrentRp.value = SettingsManager.ShowCurrentRp.Default;
		toggleMaxRp.value = SettingsManager.ShowMaxRp.Default;
		togglePercentRp.value = SettingsManager.ShowPercentRp.Default;
		toggleChatFilter.value = SettingsManager.IsChatFiltered.Default;
		toggleOtherPets.value = SettingsManager.OtherPetsVisible.Default;
		toggleSpellErrors.value = SettingsManager.ShowSpellErrors.Default;
		CameraSpeed.SetValue(SettingsManager.CameraSpeed.Default, CameraSpeedRange);
		ZoomSpeed.SetValue(SettingsManager.ZoomSpeed.Default, ZoomRange);
		FOV.SetValue(SettingsManager.FieldOfView.Default, FOVRange);
		CameraShake.SetValue(SettingsManager.CameraShake.Default, CamShakeRange);
		WindowSize.value = SettingsManager.MenuSize.Default;
		TurnSpeed.value = SettingsManager.TurnSpeed.Default;
		SettingsManager.SetCameraSettings(SettingsManager.CameraSpeed.Default, SettingsManager.ZoomSpeed.Default, SettingsManager.FieldOfView.Default, SettingsManager.CameraShake.Default, SettingsManager.InvertX.Default, SettingsManager.InvertY.Default, SettingsManager.FreeCamera.Default, SettingsManager.TargetCameraLock.Default);
	}

	public void SaveCameraSettings()
	{
		SettingsManager.SetAndSaveCameraSettings(CameraSpeed.GetValue(CameraSpeedRange), ZoomSpeed.GetValue(ZoomRange), FOV.GetValue(FOVRange), CameraShake.GetValue(CamShakeRange), toggleCameraX.value, toggleCameraY.value, toggleFreeCamera.value, toggleTargetLock.value);
		UIWindowParent.ChangeSize();
	}

	private void SetChatWindowSizeToggle(int chatSize)
	{
		switch (chatSize)
		{
		case 0:
			SChat.value = true;
			break;
		case 1:
			MChat.value = true;
			break;
		case 2:
			LChat.value = true;
			break;
		default:
			MChat.value = true;
			break;
		}
	}

	private void SetJoystickMode(int mode)
	{
		switch ((UIJoystick.Mode)mode)
		{
		case UIJoystick.Mode.Fixed:
			JoystickFixed.value = true;
			break;
		case UIJoystick.Mode.Drag:
			JoystickDrag.value = true;
			break;
		case UIJoystick.Mode.Follow:
			JoystickFollow.value = true;
			break;
		default:
			JoystickFollow.value = true;
			break;
		}
	}

	public void UpdateCameraSettings()
	{
		SettingsManager.SetCameraSettings(CameraSpeed.GetValue(CameraSpeedRange), ZoomSpeed.GetValue(ZoomRange), FOV.GetValue(FOVRange), CameraShake.GetValue(CamShakeRange), toggleCameraX.value, toggleCameraY.value, toggleFreeCamera.value, toggleTargetLock.value);
	}

	public void OnCameraZoomChanged()
	{
		SettingsManager.IsCameraZoomEnabled.Set(toggleCameraZoom.value);
	}

	public void OnTargetButtonTypeChanged()
	{
		SettingsManager.TargetButtonType.Set(TargetButtonType.CurrentIndex());
	}

	public void OnTargetLockChanged()
	{
		SettingsManager.TargetCameraLock.Set(toggleTargetLock.value);
	}

	public void OnJoystickAutorunChanged()
	{
		SettingsManager.IsAutoRunOn.Set(JoystickAutorun.value);
	}

	public void OnSingleSpellAutorunChanged()
	{
		SettingsManager.SingleSpellCastAutorun.Set(SingleSpellAutorun.value);
	}

	public void OnAreaSpellAutorunChanged()
	{
		SettingsManager.AreaSpellCastAutorun.Set(AreaSpellAutorun.value);
	}

	public void OnAoeFaceTargetChanged()
	{
		SettingsManager.AoeFaceTarget.Set(AoeFaceTarget.value);
	}

	public void OnAttackNoTargetChanged()
	{
		SettingsManager.CanAttackWithoutTarget.Set(AttackNoTarget.value);
	}

	public void OnPrioritizePvpTargetsChanged()
	{
		SettingsManager.PrioritizePvpTargets.Set(PrioritizePvpTargets.value);
	}

	public void OnAutoSheatheWeaponsChanged()
	{
		SettingsManager.AutoSheatheWeapons.Set(AutoSheatheWeapons.value);
	}

	public void OnCondenseEffectsChanged()
	{
		SettingsManager.CondenseEffects.Set(CondenseEffects.value);
	}

	public void OnFPSMonitorChanged()
	{
		SettingsManager.FPSMonitor.Set(FPSMonitor.value);
	}

	public void OnLatencyMonitorChanged()
	{
		SettingsManager.LatencyMonitor.Set(LatencyMonitor.value);
	}

	public void OnDebugOutputLogChanged()
	{
		SettingsManager.DebugOutputLog.Set(DebugOutputLog.value);
	}

	public void OnWhisperChange()
	{
		SettingsManager.CanGetWhispers.Set(toggleWhisper.value);
	}

	public void OnFriendRequestChanged()
	{
		SettingsManager.CanGetFriendRequests.Set(toggleFriends.value);
	}

	public void OnPartyInviteChanged()
	{
		SettingsManager.CanGetPartyInvites.Set(togglePartyInvites.value);
	}

	public void OnGuildInviteChanged()
	{
		SettingsManager.CanGetGuildInvites.Set(toggleGuildInvites.value);
	}

	public void OnGotoChanged()
	{
		SettingsManager.IsGotoOn.Set(toggleGoto.value);
	}

	public void OnPvPDuelRequestsChanged()
	{
		SettingsManager.CanGetPvPDuelRequests.Set(togglePvPDuelRequests.value);
	}

	public void OnTargetHighlightingChanged()
	{
		SettingsManager.IsTargetHighlightOn.Set(toggleTargetHighlighting.value);
	}

	public void OnTargetArrowChanged()
	{
		SettingsManager.IsTargetArrowOn.Set(toggleTargetArrow.value);
	}

	public void OnMyNameChanged()
	{
		SettingsManager.IsMyNameOn.Set(toggleMyName.value);
	}

	public void OnMyTitleChanged()
	{
		SettingsManager.IsMyTitleOn.Set(toggleMyTitle.value);
	}

	public void OnMyHealthBarChanged()
	{
		SettingsManager.IsMyHealthBarOn.Set(toggleMyHealth.value);
	}

	public void OnMyGuildTagChanged()
	{
		SettingsManager.IsMyGuildTagOn.Set(toggleMyGuildTag.value);
	}

	public void OnPlayerNameChanged()
	{
		SettingsManager.IsPlayerNameOn.Set(togglePlayerNames.value);
	}

	public void OnPlayerTitleChanged()
	{
		SettingsManager.IsPlayerTitleOn.Set(togglePlayerTitles.value);
	}

	public void OnPlayerHealthBarChanged()
	{
		SettingsManager.IsPlayerHealthBarOn.Set(togglePlayerHealth.value);
	}

	public void OnPlayerGuildTagChanged()
	{
		SettingsManager.IsPlayerGuildTagOn.Set(togglePlayerGuildTags.value);
	}

	public void OnNpcNameChanged()
	{
		SettingsManager.IsNPCNameOn.Set(toggleNpcNames.value);
	}

	public void OnNpcTitleChanged()
	{
		SettingsManager.IsNPCTitleOn.Set(toggleNpcTitles.value);
	}

	public void OnNpcHealthBarChanged()
	{
		SettingsManager.IsNpcHealthBarOn.Set(toggleNpcHealth.value);
	}

	public void OnChatBubbleChanged()
	{
		SettingsManager.IsChatBubbleOn.Set(toggleChatBubble.value);
	}

	public void OnChatWindowChanged()
	{
		int newValue;
		if (SChat.value)
		{
			newValue = 0;
		}
		else if (MChat.value)
		{
			newValue = 1;
		}
		else
		{
			if (!LChat.value)
			{
				return;
			}
			newValue = 2;
		}
		SettingsManager.ChatWindowSize.Set(newValue);
	}

	public void OnJoystickModeChanged()
	{
		int newValue;
		if (JoystickFixed.value)
		{
			newValue = 0;
		}
		else if (JoystickDrag.value)
		{
			newValue = 1;
		}
		else
		{
			if (!JoystickFollow.value)
			{
				return;
			}
			newValue = 2;
		}
		SettingsManager.JoystickMode.Set(newValue);
	}

	public void OnCurrentHpChanged()
	{
		SettingsManager.ShowCurrentHp.Set(toggleCurrentHp.value);
	}

	public void OnMaxHpChanged()
	{
		SettingsManager.ShowMaxHp.Set(toggleMaxHp.value);
	}

	public void OnPercentHpChanged()
	{
		SettingsManager.ShowPercentHp.Set(togglePercentHp.value);
	}

	public void OnCurrentRpChanged()
	{
		SettingsManager.ShowCurrentRp.Set(toggleCurrentRp.value);
	}

	public void OnMaxRpChanged()
	{
		SettingsManager.ShowMaxRp.Set(toggleMaxRp.value);
	}

	public void OnPercentRpChanged()
	{
		SettingsManager.ShowPercentRp.Set(togglePercentRp.value);
	}

	public void OnChatFilterChanged()
	{
		SettingsManager.IsChatFiltered.Set(toggleChatFilter.value);
	}

	public void OnOtherPetsChanged()
	{
		SettingsManager.OtherPetsVisible.Set(toggleOtherPets.value);
	}

	public void OnSpellErrorsChanged()
	{
		SettingsManager.ShowSpellErrors.Set(toggleSpellErrors.value);
	}
}

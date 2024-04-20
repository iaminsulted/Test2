using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SettingsManager
{
	private static readonly Dictionary<InputAction, Keybind> dictionary;

	private const string Pref_Camera_Speed = "CameraSpeed";

	private const string Pref_Zoom_Speed = "ZoomSpeed";

	private const string Pref_Invert_X = "InvertX";

	private const string Pref_Invert_Y = "InvertY";

	private const string Pref_Free_Camera = "FreeCamera";

	private const string Pref_Field_Of_View = "FOV";

	private const string Pref_Camera_Shake = "CameraShake";

	private const string Pref_Joystick_Mode = "JoystickMode";

	private const string Pref_Menu_Size = "WindowSize";

	private const string Pref_ChatText_Size = "ChatTextSize";

	private const string Pref_Chat_Background_Opacity = "ChatBackgroundOpacity";

	private const string Pref_Timestamp_Enabled = "TimestampEnabled";

	private const string Pref_Auto_Run = "jsAutorun";

	private const string Pref_Attack_No_Target = "AtkNoTgt";

	private const string Pref_Prioritize_Pvp_Targets = "PrioritizePvpTargets";

	private const string Pref_Auto_Sheathe_Weapons = "AutoSheatheWeapons";

	private const string Pref_Fullscreen_Type = "FullscreenType";

	private const string Pref_Resolution_Index = "ResolutionIndex";

	private const string Pref_Graphics_Quality = "QualityLevel";

	private const string Pref_Texture_Quality = "TextureQuality";

	private const string Pref_Anisotropic = "Anisotropic";

	private const string Pref_Anti_Aliasing = "AntiAliasing";

	private const string Pref_Shadow_Quality = "ShadowQuality";

	private const string Pref_Shadow_Distance = "ShadowDistance";

	private const string Pref_Vsync = "VsyncChoice";

	private const string Pref_Fps_Limit = "FpsLimit";

	private const string Pref_Particle_Limit = "ParticleLimit";

	private const string Pref_Particle_Raycasts = "ParticleQuality";

	private const string Pref_Particle_Quality = "HDParticles";

	private const string Pref_Draw_Distance = "DrawDistance";

	private const string Pref_Use_Bloom = "UseBloom";

	private const string Pref_Use_Depth_Of_Field = "UseDepth";

	private const string Pref_Grass_Distance = "GrassDistance";

	private const string Pref_Vignette_Opacity = "VignetteOpacity";

	private const string Pref_Mobile_Grass_Enabled = "isGrassEnabled";

	private const string Pref_Whisper_Enabled = "isWhisperEnabled";

	private const string Pref_Friend_Requests = "isFriendRequestEnabled";

	private const string Pref_Party_Invites = "isPartyInviteEnabled";

	private const string Pref_Guild_Invites = "isGuildInviteEnabled";

	private const string Pref_PvP_Duels = "isPvPDuelsEnabled";

	private const string Pref_Target_Highlight = "isTargetHighlightingEnabled";

	private const string Pref_Target_Arrow = "isTargetArrowEnabled";

	private const string Pref_My_Name = "isMyNameOn";

	private const string Pref_My_Guild_Tag = "isMyGuildTagOn";

	private const string Pref_My_Health = "isMyHealthOn";

	private const string Pref_My_Title = "isMyTitleOn";

	private const string Pref_Player_Name = "isPlayerNameOn";

	private const string Pref_Player_Guild_Tag = "isPlayerGuildTagOn";

	private const string Pref_Player_Health = "isPlayerHealthOn";

	private const string Pref_Player_Title = "isPlayerTitleOn";

	private const string Pref_Npc_Name = "isNpcNameOn";

	private const string Pref_Npc_Title = "isNpcTitleOn";

	private const string Pref_Npc_Health = "isNpcHealthOn";

	private const string Pref_Camera_Zoom_Distance = "cameraZoomDistance";

	private const string Pref_Camera_Zoom_Enabled = "CameraZoomOn";

	private const string Pref_Chat_Window_Size = "chatWindowSize";

	private const string Pref_Action_Bar_Slot = "ActionBarSlot";

	private const string Pref_Key_Bind = "Key";

	private const string Pref_Player_Ignore_List = "Ignore";

	private const string Pref_Chat_Filter_Enabled = "ChatFilter";

	private const string Pref_Chat_Bubble_Enabled = "ChatBubble";

	private const string Pref_Goto_Enabled = "Goto";

	private const string Pref_Tracked_Quest = "TrackedQuestID";

	private const string Pref_Turn_Speed = "RotateSpeed";

	private const string Pref_Map_Camera_Reset = "MapCameraReset";

	private const string Pref_Target_Lock = "TargetCameraLock";

	private const string Pref_Condense_Effects = "ShowAllEffects";

	private const string Pref_FPS_Monitor = "FPSMonitor";

	private const string Pref_Latency_Monitor = "LatencyMonitor";

	private const string Pref_Debug_Output_Log = "DebugOutputLog";

	private const string Pref_Show_Current_Hp = "ShowCurrentHp";

	private const string Pref_Show_Max_Hp = "ShowMaxHp";

	private const string Pref_Show_Percent_Hp = "ShowPercentHp";

	private const string Pref_Show_Current_Rp = "ShowCurrentRp";

	private const string Pref_Show_Max_Rp = "ShowMaxRp";

	private const string Pref_Show_Percent_Rp = "ShowPercentRp";

	private const string Pref_Single_Spell_Cast_Autorun = "SingleSpellCastAutorun";

	private const string Pref_Area_Spell_Cast_Autorun = "SpellCastAutorun";

	private const string Pref_Aoe_Face_Target = "AoeFaceTarget";

	private const string Pref_Target_Button_Type = "TargetButtonType";

	private const string Pref_Master_Volume = "MasterVolume";

	private const string Pref_Sfx_Volume = "SfxVolume";

	private const string Pref_Music_Volume = "MusicVolume";

	private const string Pref_Cinematic_Volume = "CinematicVolume";

	private const string Pref_Ambient_Volume = "AmbientVolume";

	private const string Pref_Sound_Only_Focus = "SoundOnlyFocus";

	private const string Pref_Master_Enabled = "MasterEnabled";

	private const string Pref_Sfx_Enabled = "SfxEnabled";

	private const string Pref_Music_Enabled = "MusicEnabled";

	private const string Pref_Cinematic_Enabled = "CinematicEnabled";

	private const string Pref_Ambient_Enabled = "AmbientEnabled";

	private const string Pref_Other_Pets = "OtherPets";

	private const string Pref_Interface_Mode = "InterfaceMode";

	private const string Pref_Show_Spell_Errors = "ShowSpellErrors";

	private const string Pref_Dev_Btn_ApopID = "DvBtnApop";

	private const string Pref_Dev_Btn_Always_Show = "DvBtnAlwaysShow";

	private const string Pref_Mail_Blocked = "MailNotBlocked";

	private const string Pref_Mail_Friends = "MailIsFriendsOnly";

	private const string Pref_Mail_All = "MailNotAll";

	private const int Action_Slot_Count = 4;

	private const float Default_Zoom_Speed = 2f;

	private const float Default_Camera_Zoom = 6f;

	private const float Default_Field_Of_View = 42f;

	private const float Default_Camera_Shake = 1f;

	private const bool Default_Invert_X = false;

	private const bool Default_Invert_Y = false;

	private const bool Default_Free_Camera = false;

	private const int Default_Joystick_Mode = 2;

	private const bool Default_Auto_Run_Enabled = true;

	private const bool Default_Attack_No_Target = true;

	private const bool Default_Prioritize_Pvp_Targets = true;

	private const bool Default_Auto_Sheathe_Weapons = false;

	private const int Default_Fullscreen_Type = 0;

	private const int Default_Resolution_Index = int.MaxValue;

	private const int Default_Vsync = 1;

	private const bool Default_Is_Whisper_Enabled = true;

	private const bool Default_Friend_Request_Enabled = true;

	private const bool Default_Party_Invites_Enabled = true;

	private const bool Default_Guild_Invites_Enabled = true;

	private const bool Default_PvP_Duels_Enabled = true;

	private const bool Default_Chat_Filter_Enabled = true;

	private const bool Default_Chat_Bubble_Enabled = true;

	private const bool Default_Goto_Enabled = true;

	private const bool Default_Target_Highlight_Enabled = true;

	private const bool Default_Target_Arrow_Enabled = true;

	private const bool Default_My_Name = true;

	private const bool Default_My_Guild_Tag = true;

	private const bool Default_My_Health = true;

	private const bool Default_My_Title = true;

	private const bool Default_Player_Name = true;

	private const bool Default_Player_Guild_Tag = true;

	private const bool Default_Player_Health = true;

	private const bool Default_Player_Title = true;

	private const bool Default_Npc_Name = true;

	private const bool Default_Npc_Title = true;

	private const bool Default_Npc_Health = true;

	private const int Default_Chat_Window_Size = 1;

	private const float Default_Turn_Speed = 1f;

	private const bool Default_Map_Camera_Reset = true;

	private const bool Default_Target_Lock = true;

	private const bool Default_Condense_Effects = true;

	private const bool Default_FPS_Monitor = false;

	private const bool Default_Latency_Monitor = false;

	private const bool Default_Debug_Output_Log = false;

	private const bool Default_Show_Current_Hp = true;

	private const bool Default_Show_Max_Hp = false;

	private const bool Default_Show_Percent_Hp = false;

	private const bool Default_Show_Current_Rp = true;

	private const bool Default_Show_Max_Rp = false;

	private const bool Default_Show_Percent_Rp = false;

	private const bool Default_Single_Spell_Cast_Autorun = true;

	private const bool Default_Area_Spell_Cast_Autorun = true;

	private const bool Default_Aoe_Face_Target = true;

	private const int Default_Target_Button_Type = 0;

	private const float Default_Master_Volume = 1f;

	private const float Default_Music_Volume = 0.75f;

	private const float Default_Cinematic_Volume = 1f;

	private const float Default_Ambient_Volume = 1f;

	private const float Default_Sfx_Volume = 1f;

	private const bool Default_Sound_Only_Focus = false;

	private const bool Default_Master_Enabled = true;

	private const bool Default_Music_Enabled = true;

	private const bool Default_Cinematic_Enabled = true;

	private const bool Default_Ambient_Enabled = true;

	private const bool Default_Sfx_Enabled = true;

	private const bool Default_Other_Pets = true;

	private const int Default_Interface_Mode = 0;

	private const bool Default_Show_Spell_Errors = true;

	private const int Default_ChatText_Size = 15;

	private const float Default_ChatBackground_Opacity = 0.5f;

	private const bool Default_Timestamp = false;

	private const string Default_Dev_Btn_ApopID = "7377";

	private const bool Default_Dev_Btn_Always_Show = false;

	private const string Default_Mail_Blocked = "";

	private const string Default_Mail_Friends = "2/19/2024 2:27:31 PM";

	private const string Default_Mail_All = "";

	private const float Default_Camera_Speed = 5f;

	private const float Default_Menu_Size = 0.8f;

	private const int Default_Graphics_Quality = 4;

	private const int Default_Texture_Quality = 2;

	private const int Default_Anisotropic = 2;

	private const int Default_Anti_Aliasing = 2;

	private const int Default_Shadow_Quality = 3;

	private const int Default_Shadow_Distance = 2;

	private const int Default_Particle_Raycasts = 4;

	private const int Default_Particle_Quality = 1;

	private const int Default_Draw_Distance = 3;

	private const bool Default_Use_Bloom = false;

	private const bool Default_Use_Depth_Of_Field = false;

	private const bool Default_Mobile_Grass_Enabled = true;

	private const float Default_Grass_Distance = 200f;

	private const float Default_Vignette_Opacity = 0.5f;

	private const bool Default_Camera_Zoom_Enabled = true;

	private const int Default_Particle_Limit = 4;

	private const int Default_Fps_Limit = 1;

	private static bool isInitialized;

	public static List<string> ignoreList;

	private static int[] actionSlotIDs;

	public static readonly Setting<bool> IsCameraZoomEnabled;

	public static readonly Setting<float> CameraSpeed;

	public static readonly Setting<float> ZoomSpeed;

	public static readonly Setting<float> CameraZoomDistance;

	public static readonly Setting<bool> InvertX;

	public static readonly Setting<bool> InvertY;

	public static readonly Setting<bool> FreeCamera;

	public static readonly Setting<float> FieldOfView;

	public static readonly Setting<float> CameraShake;

	public static readonly Setting<bool> MapCameraReset;

	public static readonly Setting<bool> TargetCameraLock;

	public static readonly Setting<int> ResolutionIndex;

	public static readonly Setting<int> DisplayMode;

	public static readonly Setting<int> GraphicsQuality;

	public static readonly Setting<int> TextureQuality;

	public static readonly Setting<int> AnisotropicFilter;

	public static readonly Setting<int> AntiAliasing;

	public static readonly Setting<int> ShadowQuality;

	public static readonly Setting<int> ShadowDistance;

	public static readonly Setting<int> ParticleRaycasts;

	public static readonly Setting<int> ParticleQuality;

	public static readonly Setting<int> DrawDistance;

	public static readonly Setting<bool> UseBloom;

	public static readonly Setting<bool> UseDepthOfField;

	public static readonly Setting<float> GrassDistance;

	public static readonly Setting<float> VignetteOpacity;

	public static readonly Setting<bool> ShowMobileGrass;

	public static readonly Setting<int> VerticalSync;

	public static readonly Setting<int> FpsLimit;

	public static readonly Setting<int> ParticleLimit;

	public static readonly Setting<int> InterfaceMode;

	public static readonly Setting<bool> CanGetWhispers;

	public static readonly Setting<bool> CanGetFriendRequests;

	public static readonly Setting<bool> CanGetPartyInvites;

	public static readonly Setting<bool> CanGetGuildInvites;

	public static readonly Setting<bool> CanGetPvPDuelRequests;

	public static readonly Setting<bool> IsChatFiltered;

	public static readonly Setting<bool> IsChatBubbleOn;

	public static readonly Setting<bool> IsGotoOn;

	public static readonly Setting<bool> IsMyNameOn;

	public static readonly Setting<bool> IsMyTitleOn;

	public static readonly Setting<bool> IsMyHealthBarOn;

	public static readonly Setting<bool> IsMyGuildTagOn;

	public static readonly Setting<bool> IsPlayerNameOn;

	public static readonly Setting<bool> IsPlayerTitleOn;

	public static readonly Setting<bool> IsPlayerHealthBarOn;

	public static readonly Setting<bool> IsPlayerGuildTagOn;

	public static readonly Setting<bool> IsNPCNameOn;

	public static readonly Setting<bool> IsNPCTitleOn;

	public static readonly Setting<bool> IsNpcHealthBarOn;

	public static readonly Setting<int> ChatWindowSize;

	public static readonly Setting<float> MenuSize;

	public static readonly Setting<int> ChatTextSize;

	public static readonly Setting<float> ChatBackgroundOpacity;

	public static readonly Setting<bool> IsTimestampEnabled;

	public static readonly Setting<bool> IsTargetHighlightOn;

	public static readonly Setting<bool> IsTargetArrowOn;

	public static readonly Setting<int> TrackedQuestID;

	public static readonly Setting<bool> CondenseEffects;

	public static readonly Setting<bool> ShowCurrentHp;

	public static readonly Setting<bool> ShowMaxHp;

	public static readonly Setting<bool> ShowCurrentRp;

	public static readonly Setting<bool> ShowPercentHp;

	public static readonly Setting<bool> ShowMaxRp;

	public static readonly Setting<bool> ShowPercentRp;

	public static readonly Setting<bool> ShowSpellErrors;

	public static readonly Setting<float> TurnSpeed;

	public static readonly Setting<bool> SingleSpellCastAutorun;

	public static readonly Setting<bool> AreaSpellCastAutorun;

	public static readonly Setting<bool> AoeFaceTarget;

	public static readonly Setting<int> TargetButtonType;

	public static readonly Setting<bool> IsAutoRunOn;

	public static readonly Setting<int> JoystickMode;

	public static readonly Setting<bool> CanAttackWithoutTarget;

	public static readonly Setting<bool> PrioritizePvpTargets;

	public static readonly Setting<bool> AutoSheatheWeapons;

	public static readonly Setting<float> MasterVolume;

	public static readonly Setting<float> MusicVolume;

	public static readonly Setting<float> CinematicVolume;

	public static readonly Setting<float> AmbientVolume;

	public static readonly Setting<float> SFXVolume;

	public static readonly Setting<bool> SoundOnlyWhenFocused;

	public static readonly Setting<bool> MasterEnabled;

	public static readonly Setting<bool> MusicEnabled;

	public static readonly Setting<bool> CinematicEnabled;

	public static readonly Setting<bool> AmbientEnabled;

	public static readonly Setting<bool> SFXEnabled;

	public static readonly Setting<bool> OtherPetsVisible;

	public static readonly Setting<bool> FPSMonitor;

	public static readonly Setting<bool> LatencyMonitor;

	public static readonly Setting<bool> DebugOutputLog;

	public static readonly Setting<string> DevBtnApopID;

	public static readonly Setting<bool> DevBtnAlwaysShow;

	private static readonly Setting<string> Macro1;

	private static readonly Setting<string> Macro2;

	private static readonly Setting<string> Macro3;

	private static readonly Setting<string> Macro4;

	private static readonly Setting<string> Macro5;

	private static readonly Setting<string> Macro6;

	private static readonly Setting<string> Macro7;

	private static readonly Setting<string> Macro8;

	private static readonly Setting<string> Macro9;

	private static readonly Setting<string> Macro0;

	private static readonly List<Setting<string>> AllMacros;

	public static readonly Setting<string> IsMailBlocked;

	public static readonly Setting<string> IsMailFriends;

	public static readonly Setting<string> IsMailAll;

	public static Dictionary<InputAction, Keybind> Default_Key_Maps => dictionary;

	public static int MacroCount => AllMacros.Count;

	public static bool IsFullScreen => GetFullScreenModeBySetting(DisplayMode) == FullScreenMode.ExclusiveFullScreen;

	public static Dictionary<InputAction, Keybind> Keybinds { get; private set; }

	public static List<Resolution> AvailableResolutions
	{
		get
		{
			List<Resolution> list = new List<Resolution>();
			Resolution resolution = default(Resolution);
			Resolution[] resolutions = Screen.resolutions;
			for (int i = 0; i < resolutions.Length; i++)
			{
				Resolution resolution2 = resolutions[i];
				if (resolution2.width >= 600 && (resolution.width != resolution2.width || resolution.height != resolution2.height))
				{
					list.Add(resolution2);
				}
				resolution = resolution2;
			}
			return list;
		}
	}

	public static Resolution CurrentResolution
	{
		get
		{
			if ((int)ResolutionIndex >= AvailableResolutions.Count)
			{
				ResolutionIndex.Set(AvailableResolutions.Count - 1, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
			}
			return AvailableResolutions[ResolutionIndex];
		}
	}

	public static List<int> AvailableFpsLimits
	{
		get
		{
			List<int> list = new List<int> { 30, 60 };
			if (Platform.IsMobile)
			{
				return list;
			}
			list = list.Union(from res in Screen.resolutions
				where res.width == CurrentResolution.width && res.height == CurrentResolution.height && res.refreshRate > 30
				select res.refreshRate).Distinct().ToList();
			list.Add(-1);
			return list;
		}
	}

	public static event Action<bool> BloomUpdated;

	public static event Action<bool> DepthOfFieldUpdated;

	public static event Action<UIJoystick.Mode> JoystickModeUpdated;

	public static event Action<bool> GrassUpdated;

	public static event Action<float> VignetteUpdated;

	public static event Action<bool> MasterEnabledUpdated;

	public static event Action<bool> MusicEnabledUpdated;

	public static event Action<bool> SfxEnabledUpdated;

	public static event Action<bool> CinematicEnabledUpdated;

	public static event Action<bool> AmbientEnabledUpdated;

	public static event Action<bool> OtherPetsVisibleUpdated;

	public static event Action<bool> TimestampEnabledUpdated;

	public static event Action<bool> ToggleFPSMonitorUpdated;

	public static event Action<bool> ToggleLatencyMonitorUpdated;

	public static event Action<bool> ToggleDebugOutputLogUpdated;

	public static event Action<float> MenuSizeUpdated;

	public static event Action<int> ChatWindowUpdated;

	public static event Action<int> DrawDistanceUpdated;

	public static event Action<int> ChatTextSizeUpdated;

	public static event Action<float> ChatBackgroundOpacityUpdated;

	public static event Action<float> MasterVolumeUpdated;

	public static event Action<float> MusicVolumeUpdated;

	public static event Action<float> CinematicVolumeUpdated;

	public static event Action<float> AmbientVolumeUpdated;

	public static event Action<float> SfxVolumeUpdated;

	public static event Action NamePlateSettingUpdate;

	public static event Action KeyMappingUpdated;

	public static event Action CameraSettingUpdated;

	public static event Action IgnoreSettingUpdated;

	public static event Action HpRpBarUpdated;

	public static event Action<bool> DevBtnAlwaysShowUpdated;

	public static event Action<bool> AutoSheathingUpdated;

	public static KeyCode GetKeyCodeByAction(InputAction action)
	{
		return Keybinds[action].key;
	}

	public static string GetHotkeyByAction(InputAction action)
	{
		return ArtixUnity.KeyCodeName(GetKeyCodeByAction(action));
	}

	static SettingsManager()
	{
		dictionary = new Dictionary<InputAction, Keybind>
		{
			{
				InputAction.Forward,
				new Keybind
				{
					name = "Forward",
					key = KeyCode.W,
					action = InputAction.Forward
				}
			},
			{
				InputAction.Backward,
				new Keybind
				{
					name = "Backward",
					key = KeyCode.S,
					action = InputAction.Backward
				}
			},
			{
				InputAction.LeftRotate,
				new Keybind
				{
					name = "Left Rotate",
					key = KeyCode.A,
					action = InputAction.LeftRotate
				}
			},
			{
				InputAction.RightRotate,
				new Keybind
				{
					name = "Right Rotate",
					key = KeyCode.D,
					action = InputAction.RightRotate
				}
			},
			{
				InputAction.LeftStrafe,
				new Keybind
				{
					name = "Left Strafe",
					key = KeyCode.Q,
					action = InputAction.LeftStrafe
				}
			},
			{
				InputAction.RightStrafe,
				new Keybind
				{
					name = "Right Strafe",
					key = KeyCode.E,
					action = InputAction.RightStrafe
				}
			},
			{
				InputAction.Jump,
				new Keybind
				{
					name = "Jump",
					key = KeyCode.Space,
					action = InputAction.Jump
				}
			},
			{
				InputAction.Autorun,
				new Keybind
				{
					name = "Autorun",
					key = KeyCode.Numlock,
					action = InputAction.Autorun
				}
			},
			{
				InputAction.ZoomIn,
				new Keybind
				{
					name = "Camera Zoom In",
					key = KeyCode.PageUp,
					action = InputAction.ZoomIn
				}
			},
			{
				InputAction.ZoomOut,
				new Keybind
				{
					name = "Camera Zoom Out",
					key = KeyCode.PageDown,
					action = InputAction.ZoomOut
				}
			},
			{
				InputAction.Target_Next,
				new Keybind
				{
					name = "Target Next Enemy",
					key = KeyCode.Tab,
					action = InputAction.Target_Next
				}
			},
			{
				InputAction.TargetClosest,
				new Keybind
				{
					name = "Target Closest Enemy",
					key = KeyCode.BackQuote,
					action = InputAction.TargetClosest
				}
			},
			{
				InputAction.Spell_1,
				new Keybind
				{
					name = "Skill 1",
					key = KeyCode.Alpha1,
					action = InputAction.Spell_1
				}
			},
			{
				InputAction.Spell_2,
				new Keybind
				{
					name = "Skill 2",
					key = KeyCode.Alpha2,
					action = InputAction.Spell_2
				}
			},
			{
				InputAction.Spell_3,
				new Keybind
				{
					name = "Skill 3",
					key = KeyCode.Alpha3,
					action = InputAction.Spell_3
				}
			},
			{
				InputAction.Spell_4,
				new Keybind
				{
					name = "Skill 4",
					key = KeyCode.Alpha4,
					action = InputAction.Spell_4
				}
			},
			{
				InputAction.Spell_5,
				new Keybind
				{
					name = "Skill 5",
					key = KeyCode.Alpha5,
					action = InputAction.Spell_5
				}
			},
			{
				InputAction.Cross_Skill,
				new Keybind
				{
					name = "Cross Skill",
					key = KeyCode.R,
					action = InputAction.Cross_Skill
				}
			},
			{
				InputAction.CustomAction_1,
				new Keybind
				{
					name = "Item Slot 1",
					key = KeyCode.Z,
					action = InputAction.CustomAction_1
				}
			},
			{
				InputAction.CustomAction_2,
				new Keybind
				{
					name = "Item Slot 2",
					key = KeyCode.X,
					action = InputAction.CustomAction_2
				}
			},
			{
				InputAction.CustomAction_3,
				new Keybind
				{
					name = "Item Slot 3",
					key = KeyCode.C,
					action = InputAction.CustomAction_3
				}
			},
			{
				InputAction.CustomAction_4,
				new Keybind
				{
					name = "Item Slot 4",
					key = KeyCode.V,
					action = InputAction.CustomAction_4
				}
			},
			{
				InputAction.TargetParty_1,
				new Keybind
				{
					name = "Target Party Member 1",
					key = KeyCode.F1,
					action = InputAction.TargetParty_1
				}
			},
			{
				InputAction.TargetParty_2,
				new Keybind
				{
					name = "Target Party Member 2",
					key = KeyCode.F2,
					action = InputAction.TargetParty_2
				}
			},
			{
				InputAction.TargetParty_3,
				new Keybind
				{
					name = "Target Party Member 3",
					key = KeyCode.F3,
					action = InputAction.TargetParty_3
				}
			},
			{
				InputAction.TargetParty_4,
				new Keybind
				{
					name = "Target Party Member 4",
					key = KeyCode.F4,
					action = InputAction.TargetParty_4
				}
			},
			{
				InputAction.OpenLoot,
				new Keybind
				{
					name = "Open/Close Loot",
					key = KeyCode.F,
					action = InputAction.OpenLoot
				}
			},
			{
				InputAction.LootAll,
				new Keybind
				{
					name = "Loot All",
					key = KeyCode.None,
					action = InputAction.LootAll
				}
			},
			{
				InputAction.UI_Toggle_Sheathe,
				new Keybind
				{
					name = "Sheathe/Unsheathe Weapons",
					key = KeyCode.None,
					action = InputAction.UI_Toggle_Sheathe
				}
			},
			{
				InputAction.UI_Toggle_Inventory,
				new Keybind
				{
					name = "Inventory",
					key = KeyCode.I,
					action = InputAction.UI_Toggle_Inventory
				}
			},
			{
				InputAction.UI_Toggle_Quests,
				new Keybind
				{
					name = "Quest Log",
					key = KeyCode.U,
					action = InputAction.UI_Toggle_Quests
				}
			},
			{
				InputAction.UI_Toggle_Travel,
				new Keybind
				{
					name = "Travel",
					key = KeyCode.M,
					action = InputAction.UI_Toggle_Travel
				}
			},
			{
				InputAction.UI_Toggle_Character,
				new Keybind
				{
					name = "Character",
					key = KeyCode.H,
					action = InputAction.UI_Toggle_Character
				}
			},
			{
				InputAction.UI_Toggle_Crafting,
				new Keybind
				{
					name = "Crafting",
					key = KeyCode.T,
					action = InputAction.UI_Toggle_Crafting
				}
			},
			{
				InputAction.UI_Toggle_Social,
				new Keybind
				{
					name = "Social",
					key = KeyCode.O,
					action = InputAction.UI_Toggle_Social
				}
			},
			{
				InputAction.UI_Toggle_Guild,
				new Keybind
				{
					name = "Guild",
					key = KeyCode.G,
					action = InputAction.UI_Toggle_Guild
				}
			},
			{
				InputAction.UI_Toggle_Potions,
				new Keybind
				{
					name = "Potions",
					key = KeyCode.P,
					action = InputAction.UI_Toggle_Potions
				}
			},
			{
				InputAction.UI_Toggle_Classes,
				new Keybind
				{
					name = "Classes",
					key = KeyCode.L,
					action = InputAction.UI_Toggle_Classes
				}
			},
			{
				InputAction.UI_Toggle_DailyTasks,
				new Keybind
				{
					name = "Daily Tasks",
					key = KeyCode.K,
					action = InputAction.UI_Toggle_DailyTasks
				}
			},
			{
				InputAction.UI_Toggle_Pvp,
				new Keybind
				{
					name = "PvP Menu",
					key = KeyCode.None,
					action = InputAction.UI_Toggle_Pvp
				}
			},
			{
				InputAction.UI_Toggle,
				new Keybind
				{
					name = "Toggle UI",
					key = KeyCode.None,
					action = InputAction.UI_Toggle
				}
			}
		};
		ignoreList = new List<string>();
		actionSlotIDs = new int[10];
		IsCameraZoomEnabled = new Setting<bool>("CameraZoomOn", defaultValue: true);
		CameraSpeed = new Setting<float>("CameraSpeed", 5f);
		ZoomSpeed = new Setting<float>("ZoomSpeed", 2f);
		CameraZoomDistance = new Setting<float>("cameraZoomDistance", 6f);
		InvertX = new Setting<bool>("InvertX", defaultValue: false);
		InvertY = new Setting<bool>("InvertY", defaultValue: false);
		FreeCamera = new Setting<bool>("FreeCamera", defaultValue: false);
		FieldOfView = new Setting<float>("FOV", 42f);
		CameraShake = new Setting<float>("CameraShake", 1f);
		MapCameraReset = new Setting<bool>("MapCameraReset", defaultValue: true);
		TargetCameraLock = new Setting<bool>("TargetCameraLock", defaultValue: true);
		ResolutionIndex = new Setting<int>("ResolutionIndex", int.MaxValue);
		DisplayMode = new Setting<int>("FullscreenType", 0);
		GraphicsQuality = new Setting<int>("QualityLevel", 4);
		TextureQuality = new Setting<int>("TextureQuality", 2, TextureQualityAction);
		AnisotropicFilter = new Setting<int>("Anisotropic", 2, AnisotropicAction);
		AntiAliasing = new Setting<int>("AntiAliasing", 2, AntiAliasingAction);
		ShadowQuality = new Setting<int>("ShadowQuality", 3, ShadowQualityAction);
		ShadowDistance = new Setting<int>("ShadowDistance", 2, ShadowDistanceAction);
		ParticleRaycasts = new Setting<int>("ParticleQuality", 4, ParticleRaycastsAction);
		ParticleQuality = new Setting<int>("HDParticles", 1);
		DrawDistance = new Setting<int>("DrawDistance", 3, DrawDistanceAction);
		UseBloom = new Setting<bool>("UseBloom", defaultValue: false, BloomAction);
		UseDepthOfField = new Setting<bool>("UseDepth", defaultValue: false, DepthOfFieldAction);
		GrassDistance = new Setting<float>("GrassDistance", 200f, GrassDistanceAction);
		VignetteOpacity = new Setting<float>("VignetteOpacity", 0.5f, VignetteOpacityAction);
		ShowMobileGrass = new Setting<bool>("isGrassEnabled", defaultValue: true, MobileGrassEnabledAction);
		VerticalSync = new Setting<int>("VsyncChoice", 1, VerticalSyncAction);
		FpsLimit = new Setting<int>("FpsLimit", 1, FpsLimitAction);
		ParticleLimit = new Setting<int>("ParticleLimit", 4);
		InterfaceMode = new Setting<int>("InterfaceMode", 0);
		CanGetWhispers = new Setting<bool>("isWhisperEnabled", defaultValue: true, WhisperSettingAction);
		CanGetFriendRequests = new Setting<bool>("isFriendRequestEnabled", defaultValue: true, FriendRequestAction);
		CanGetPartyInvites = new Setting<bool>("isPartyInviteEnabled", defaultValue: true, PartyInvitesAction);
		CanGetGuildInvites = new Setting<bool>("isGuildInviteEnabled", defaultValue: true, GuildInvitesActions);
		CanGetPvPDuelRequests = new Setting<bool>("isPvPDuelsEnabled", defaultValue: true, PvPDuelsAction);
		IsChatFiltered = new Setting<bool>("ChatFilter", defaultValue: true);
		IsChatBubbleOn = new Setting<bool>("ChatBubble", defaultValue: true);
		IsGotoOn = new Setting<bool>("Goto", defaultValue: true, AllowGotoUpdated);
		IsMyNameOn = new Setting<bool>("isMyNameOn", defaultValue: true, NamePlateSettingUpdated);
		IsMyTitleOn = new Setting<bool>("isMyTitleOn", defaultValue: true, NamePlateSettingUpdated);
		IsMyHealthBarOn = new Setting<bool>("isMyHealthOn", defaultValue: true, NamePlateSettingUpdated);
		IsMyGuildTagOn = new Setting<bool>("isMyGuildTagOn", defaultValue: true, NamePlateSettingUpdated);
		IsPlayerNameOn = new Setting<bool>("isPlayerNameOn", defaultValue: true, NamePlateSettingUpdated);
		IsPlayerTitleOn = new Setting<bool>("isPlayerTitleOn", defaultValue: true, NamePlateSettingUpdated);
		IsPlayerHealthBarOn = new Setting<bool>("isPlayerHealthOn", defaultValue: true, NamePlateSettingUpdated);
		IsPlayerGuildTagOn = new Setting<bool>("isPlayerGuildTagOn", defaultValue: true, NamePlateSettingUpdated);
		IsNPCNameOn = new Setting<bool>("isNpcNameOn", defaultValue: true, NamePlateSettingUpdated);
		IsNPCTitleOn = new Setting<bool>("isNpcTitleOn", defaultValue: true, NamePlateSettingUpdated);
		IsNpcHealthBarOn = new Setting<bool>("isNpcHealthOn", defaultValue: true, NamePlateSettingUpdated);
		ChatWindowSize = new Setting<int>("chatWindowSize", 1, ChatWindowAction);
		MenuSize = new Setting<float>("WindowSize", 0.8f, MenuSizeAction);
		ChatTextSize = new Setting<int>("ChatTextSize", 15, ChatTextAction);
		ChatBackgroundOpacity = new Setting<float>("ChatBackgroundOpacity", 0.5f, ChatBackgroundOpacityAction);
		IsTimestampEnabled = new Setting<bool>("TimestampEnabled", defaultValue: false, TimestampEnabledAction);
		IsTargetHighlightOn = new Setting<bool>("isTargetHighlightingEnabled", defaultValue: true);
		IsTargetArrowOn = new Setting<bool>("isTargetArrowEnabled", defaultValue: true);
		TrackedQuestID = new Setting<int>("TrackedQuestID", 0);
		CondenseEffects = new Setting<bool>("ShowAllEffects", defaultValue: true);
		ShowCurrentHp = new Setting<bool>("ShowCurrentHp", defaultValue: true, HpRpBarAction);
		ShowMaxHp = new Setting<bool>("ShowMaxHp", defaultValue: false, HpRpBarAction);
		ShowCurrentRp = new Setting<bool>("ShowCurrentRp", defaultValue: true, HpRpBarAction);
		ShowPercentHp = new Setting<bool>("ShowPercentHp", defaultValue: false, HpRpBarAction);
		ShowMaxRp = new Setting<bool>("ShowMaxRp", defaultValue: false, HpRpBarAction);
		ShowPercentRp = new Setting<bool>("ShowPercentRp", defaultValue: false, HpRpBarAction);
		ShowSpellErrors = new Setting<bool>("ShowSpellErrors", defaultValue: true);
		TurnSpeed = new Setting<float>("RotateSpeed", 1f);
		SingleSpellCastAutorun = new Setting<bool>("SingleSpellCastAutorun", defaultValue: true);
		AreaSpellCastAutorun = new Setting<bool>("SpellCastAutorun", defaultValue: true);
		AoeFaceTarget = new Setting<bool>("AoeFaceTarget", defaultValue: true);
		TargetButtonType = new Setting<int>("TargetButtonType", 0);
		IsAutoRunOn = new Setting<bool>("jsAutorun", defaultValue: true);
		JoystickMode = new Setting<int>("JoystickMode", 2, JoystickModeAction);
		CanAttackWithoutTarget = new Setting<bool>("AtkNoTgt", defaultValue: true);
		PrioritizePvpTargets = new Setting<bool>("PrioritizePvpTargets", defaultValue: true);
		AutoSheatheWeapons = new Setting<bool>("AutoSheatheWeapons", defaultValue: false, AutoSheathingAction);
		MasterVolume = new Setting<float>("MasterVolume", 1f, MasterVolumeAction);
		MusicVolume = new Setting<float>("MusicVolume", 0.75f, MusicVolumeAction);
		CinematicVolume = new Setting<float>("CinematicVolume", 1f, CinematicVolumeAction);
		AmbientVolume = new Setting<float>("AmbientVolume", 1f, AmbientVolumeAction);
		SFXVolume = new Setting<float>("SfxVolume", 1f, SfxVolumeAction);
		SoundOnlyWhenFocused = new Setting<bool>("SoundOnlyFocus", defaultValue: false);
		MasterEnabled = new Setting<bool>("MasterEnabled", defaultValue: true, MasterEnabledAction);
		MusicEnabled = new Setting<bool>("MusicEnabled", defaultValue: true, MusicEnabledAction);
		CinematicEnabled = new Setting<bool>("CinematicEnabled", defaultValue: true, CinematicEnabledAction);
		AmbientEnabled = new Setting<bool>("AmbientEnabled", defaultValue: true, AmbientEnabledAction);
		SFXEnabled = new Setting<bool>("SfxEnabled", defaultValue: true, SfxEnabledAction);
		OtherPetsVisible = new Setting<bool>("OtherPets", defaultValue: true, OtherPetsAction);
		FPSMonitor = new Setting<bool>("FPSMonitor", defaultValue: false, ToggleFPSMonitor);
		LatencyMonitor = new Setting<bool>("LatencyMonitor", defaultValue: false, ToggleLatencyMonitor);
		DebugOutputLog = new Setting<bool>("DebugOutputLog", defaultValue: false, ToggleDebugOutputLog);
		DevBtnApopID = new Setting<string>("DvBtnApop", "7377");
		DevBtnAlwaysShow = new Setting<bool>("DvBtnAlwaysShow", defaultValue: false, DevBtnAlwaysShowAction);
		Macro1 = new Setting<string>("Macro1", "");
		Macro2 = new Setting<string>("Macro2", "");
		Macro3 = new Setting<string>("Macro3", "");
		Macro4 = new Setting<string>("Macro4", "");
		Macro5 = new Setting<string>("Macro5", "");
		Macro6 = new Setting<string>("Macro6", "");
		Macro7 = new Setting<string>("Macro7", "");
		Macro8 = new Setting<string>("Macro8", "");
		Macro9 = new Setting<string>("Macro9", "");
		Macro0 = new Setting<string>("Macro0", "");
		AllMacros = new List<Setting<string>> { Macro0, Macro1, Macro2, Macro3, Macro4, Macro5, Macro6, Macro7, Macro8, Macro9 };
		IsMailBlocked = new Setting<string>("MailNotBlocked", "");
		IsMailFriends = new Setting<string>("MailIsFriendsOnly", "2/19/2024 2:27:31 PM");
		IsMailAll = new Setting<string>("MailNotAll", "");
		Init();
	}

	private static void CheckKeyBindings()
	{
		foreach (InputAction key in Keybinds.Keys)
		{
			if (PlayerPrefs.HasKey("Key" + key))
			{
				Keybinds[key].key = (KeyCode)PlayerPrefs.GetInt("Key" + key);
			}
		}
		foreach (Keybind keybind in Keybinds.Values)
		{
			if (keybind.key != 0 && Keybinds.Values.Count((Keybind k) => k.key == keybind.key) > 1)
			{
				Keybind keybind2 = Default_Key_Maps.Values.FirstOrDefault((Keybind k) => k.key == keybind.key);
				if (keybind == keybind2)
				{
					Keybinds[keybind.action].key = KeyCode.None;
				}
			}
		}
	}

	public static void SetActionSlotID(CombatSpellSlot slot, int ID)
	{
		if (GetActionSlotID(slot) != ID)
		{
			int num = (int)slot;
			PlayerPrefs.SetInt("ActionBarSlot" + num, ID);
			PlayerPrefs.Save();
		}
	}

	public static int GetActionSlotID(CombatSpellSlot slot)
	{
		int num = (int)slot;
		if (PlayerPrefs.HasKey("ActionBarSlot" + num))
		{
			num = (int)slot;
			return PlayerPrefs.GetInt("ActionBarSlot" + num);
		}
		return 0;
	}

	public static void Init()
	{
		for (int i = 0; i < 4; i++)
		{
			if (PlayerPrefs.HasKey("ActionBarSlot" + i))
			{
				actionSlotIDs[i] = PlayerPrefs.GetInt("ActionBarSlot" + i);
			}
			else
			{
				actionSlotIDs[i] = 0;
			}
		}
		if (!isInitialized)
		{
			Setting<float>.LoadAllValuesFromPlayerPrefs();
			Setting<int>.LoadAllValuesFromPlayerPrefs();
			Setting<string>.LoadAllValuesFromPlayerPrefs();
			Setting<bool>.LoadAllValuesFromPlayerPrefs();
			Keybinds = Default_Key_Maps.ToDictionary((KeyValuePair<InputAction, Keybind> entry) => entry.Key, (KeyValuePair<InputAction, Keybind> entry) => new Keybind(entry.Value));
			if (Platform.IsEditor)
			{
				CheckKeyBindings();
			}
			else if (Platform.IsDesktop)
			{
				CheckKeyBindings();
				ApplyResolutionSettings();
			}
			else if (Platform.IsMobile)
			{
				FpsLimitAction(FpsLimit);
			}
			QualitySettings.realtimeReflectionProbes = false;
			QualitySettings.resolutionScalingFixedDPIFactor = 1f;
			QualitySettings.asyncUploadTimeSlice = 2;
			QualitySettings.asyncUploadBufferSize = 4;
			if (PlayerPrefs.HasKey("Ignore") && PlayerPrefs.GetString("Ignore") != "")
			{
				ignoreList = PlayerPrefs.GetString("Ignore").Split(',').ToList();
				ShrinkIgnoreList();
			}
			isInitialized = true;
		}
	}

	public static string ResolutionName(Resolution resolution)
	{
		return resolution.width + " x " + resolution.height;
	}

	private static FullScreenMode GetFullScreenModeBySetting(int fullscreenSetting)
	{
		return fullscreenSetting switch
		{
			1 => FullScreenMode.Windowed, 
			2 => FullScreenMode.FullScreenWindow, 
			_ => FullScreenMode.ExclusiveFullScreen, 
		};
	}

	private static void AutoSheathingAction(bool autoSheatheEnabled)
	{
		SettingsManager.AutoSheathingUpdated?.Invoke(autoSheatheEnabled);
	}

	private static void DevBtnAlwaysShowAction(bool alwaysShow)
	{
		SettingsManager.DevBtnAlwaysShowUpdated?.Invoke(alwaysShow);
	}

	private static void OtherPetsAction(bool areVisible)
	{
		SettingsManager.OtherPetsVisibleUpdated?.Invoke(areVisible);
	}

	private static void JoystickModeAction(int mode)
	{
		SettingsManager.JoystickModeUpdated?.Invoke((UIJoystick.Mode)mode);
	}

	private static void GrassDistanceAction(float distance)
	{
		Shader.SetGlobalFloat("_MaxCameraDistance", distance);
		Shader.SetGlobalFloat("_Transition", distance * 0.3f);
		SettingsManager.GrassUpdated?.Invoke(distance > 0f);
	}

	private static void VignetteOpacityAction(float opacity)
	{
		SettingsManager.VignetteUpdated?.Invoke(opacity);
	}

	private static void MobileGrassEnabledAction(bool isEnabled)
	{
	}

	private static void VerticalSyncAction(int choice)
	{
		QualitySettings.vSyncCount = choice;
	}

	private static void FpsLimitAction(int choice)
	{
		choice = choice.Clamp(0, AvailableFpsLimits.Count - 1);
		Application.targetFrameRate = AvailableFpsLimits[choice];
	}

	private static void TextureQualityAction(int quality)
	{
		switch (quality)
		{
		case 0:
			QualitySettings.masterTextureLimit = 2;
			QualitySettings.skinWeights = SkinWeights.TwoBones;
			break;
		case 1:
			QualitySettings.masterTextureLimit = 1;
			QualitySettings.skinWeights = SkinWeights.TwoBones;
			break;
		case 2:
			QualitySettings.masterTextureLimit = 0;
			QualitySettings.skinWeights = SkinWeights.FourBones;
			break;
		}
	}

	private static void AnisotropicAction(int filterLevel)
	{
		switch (filterLevel)
		{
		case 0:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
			break;
		case 1:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
			break;
		case 2:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			break;
		}
	}

	private static void AntiAliasingAction(int aliasLevel)
	{
		switch (aliasLevel)
		{
		case 0:
			QualitySettings.antiAliasing = 0;
			break;
		case 1:
			QualitySettings.antiAliasing = 2;
			break;
		case 2:
			QualitySettings.antiAliasing = 4;
			break;
		case 3:
			QualitySettings.antiAliasing = 8;
			break;
		}
	}

	private static void ShadowQualityAction(int quality)
	{
		QualitySettings.shadowProjection = ShadowProjection.StableFit;
		switch (quality)
		{
		case 0:
			QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
			QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
			QualitySettings.shadowResolution = ShadowResolution.Low;
			QualitySettings.shadowCascades = 0;
			break;
		case 1:
			QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
			QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
			QualitySettings.shadowResolution = ShadowResolution.Low;
			QualitySettings.shadowCascades = 2;
			break;
		case 2:
			QualitySettings.shadows = UnityEngine.ShadowQuality.All;
			QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			QualitySettings.shadowCascades = 2;
			break;
		case 3:
			QualitySettings.shadows = UnityEngine.ShadowQuality.All;
			QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
			QualitySettings.shadowResolution = ShadowResolution.High;
			QualitySettings.shadowCascades = 2;
			break;
		case 4:
			QualitySettings.shadows = UnityEngine.ShadowQuality.All;
			QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
			QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
			QualitySettings.shadowCascades = 4;
			break;
		}
		if (quality <= 0 || Platform.IsMobile)
		{
			Shader.DisableKeyword("HIGHQUALITYWATER");
		}
		else
		{
			Shader.EnableKeyword("HIGHQUALITYWATER");
		}
	}

	private static void ShadowDistanceAction(int distanceChoice)
	{
		QualitySettings.shadowNearPlaneOffset = 2f;
		switch (distanceChoice)
		{
		case 0:
			QualitySettings.shadowDistance = 20f;
			break;
		case 1:
			QualitySettings.shadowDistance = 40f;
			break;
		case 2:
			QualitySettings.shadowDistance = 80f;
			break;
		case 3:
			QualitySettings.shadowDistance = 160f;
			break;
		}
	}

	private static void ParticleRaycastsAction(int quality)
	{
		switch (quality)
		{
		case 0:
			QualitySettings.particleRaycastBudget = 4;
			break;
		case 1:
			QualitySettings.particleRaycastBudget = 16;
			break;
		case 2:
			QualitySettings.particleRaycastBudget = 64;
			break;
		case 3:
			QualitySettings.particleRaycastBudget = 256;
			break;
		case 4:
			QualitySettings.particleRaycastBudget = 1024;
			break;
		case 5:
			QualitySettings.particleRaycastBudget = 4096;
			break;
		}
	}

	private static void DrawDistanceAction(int distanceChoice)
	{
		SettingsManager.DrawDistanceUpdated?.Invoke(distanceChoice);
	}

	private static void BloomAction(bool isEnabled)
	{
		SettingsManager.BloomUpdated?.Invoke(isEnabled);
	}

	private static void DepthOfFieldAction(bool isEnabled)
	{
		SettingsManager.DepthOfFieldUpdated?.Invoke(isEnabled);
	}

	private static void NamePlateSettingUpdated(bool _)
	{
		SettingsManager.NamePlateSettingUpdate?.Invoke();
	}

	private static void ChatWindowAction(int size)
	{
		SettingsManager.ChatWindowUpdated?.Invoke(size);
	}

	private static void MenuSizeAction(float size)
	{
		SettingsManager.MenuSizeUpdated?.Invoke(size);
	}

	private static void ChatTextAction(int newSize)
	{
		SettingsManager.ChatTextSizeUpdated?.Invoke(newSize);
	}

	private static void ChatBackgroundOpacityAction(float newOpacity)
	{
		SettingsManager.ChatBackgroundOpacityUpdated?.Invoke(newOpacity);
	}

	private static void TimestampEnabledAction(bool isEnabled)
	{
		SettingsManager.TimestampEnabledUpdated?.Invoke(isEnabled);
	}

	private static void HpRpBarAction(bool unusedValue)
	{
		SettingsManager.HpRpBarUpdated?.Invoke();
	}

	private static void ToggleFPSMonitor(bool enabledValue)
	{
		SettingsManager.ToggleFPSMonitorUpdated?.Invoke(enabledValue);
	}

	private static void ToggleLatencyMonitor(bool enabledValue)
	{
		SettingsManager.ToggleLatencyMonitorUpdated?.Invoke(enabledValue);
	}

	private static void ToggleDebugOutputLog(bool enabled)
	{
		SettingsManager.ToggleDebugOutputLogUpdated?.Invoke(enabled);
	}

	private static void WhisperSettingAction(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void FriendRequestAction(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void PartyInvitesAction(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void GuildInvitesActions(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void AllowGotoUpdated(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void PvPDuelsAction(bool isEnabled)
	{
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	private static void MasterVolumeAction(float volume)
	{
		SettingsManager.MasterVolumeUpdated?.Invoke(volume);
	}

	private static void MusicVolumeAction(float volume)
	{
		SettingsManager.MusicVolumeUpdated?.Invoke(volume);
	}

	private static void CinematicVolumeAction(float volume)
	{
		SettingsManager.CinematicVolumeUpdated?.Invoke(volume);
	}

	private static void AmbientVolumeAction(float volume)
	{
		SettingsManager.AmbientVolumeUpdated?.Invoke(volume);
	}

	private static void SfxVolumeAction(float volume)
	{
		SettingsManager.SfxVolumeUpdated?.Invoke(volume);
	}

	private static void MasterEnabledAction(bool isEnabled)
	{
		SettingsManager.MasterEnabledUpdated?.Invoke(isEnabled);
	}

	private static void MusicEnabledAction(bool isEnabled)
	{
		SettingsManager.MusicEnabledUpdated?.Invoke(isEnabled);
	}

	private static void CinematicEnabledAction(bool isEnabled)
	{
		SettingsManager.CinematicEnabledUpdated?.Invoke(isEnabled);
	}

	private static void AmbientEnabledAction(bool isEnabled)
	{
		SettingsManager.AmbientEnabledUpdated?.Invoke(isEnabled);
	}

	private static void SfxEnabledAction(bool isEnabled)
	{
		SettingsManager.SfxEnabledUpdated?.Invoke(isEnabled);
	}

	public static void SaveAndApplyResolutionSettings(int resolutionIndex, int fullscreenChoice)
	{
		if (Platform.IsDesktop && ((int)ResolutionIndex != resolutionIndex || (int)DisplayMode != fullscreenChoice))
		{
			ResolutionIndex.Set(resolutionIndex, updatePref: true, savePrefsToDisk: false);
			DisplayMode.Set(fullscreenChoice);
			ApplyResolutionSettings();
		}
	}

	private static void ApplyResolutionSettings()
	{
		FullScreenMode fullScreenModeBySetting = GetFullScreenModeBySetting(DisplayMode);
		Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, fullScreenModeBySetting);
		Cursor.lockState = (IsFullScreen ? CursorLockMode.Confined : CursorLockMode.None);
	}

	public static void SaveKeySettings(List<Keybind> keys)
	{
		if (!Platform.IsDesktop)
		{
			return;
		}
		try
		{
			bool flag = false;
			foreach (Keybind key in keys)
			{
				if (Keybinds.ContainsKey(key.action) && Keybinds[key.action].key != key.key)
				{
					Keybinds[key.action].key = key.key;
					PlayerPrefs.SetInt("Key" + key.action, (int)key.key);
					flag = true;
				}
			}
			if (flag)
			{
				PlayerPrefs.Save();
				SettingsManager.KeyMappingUpdated?.Invoke();
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError("Hotkey Saving Error", "System was unable to save hotkeys to disk.", ex.Message);
		}
	}

	public static void SaveGraphicsSettings(int graphicsQualityIndex, int textureQualityIndex, int anisotropicIndex, int antiAliasingIndex, int shadowQualityIndex, int shadowDistanceIndex, int particleQualityIndex, int hdParticlesIndex, int drawDistanceIndex, int particleLimitIndex, float grassDistance, float vignetteopacity, bool useBloom, bool useDepthOfField, int vsyncIndex, int fpsLimitIndex, int interfaceModeIndex)
	{
		if (Platform.IsMobile)
		{
			useDepthOfField = false;
			grassDistance = 0f;
		}
		if (SpellFXContainer.mInstance != null && hdParticlesIndex != (int)ParticleQuality)
		{
			SpellFXContainer.mInstance.LoadStandardImpact();
		}
		GraphicsQuality.Set(graphicsQualityIndex, updatePref: true, savePrefsToDisk: false);
		TextureQuality.Set(textureQualityIndex, updatePref: true, savePrefsToDisk: false);
		AnisotropicFilter.Set(anisotropicIndex, updatePref: true, savePrefsToDisk: false);
		AntiAliasing.Set(antiAliasingIndex, updatePref: true, savePrefsToDisk: false);
		ShadowQuality.Set(shadowQualityIndex, updatePref: true, savePrefsToDisk: false);
		ShadowDistance.Set(shadowDistanceIndex, updatePref: true, savePrefsToDisk: false);
		ParticleRaycasts.Set(particleQualityIndex, updatePref: true, savePrefsToDisk: false);
		ParticleQuality.Set(hdParticlesIndex, updatePref: true, savePrefsToDisk: false);
		DrawDistance.Set(drawDistanceIndex, updatePref: true, savePrefsToDisk: false);
		ParticleLimit.Set(particleLimitIndex, updatePref: true, savePrefsToDisk: false);
		GrassDistance.Set(grassDistance, updatePref: true, savePrefsToDisk: false);
		VignetteOpacity.Set(vignetteopacity, updatePref: true, savePrefsToDisk: false);
		VerticalSync.Set(vsyncIndex, updatePref: true, savePrefsToDisk: false);
		FpsLimit.Set(fpsLimitIndex, updatePref: true, savePrefsToDisk: false);
		InterfaceMode.Set(interfaceModeIndex, updatePref: true, savePrefsToDisk: false);
		UseBloom.Set(useBloom, updatePref: true, savePrefsToDisk: false);
		UseDepthOfField.Set(useDepthOfField, updatePref: true, savePrefsToDisk: false);
		PlayerPrefs.Save();
	}

	public static void SetCameraSettings(float speed, float zoom, float newFOV, float cameraShake, bool invertedX, bool invertedY, bool freeCamera, bool targetLock)
	{
		CameraSpeed.Set(speed, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		ZoomSpeed.Set(zoom, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		FieldOfView.Set(newFOV, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		CameraShake.Set(cameraShake, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		InvertX.Set(invertedX, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		InvertY.Set(invertedY, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		FreeCamera.Set(freeCamera, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		TargetCameraLock.Set(targetLock, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
		SettingsManager.CameraSettingUpdated?.Invoke();
	}

	public static void SetAndSaveCameraSettings(float speed, float zoom, float newFOV, float cameraShake, bool invertedX, bool invertedY, bool freeCamera, bool targetLock)
	{
		SetCameraSettings(speed, zoom, newFOV, cameraShake, invertedX, invertedY, freeCamera, targetLock);
		CameraSpeed.SaveToPrefs(savePrefsToDisk: false);
		ZoomSpeed.SaveToPrefs(savePrefsToDisk: false);
		FieldOfView.SaveToPrefs(savePrefsToDisk: false);
		CameraShake.SaveToPrefs(savePrefsToDisk: false);
		InvertX.SaveToPrefs(savePrefsToDisk: false);
		InvertY.SaveToPrefs(savePrefsToDisk: false);
		FreeCamera.SaveToPrefs(savePrefsToDisk: false);
		TargetCameraLock.SaveToPrefs(savePrefsToDisk: false);
		PlayerPrefs.Save();
	}

	public static void Ignore(string name)
	{
		name = name.ToLower();
		if (ignoreList.IndexOf(name) == -1)
		{
			ignoreList.Add(name);
			ShrinkIgnoreList();
		}
	}

	private static void ShrinkIgnoreList()
	{
		if (ignoreList.Count > 30)
		{
			while (ignoreList.Count > 30)
			{
				ignoreList.RemoveAt(0);
			}
		}
		SaveIgnorePrefs();
	}

	public static void UnIgnore(string playerName)
	{
		string item = playerName.ToLower();
		if (ignoreList.IndexOf(item) > -1)
		{
			Chat.AddMessage(InterfaceColors.Chat.Light_Blue.ToBBCode() + "'" + playerName + "'[-]" + InterfaceColors.Chat.White.ToBBCode() + " has been removed from the ignore list.[-]");
			ignoreList.Remove(item);
			SaveIgnorePrefs();
		}
	}

	private static void SaveIgnorePrefs()
	{
		string value = string.Join(",", Enumerable.ToArray(ignoreList));
		PlayerPrefs.SetString("Ignore", value);
		PlayerPrefs.Save();
		SettingsManager.IgnoreSettingUpdated?.Invoke();
	}

	public static bool IsIgnoring(string name)
	{
		if (!string.IsNullOrEmpty(name) && ignoreList != null)
		{
			return ignoreList.Contains(name.ToLower());
		}
		return false;
	}

	private static Setting<string> GetMacroSettingBySlot(int macroSlot)
	{
		if (!macroSlot.Between(0, AllMacros.Count - 1))
		{
			Debug.LogWarning("Trying to access invalid macro: " + macroSlot);
			return null;
		}
		return AllMacros[macroSlot];
	}

	public static void SaveMacro(int macroSlot, string macroText)
	{
		GetMacroSettingBySlot(macroSlot)?.Set(macroText);
	}

	public static string GetMacroText(int macroSlot)
	{
		Setting<string> macroSettingBySlot = GetMacroSettingBySlot(macroSlot);
		if (macroSettingBySlot == null)
		{
			return "";
		}
		return macroSettingBySlot;
	}

	public static void ResetAllMacros()
	{
		foreach (Setting<string> allMacro in AllMacros)
		{
			allMacro.ResetToDefault(updatePref: true, savePrefsToDisk: false, broadcastUpdate: true);
		}
		PlayerPrefs.Save();
	}
}

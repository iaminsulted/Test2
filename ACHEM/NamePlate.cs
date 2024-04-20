using System;
using System.Collections;
using Assets.Scripts.Game;
using UnityEngine;

public class NamePlate : MonoBehaviour, IClickable, IInteractable
{
	private const int Visibility_Check_Frame_Interval = 5;

	private const float Bot_Slider_Delay = 0.5f;

	private const float Bot_Slider_Max_Delay = 2.5f;

	private const float Top_Slider_Percent_Rate = 3.5f;

	private const float Top_Slider_Increase_Rate = 1.75f;

	private const float Bot_Slider_Percent_Rate = 1f;

	private const float Max_Visibility_Distance_From_Player = 37f;

	private const float Close_Distance_From_Player = 33f;

	private const float Pvp_Visibility_Multiplier = 2f;

	private const float Text_Size = 0.2f;

	private Camera mainCamera;

	private Vector3 uiPos;

	private Entity entity;

	public UILabel label;

	public UILabel TitleLabel;

	public UILabel chatText;

	public UIWidget chatBubble;

	public UIWidget chatTail;

	public GameObject chatObject;

	public UISprite targetArrow;

	public GameObject emojiObject;

	public UISprite emojiSprite;

	public Texture2D[] emojis;

	public GameObject hpbarGO;

	public UISprite healthTop;

	public UISprite healthBack;

	public UISprite healthMid;

	public UISprite bossBorder;

	public UISlider healthSlider;

	public UISlider healthDmgSlider;

	public UISprite samMarkerTop;

	public UISprite samMarkerBot;

	public UISprite npcButton;

	public UISprite apopButton;

	public UISprite npcIcon;

	public UISprite apopIcon;

	public GameObject SamuraiExecuteMarkerGO;

	public DevButton npcRef;

	public DevButton apopRef;

	private Vector3 headSpotOffset;

	private Collider clickCollider;

	private float timerSeconds = 5f;

	private IEnumerator a;

	private float hpBotSliderDelay;

	private float hpBotSliderTotalDelay;

	private bool isVisible = true;

	private int frameCount = ArtixRandom.Range(1, 5);

	private float MaxVisibleDistance => (37f + Game.Instance.camController.cameraDistanceCurrent) * PvpVisibilityMultiplier;

	private float CloseDistance => (33f + Game.Instance.camController.cameraDistanceCurrent) * PvpVisibilityMultiplier;

	private float PvpVisibilityMultiplier
	{
		get
		{
			AreaData areaData = Game.Instance.AreaData;
			if (areaData == null || !areaData.HasPvp)
			{
				return 1f;
			}
			return 2f;
		}
	}

	private bool IsVisible
	{
		get
		{
			return isVisible;
		}
		set
		{
			if (isVisible != value)
			{
				isVisible = value;
				label.enabled = value;
				TitleLabel.enabled = value;
				chatBubble.enabled = value;
				chatText.enabled = value;
				chatTail.enabled = value;
				emojiSprite.enabled = value;
				targetArrow.enabled = value;
				healthBack.enabled = value;
				healthTop.enabled = value;
				healthSlider.enabled = value;
				bossBorder.enabled = value;
				healthDmgSlider.enabled = value;
				healthMid.enabled = value;
				samMarkerBot.enabled = value;
				samMarkerTop.enabled = value;
				apopButton.enabled = value;
				npcButton.enabled = value;
				apopIcon.enabled = value;
				npcIcon.enabled = value;
				if (value && ShouldShowHealthBar)
				{
					healthDmgSlider.value = entity.HealthPercent;
					healthSlider.value = entity.HealthPercent;
				}
				if (!entity.isMe)
				{
					clickCollider.enabled = value;
					NGUITools.UpdateWidgetCollider(base.gameObject);
				}
			}
		}
	}

	private bool ShouldShowName
	{
		get
		{
			if ((!entity.isMe || !SettingsManager.IsMyNameOn) && (entity.isMe || entity.type != Entity.Type.Player || !SettingsManager.IsPlayerNameOn))
			{
				if (entity.type == Entity.Type.NPC)
				{
					return SettingsManager.IsNPCNameOn;
				}
				return false;
			}
			return true;
		}
	}

	private bool ShouldShowGuildTag
	{
		get
		{
			if (entity is Player { IsInGuild: not false } player && (player.teamID == Entities.Instance.me.teamID || !Game.Instance.AreaData.HasPvp))
			{
				if (!player.isMe || !SettingsManager.IsMyGuildTagOn)
				{
					if (!player.isMe)
					{
						return SettingsManager.IsPlayerGuildTagOn;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	private bool ShouldShowTitle
	{
		get
		{
			bool num = (entity.isMe && (bool)SettingsManager.IsMyTitleOn) || (!entity.isMe && entity.type == Entity.Type.Player && (bool)SettingsManager.IsPlayerTitleOn) || (entity.type == Entity.Type.NPC && (bool)SettingsManager.IsNPCTitleOn);
			bool flag = entity.IsInPvp && entity.CanAttack(Entities.Instance.me);
			if (num && !string.IsNullOrEmpty(entity.TitleName))
			{
				return !flag;
			}
			return false;
		}
	}

	private bool IsHealthBarOn
	{
		get
		{
			if ((!entity.isMe || !SettingsManager.IsMyHealthBarOn) && (entity.isMe || entity.type != Entity.Type.Player || !SettingsManager.IsPlayerHealthBarOn))
			{
				if (entity.type == Entity.Type.NPC)
				{
					return SettingsManager.IsNpcHealthBarOn;
				}
				return false;
			}
			return true;
		}
	}

	private bool ShouldShowHealthBar
	{
		get
		{
			bool flag = Entities.Instance.me.CanAttack(entity) && entity.visualState == Entity.State.InCombat;
			bool flag2 = !Entities.Instance.me.CanAttack(entity) && entity.HealthPercent < 1f && entity.visualState != Entity.State.Dead;
			if (IsHealthBarOn)
			{
				return flag || flag2;
			}
			return false;
		}
	}

	private int TextLinesVisible
	{
		get
		{
			int num = 0;
			if (!string.IsNullOrEmpty(TitleLabel.text) && ShouldShowTitle)
			{
				num++;
			}
			if (!string.IsNullOrEmpty(entity.name) && ShouldShowName)
			{
				num++;
			}
			return num;
		}
	}

	public void Init(Entity setEntity, Camera MainCamera)
	{
		base.gameObject.SetActive(value: true);
		entity = setEntity;
		mainCamera = MainCamera;
		entity.ReactUpdated += OnEntityReactUpdate;
		entity.StatUpdateEvent += OnEntityStatUpdate;
		entity.VisualStateChanged += OnEntityVisualStateChanged;
		entity.ServerStateChanged += OnServerStateChanged;
		entity.TitleUpdated += UpdateTitle;
		entity.ChangeAFKStatus += OnAfkStatusUpdate;
		entity.ClassUpdated += OnClassUpdated;
		Session.MyPlayerData.FriendsUpdated += OnFriendUpdated;
		PartyManager.PartyUpdated += OnPartyUpdated;
		SettingsManager.NamePlateSettingUpdate += OnNamePlateSettingUpdated;
		headSpotOffset = entity.wrapper.transform.InverseTransformPoint(entity.HeadSpot.position);
		UpdateNameColor();
		UpdateName();
		SetMyTarget(Entities.Instance.me.Target == entity);
		TitleLabel.text = entity.TitleName;
		healthMid.color = (entity.isMe ? new Color32(byte.MaxValue, 87, 57, 172) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		clickCollider.enabled = !entity.isMe && IsVisible;
		bossBorder.gameObject.SetActive(entity.isBoss);
		if (entity.isBoss)
		{
			hpbarGO.gameObject.transform.localScale = new Vector3(800f, 800f, 800f);
			hpbarGO.gameObject.transform.localPosition = new Vector3(0f, -50f, 0f);
		}
		else
		{
			hpbarGO.gameObject.transform.localScale = new Vector3(420f, 420f, 420f);
			hpbarGO.gameObject.transform.localPosition = new Vector3(0f, -30f, 0f);
		}
		UpdateVisibility();
	}

	public void RemoveNamePlate()
	{
		entity.ReactUpdated -= OnEntityReactUpdate;
		entity.StatUpdateEvent -= OnEntityStatUpdate;
		entity.VisualStateChanged -= OnEntityVisualStateChanged;
		entity.ServerStateChanged -= OnServerStateChanged;
		entity.TitleUpdated -= UpdateTitle;
		entity.ChangeAFKStatus -= OnAfkStatusUpdate;
		entity.ClassUpdated -= OnClassUpdated;
		Session.MyPlayerData.FriendsUpdated -= OnFriendUpdated;
		PartyManager.PartyUpdated -= OnPartyUpdated;
		SettingsManager.NamePlateSettingUpdate -= OnNamePlateSettingUpdated;
		ChatBubbleClear();
	}

	public void Awake()
	{
		clickCollider = base.gameObject.GetComponent<Collider>();
	}

	public void Start()
	{
		NGUITools.UpdateWidgetCollider(base.gameObject);
	}

	private void LateUpdate()
	{
		if (!(entity.wrapper == null))
		{
			UpdatePositionAndVisibility();
			if (IsVisible)
			{
				UpdateSlider(healthSlider, healthDmgSlider, entity.HealthPercent, ref hpBotSliderDelay, ref hpBotSliderTotalDelay);
			}
			UpdateScale();
			DevButtonsToggle();
		}
	}

	private void OnNamePlateSettingUpdated()
	{
		UpdateName();
		UpdateVisibility();
	}

	private void UpdateTitle(int _, string __)
	{
		TitleLabel.text = entity.TitleName;
		UpdateVisibility();
	}

	private void UpdateVisibility()
	{
		TitleLabel.gameObject.SetActive(ShouldShowTitle && !ShouldShowHealthBar);
		UpdateHealthBarVisibility();
		NGUITools.UpdateWidgetCollider(base.gameObject);
	}

	public void SetMyTarget(bool isMyTarget)
	{
		if (isMyTarget)
		{
			label.fontStyle = FontStyle.Bold;
			bool active = (bool)SettingsManager.IsTargetArrowOn && (!(entity is NPC nPC) || !(nPC.npciatrigger != null));
			targetArrow.gameObject.SetActive(active);
			UpdateNameColor();
		}
		else
		{
			label.fontStyle = FontStyle.Normal;
			targetArrow.gameObject.SetActive(value: false);
		}
	}

	public void OnClick(Vector3 hitpoint)
	{
		entity.Interact();
	}

	public void OnDoubleClick()
	{
		if (Entities.Instance.me.CanAttack(entity))
		{
			Game.Instance.ApplyAction(InputAction.Spell_1);
		}
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
	}

	public void OnHover()
	{
		if (Entities.Instance.me.CanAttack(entity))
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Combat);
		}
		else if (entity is NPC nPC && nPC.npciatrigger?.CurrentApop != null)
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Talk);
		}
		else
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
		}
	}

	private int GetLayerMask()
	{
		if (!entity.isMe)
		{
			return Layers.MASK_NAMEPLATE;
		}
		return Layers.MASK_MY_NAMEPLATE;
	}

	private void UpdatePositionAndVisibility()
	{
		Vector3 vector = entity.wrapper.transform.TransformPoint(headSpotOffset);
		Vector3 vector2 = mainCamera.WorldToViewportPoint(vector);
		vector.y = Mathf.Max(vector.y, entity.HeadSpot.position.y);
		float num = ((ShouldShowTitle || ShouldShowHealthBar) ? 0.4f : 0.2f);
		if (entity.isBoss && ShouldShowHealthBar)
		{
			num += 0.3f;
		}
		num *= Math.Min(vector2.z, CloseDistance) / 12f;
		vector += Vector3.up * num;
		vector += Vector3.down * 0.2f * TextLinesVisible * (1f - GetSizeFromDistance(vector2.z)) * 3f;
		if (entity is NPC nPC && nPC.npciatrigger != null)
		{
			if (nPC.teamID > 0 && Entities.Instance.me.teamID > 0 && nPC.teamID != Entities.Instance.me.teamID)
			{
				nPC.npciatrigger.gameObject.SetActive(value: false);
			}
			else
			{
				nPC.npciatrigger.gameObject.SetActive(nPC.HasActiveApop() || nPC.HasTalkObjective);
			}
			Vector3 position = vector + Vector3.up * 0.2f * 2.5f;
			if (!ShouldShowName)
			{
				position += Vector3.down * 0.2f;
			}
			nPC.npciatrigger.transform.position = position;
		}
		if (vector2.z < 0f || vector2.z > MaxVisibleDistance || vector2.x < 0f || vector2.x > 1f || vector2.y < 0f || vector2.y > 1f)
		{
			frameCount = 0;
			IsVisible = false;
			return;
		}
		MathUtil.LinePlaneIntersection(out uiPos, mainCamera.transform.position, vector - mainCamera.transform.position, mainCamera.transform.forward, base.transform.position);
		base.transform.position = uiPos;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
		healthTop.depth = (int)(0f - vector2.z);
		if (entity.isMe)
		{
			healthTop.depth += 3;
		}
		samMarkerBot.depth = healthTop.depth;
		samMarkerTop.depth = healthTop.depth;
		chatBubble.depth = healthTop.depth;
		emojiSprite.depth = healthTop.depth;
		chatText.depth = healthTop.depth + 1;
		chatTail.depth = healthTop.depth - 1;
		bossBorder.depth = healthTop.depth + 1;
		healthMid.depth = healthTop.depth - 1;
		healthBack.depth = healthMid.depth - 1;
		label.depth = healthTop.depth;
		TitleLabel.depth = healthTop.depth;
		apopButton.depth = healthTop.depth + 2;
		npcButton.depth = healthTop.depth + 2;
		apopIcon.depth = healthTop.depth + 3;
		npcIcon.depth = healthTop.depth + 3;
		if (entity == Entities.Instance.me.Target)
		{
			IsVisible = true;
			return;
		}
		if (frameCount == 0)
		{
			if (entity.isMe)
			{
				IsVisible = true;
			}
			else
			{
				bool flag = !entity.IsNamePlateHidden || entity.visualState != Entity.State.Normal;
				IsVisible = flag && !Physics.Linecast(mainCamera.transform.position, vector, GetLayerMask());
			}
		}
		frameCount = (frameCount + 1) % 5;
	}

	private void UpdateScale()
	{
		if (IsVisible)
		{
			float z = mainCamera.transform.InverseTransformPoint(entity.wrapper.transform.position).z;
			float sizeFromDistance = GetSizeFromDistance(z);
			float num = Mathf.Tan(MathF.PI / 360f * mainCamera.fieldOfView) / Mathf.Tan(MathF.PI * 7f / 60f);
			sizeFromDistance *= num;
			base.transform.localScale = new Vector3(sizeFromDistance, sizeFromDistance, 1f);
		}
	}

	private float GetSizeFromDistance(float distance)
	{
		if (distance < CloseDistance)
		{
			return 1f - (0f + 0.4f * distance / CloseDistance);
		}
		if (distance < MaxVisibleDistance)
		{
			return 1f - (0.4f + 0.6f * (distance - CloseDistance) / (MaxVisibleDistance - CloseDistance));
		}
		return 0f;
	}

	private void OnEntityReactUpdate(Entity entity)
	{
		UpdateNameColor();
	}

	private void OnEntityStatUpdate()
	{
		UpdateVisibility();
	}

	private void OnEntityVisualStateChanged(Entity entity)
	{
		UpdateNameColor();
		UpdateVisibility();
	}

	private void UpdateHealthBarVisibility()
	{
		if (!IsHealthBarOn)
		{
			hpbarGO.SetActive(value: false);
			return;
		}
		hpbarGO.SetActive(ShouldShowHealthBar);
		if (entity.isMe)
		{
			UpdateHealthBarColor(InterfaceColors.Names.Staff);
		}
		else if (entity.type == Entity.Type.Player)
		{
			if (entity.CanAttack(Entities.Instance.me))
			{
				UpdateHealthBarColor(InterfaceColors.EntityReaction.Hostile);
			}
			else
			{
				UpdateHealthBarColor(InterfaceColors.EntityReaction.Inactive);
			}
		}
		else
		{
			UpdateHealthBarColor(entity.GetNamePlateColor());
		}
		if (entity != Entities.Instance.me)
		{
			if (Session.MyPlayerData.EquippedClassID == 24 || Session.MyPlayerData.EquippedClassID == 25)
			{
				SamuraiExecuteMarkerGO.SetActive(value: true);
				if ((double)entity.HealthPercent <= 0.4)
				{
					healthTop.color = Color.yellow;
				}
				else
				{
					healthTop.color = entity.GetNamePlateColor();
				}
			}
		}
		else
		{
			SamuraiExecuteMarkerGO.SetActive(value: false);
		}
	}

	private void UpdateHealthBarColor(Color32 color)
	{
		healthTop.color = color;
	}

	private void OnServerStateChanged(Entity.State previousState, Entity.State newState)
	{
		if (newState == Entity.State.Dead)
		{
			clickCollider.enabled = false;
		}
	}

	private void OnFriendUpdated()
	{
		if (entity.type == Entity.Type.Player)
		{
			UpdateNameColor();
			UpdateHealthBarColor(entity.GetNamePlateColor());
		}
	}

	private void OnPartyUpdated()
	{
		if (entity.type == Entity.Type.Player)
		{
			UpdateNameColor();
			UpdateHealthBarColor(entity.GetNamePlateColor());
		}
	}

	private void OnAfkStatusUpdate(bool _)
	{
		UpdateName();
	}

	private void OnClassUpdated()
	{
		UpdateName();
	}

	private void UpdateName()
	{
		if (!ShouldShowName && !ShouldShowGuildTag)
		{
			label.text = "";
			return;
		}
		string text = "";
		if (entity.isAFK)
		{
			text += "<AFK> ";
		}
		if (ShouldShowName)
		{
			if (entity.IsInPvp && entity.CanAttack(Entities.Instance.me))
			{
				text += entity.CombatClass.Name;
				label.text = text;
				return;
			}
			text += entity.name;
		}
		if (ShouldShowGuildTag && entity is Player player)
		{
			if (ShouldShowName)
			{
				text += " ";
			}
			text = text + "[" + player.guildTag + "]";
		}
		label.text = text;
	}

	private void UpdateNameColor()
	{
		label.color = entity.GetNamePlateColor();
		targetArrow.color = entity.GetNamePlateColor();
	}

	public void ChatBubble(string msg)
	{
		if (!SettingsManager.IsChatBubbleOn)
		{
			return;
		}
		if (msg.Length > 20)
		{
			int num = msg.Length / 20;
			for (int i = 1; i < num; i++)
			{
				num = msg.Length / 20;
				string text = msg.Substring((i - 1) * 20, 20);
				if (!text.Contains(" ") && i > 1)
				{
					msg = msg.Insert(20 * i + (i - 1), "\n");
				}
				else if (!text.Contains(" ") && i == 1)
				{
					msg = msg.Insert(20 * i, "\n");
				}
			}
			num = msg.Length / 20;
			if (msg.Length > num * 20 + num && !msg.Substring(20 * (num - 1)).Contains(" "))
			{
				msg = msg.Insert(20 * num + (num - 1), "\n");
			}
		}
		chatText.text = msg;
		timerSeconds = 2f + (float)msg.Length / 30f;
		if (timerSeconds > 7f)
		{
			timerSeconds = 7f;
		}
		a = ChatAndTimer(timerSeconds, chatObject);
		StopAllCoroutines();
		StartCoroutine(a);
	}

	public void ChatBubbleClear()
	{
		StopAllCoroutines();
		chatObject.SetActive(value: false);
		emojiObject.SetActive(value: false);
	}

	private IEnumerator ChatAndTimer(float waitTime, GameObject go)
	{
		go.SetActive(value: true);
		while (waitTime > 0f)
		{
			yield return new WaitForSeconds(1f);
			waitTime -= 1f;
		}
		go.SetActive(value: false);
	}

	public void CutsceneStart()
	{
		chatText.text = "...";
		chatObject.SetActive(value: true);
	}

	public void CutsceneStop()
	{
		chatObject.SetActive(value: false);
	}

	public void EmojiShow(string name)
	{
		chatObject.SetActive(value: false);
		if (!(name == "Cry"))
		{
			if (name == ":)")
			{
				emojiSprite.spriteName = emojis[3].name;
			}
		}
		else
		{
			emojiSprite.spriteName = emojis[4].name;
		}
		a = ChatAndTimer(timerSeconds, emojiObject);
		StopAllCoroutines();
		StartCoroutine(a);
	}

	private void UpdateSlider(UISlider topSlider, UISlider botSlider, float newValue, ref float botSliderDelay, ref float botSliderTotalDelay)
	{
		if (topSlider.value > newValue)
		{
			topSlider.value += GetSliderDelta(3.5f, newValue, topSlider.value);
			botSliderDelay = 0.5f;
		}
		else
		{
			topSlider.value += GetSliderDelta(1.75f, newValue, topSlider.value);
		}
		if (botSliderDelay > 0f)
		{
			botSliderDelay -= Time.deltaTime;
			botSliderTotalDelay += Time.deltaTime;
		}
		if ((botSliderDelay <= 0f || botSliderTotalDelay >= 2.5f) && botSlider.value > topSlider.value)
		{
			botSlider.value += GetSliderDelta(1f, topSlider.value, botSlider.value);
			if (botSlider.value <= topSlider.value)
			{
				botSliderTotalDelay = 0f;
			}
		}
		if (botSlider.value < topSlider.value)
		{
			botSlider.value = topSlider.value;
		}
	}

	private float GetSliderDelta(float rate, float newValue, float oldValue)
	{
		float value = Time.deltaTime * rate;
		value = Mathf.Clamp(value, 0f, Mathf.Abs(newValue - oldValue));
		return (newValue - oldValue > 0f) ? value : (0f - value);
	}

	public void DevButtonsToggle()
	{
		if (!Session.MyPlayerData.devMode)
		{
			apopButton.gameObject.SetActive(value: false);
			npcButton.gameObject.SetActive(value: false);
		}
		if (Entities.Instance.me.AccessLevel >= 100 && Main.Environment == Environment.Content && entity.type == Entity.Type.NPC && npcRef != null && apopRef != null && Session.MyPlayerData.devMode)
		{
			if (((NPC)entity).ApopIDs != null)
			{
				apopButton.gameObject.SetActive(value: true);
			}
			else
			{
				apopButton.gameObject.SetActive(value: false);
			}
			npcButton.gameObject.SetActive(value: true);
			apopRef.ButtonType = 1;
			npcRef.ButtonType = 2;
			npcRef.entity = entity;
			apopRef.entity = entity;
		}
	}

	public bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public int GetPriority()
	{
		return 0;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
	}
}

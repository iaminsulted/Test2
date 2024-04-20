using System;
using UnityEngine;

public abstract class UIActionButton : MonoBehaviour
{
	public InputAction Action;

	public UISprite Icon;

	public UISprite SprCooldown;

	public UISprite SprFlash;

	public UISprite SprRing;

	public UILabel LabelKey;

	public UITweener FlashTween;

	public GameObject Lock;

	protected bool isCastable;

	protected bool isLocked;

	private float totalCooldown;

	private float remainingCooldown;

	private bool isPressed;

	public SpellTemplate spellT { get; protected set; }

	public virtual bool IsAssigned => spellT != null;

	private bool IsPressed
	{
		set
		{
			if (isPressed != value)
			{
				isPressed = value;
				base.gameObject.SendMessage("OnPress", isPressed, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public event Action<InputAction> Clicked;

	public event Action<InputAction, bool> Hovered;

	protected abstract bool IsLocked();

	protected virtual void Awake()
	{
		if (Platform.IsMobile && LabelKey != null)
		{
			LabelKey.gameObject.SetActive(value: false);
		}
	}

	protected virtual void Start()
	{
		if (SettingsManager.Keybinds.ContainsKey(Action))
		{
			UpdateHotkeyDisplay();
		}
	}

	protected virtual void OnEnable()
	{
		SettingsManager.KeyMappingUpdated += UpdateHotkeyDisplay;
		Game.Instance.combat.QueuedSpellChange += OnQueuedSpellChange;
		Game.Instance.combat.CDTrigger += OnSpellCast;
		Game.Instance.combat.ResetSpellCD += OnResetSpellCooldown;
		Entities.Instance.me.PvpStateCheck += CheckForUnlock;
	}

	protected virtual void OnDisable()
	{
		SettingsManager.KeyMappingUpdated -= UpdateHotkeyDisplay;
		if (Game.Instance.combat != null)
		{
			Game.Instance.combat.QueuedSpellChange -= OnQueuedSpellChange;
			Game.Instance.combat.CDTrigger -= OnSpellCast;
			Game.Instance.combat.ResetSpellCD -= OnResetSpellCooldown;
		}
		if (Entities.Instance?.me != null)
		{
			Entities.Instance.me.PvpStateCheck -= CheckForUnlock;
		}
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual void Update()
	{
		IsPressed = InputManager.GetActionKey(Action);
		if (remainingCooldown > 0f && spellT != null && !spellT.isUlt)
		{
			remainingCooldown = Game.Instance.combat.GetRemainingCooldown(spellT);
			if (remainingCooldown <= 0f)
			{
				FinishCooldown();
			}
			else
			{
				SprCooldown.fillAmount = remainingCooldown / totalCooldown;
			}
		}
	}

	public virtual void Refresh()
	{
		if (spellT != null)
		{
			UpdateSpell(spellT.ID);
		}
	}

	protected void UpdateSpell(int spellID)
	{
		Player me = Entities.Instance.me;
		if (me == null)
		{
			return;
		}
		SpellTemplate spellTemplate = SpellTemplates.Get(spellID, me.effects, me.ScaledClassRank, me.EquippedClassID, 0);
		if (spellTemplate == null)
		{
			ShowEmpty();
		}
		else if (spellT != spellTemplate)
		{
			spellT = spellTemplate;
			if (!spellT.isAA)
			{
				Icon.spriteName = spellT.icon;
			}
			if (!spellT.isUlt && !spellT.isAA)
			{
				totalCooldown = Game.Instance.combat.GetSpellCooldown(spellT);
				remainingCooldown = Game.Instance.combat.GetRemainingCooldown(spellT);
				SprCooldown.gameObject.SetActive(remainingCooldown > 0f);
			}
		}
	}

	protected virtual void ShowEmpty()
	{
		Icon.spriteName = "aq3dui-goldbordercircle";
	}

	private void OnQueuedSpellChange(int spellID)
	{
		if (spellT != null && !spellT.isAA)
		{
			if (spellT.ID == spellID && !SprRing.gameObject.activeSelf)
			{
				SprRing.gameObject.SetActive(value: true);
			}
			else if (SprRing.gameObject.activeSelf)
			{
				SprRing.gameObject.SetActive(value: false);
			}
		}
	}

	private void OnSpellCast(int spellID, float cooldown)
	{
		if (spellT != null && !spellT.isAA && spellT.ID == spellID && !IsLocked())
		{
			CoolDown(cooldown);
		}
	}

	private void OnResetSpellCooldown(int spellID, float cooldown)
	{
		if (spellT != null && !spellT.isAA && spellT.ID == spellID && !IsLocked())
		{
			FinishCooldown();
		}
	}

	public virtual void OnClick()
	{
		this.Clicked?.Invoke(Action);
	}

	public virtual void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(GetTooltip(), UIGame.Instance.FixedToolTipPosition);
		}
		else
		{
			Tooltip.Hide();
		}
		this.Hovered?.Invoke(Action, show);
	}

	protected virtual string GetTooltip()
	{
		if (spellT == null)
		{
			return "";
		}
		return spellT.ToolTipText;
	}

	protected virtual void UpdateLock()
	{
		isLocked = IsLocked();
		Lock.SetActive(isLocked);
		LabelKey.gameObject.SetActive(!isLocked && UIGame.ControlScheme == ControlScheme.PC);
		Icon.color = (isLocked ? InterfaceColors.ActionButton.Unable_To_Cast : InterfaceColors.ActionButton.Default_Color);
	}

	protected virtual void CheckForUnlock()
	{
		bool flag = IsLocked();
		if (isLocked && !flag)
		{
			PlayUnlock();
		}
		UpdateLock();
	}

	private void PlayUnlock()
	{
	}

	protected virtual void UpdateHotkeyDisplay()
	{
		LabelKey.text = SettingsManager.GetHotkeyByAction(Action);
	}

	private void CoolDown(float cooldown)
	{
		if (!spellT.isUlt && !spellT.isAA && cooldown > 0f)
		{
			totalCooldown = cooldown;
			remainingCooldown = cooldown;
			SprCooldown.gameObject.SetActive(value: true);
		}
	}

	private void FinishCooldown()
	{
		remainingCooldown = 0f;
		SprCooldown.fillAmount = 0f;
		SprCooldown.gameObject.SetActive(value: false);
		SprFlash.gameObject.SetActive(value: true);
		FlashTween.tweenFactor = 0f;
		FlashTween.PlayForward();
	}

	public void FlashEnd()
	{
		SprFlash.gameObject.SetActive(value: false);
	}

	public void CheckForOverride()
	{
		if (spellT != null && !spellT.isAA)
		{
			UpdateSpell(spellT.ID);
		}
	}

	public virtual void StatUpdate(int resourceCost, float playerResource, float playerHealth, float ultCharge)
	{
		if (!IsLocked())
		{
			CheckCastable(resourceCost, playerResource, playerHealth);
		}
	}

	public void CheckCastable(int resourceCost, float playerResource, float playerHealth)
	{
		if (spellT != null && !IsLocked())
		{
			bool flag = CanCastSpell(resourceCost, playerResource, playerHealth);
			if (flag != isCastable)
			{
				isCastable = flag;
				UpdateCastableView();
			}
		}
	}

	protected bool CanCastSpell(int resourceCost, float playerResource, float playerHealth)
	{
		bool num = (float)resourceCost <= playerResource;
		bool flag = playerHealth > 0f;
		bool flag2 = !Entities.Instance.me.HasStatus(Entity.StatusType.NoSpells) || spellT.isAA || spellT.canUseDuringCC;
		bool flag3 = !Entities.Instance.me.HasStatus(Entity.StatusType.NoAA) || !spellT.isAA || spellT.canUseDuringCC;
		return num && flag && flag2 && flag3;
	}

	protected virtual void UpdateCastableView()
	{
		if (!isCastable)
		{
			Icon.color = InterfaceColors.ActionButton.Unable_To_Cast;
		}
		else
		{
			Icon.color = InterfaceColors.ActionButton.Default_Color;
		}
	}
}

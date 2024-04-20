using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionBar : MonoBehaviour
{
	[FormerlySerializedAs("btns")]
	public UIActionSpell[] spellButtons;

	public List<UIActionButton> customButtons;

	public List<UIActionButton> customItemButtons;

	public List<UIActionButton> customPvpButtons;

	public UIButton SwapButton;

	public UIActionCrossSkill crossSkillButton;

	public PulseColor AAPulse;

	public UIJumpButton JumpButton;

	private CombatSolver combat;

	private Entity player;

	private bool isSwapActive;

	private readonly List<UIActionButton> allButtons = new List<UIActionButton>();

	public event Action<InputAction, bool> ActionHovered;

	public void Init(CombatSolver combat, Entity player)
	{
		this.combat = combat;
		this.player = player;
		combat.SpellsUpdated += OnSpellUpdated;
		combat.AAStateChanged += OnAAStateChanged;
		player.StatUpdateEvent += OnPlayerStatUpdate;
		player.EffectAdded += OnPlayerEffectAdded;
		player.EffectRemoved += OnPlayerEffectRemoved;
		player.LevelUpdated += OnPlayerLevelUpdated;
		player.ServerStateChanged += OnPlayerStateChanged;
		player.PvpStateCheck += OnPvpStateCheck;
		Session.MyPlayerData.ClassRankUpdated += ClassRankUpdated;
		SpellTemplates.SpellsLoaded += OnSpellsLoaded;
		ResetCustomButtons();
		UpdateCustomButtons(player.IsInPvp || Game.Instance.AreaData.IsPvpLobby);
		allButtons.Clear();
		allButtons.AddRange(spellButtons);
		allButtons.AddRange(customItemButtons);
		allButtons.AddRange(customPvpButtons);
		allButtons.Add(crossSkillButton);
		foreach (UIActionButton allButton in allButtons)
		{
			allButton.Clicked += OnActionClicked;
			allButton.Hovered += OnActionHovered;
		}
		if (UICamera.currentScheme != 0)
		{
			UIJumpButton jumpButton = JumpButton;
			jumpButton.Pressed = (Action)Delegate.Combine(jumpButton.Pressed, new Action(OnJumpAction));
		}
		OnSpellUpdated(combat.spellIDs);
		OnPlayerStatUpdate();
		UpdateSwapVisible();
		RefreshButtons();
	}

	public void OnDestroy()
	{
		foreach (UIActionButton allButton in allButtons)
		{
			allButton.Clicked -= OnActionClicked;
			allButton.Hovered -= OnActionHovered;
		}
		if (combat != null)
		{
			combat.SpellsUpdated -= OnSpellUpdated;
			combat.AAStateChanged -= OnAAStateChanged;
		}
		if (player != null)
		{
			player.StatUpdateEvent -= OnPlayerStatUpdate;
			player.EffectAdded -= OnPlayerEffectAdded;
			player.EffectRemoved -= OnPlayerEffectRemoved;
			player.LevelUpdated -= OnPlayerLevelUpdated;
			player.ServerStateChanged -= OnPlayerStateChanged;
			player.PvpStateCheck -= OnPvpStateCheck;
		}
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassRankUpdated -= ClassRankUpdated;
		}
		SpellTemplates.SpellsLoaded -= OnSpellsLoaded;
	}

	private void ResetCustomButtons()
	{
		foreach (UIActionButton customItemButton in customItemButtons)
		{
			customItemButton.gameObject.SetActive(value: false);
		}
		foreach (UIActionButton customPvpButton in customPvpButtons)
		{
			customPvpButton.gameObject.SetActive(value: false);
		}
	}

	private void UpdateCustomButtons(bool isInPvp)
	{
		foreach (UIActionButton customButton in customButtons)
		{
			customButton.gameObject.SetActive(value: false);
		}
		customButtons.Clear();
		if (isInPvp)
		{
			customButtons.AddRange(customPvpButtons);
			if (customButtons.First((UIActionButton b) => b.Action == InputAction.CustomAction_4) is UIActionSpell uIActionSpell)
			{
				uIActionSpell.Init(2008);
			}
		}
		else
		{
			customButtons.AddRange(customItemButtons);
		}
		foreach (UIActionButton customButton2 in customButtons)
		{
			customButton2.gameObject.SetActive(value: true);
		}
	}

	public void OnTargetCycle()
	{
		switch (SettingsManager.TargetButtonType)
		{
		case 0L:
			combat.TargetNextEnemy();
			break;
		case 1L:
			combat.TargetClosestEnemy();
			break;
		}
	}

	private void OnPlayerStateChanged(Entity.State oldState, Entity.State newState)
	{
		if (isSwapActive && newState == Entity.State.InCombat)
		{
			ToggleSwap();
		}
	}

	private void OnPvpStateCheck()
	{
		Player me = Entities.Instance.me;
		UpdateCustomButtons((me != null && me.IsInPvp) || (Game.Instance.AreaData?.IsPvpLobby ?? false));
	}

	public void ToggleSwap()
	{
		isSwapActive = !isSwapActive;
		SwapButton.defaultColor = (isSwapActive ? Color.white : ((Color)new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 109)));
		if (isSwapActive)
		{
			SwapButton.normalSprite = "Icon-Swap-Down";
			SwapButton.hoverSprite = "Icon-Swap-Down";
		}
		else
		{
			SwapButton.normalSprite = "Icon-Swap-Up";
			SwapButton.hoverSprite = "Icon-Swap-Up";
		}
		foreach (UIActionButton customButton in customButtons)
		{
			if (customButton is UICustomActionButton uICustomActionButton)
			{
				uICustomActionButton.ChangeSwapState(isSwapActive);
			}
		}
		crossSkillButton.ChangeSwapState(isSwapActive);
	}

	public void OnJumpAction()
	{
		((OmniMovementController)player.moveController).Jump();
	}

	private void OnAAStateChanged(bool enabled)
	{
		AAPulse.gameObject.SetActive(enabled);
	}

	private void OnSpellUpdated(Dictionary<InputAction, int> spells)
	{
		if (player != null)
		{
			UIActionSpell[] array = spellButtons;
			foreach (UIActionSpell uIActionSpell in array)
			{
				uIActionSpell.Init(spells[uIActionSpell.Action]);
			}
		}
	}

	private void OnActionClicked(InputAction action)
	{
		InputManager.OnActionEvent(action);
	}

	private void OnActionHovered(InputAction action, bool show)
	{
		this.ActionHovered?.Invoke(action, show);
	}

	private void OnPlayerStatUpdate()
	{
		float playerResource = player.statsCurrent[Stat.Resource];
		float playerHealth = player.statsCurrent[Stat.Health];
		float ultCharge = player.statsCurrent[Stat.UltCharge];
		foreach (UIActionButton allButton in allButtons)
		{
			if (allButton.spellT != null)
			{
				allButton.StatUpdate(allButton.spellT.GetResourceCost(player), playerResource, playerHealth, ultCharge);
			}
		}
	}

	private void OnPlayerEffectAdded(Effect effect)
	{
		UpdateCastableButtons();
		UIActionSpell[] array = spellButtons;
		foreach (UIActionSpell uIActionSpell in array)
		{
			if (uIActionSpell.spellT.ID == effect.template.spellHighlightID)
			{
				uIActionSpell.MakeSpellFlash(shouldFlash: true);
			}
		}
	}

	private void OnPlayerEffectRemoved(Effect effect)
	{
		UpdateCastableButtons();
		if (effect.template.spellHighlightID == -1)
		{
			return;
		}
		UIActionSpell[] array = spellButtons;
		foreach (UIActionSpell uIActionSpell in array)
		{
			if (uIActionSpell.spellT.ID == effect.template.spellHighlightID)
			{
				uIActionSpell.MakeSpellFlash(shouldFlash: false);
			}
		}
	}

	private void UpdateCastableButtons()
	{
		float playerResource = player.statsCurrent[Stat.Resource];
		float playerHealth = player.statsCurrent[Stat.Health];
		foreach (UIActionButton allButton in allButtons)
		{
			if (allButton.spellT != null)
			{
				allButton.CheckForOverride();
				allButton.CheckCastable(allButton.spellT.GetResourceCost(player), playerResource, playerHealth);
			}
		}
	}

	private void OnSpellsLoaded()
	{
		foreach (UIActionButton allButton in allButtons)
		{
			allButton.Refresh();
		}
	}

	private void ClassRankUpdated(int classID, int classXP)
	{
		UpdateSwapVisible();
	}

	private void OnPlayerLevelUpdated()
	{
		UpdateSwapVisible();
	}

	private void UpdateSwapVisible()
	{
		int num = Game.Instance.LevelReqForAction(InputAction.CustomAction_1);
		bool flag = Session.MyPlayerData.UnlockedCrossSkillIDs.Count > 0;
		SwapButton.gameObject.SetActive(flag || player.Level >= num);
	}

	public InventoryItem GetCustomActionItem(InputAction action)
	{
		UICustomActionItem uICustomActionItem = customButtons.OfType<UICustomActionItem>().FirstOrDefault((UICustomActionItem btn) => btn.Action == action);
		if (uICustomActionItem == null)
		{
			return null;
		}
		return uICustomActionItem.item;
	}

	public SpellTemplate GetPvpAction(InputAction action)
	{
		UIActionButton uIActionButton = customButtons.FirstOrDefault((UIActionButton btn) => btn.Action == action);
		if (uIActionButton == null)
		{
			return null;
		}
		return uIActionButton.spellT;
	}

	private void RefreshButtons()
	{
		foreach (UIActionButton allButton in allButtons)
		{
			allButton.Refresh();
		}
	}

	public void DisableJump()
	{
		UIJumpButton jumpButton = JumpButton;
		jumpButton.Pressed = (Action)Delegate.Remove(jumpButton.Pressed, new Action(OnJumpAction));
	}

	public void EnableJump()
	{
		UIJumpButton jumpButton = JumpButton;
		jumpButton.Pressed = (Action)Delegate.Combine(jumpButton.Pressed, new Action(OnJumpAction));
	}

	public CombatSpellSlot ReturnFirstEmptySlot()
	{
		for (int i = 0; i < customItemButtons.Count; i++)
		{
			if (!customButtons[i].IsAssigned)
			{
				return slotConverter(i);
			}
		}
		return slotConverter(-1);
	}

	public CombatSpellSlot slotConverter(int index)
	{
		return index switch
		{
			0 => CombatSpellSlot.CustomAction1, 
			1 => CombatSpellSlot.CustomAction2, 
			2 => CombatSpellSlot.CustomAction3, 
			3 => CombatSpellSlot.CustomAction4, 
			_ => CombatSpellSlot.Passive, 
		};
	}
}

public class UICustomActionPvp : UICustomActionButton
{
	protected override void Start()
	{
		base.Start();
		UpdateLock();
		if (!IsLocked())
		{
			UpdateDataFromDisk();
		}
		SprCooldown.gameObject.SetActive(value: false);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Session.MyPlayerData.PvpActionEquipped += PvpActionEquipped;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Session.MyPlayerData.PvpActionEquipped -= PvpActionEquipped;
	}

	private void UpdateDataFromDisk()
	{
		int actionSlotID = SettingsManager.GetActionSlotID(SlotNumber);
		UpdatePvpAction(actionSlotID);
	}

	private void UpdatePvpAction(int spellID)
	{
		UpdateSpell(spellID);
		if (!base.spellT.isPvpAction)
		{
			base.spellT = null;
			ShowEmpty();
		}
		else if (base.spellT != null)
		{
			EmptyIcon.gameObject.SetActive(value: false);
		}
	}

	protected override bool IsLocked()
	{
		return Action == InputAction.CustomAction_4;
	}

	protected override void OpenSelectMenu()
	{
		if (!IsLocked())
		{
			if (base.spellT != null && Game.Instance.combat.GetRemainingCooldown(base.spellT) > 0f)
			{
				Notification.ShowWarning("Cannot swap while on cooldown");
			}
			else
			{
				UICustomActionSelection.Load(UICustomActionSelection.Mode.PvpAction, SlotNumber);
			}
		}
	}

	private void PvpActionEquipped(CombatSpellSlot slot, int spellID)
	{
		if (SlotNumber == slot)
		{
			UpdatePvpAction(spellID);
		}
	}
}

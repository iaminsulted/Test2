public class UIActionCrossSkill : UICustomActionButton
{
	protected override void OnEnable()
	{
		base.OnEnable();
		Session.MyPlayerData.ClassRankUpdated += ClassRankUpdated;
		Session.MyPlayerData.CrossSkillEquipped += CrossSkillUpdated;
	}

	protected override void Start()
	{
		base.Start();
		UpdateCrossSkillLockStatus();
		UpdateLock();
		if (Session.MyPlayerData.CurrentCrossSkill > 0 && Session.MyPlayerData.UnlockedCrossSkillIDs.Count > 0)
		{
			UpdateCrossSkill(Session.MyPlayerData.CurrentCrossSkill);
		}
	}

	private void UpdateCrossSkill(int spellID)
	{
		UpdateSpell(spellID);
		if (base.spellT != null)
		{
			EmptyIcon.gameObject.SetActive(value: false);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassRankUpdated -= ClassRankUpdated;
			Session.MyPlayerData.CrossSkillEquipped -= CrossSkillUpdated;
		}
	}

	private void ClassRankUpdated(int classID, int classXP)
	{
		UpdateCrossSkillLockStatus();
		CheckForUnlock();
	}

	protected override void CheckForUnlock()
	{
		base.CheckForUnlock();
		if (base.spellT != null && !Session.MyPlayerData.UnlockedCrossSkillIDs.Contains(base.spellT.ID))
		{
			ShowEmpty();
		}
	}

	private void UpdateCrossSkillLockStatus()
	{
		isLocked = Session.MyPlayerData.UnlockedCrossSkillIDs.Count == 0;
	}

	private void CrossSkillUpdated(int spellID)
	{
		UpdateCrossSkill(spellID);
	}

	protected override void UpdateLock()
	{
		base.UpdateLock();
		if (!IsLocked() && base.spellT != null)
		{
			UpdateCrossSkill(base.spellT.ID);
		}
	}

	protected override bool IsLocked()
	{
		return isLocked;
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
				UICustomActionSelection.Load(UICustomActionSelection.Mode.CrossSkill, SlotNumber);
			}
		}
	}
}

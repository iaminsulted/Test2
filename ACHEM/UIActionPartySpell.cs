using System;

public class UIActionPartySpell : UIActionButton
{
	private Entity entity;

	public Action<bool> OnHoverAction;

	protected override bool IsLocked()
	{
		return false;
	}

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void Update()
	{
	}

	public override void OnTooltip(bool show)
	{
	}

	protected override void UpdateHotkeyDisplay()
	{
		LabelKey.text = "";
	}

	public void OnHover(bool isHover)
	{
		OnHoverAction?.Invoke(isHover);
	}

	public void Init(SpellTemplate spellT, Entity entity)
	{
		this.entity = entity;
		UpdateSpell(spellT);
		UpdateHotkeyDisplay();
	}

	public void UpdateSpell(SpellTemplate spellT)
	{
		if (spellT == null)
		{
			ShowEmpty();
			return;
		}
		base.spellT = spellT;
		Icon.spriteName = spellT.icon;
	}

	public override void OnClick()
	{
		int id = 0;
		Entity.Type type = Entity.Type.Undefined;
		if (Entities.Instance.me.Target != null)
		{
			id = Entities.Instance.me.Target.ID;
			type = Entities.Instance.me.Target.type;
		}
		Entities.Instance.me.Target = this.entity;
		Game.Instance.combat.TryCastSpell(base.spellT);
		ClientMovementController clientMovementController = Entities.Instance.me.moveController as ClientMovementController;
		if (clientMovementController == null || !clientMovementController.isAutoRunEnabled || clientMovementController.State.IsMoving())
		{
			Entity entity = Entities.Instance.GetEntity(type, id);
			if (entity != null)
			{
				Entities.Instance.me.Target = entity;
			}
		}
	}
}

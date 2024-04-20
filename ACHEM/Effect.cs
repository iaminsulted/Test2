using System;
using Assets.Scripts.Game;

public class Effect
{
	public enum Operation
	{
		Remove,
		Add,
		Update,
		Immune
	}

	public int ID;

	public float timestampApplied;

	public int stacks = 1;

	public float duration;

	public float clientTimeApplied;

	public int casterID;

	public Entity.Type casterType;

	public EffectTemplate template;

	public Entity caster => Entities.Instance.GetEntity(casterType, casterID);

	public event Action Updated;

	public event Action Destroyed;

	public Effect(ComEffect e)
	{
		ID = e.ID;
		template = EffectTemplates.Get(e.templateID, e.upgradeID);
		duration = e.duration;
		stacks = e.stacks;
		casterID = e.casterID;
		casterType = e.casterType;
		timestampApplied = e.timestampApplied;
		clientTimeApplied = e.timestampApplied;
		template.spellHighlightID = e.spellHighlightID;
	}

	public void Update(ComEffect e)
	{
		float num = timestampApplied;
		timestampApplied = e.timestampApplied;
		if (num < e.timestampApplied)
		{
			clientTimeApplied = GameTime.realtimeSinceServerStartup;
		}
		stacks = e.stacks;
		this.Updated?.Invoke();
	}

	public void Destroy()
	{
		this.Destroyed?.Invoke();
	}
}

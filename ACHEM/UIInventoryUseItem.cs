using UnityEngine;

public class UIInventoryUseItem : UIItem
{
	public UISprite CD;

	public float totalCoolDown;

	public float remainingCoolDown;

	public SpellTemplate spell;

	public override void Init(Item item)
	{
		base.Init(item);
		Player me = Entities.Instance.me;
		spell = SpellTemplates.Get(((InventoryItem)item).SpellID, me.effects, me.ScaledClassRank, me.EquippedClassID, 0);
		float spellTimestamp = Game.Instance.combat.GetSpellTimestamp(spell);
		float spellCooldown = Game.Instance.combat.GetSpellCooldown(spell);
		CoolDown(spellTimestamp, spellCooldown);
	}

	public void Update()
	{
		if (!(remainingCoolDown <= 0f))
		{
			remainingCoolDown -= Time.deltaTime;
			if (remainingCoolDown < 0f)
			{
				remainingCoolDown = 0f;
				CD.gameObject.SetActive(value: false);
			}
			else
			{
				CD.fillAmount = remainingCoolDown / totalCoolDown;
			}
		}
	}

	public void CoolDown(float timeStamp, float cooldown)
	{
		if (timeStamp > 0f && cooldown > 0f && GameTime.realtimeSinceServerStartup - timeStamp < cooldown)
		{
			totalCoolDown = cooldown;
			remainingCoolDown = cooldown - (GameTime.realtimeSinceServerStartup - timeStamp);
			if (remainingCoolDown > 0f)
			{
				CD.gameObject.SetActive(value: true);
			}
		}
	}
}

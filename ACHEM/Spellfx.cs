using System;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts.Game;
using UnityEngine;

public class Spellfx : MonoBehaviour
{
	public enum AttachSpot
	{
		Base,
		Head,
		Cast,
		Hit
	}

	public static int Count;

	public AttachSpot spot;

	public bool isSticky;

	public bool rotateWithSpot;

	public float LifeTime = 1f;

	public bool LiveForever;

	public string[] impacts;

	private List<float> impactTimes = new List<float>();

	private List<float> impactStatPercents = new List<float>();

	private float duration;

	private float prevDuration;

	private Entity target;

	private KeyframeSpellData spellData;

	private EffectEventTrigger eventTrigger;

	private void Start()
	{
		Count++;
		duration = 0f;
	}

	private void Update()
	{
		prevDuration = duration;
		duration += Time.deltaTime;
		CheckForImpacts();
		if (!LiveForever)
		{
			LifeTime -= Time.deltaTime;
			if (LifeTime <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		if (target != null)
		{
			target.DeathEvent -= OnTargetDeath;
		}
		Count--;
	}

	public void Init(Entity target, KeyframeSpellData initSpellData, SpellAction spellAction)
	{
		this.target = target;
		target.DeathEvent += OnTargetDeath;
		if (initSpellData != null && initSpellData.spellAction.usesFXImpacts)
		{
			spellData = new KeyframeSpellData(initSpellData);
			spellData.caster.ClearKeyframeSpellData();
		}
		eventTrigger = base.gameObject.GetComponent<EffectEventTrigger>();
		if (eventTrigger != null)
		{
			float num = spellAction?.aura?.duration ?? 1f;
			eventTrigger.Init(num);
		}
		if (impacts == null || impacts.Length == 0)
		{
			ImpactTarget(1, 1f);
			return;
		}
		for (int i = 0; i < impacts.Length; i++)
		{
			float item = 0f;
			float num2 = 0f;
			string[] array = impacts[i].Replace(" ", "").Split(',');
			if (array.Length != 0)
			{
				item = Convert.ToSingle(array[0], CultureInfo.InvariantCulture);
			}
			num2 = 1f / (float)impacts.Length;
			if (array.Length > 1)
			{
				num2 = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
			}
			impactTimes.Add(item);
			impactStatPercents.Add(num2);
		}
	}

	private void CheckForImpacts()
	{
		if (target == null || spellData == null || !spellData.spellAction.usesFXImpacts)
		{
			return;
		}
		for (int i = 0; i < impactTimes.Count; i++)
		{
			float num = impactTimes[i];
			bool num2 = prevDuration < num && duration >= num;
			bool flag = num == 0f && prevDuration == 0f && duration > num;
			if ((num2 || flag) && impactStatPercents.Count > i)
			{
				ImpactTarget(impactTimes.Count, impactStatPercents[i]);
			}
		}
	}

	private void ImpactTarget(int totalImpacts, float statDeltaPercent)
	{
		if (spellData != null && spellData.spellAction.usesFXImpacts)
		{
			target.HandleSpellImpact(spellData, Entity.ImpactSource.FX, totalImpacts, statDeltaPercent);
		}
	}

	private void OnTargetDeath(Entity _)
	{
		if (eventTrigger != null)
		{
			eventTrigger.ForceEffectsEnd();
		}
	}
}

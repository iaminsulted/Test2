using UnityEngine;

public class UICastBar : MonoBehaviour
{
	public UILabel Text;

	public UIProgressBar Slider;

	public UISprite bg;

	public TweenColor tweencolor;

	private float ts;

	private float chargeTime;

	private bool isCharging;

	private bool isChannel;

	private Entity entity;

	public void Hide()
	{
		NGUITools.SetActive(base.gameObject, state: false);
		Clear();
	}

	public void OnDestroy()
	{
		Clear();
	}

	public void Update()
	{
		if (isCharging)
		{
			UpdateSlider();
		}
	}

	private void UpdateSlider()
	{
		float num = (GameTime.realtimeSinceServerStartup - ts) / chargeTime;
		Slider.value = (isChannel ? (1f - num) : num);
	}

	private void Clear()
	{
		if (entity != null)
		{
			entity.ChargeStarted -= OnChargeStart;
			entity.ChannelStarted -= OnChannelStart;
			entity.CastCanceled -= OnCastCancel;
			entity.ChargeEnded -= OnChargeEnd;
			entity.ChannelEnded -= OnChannelEnd;
			entity = null;
		}
	}

	public void Init(Entity entity)
	{
		Clear();
		this.entity = entity;
		entity.ChargeStarted += OnChargeStart;
		entity.ChannelStarted += OnChannelStart;
		entity.CastCanceled += OnCastCancel;
		entity.ChargeEnded += OnChargeEnd;
		entity.ChannelEnded += OnChannelEnd;
		if (entity.spellCastData != null)
		{
			SpellTemplate spellT = entity.spellCastData.spellT;
			float num = Mathf.Max(entity.spellCastData.chargeTime, entity.spellCastData.channelTime);
			if (spellT != null && !spellT.hideCastBar && num > 0f)
			{
				string spellName = GetSpellName(entity, spellT);
				Show(spellName + "...", num, GameTime.realtimeSinceServerStartup - entity.spellCastData.ts);
			}
		}
		else
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
	}

	private static string GetSpellName(Entity caster, SpellTemplate spellT)
	{
		string result = spellT.name;
		if (caster is NPC nPC)
		{
			NpcSpell currentNpcSpell = nPC.GetCurrentNpcSpell();
			if (!string.IsNullOrEmpty(currentNpcSpell?.SpellOptions?.name))
			{
				result = currentNpcSpell.SpellOptions.name;
			}
		}
		return result;
	}

	private void OnChargeStart(SpellTemplate spellT, float chargeTime)
	{
		if (chargeTime > 0f && !spellT.hideCastBar)
		{
			string spellName = GetSpellName(entity, spellT);
			Show(spellName + "...", chargeTime);
		}
	}

	private void OnChannelStart(SpellTemplate spellT, int channelTicks, float channelTime)
	{
		if (channelTime > 0f && !spellT.hideCastBar)
		{
			string spellName = GetSpellName(entity, spellT);
			Show(spellName + "...", channelTime, 0f, isChannel: true);
		}
	}

	private void OnChargeEnd()
	{
		Complete();
	}

	private void OnChannelEnd()
	{
		Complete();
	}

	private void OnCastCancel()
	{
		Interrupt();
	}

	public void Show(string text, float duration, float timeElapsed = 0f, bool isChannel = false)
	{
		if (!(duration <= 0f))
		{
			Text.text = text;
			bg.color = (isChannel ? InterfaceColors.CastBar.Channel : InterfaceColors.CastBar.Charge);
			tweencolor.enabled = false;
			ts = GameTime.realtimeSinceServerStartup - timeElapsed;
			chargeTime = duration;
			this.isChannel = isChannel;
			UpdateSlider();
			NGUITools.SetActive(base.gameObject, state: true);
			isCharging = true;
		}
	}

	public void Interrupt()
	{
		Text.text = "CANCELLED";
		isCharging = false;
		tweencolor.ResetToBeginning();
		tweencolor.duration = 0.5f;
		tweencolor.from = bg.color;
		tweencolor.to = InterfaceColors.CastBar.Interrupt;
		tweencolor.PlayForward();
	}

	public void Complete()
	{
		isCharging = false;
		NGUITools.SetActive(base.gameObject, state: false);
	}

	public void OnCastBarTweenColorComplete()
	{
		NGUITools.SetActive(base.gameObject, state: false);
	}
}

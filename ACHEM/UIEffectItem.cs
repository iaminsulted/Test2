using System;
using UnityEngine;

public class UIEffectItem : MonoBehaviour, IComparable<UIEffectItem>
{
	public static Color32 BuffColor = new Color32(0, 191, 67, byte.MaxValue);

	public static Color32 DebuffColor = new Color32(234, 57, 0, byte.MaxValue);

	public static Color32 ColorOutlineGlow = new Color32(byte.MaxValue, 228, 122, byte.MaxValue);

	public static Color32 ColorOutlineWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public UISprite BG;

	public UISprite CD;

	public UISprite Icon;

	public UILabel lblCount;

	private float dbDuration;

	private float clientStartTime;

	private float viewDuration;

	public Effect effect;

	private Entity caster;

	public event Action<UIEffectItem> Destroyed;

	public virtual void SetItem(Effect newEffect, bool forceLarge = false)
	{
		Clear();
		effect = newEffect;
		effect.Updated += OnEffectUpdated;
		effect.Destroyed += OnEffectDestroyed;
		caster = effect.caster;
		float num = ((forceLarge || IsLarge()) ? 1f : 0.85f);
		base.gameObject.transform.localScale = new Vector3(num, num);
		if (effect.template.isHarmful)
		{
			UISprite bG = BG;
			UIBasicSprite.Flip flip2 = (CD.flip = UIBasicSprite.Flip.Nothing);
			bG.flip = flip2;
			Icon.transform.localPosition = new Vector3(14.5f, -24f, 0f);
			BG.transform.localPosition = new Vector3(2.5f, -4f, 0f);
			BG.color = DebuffColor;
		}
		else
		{
			UISprite bG2 = BG;
			UIBasicSprite.Flip flip2 = (CD.flip = UIBasicSprite.Flip.Vertically);
			bG2.flip = flip2;
			Icon.transform.localPosition = new Vector3(14.5f, -21f, 0f);
			BG.transform.localPosition = new Vector3(2.5f, -3f, 0f);
			BG.color = BuffColor;
		}
		Icon.spriteName = effect.template.icon;
		UpdateCount();
		clientStartTime = effect.clientTimeApplied;
		dbDuration = effect.duration;
		viewDuration = dbDuration - (clientStartTime - effect.timestampApplied);
		UpdateDurationFill();
	}

	private void OnEffectUpdated()
	{
		UpdateCount();
		clientStartTime = effect.clientTimeApplied;
		dbDuration = effect.duration;
		viewDuration = dbDuration - (clientStartTime - effect.timestampApplied);
	}

	private void UpdateCount()
	{
		if (lblCount == null || effect == null)
		{
			Debug.LogException(new Exception("UIEffectItem error: lblCount null? " + (lblCount == null) + ", effect null? " + (effect == null)));
			return;
		}
		if (effect.template == null)
		{
			Debug.LogException(new Exception("UIEffectItem error: effect.template is null"));
			return;
		}
		lblCount.enabled = effect.template.maxStack > 1;
		if (effect.template.maxStack > 1 && effect.stacks == effect.template.maxStack)
		{
			lblCount.fontStyle = FontStyle.Bold;
		}
		else
		{
			lblCount.fontStyle = FontStyle.Normal;
		}
		if (effect.template.maxStack > 1)
		{
			lblCount.text = effect.stacks.ToString();
		}
	}

	public void Clear()
	{
		if (effect != null)
		{
			effect.Updated -= OnEffectUpdated;
			effect.Destroyed -= OnEffectDestroyed;
			effect = null;
		}
	}

	private void OnEffectDestroyed()
	{
		Clear();
		if (this.Destroyed != null)
		{
			this.Destroyed(this);
		}
	}

	public void Update()
	{
		CheckForDestroyFallback();
		UpdateDurationFill();
	}

	private void CheckForDestroyFallback()
	{
		if (effect.duration > 0f && GameTime.realtimeSinceServerStartup - clientStartTime > effect.duration * 2f)
		{
			OnEffectDestroyed();
		}
	}

	private void UpdateDurationFill()
	{
		if (viewDuration > 0f)
		{
			CD.fillAmount = 1f - (GameTime.realtimeSinceServerStartup - clientStartTime) / viewDuration;
		}
		else
		{
			CD.fillAmount = 1f;
		}
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(effect.template.ToolTipText, UIGame.Instance.FixedToolTipPosition);
		}
		else
		{
			Tooltip.Hide();
		}
	}

	private bool IsLarge()
	{
		bool flag = effect.template.isHarmful && Entities.Instance.me.effects.Contains(effect);
		if (caster != null)
		{
			if (!flag)
			{
				return caster.isMe;
			}
			return true;
		}
		return false;
	}

	public int CompareTo(UIEffectItem other)
	{
		if (effect.template.isHarmful != other.effect.template.isHarmful)
		{
			if (!effect.template.isHarmful)
			{
				return -1;
			}
			return 1;
		}
		if (IsLarge() != other.IsLarge())
		{
			if (!IsLarge())
			{
				return 1;
			}
			return -1;
		}
		return effect.timestampApplied.CompareTo(other.effect.timestampApplied);
	}
}

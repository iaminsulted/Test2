using System.Collections;
using UnityEngine;

public class UIActionSpell : UIActionButton
{
	public UISprite UltMeter;

	public UISprite UltDisable;

	public TweenFill UltTween;

	public PulseColor UltPulse;

	public ParticleSystem UltParticle;

	public UITweener FlashingIndicator;

	private float ultCharge;

	protected override bool IsLocked()
	{
		if (base.spellT == null)
		{
			return true;
		}
		return base.spellT.reqClassRank > Session.MyPlayerData.ScaledClassRank;
	}

	public void Init(int spellID)
	{
		UpdateSpell(spellID);
		if (base.spellT != null)
		{
			UpdateLock();
			if (base.spellT.isUlt)
			{
				UpdateUltCharge(0f);
			}
			Session.MyPlayerData.ClassRankUpdated += RankUpdated;
			Session.MyPlayerData.ClassEquipped += ClassEquipped;
		}
	}

	protected override void UpdateLock()
	{
		base.UpdateLock();
		if (base.spellT.isUlt)
		{
			UpdateUltCharge(0f);
		}
	}

	public override void StatUpdate(int resourceCost, float playerResource, float playerHealth, float ultCharge)
	{
		base.StatUpdate(resourceCost, playerResource, playerHealth, ultCharge);
		UpdateUltCharge(ultCharge);
	}

	private void RankUpdated(int classID, int newRank)
	{
		UpdateSpell(base.spellT.ID);
		CheckForUnlock();
	}

	private void ClassEquipped(int classID)
	{
		CheckForUnlock();
	}

	public void UpdateUltCharge(float newUltCharge)
	{
		if (!base.spellT.isUlt)
		{
			return;
		}
		if (IsLocked())
		{
			UltDisable.gameObject.SetActive(value: true);
			UltMeter.fillAmount = 1f;
			UltPulse.enabled = false;
			Icon.color = InterfaceColors.ActionButton.Default_Color;
			return;
		}
		newUltCharge = Mathf.Clamp(newUltCharge, 0f, 1000f);
		float num = newUltCharge - ultCharge;
		ultCharge = newUltCharge;
		if (isCastable)
		{
			UltDisable.gameObject.SetActive(ultCharge < 1000f);
			UltPulse.enabled = ultCharge >= 1000f;
			if (!UltPulse.isActiveAndEnabled)
			{
				Icon.color = InterfaceColors.ActionButton.Default_Color;
			}
		}
		else
		{
			UltDisable.gameObject.SetActive(value: false);
			UltPulse.enabled = false;
			Icon.color = InterfaceColors.ActionButton.Unable_To_Cast;
		}
		if (num != 0f)
		{
			float num2 = Mathf.Clamp(num / 1000f, 0.1f, 1f);
			UltTween.duration = num2;
			UltTween.tweenFactor = 0f;
			UltTween.from = UltMeter.fillAmount;
			UltTween.to = (1000f - newUltCharge) / 1000f;
			UltTween.PlayForward();
			if (newUltCharge >= 1000f)
			{
				StartCoroutine(UltReadyRoutine(num2));
			}
		}
	}

	private IEnumerator UltReadyRoutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		UltParticle.Play();
		AudioManager.Play2DSFX("UltimateReady");
	}

	public void MakeSpellFlash(bool shouldFlash)
	{
		if (shouldFlash)
		{
			FlashTween.style = UITweener.Style.Loop;
			FlashTween.enabled = true;
		}
		else
		{
			FlashTween.style = UITweener.Style.Once;
		}
	}
}

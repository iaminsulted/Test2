using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectTrigger))]
public class EffectEventTrigger : MonoBehaviour
{
	[Serializable]
	public class EffectEvent
	{
		public enum TargetType
		{
			ParticleSystem,
			Animator
		}

		public enum AnimatorActions
		{
			Play,
			CrossFade,
			SetFloat,
			SetTrigger,
			SetInt
		}

		public enum ParticleActions
		{
			Play,
			Pause,
			Stop
		}

		public enum EventType
		{
			Any,
			Starting,
			Looping,
			Ending
		}

		public EffectTrigger et;

		public TargetType targetType;

		public AnimatorActions animAction;

		public ParticleActions psAction;

		public EventType eventType;

		public int selectedParticle;

		public Animator selectedAnimator;

		public bool disableAfter;

		public float disableAfterTime;

		public float startDelay;

		public string stateName;

		public float cfTime;

		public float pauseTime;

		public int particleIndex;

		public int intParam;

		public float fltParam;

		public string parameterName;

		public EffectEvent(EffectTrigger trigger)
		{
			et = trigger;
		}

		public void ExecuteEffect()
		{
			switch (targetType)
			{
			case TargetType.Animator:
				switch (animAction)
				{
				case AnimatorActions.Play:
					selectedAnimator.Play(stateName);
					break;
				case AnimatorActions.CrossFade:
					selectedAnimator.CrossFadeInFixedTime(stateName, cfTime);
					break;
				case AnimatorActions.SetFloat:
					selectedAnimator.SetFloat(parameterName, fltParam);
					break;
				case AnimatorActions.SetInt:
					selectedAnimator.SetInteger(parameterName, intParam);
					break;
				case AnimatorActions.SetTrigger:
					selectedAnimator.SetTrigger(parameterName);
					break;
				}
				if (disableAfter)
				{
					et.DisableObjectAfterTimeInit(selectedAnimator.gameObject, disableAfterTime);
				}
				break;
			case TargetType.ParticleSystem:
				switch (psAction)
				{
				case ParticleActions.Play:
					et.PlayEffect(selectedParticle);
					break;
				case ParticleActions.Pause:
					et.PauseEffect(selectedParticle);
					break;
				case ParticleActions.Stop:
					et.StopEffect(selectedParticle);
					break;
				}
				break;
			}
		}

		public void StopEffect()
		{
			if (targetType == TargetType.ParticleSystem)
			{
				et.StopEffect(selectedParticle);
			}
		}

		public IEnumerator DisableAnimatorObject()
		{
			yield return new WaitForSeconds(disableAfterTime);
			selectedAnimator.gameObject.SetActive(value: false);
		}
	}

	[Tooltip("After Starting events are called, how long in seconds should the system wait to play all Ending events.")]
	public float lifetime;

	[Tooltip("Play automatically? Should be true for SpellFX or Aura effects and false for anything stored on a monster's Prefab.")]
	public bool autoStart = true;

	public List<EffectEvent> effectEvents = new List<EffectEvent>();

	public List<EffectEvent> startEffects = new List<EffectEvent>();

	public List<EffectEvent> loopEffects = new List<EffectEvent>();

	public List<EffectEvent> endEffects = new List<EffectEvent>();

	public void PlayStartEffects()
	{
		foreach (EffectEvent startEffect in startEffects)
		{
			StartCoroutine(CheckDelayAndExecute(startEffect));
		}
		PlayLoopEffects();
	}

	public void PlayLoopEffects()
	{
		foreach (EffectEvent loopEffect in loopEffects)
		{
			StartCoroutine(CheckDelayAndExecute(loopEffect));
		}
	}

	public void PlayEndEffects()
	{
		foreach (EffectEvent endEffect in endEffects)
		{
			StartCoroutine(CheckDelayAndExecute(endEffect));
		}
	}

	public void ForceEffectsEnd()
	{
		StopAllCoroutines();
		foreach (EffectEvent loopEffect in loopEffects)
		{
			loopEffect.StopEffect();
		}
		foreach (EffectEvent endEffect in endEffects)
		{
			StartCoroutine(CheckDelayAndExecute(endEffect));
		}
	}

	public IEnumerator CheckDelayAndExecute(EffectEvent effect)
	{
		float seconds = ((effect.targetType != 0 || effect.psAction != EffectEvent.ParticleActions.Pause) ? effect.startDelay : (effect.startDelay + effect.pauseTime));
		yield return new WaitForSeconds(seconds);
		effect.ExecuteEffect();
	}

	public IEnumerator EventTimeline()
	{
		PlayStartEffects();
		yield return new WaitForSeconds(lifetime);
		PlayEndEffects();
	}

	public void SetLifetime(float newLifetime)
	{
		lifetime = newLifetime;
	}

	public void Init(float duration)
	{
		SetLifetime(duration);
		StartCoroutine("EventTimeline");
	}

	public void Start()
	{
		if (autoStart)
		{
			Init(lifetime);
		}
	}
}

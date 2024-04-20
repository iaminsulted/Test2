using UnityEngine;

public class ManageTriggerOnStateEvent : StateMachineBehaviour
{
	public StateEventWhenToManageTrigger stateEventWhenToManageTrigger;

	public TriggerActionToManageOnStateEvent triggerActionToManageOnStateEvent;

	public string parameterToTriggerOnStateEvent = "";

	public float delayToManageTriggerMin;

	public bool randomizeDelayBetweenTwoValues;

	public float delayToManageTriggerMax;

	private float timeToManageTrigger;

	public float layerWeightThresholdToTrigger;

	private bool manageTriggerStarted;

	public void SetTrigger(Animator animator)
	{
		animator.SetTrigger(parameterToTriggerOnStateEvent);
	}

	public void ResetTrigger(Animator animator)
	{
		animator.ResetTrigger(parameterToTriggerOnStateEvent);
	}

	public void ProcessTrigger(Animator animator)
	{
		if (triggerActionToManageOnStateEvent == TriggerActionToManageOnStateEvent.SetTrigger)
		{
			animator.SetTrigger(parameterToTriggerOnStateEvent);
		}
		else if (triggerActionToManageOnStateEvent == TriggerActionToManageOnStateEvent.ResetTrigger)
		{
			animator.SetTrigger(parameterToTriggerOnStateEvent);
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger || stateEventWhenToManageTrigger != 0)
		{
			return;
		}
		if (delayToManageTriggerMin == 0f)
		{
			ProcessTrigger(animator);
			manageTriggerStarted = false;
			return;
		}
		if (randomizeDelayBetweenTwoValues)
		{
			timeToManageTrigger = Time.time + Random.Range(delayToManageTriggerMin, delayToManageTriggerMax);
		}
		else
		{
			timeToManageTrigger = Time.time + delayToManageTriggerMin;
		}
		manageTriggerStarted = true;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (!(animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger) && stateEventWhenToManageTrigger == StateEventWhenToManageTrigger.OnStateExit)
		{
			SetTrigger(animator);
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (!(animator.GetLayerWeight(layerIndex) < layerWeightThresholdToTrigger))
		{
			if (stateEventWhenToManageTrigger == StateEventWhenToManageTrigger.OnStateUpdate && Time.time > timeToManageTrigger)
			{
				ProcessTrigger(animator);
			}
			else if (manageTriggerStarted && Time.time > timeToManageTrigger)
			{
				manageTriggerStarted = false;
				ProcessTrigger(animator);
			}
		}
	}
}
